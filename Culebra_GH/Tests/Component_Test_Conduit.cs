using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Drawing;
using System.Reflection;
using ikvm;
using processing.core;
using culebra.behaviors;
using CulebraData;
using CulebraData.Objects;
using CulebraData.Utilities;
using CulebraData.Drawing;

namespace Culebra_GH.Tests
{
    public class Component_Test_Conduit : GH_Component
    {
        //-----------------Global Variables---------------------------
        private List<Vector3d> moveList = new List<Vector3d>();
        private List<Vector3d> startList = new List<Vector3d>();
        private DataTree<Point3d> posTree;
        private Creeper creep;
        private List<Creeper> creepList = new List<Creeper>();
        private List<Point3d> currentPosList = new List<Point3d>();
        private List<Line> networkList = new List<Line>();
        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private BoundingBox bb;
        private int dimensions;
        private int minthick = new int();
        private int maxthick = new int();
        private bool convert = new bool();

        /// <summary>
        /// Initializes a new instance of the ComponentTest class.
        /// </summary>
        public Component_Test_Conduit()
            : base("Compolnent_Test_Conduit", "Nickname",
                "Description",
                "Culebra_GH", "Subcategory")
        {
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("reset", "r", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MinThick", "MT", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("MaxThick", "MT", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ConvertGeometry", "CG", "", GH_ParamAccess.item);

        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Creeps", "C", "C", GH_ParamAccess.list);
            pManager.AddPointParameter("Trails", "T", "T", GH_ParamAccess.tree);
            pManager.AddLineParameter("Network", "N", "N", GH_ParamAccess.tree);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("culebra"));
            ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("IKVM.OpenJDK.Core"));

            Random rnd = new Random();
            List<Vector3d> vecs = new List<Vector3d>();
            List<Point3d> returnedvecs = new List<Point3d>();
            
            bool reset = new bool();
            int ptCount = 500;
            int dimension = 0;
            bool trail = true;
           
            float moveValue = 3.44f;
            float searchRad = 44.04f;
            float aligVal = 0.04f;
            float sepVal = 0.15f;
            float cohVal = 0.24f;

            if(!DA.GetData(0, ref reset))return;
            if (!DA.GetData(1, ref this.minthick)) return;
            if (!DA.GetData(2, ref this.maxthick)) return;
            if (!DA.GetData(3, ref this.convert)) return;

            if (reset)
            { //we are using the reset to reinitialize all the variables and positions to pass to the class once we are running
                this.moveList = new List<Vector3d>();
                this.startList = new List<Vector3d>();
                this.posTree = new DataTree<Point3d>();
                this.dimensions = dimension;
                creepList = new List<Creeper>();
                currentPosList = new List<Point3d>();
                networkList = new List<Line>();
                if (this.dimensions == 0)
                {
                    this.bb = new BoundingBox(-250, -250, 0, 250, 250, 0);
                }
                else
                {
                    this.bb = new BoundingBox(-250, -250, -250, 250, 250, 500);
                }
                for (int i = 0; i < ptCount; i++)
                {
                    if (this.dimensions == 0)
                    { //IF WE WANT 2D
                        //this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only
                        //this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area
                        this.moveVec = new Vector3d(rnd.Next(-1, 2) * 0.5, rnd.Next(-1, 2) * 0.5, 0); //move randomly in any direction 2d
                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area

                        this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                        this.creepList.Add(this.creep);                     
                    }
                    else
                    { //IF WE WANT 3D
                        this.moveVec = new Vector3d(rnd.Next(-1, 2), rnd.Next(-1, 2), 0.5); //move randomly in the xy axis and up in the z axis
                        this.moveVec *= moveValue;
                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), (int)bb.Min[2]); //start randomly on the lowest plane of the 3d bounds
                    }
                    this.startList.Add(this.startPos); //add the initial starting positions to the list to pass once we start running
                    this.moveList.Add(this.moveVec); //add the initial move vectors to the list to pass once we start running
                }
                DA.SetDataList(0, this.startList);
            }
            else
            {
                currentPosList = new List<Point3d>();
                DataTree<Point3d> trailTree = new DataTree<Point3d>();
                DataTree<Line> networkTree = new DataTree<Line>();
                particleSystem = new ParticleSystem();
                int counter = 0;
                randomGen = new Random();
                foreach (Creeper c in this.creepList)
                {                  
                    networkList = new List<Line>();
                    c.attributes.setMoveAttributes(3.44f, 0.130f, 1.5f);
                    //c.behaviors.wander2D();
                    c.behaviors.flock2D(searchRad, cohVal, sepVal, aligVal, 360f, this.creepList, false);                 
                    GH_Path path = new GH_Path(counter);                  
                    if (convert)
                    {
                        List<Vector3d> testList = c.attributes.getNetwork();
                        if (testList.Count > 0)
                        {

                            foreach (Vector3d v in testList)
                            {
                                Line l = new Line(c.attributes.getLocation(), (Point3d)v);
                                networkList.Add(l);

                            }
                            networkTree.AddRange(networkList, path);
                        }
                    }
                    c.actions.move();
                    c.actions.bounce(bb);
                    //c.actions.respawn(bb);                 
                    //-------ADD POINTS TO GRAPHIC POINTS LIST---------
                    particleList.Add(c.attributes.getLocation());
                    this.particleSet.AddRange(c.attributes.getTrailPoints(), path);
                    //-------PARTICLE STUFF---------
                    //Particle particle = new Particle();    
                    //particle.Location = c.attributes.getLocation();
                    //particle.Color = Color.FromArgb(50, 255, 0, 255);
                    //particle.Size = 10;
                    //this.particleSystem.Add(particle);
                    if (convert)
                    {
                        currentPosList.Add(c.attributes.getLocation());
                        trailTree.AddRange(c.attributes.getTrailPoints(),path);                                  
                    }
                    counter++;
                }
                if (convert)
                {
                    DA.SetDataList(0, currentPosList);
                    if (trail)
                    {
                        DA.SetDataTree(1, trailTree);
                    }
                    DA.SetDataTree(2, networkTree);
                }
            }
        }
        public List<Point3d> particleList = new List<Point3d>();
        public DataTree<Point3d> particleSet = new DataTree<Point3d>();
        public ParticleSystem particleSystem = new ParticleSystem();
        public Random randomGen = new Random();
        public Color randomColorAction = new Color();
        private BoundingBox _clippingBox;
        public Vizualization viz = new Vizualization();
        public string file = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + @"\NeGeo3.png";

        protected override void BeforeSolveInstance()
        {
            if (!this.convert)
            {
                this.particleList.Clear();
                this.particleSet.Clear();
                this.particleSystem.Clear();
                _clippingBox = BoundingBox.Empty;
            }
        }
        protected override void AfterSolveInstance()
        {
            if (!this.convert)
            {
                _clippingBox = new BoundingBox(particleList);
            }
        }
        public override BoundingBox ClippingBox
        {
            get
            {
                return _clippingBox;
            }
        }
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (!this.convert)
            {
                viz.drawSprites(args, file, particleList);
                //viz.drawDiscoTrails(args, file, particleList, particleSet, randomGen, randomColorAction, this.minthick, this.maxthick);
                viz.drawGradientTrails(args, file, particleList, particleSet, randomColorAction, this.minthick, this.maxthick);
            }
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{ebac0952-2092-4c4c-893b-ec26f8bd7a2f}"); }
        }
    }
}
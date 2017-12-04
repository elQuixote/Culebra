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
    public class Component_Test_MapBehavior : GH_Component
    {
        //-----------------Global Variables---------------------------
        private List<Vector3d> moveList = new List<Vector3d>();
        private List<Vector3d> startList = new List<Vector3d>();
        private DataTree<Point3d> posTree;
        private Creeper creep;
        private List<CulebraObject> creepList = new List<CulebraObject>();
        private List<Point3d> currentPosList = new List<Point3d>();
        private List<Line> networkList = new List<Line>();
        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private BoundingBox bb;
        private bool dimensions;
        private int minthick = new int();
        private int maxthick = new int();
        private bool convert = new bool();

        private bool triggerBabies = false;
        private List<Vector3d> childSpawners = new List<Vector3d>();
        private List<int> childSpawnType = new List<int>();

        /// <summary>
        /// Initializes a new instance of the ComponentTest class.
        /// </summary>
        public Component_Test_MapBehavior()
            : base("Component_Test_MapBehavior", "Nickname",
                "Description",
                "Culebra_GH", "Testing")
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
            pManager.AddBooleanParameter("3D", "3D", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("Polyline", "P", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("TriggerBabies", "TB", "TB", GH_ParamAccess.item);
            pManager.AddIntegerParameter("ObjectCount", "OC", "OC", GH_ParamAccess.item);
            pManager.AddMeshParameter("ColorMesh", "CM", "ColoredMeshInput", GH_ParamAccess.item);

            pManager[5].Optional = true;
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Creeps", "C", "C", GH_ParamAccess.list);
            pManager.AddPointParameter("Trails", "T", "T", GH_ParamAccess.tree);
            pManager.AddLineParameter("Network", "N", "N", GH_ParamAccess.tree);
            pManager.AddBoxParameter("BoundingBox", "BB", "BB", GH_ParamAccess.list);
            pManager.AddPointParameter("BabyType_A", "BTA", "BTA", GH_ParamAccess.tree);
            pManager.AddPointParameter("BabyType_B", "BTB", "BTB", GH_ParamAccess.tree);
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

            List<Polyline> plineList = new List<Polyline>();
            List<Curve> crvList = new List<Curve>();

            bool reset = new bool();
            int ptCount = new int();
            bool trail = true;
            bool td = new bool();

            float moveValue = 3.44f;
            float searchRad = 40.04f;
            float aligVal = 0.04f;
            float sepVal = 0.09f;
            float cohVal = 0.24f;

            Mesh colorMesh = new Mesh();

            if (!DA.GetData(0, ref reset)) return;
            if (!DA.GetData(1, ref this.minthick)) return;
            if (!DA.GetData(2, ref this.maxthick)) return;
            if (!DA.GetData(3, ref this.convert)) return;
            if (!DA.GetData(4, ref td)) return;
            if (!DA.GetDataList(5, crvList)) return;
            if (!DA.GetData(6, ref this.triggerBabies)) return;
            if (!DA.GetData(7, ref ptCount)) return;
            if (!DA.GetData(8, ref colorMesh)) return;

            foreach (Curve crv in crvList)
            {
                Polyline polyline = new Polyline();
                crv.TryGetPolyline(out polyline);
                plineList.Add(polyline);
            }

            if (reset)
            { //we are using the reset to reinitialize all the variables and positions to pass to the class once we are running
                this.moveList = new List<Vector3d>();
                this.startList = new List<Vector3d>();
                this.posTree = new DataTree<Point3d>();
                this.dimensions = td;
                creepList = new List<CulebraObject>();
                currentPosList = new List<Point3d>();
                networkList = new List<Line>();

                this.childSpawners = new List<Vector3d>();
                this.childSpawnType = new List<int>();

                if (this.dimensions == false)
                {
                    this.bb = new BoundingBox(-250, -250, 0, 250, 250, 0);
                }
                else
                {
                    this.bb = new BoundingBox(-250, -250, -250, 250, 250, 500);
                }
                for (int i = 0; i < ptCount; i++)
                {
                    if (this.dimensions == false)
                    { //IF WE WANT 2D
                        this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only
                        this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area
                        //this.moveVec = new Vector3d(rnd.Next(-1, 2) * 0.5, rnd.Next(-1, 2) * 0.5, 0); //move randomly in any direction 2d
                        //this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area

                        this.creep = new Creeper(this.startPos, this.moveVec, true, dimensions);
                        this.creepList.Add(this.creep);
                    }
                    else
                    { //IF WE WANT 3D
                        //this.moveVec = new Vector3d(rnd.Next(-1, 2), rnd.Next(-1, 2), 0.5); //move randomly in the xy axis and up in the z axis
                        //this.moveVec *= moveValue;
                        //this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), (int)bb.Min[2]); //start randomly on the lowest plane of the 3d bounds

                        this.moveVec = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5); //move randomly in any direction 3d
                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), rnd.Next((int)bb.Min[2], (int)bb.Max[2])); //spawn randomly inside the bounding area

                        this.creep = new Creeper(this.startPos, this.moveVec, true, dimensions);
                        this.creepList.Add(this.creep);
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

                DataTree<Point3d> trailTree_ChildA = new DataTree<Point3d>();
                DataTree<Point3d> trailTree_ChildB = new DataTree<Point3d>();
                foreach (Creeper c in this.creepList)
                {
                    networkList = new List<Line>();
                    c.attributes.SetMoveAttributes(3.44f, 0.330f, 1.5f);

                    //c.behaviors.multiPolylineTracker(plineList, 500.0f, 50.0f, 15.0f);
                    //c.behaviors.multiPolylineTrackerBabyMaker(plineList, 500.0f, 50.0f, 15.0f, this.triggerBabies, 2, true, this.childSpawners, this.childSpawnType);
                    this.childSpawners = c.behaviors.GetChildStartPositions();
                    this.childSpawnType = c.behaviors.GetChildSpawnTypes();

                    GH_Path path = new GH_Path(counter);
                    if (c is BabyCreeper)
                    {
                        c.behaviors.Wander3D(2.0f, 10.0f, 20.0f, 6.0f);

                        //c.behaviors.flock3D(searchRad, cohVal, 0.09f, aligVal, 360f, this.creepList, false);

                        if (c.attributes.GetChildType() == "a")
                        {
                            //-------ADD POINTS TO GRAPHIC POINTS LIST---------
                            particleBabyAList.Add(c.attributes.GetLocation());
                            this.particleBabyASet.AddRange(c.attributes.GetTrailPoints(), path);

                            if (convert)
                            {
                                currentPosList.Add(c.attributes.GetLocation());
                                trailTree_ChildA.AddRange(c.attributes.GetTrailPoints(), path);
                            }
                        }
                        else
                        {
                            //-------ADD POINTS TO GRAPHIC POINTS LIST---------
                            particleBabyBList.Add(c.attributes.GetLocation());
                            this.particleBabyBSet.AddRange(c.attributes.GetTrailPoints(), path);
                            if (convert)
                            {
                                currentPosList.Add(c.attributes.GetLocation());
                                trailTree_ChildB.AddRange(c.attributes.GetTrailPoints(), path);
                            }
                        }
                    }
                    else
                    {
                        //c.behaviors.Wander3D(2.0f, 10.0f, 20.0f, 6.0f);

                        //c.behaviors.Flock2D(searchRad, cohVal, sepVal, aligVal, 360f, this.creepList, false, true, true, true, colorMesh);
                        c.behaviors.Separate(10, this.creepList, true, 0, 2, colorMesh);

                        //c.behaviors.flock3D(searchRad, cohVal, sepVal, aligVal, 360f, this.creepList, false);
                        //-------ADD POINTS TO GRAPHIC POINTS LIST---------
                        particleList.Add(c.attributes.GetLocation());
                        this.particleSet.AddRange(c.attributes.GetTrailPoints(), path);

                        if (convert)
                        {
                            currentPosList.Add(c.attributes.GetLocation());
                            trailTree.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                    }
                    if (convert)
                    {
                        List<Vector3d> testList = c.attributes.GetNetwork();
                        if (testList.Count > 0)
                        {

                            foreach (Vector3d v in testList)
                            {
                                Line l = new Line(c.attributes.GetLocation(), (Point3d)v);
                                networkList.Add(l);

                            }
                            networkTree.AddRange(networkList, path);
                        }
                    }
                    //c.actions.move();
                    c.actions.Move(0, 400);
                    c.actions.Bounce(bb);
                    //c.actions.respawn(bb);                 
                    //-------PARTICLE STUFF---------
                    //Particle particle = new Particle();    
                    //particle.Location = c.attributes.getLocation();
                    //particle.Color = Color.FromArgb(50, 255, 0, 255);
                    //particle.Size = 10;
                    //this.particleSystem.Add(particle);
                    counter++;
                }
                if (convert)
                {
                    DA.SetDataList(0, currentPosList);
                    if (trail)
                    {
                        DA.SetDataTree(1, trailTree);
                        DA.SetDataTree(4, trailTree_ChildA);
                        DA.SetDataTree(5, trailTree_ChildB);
                    }
                    DA.SetDataTree(2, networkTree);
                }
                if (this.childSpawners.Count > 0)
                {
                    newDude();
                    this.childSpawners = new List<Vector3d>();
                    this.childSpawnType = new List<int>();
                }
                DA.SetDataList(3, this.bb.GetEdges());
            }
        }
        public void newDude()
        {
            Random rnd = new Random();
            int babyCount = 0;
            foreach (Vector3d px in this.childSpawners)
            {
                Vector3d speed;
                if (this.dimensions == false)
                {
                    speed = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, 0);

                }
                else
                {
                    speed = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5);
                }
                Creeper a;

                if (this.childSpawnType[babyCount] % 2 == 0)
                {
                    a = new BabyCreeper(new Vector3d(px.X, px.Y, px.Z), speed, false, "a", false);
                    this.creepList.Add(a);
                }
                else
                {
                    a = new BabyCreeper(new Vector3d(px.X, px.Y, px.Z), speed, false, "b", false);
                    this.creepList.Add(a);
                }
                babyCount++;
            }
        }
        public List<Point3d> particleList = new List<Point3d>();
        public List<Point3d> particleBabyAList = new List<Point3d>();
        public List<Point3d> particleBabyBList = new List<Point3d>();
        public DataTree<Point3d> particleSet = new DataTree<Point3d>();
        public DataTree<Point3d> particleBabyASet = new DataTree<Point3d>();
        public DataTree<Point3d> particleBabyBSet = new DataTree<Point3d>();
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
                this.particleBabyAList.Clear();
                this.particleBabyBList.Clear();
                this.particleSet.Clear();
                this.particleBabyASet.Clear();
                this.particleBabyBSet.Clear();
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
                viz.DrawSprites(args, file, particleList);
                //viz.drawDiscoTrails(args, file, particleList, particleSet, randomGen, this.minthick, this.maxthick);
                viz.DrawGradientTrails(args, particleSet, 0, this.minthick, this.maxthick);
                viz.DrawGradientTrails(args, particleBabyASet, 1, this.minthick, this.maxthick);
                viz.DrawGradientTrails(args, particleBabyBSet, 2, this.minthick, this.maxthick);
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
                return Culebra_GH.Properties.Resources.Testing;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{fbd507a3-a56b-41e8-b0f8-89f0fc192869}"); }
        }
    }
}
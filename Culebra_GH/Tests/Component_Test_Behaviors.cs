using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using System.Reflection;
using ikvm;
using processing.core;
using culebra.behaviors;
using CulebraData;
using CulebraData.Objects;


namespace Culebra_GH.Tests
{
    public class Component_Test_Behaviors : GH_Component
    {
        //-----------------Global Variables---------------------------
        private List<Vector3d> moveList = new List<Vector3d>();
        private List<Vector3d> startList = new List<Vector3d>();
        private DataTree<Point3d> posTree;

        private BabyCreeper babyCreep;
        private Creeper creep;
        private List<CulebraObject> creepList = new List<CulebraObject>();
        private List<Point3d> currentPosList = new List<Point3d>();

        private List<Line> networkList = new List<Line>();

        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private BoundingBox bb;
        private int dimensions;
        
        /// <summary>
        /// Initializes a new instance of the ComponentTest class.
        /// </summary>
        public Component_Test_Behaviors()
            : base("Component_Test_Behaviors", "Nickname",
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
            pManager.AddBooleanParameter("Wander2D", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Wander2D_B", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("SuperWander2D", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Align", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Separate", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Separate2", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("cohesion", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("seek", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("attract", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("repel", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("perlin", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("noiseModified_A", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("noiseModified_B", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("noiseModified_C", "W", "W", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RandomSpawn", "RS", "W", GH_ParamAccess.item);


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
            int ptCount = 300;
            int dimension = 0;
            bool trail = true;

            float moveValue = 3.44f;
            float searchRad = 44.04f;
            float aligVal = 0.04f;
            float sepVal = 0.15f;
            float cohVal = 0.24f;

            bool w = new bool();
            bool w2 = new bool();
            bool sw = new bool();
            bool a = new bool();
            bool s = new bool();
            bool s2 = new bool();
            bool c2 = new bool();
            bool s3 = new bool();
            bool att = new bool();
            bool r = new bool();
            bool p = new bool();
            bool nma = new bool();
            bool nmb = new bool();
            bool nmc= new bool();
            bool rs = new bool();

            if(!DA.GetData(0, ref reset))return;

            if (!DA.GetData(1, ref w)) return;
            if (!DA.GetData(2, ref w2)) return;
            if (!DA.GetData(3, ref sw)) return;
            if (!DA.GetData(4, ref a)) return;
            if (!DA.GetData(5, ref s)) return;
            if (!DA.GetData(6, ref s2)) return;
            if (!DA.GetData(7, ref c2)) return;
            if (!DA.GetData(8, ref s3)) return;
            if (!DA.GetData(9, ref att)) return;
            if (!DA.GetData(10, ref r)) return;
            if (!DA.GetData(11, ref p)) return;
            if (!DA.GetData(12, ref nma)) return;
            if (!DA.GetData(13, ref nmb)) return;
            if (!DA.GetData(14, ref nmc)) return;
            if (!DA.GetData(15, ref rs)) return;


            if (reset)
            { //we are using the reset to reinitialize all the variables and positions to pass to the class once we are running

                this.moveList = new List<Vector3d>();
                this.startList = new List<Vector3d>();
                this.posTree = new DataTree<Point3d>();
                this.dimensions = dimension;
                creepList = new List<CulebraObject>();
                currentPosList = new List<Point3d>();

                networkList = new List<Line>();

                if (this.dimensions == 0)
                {
                    this.bb = new BoundingBox(-400, -400, 0, 800, 400, 0);
                }
                else
                {
                    this.bb = new BoundingBox(-250, -250, -250, 250, 250, 500);
                }

                for (int i = 0; i < ptCount; i++)
                {

                    if (this.dimensions == 0)
                    { //IF WE WANT 2D
                        if (rs)
                        {
                            this.moveVec = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, 0); //move randomly in any direction 2d
                            this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area
                        }
                        else
                        {
                            this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only
                            this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area
                        }



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


                int counter = 0;
                foreach (Creeper c in this.creepList)
                {
                    networkList = new List<Line>();

                    c.attributes.SetMoveAttributes(3.44f, 0.330f, 1.5f);
                    //c.behaviors.flock2D(searchRad, cohVal, sepVal, aligVal, 360f, this.creepList, true);
                    //bool w, w2, sw, a, s, s2, c, s3, att, r, p, nma, nmb, nmc = new bool();
                    if (w) c.behaviors.Wander2D(true, false, 2.0f, 80.0f, 26.0f);
                    if (w2) c.behaviors.Wander2D(2.0f, 80.0f, 26.0f);
                    if (sw) c.behaviors.SuperWander2D(100.0f, 60.0f, 60.0f, 6.0f);
                    //if (a) c.behaviors.Align(30.0f, 0.045f, this.creepList);
                    if (a) c.behaviors.Flock2D(200.0f, 0.24f, 0.09f, 0.04f, 360, this.creepList, false);
                    if (s) c.behaviors.Separate(10.0f, this.creepList);
                    if (s2) c.behaviors.Separate(30.0f, 0.09f, this.creepList);
                    if (c2) c.behaviors.Cohesion(30.0f, 0.024f, this.creepList);
                    if (s3) c.behaviors.Seek(new Vector3d(0, 0, 0));
                    if (att) c.behaviors.Attract(new Vector3d(0, 0, 0), 30.0f, 1.2f, 5.0f);
                    if (r) c.behaviors.Repel(new Vector3d(0, 0, 0), 30.0f, 1.2f, 5.0f);
                    if (p) c.behaviors.Perlin(200.0f, 20.5f, 0.0f, 2.0f);
                    if (nma) c.behaviors.NoiseModified_A(500, 7.0f, 1.5f, 1.0f, 10.0f, 5.0f);
                    if (nmb) c.behaviors.NoiseModified_B(500.0f, 7.0f, 1.5f, 1.0f, 10.0f);
                    if (nmc) c.behaviors.NoiseModified_C(500, 7.0f, 1.5f, 1.0f, 10.0f);
    
                    GH_Path path = new GH_Path(counter);

                    List<Vector3d> testList = c.attributes.GetNetwork();
                    if (testList.Count > 0)
                    {

                        foreach (Vector3d v in testList)
                        {
                            Line l = new Line(c.attributes.GetLocation(),(Point3d)v);
                            networkList.Add(l);
                            
                        }
                        networkTree.AddRange(networkList, path);
                    }

                    c.actions.Move(5,2000);


                    //c.actions.bounce(bb);
                    c.actions.Respawn(bb);
                    currentPosList.Add(c.attributes.GetLocation());
                                    
                    
                    trailTree.AddRange(c.attributes.GetTrailPoints(),path);
                    
                    counter++;
                }
                DA.SetDataList(0, currentPosList);
                if (trail)
                {
                    DA.SetDataTree(1, trailTree);
                }
                DA.SetDataTree(2, networkTree);
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
            get { return new Guid("{cb42fa16-3188-4e9d-bc34-2752b927e949}"); }
        }
    }
}
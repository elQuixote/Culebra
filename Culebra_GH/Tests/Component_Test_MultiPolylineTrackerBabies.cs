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
using CulebraData.Utilities;
namespace Culebra_GH.Tests
{
    public class Component_Test_MultiPolylineTrackerBabies : GH_Component
    {
        //-----------------Global Variables---------------------------
        private List<Vector3d> moveList = new List<Vector3d>();
        private List<Vector3d> startList = new List<Vector3d>();
        private DataTree<Point3d> posTree;

        private Creeper creep;
        private List<Creeper> creepList = new List<Creeper>();
        private List<Vector3d> currentPosList = new List<Vector3d>();

        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private BoundingBox bb;
        private int dimensions;

        private bool triggerBabies = true;
        private List<Vector3d> childSpawners = new List<Vector3d>();
        private List<int> childSpawnType = new List<int>();
        //private java.util.ArrayList childSpawnType = new java.util.ArrayList();
        
        /// <summary>
        /// Initializes a new instance of the ComponentTest class.
        /// </summary>
        public Component_Test_MultiPolylineTrackerBabies()
            : base("Component_Test_MultiPolylineTrackerBabies", "Nickname",
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
            pManager.AddCurveParameter("Polyline", "P", "", GH_ParamAccess.list);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Creeps", "C", "C", GH_ParamAccess.list);
            pManager.AddPointParameter("Trails", "T", "T", GH_ParamAccess.tree);
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
            List<Curve> crvList = new List<Curve>();

            List<Polyline> plineList = new List<Polyline>();

            bool reset = new bool();
            int ptCount = 10;
            int dimension = 0;
            bool trail = true;

            float moveValue = 3.44f;
            float searchRad = 44.04f;
            float aligVal = 0.04f;
            float sepVal = 0.15f;
            float cohVal = 0.24f;

            if(!DA.GetData(0, ref reset))return;
            if (!DA.GetDataList(1,crvList)) return;

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
                this.dimensions = dimension;

                this.creepList = new List<Creeper>();
                this.currentPosList = new List<Vector3d>();

                this.childSpawners = new List<Vector3d>();
                this.childSpawnType = new List<int>();

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
                        this.moveVec = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, 0); //move randomly in any direction 2d
                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area

                        if (i <= ptCount / 2)
                        {
                            this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                            this.creepList.Add(this.creep);
                        }
                        else
                        {
                            this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                            this.creepList.Add(this.creep);
                        }                      
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
                currentPosList = new List<Vector3d>();
                DataTree<Point3d> trailTree = new DataTree<Point3d>();
                DataTree<Point3d> trailTree_ChildA = new DataTree<Point3d>();
                DataTree<Point3d> trailTree_ChildB = new DataTree<Point3d>();

                int counter = 0;
                foreach (Creeper c in this.creepList)
                {                
                    c.attributes.setMoveAttributes(3.44f, 0.3f, 1.5f);
                    /*
                    if (c is BabyCreeper)
                    {
                        c.behaviors.multiPolylineTrackerBabyMaker(plineList, 500.0f, 20.0f, 15.0f, triggerBabies, 2, false, "Child", this.childSpawners, this.childSpawnType);
                    }
                    else
                    {
                        c.behaviors.multiPolylineTrackerBabyMaker(plineList, 500.0f, 20.0f, 15.0f, triggerBabies, 2, true, "Parent", this.childSpawners, this.childSpawnType);
                    }
                    */
                    /*
                    Rhino.RhinoApp.WriteLine(c.attributes.getObjType());
                    Rhino.RhinoApp.WriteLine(c.attributes.getSuperClass());
                    Rhino.RhinoApp.WriteLine("---------------------");
                    */
                    c.behaviors.multiPolylineTrackerBabyMaker(plineList, 500.0f, 20.0f, 15.0f, triggerBabies, 2, true, this.childSpawners, this.childSpawnType);

                    this.childSpawners = c.behaviors.getChildStartPositions();
                    this.childSpawnType = c.behaviors.getChildSpawnTypes();
                    //c.behaviors.wander2D(true,false,2.0f, 80.0f, 26.0f);
                    c.behaviors.wander2D(true, false, 100.0f, 60.0f, 60.0f);
                    //c.behaviors.flock2D(searchRad, cohVal, sepVal, aligVal, 360f, this.creepList, false);                
                    c.actions.applyMove(5,1000);
                    c.actions.bounce(bb);

                    GH_Path path = new GH_Path(counter);
                    if (c is BabyCreeper)
                    {
                        if (c.attributes.getChildType() == "a")
                        {
                            trailTree_ChildA.AddRange(c.attributes.getTrailPoints(), path);
                        }
                        else
                        {
                            trailTree_ChildB.AddRange(c.attributes.getTrailPoints(), path);
                        }
                    }
                    else
                    {                      
                        trailTree.AddRange(c.attributes.getTrailPoints(), path);
                    }

                    currentPosList.Add(c.attributes.getVecLocation());
                 
                    counter++;
                }                            
                DA.SetDataList(0, currentPosList);
                if (trail)
                {
                    DA.SetDataTree(1, trailTree);
                    DA.SetDataTree(2, trailTree_ChildA);
                    DA.SetDataTree(3, trailTree_ChildB);
                }
                if (this.childSpawners.Count > 0)
                {
                    newDude();
                    this.childSpawners = new List<Vector3d>();
                    this.childSpawnType = new List<int>();
                }
            }
        }
        public void newDude() {
          Random rnd = new Random();
          int babyCount = 0;
          foreach(Vector3d px in this.childSpawners) {
            Vector3d speed;
            speed = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, 0);
            Creeper a;
            /*
            string babySequencer = this.childSpawnType.get(babyCount).ToString();
            int convertBabySequencer = 0;
            Int32.TryParse(this.childSpawnType.get(babyCount).ToString(), out convertBabySequencer);             
            if (convertBabySequencer % 2 == 0) {
            */
            if (this.childSpawnType[babyCount] % 2 == 0) {
              a = new BabyCreeper(new Vector3d(px.X, px.Y, px.Z), speed, false, "a", false);
              this.creepList.Add(a);
            } else {
              a = new BabyCreeper(new Vector3d(px.X, px.Y, px.Z), speed, false, "b", false);
              this.creepList.Add(a);
            }          
            babyCount++;
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
            get { return new Guid("{c1e625fb-8750-43ab-a47f-1c02cfd0d858}"); }
        }
    }
}
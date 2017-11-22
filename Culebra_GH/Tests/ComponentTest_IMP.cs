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

namespace Culebra_GH
{
    public class ComponentTest_IMP : GH_Component
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
        public ComponentTest_IMP()
            : base("Culebra_GH_IMP", "Nickname",
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
            pManager.AddMeshParameter("Mesh", "M", "M", GH_ParamAccess.item);
            pManager.AddNumberParameter("Threshold", "T", "T", GH_ParamAccess.item);
            pManager.AddNumberParameter("Amplitude", "A", "A", GH_ParamAccess.item);
            pManager.AddNumberParameter("Mult", "M", "M", GH_ParamAccess.item);
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
            Mesh m = new Mesh();
            double amp = new double();
            double thresh = new double();

            bool reset = new bool();
            int ptCount = 200;
            int dimension = 1;
            bool trail = true;
            double mult = new double();

            float moveValue = 3.44f;

            if(!DA.GetData(0, ref reset))return;
            if (!DA.GetData(1, ref m)) return;
            if (!DA.GetData(2, ref thresh)) return;
            if (!DA.GetData(3, ref amp)) return;
            if (!DA.GetData(4, ref mult)) return;


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
                    this.bb = new BoundingBox(-500, -500, 0, 500, 500, 0);
                }
                else
                {
                    this.bb = new BoundingBox(-500, -500, -500, 500, 500, 500);
                }

                for (int i = 0; i < ptCount; i++)
                {

                    if (this.dimensions == 0)
                    { //IF WE WANT 2D
                      //this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only
                        this.moveVec = new Vector3d(rnd.Next(-1, 2) * 0.7, rnd.Next(-1, 2) * 0.7, 0); //move randomly in any direction 2d 

                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area

                        //this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area
                        /*
                        if (i <= ptCount / 2)
                        {
                            this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                            this.creepList.Add(this.creep);
                        }
                        else
                        {
                            this.babyCreep = new BabyCreeper(this.startPos, this.moveVec, true, "a", false);
                            this.creepList.Add(this.babyCreep);
                        }
                        */
                    }
                    else
                    { //IF WE WANT 3D
                      /*
                      this.moveVec = new Vector3d(rnd.Next(-1, 2), rnd.Next(-1, 2), 0.5); //move randomly in the xy axis and up in the z axis
                      this.moveVec *= moveValue;
                      this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), (int)bb.Min[2]); //start randomly on the lowest plane of the 3d bounds
                     
                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), rnd.Next((int)bb.Min[2], (int)bb.Max[2])); //start randomly inside the 3d bounds
                        this.moveVec = new Vector3d(rnd.Next(-2, 2) * 0.7, rnd.Next(-2, 2) * 0.7, rnd.Next(-2, 2) * 0.7); //move randomly in any direction 3d
                        */
                        this.startPos = new Vector3d((int)bb.Max[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), rnd.Next((int)bb.Min[2], (int)bb.Max[2])); //start randomly inside the 3d bounds
                        this.moveVec = new Vector3d(rnd.Next(-2, 2) * 0.7, rnd.Next(-2, 2) * 0.7, rnd.Next(-2, 2) * 0.7); //move randomly in any direction 3d

                    }
                    this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                    this.creepList.Add(this.creep);

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
                    c.attributes.SetMoveAttributes(6.00f, 0.295f, 1.5f);

                    //c.behaviors.MeshWalk(m, (float)thresh, CulebraData.Utilities.Convert.ToPVec(c.attributes.GetLocation()), CulebraData.Utilities.Convert.ToPVec(c.attributes.GetSpeed()), (float)amp, (float)mult);
                    //c.behaviors.ApplyForce(dir);
                    c.behaviors.Wander3D_Mod2(100.0f, 10.0f, 20.0f);

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

                    c.actions.Move(0,1000);
                    c.actions.Bounce(bb);
                    //c.actions.respawn(bb);
                    currentPosList.Add(c.attributes.GetLocation());
                                                       
                    trailTree.AddRange(c.attributes.GetTrailPoints(),path);
                    
                    counter++;
                }
                DA.SetDataList(0, currentPosList);

                DA.SetDataTree(1, trailTree);

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{0829429e-4d31-4dea-aeb1-a8b74da6d0df}"); }
        }
    }
}
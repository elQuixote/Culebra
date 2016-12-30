using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;


namespace Culebra_GH
{
    public class ComponentTest : GH_Component
    {

        //-----------------Global Variables---------------------------
        private List<Vector3d> moveList = new List<Vector3d>();
        private List<Vector3d> startList = new List<Vector3d>();
        private DataTree<Point3d> posTree;

        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private BoundingBox bb;
        private int dimensions;

        /// <summary>
        /// Initializes a new instance of the ComponentTest class.
        /// </summary>
        public ComponentTest()
            : base("Culebra_GH_TEST", "Nickname",
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
            /*
            pManager.AddIntegerParameter("ptCount", "r", "", GH_ParamAccess.item);
            pManager.AddIntegerParameter("dimension", "r", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("connect", "r", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("swarm", "r", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("bounds", "r", "", GH_ParamAccess.item);
            */
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Creeps", "C", "C", GH_ParamAccess.list);
            pManager.AddPointParameter("Trails", "T", "T", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Random rnd = new Random();
            List<Vector3d> vecs = new List<Vector3d>();
            List<Point3d> returnedvecs = new List<Point3d>();

            bool reset = new bool();
            int ptCount = 95;
            int dimension = 0;
            bool connect = false;
            bool swarm = true;
            bool bounds = true;
            bool trail = true;

            double conn_thresh = 44.04;
            double moveValue = 3.44;
            double searchRad = 44.04;
            double aligVal = 0.04;
            double sepVal = 0.15;
            double cohVal = 0.24;

            if(!DA.GetData(0, ref reset))return;
            

            if (reset)
            { //we are using the reset to reinitialize all the variables and positions to pass to the class once we are running

                this.moveList = new List<Vector3d>();
                this.startList = new List<Vector3d>();
                this.posTree = new DataTree<Point3d>();
                this.dimensions = dimension;

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
                        //this.moveVec = new Vector3d(rnd.Next(-1, 2), rnd.Next(-1, 2), 0); //move randomly in any direction 2d
                        this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only
                        //this.startPos = new Vector3d(rnd.Next((int) bb.Min[0], (int) bb.Max[0]), rnd.Next((int) bb.Min[1], (int) bb.Max[1]), 0); //spawn randomly inside the bounding area
                        this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area

                    }
                    else
                    { //IF WE WANT 3D
                        this.moveVec = new Vector3d(rnd.Next(-1, 2), rnd.Next(-1, 2), 0.5); //move randomly in the xy axis and up in the z axis
                        //this.moveVec = new Vector3d(rnd.Next(-1, 2), rnd.Next(-1, 2), rnd.Next(-1, 2)); //move randomly in the xyz axis
                        this.moveVec *= moveValue;
                        //this.startPos = new Vector3d(rnd.Next((int) bb.Min[0], (int) bb.Max[0]), rnd.Next((int) bb.Min[1], (int) bb.Max[1]), ((int) bb.Min[2] + (int) bb.Max[2]) / 2); //start on the plane in the middle of the 3d bounds
                        this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), (int)bb.Min[2]); //start randomly on the lowest plane of the 3d bounds
                        //this.startPos = new Vector3d(rnd.Next((int) bb.Min[0], (int) bb.Max[0]), rnd.Next((int) bb.Min[1], (int) bb.Max[1]), rnd.Next((int) bb.Min[2],(int) bb.Max[2])); //start randomly inside the 3d bounds
                    }
                    this.startList.Add(this.startPos); //add the initial starting positions to the list to pass once we start running
                    this.moveList.Add(this.moveVec); //add the initial move vectors to the list to pass once we start running
                }

                //Creeps = this.startList;
                DA.SetDataList(0, this.startList);

            }
            else
            {

               
                creepers creeperPts = new creepers(this.startList, this.moveList, bb, this.dimensions, connect, conn_thresh, swarm, searchRad, aligVal, sepVal, cohVal, this.posTree, moveValue, bounds, trail);
                creeperPts.action();

                this.startList = creeperPts.outputList;
                this.moveList = creeperPts.outputMoveList;

                //Creeps = this.startList;
                DA.SetDataList(0, this.startList);

                //Connectivity = creeperPts.lineList;
                //BoundingBox = this.bb;
                if (trail)
                {
                    //TrailPoints = creeperPts.posTree;
                    DA.SetDataTree(1, creeperPts.posTree);
                }
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
            get { return new Guid("{3f677767-de0e-4130-90d6-3c840b118f8a}"); }
        }
    }
}
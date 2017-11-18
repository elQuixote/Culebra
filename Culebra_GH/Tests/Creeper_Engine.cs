using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Objects;
using Grasshopper;
using System.Reflection;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Drawing;
using ikvm;
using processing.core;
using culebra.behaviors;
using CulebraData;
using CulebraData.Objects;
using CulebraData.Utilities;
using CulebraData.Drawing;

namespace Culebra_GH.Engine
{
    public class Creeper_Engine : GH_Component
    {
        private List<Vector3d> moveList;
        private List<Vector3d> startList;
        private DataTree<Point3d> posTree;
        private List<Point3d> ptList;

        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private BoundingBox bb;
        private int dimensions;

        private bool bounds;

        private Rhino.Geometry.Box box;

        private int spawnType;
        private int pointCount;
        private string spawnData;

        private Creeper creep;
        private List<CulebraObject> creepList = new List<CulebraObject>();
        private List<Point3d> currentPosList = new List<Point3d>();
        private List<Line> networkList = new List<Line>();

        private double initialSpeed, maxSpeed, maxForce, velMultiplier;


        /// <summary>
        /// Initializes a new instance of the Creeper_Engine class.
        /// </summary>
        public Creeper_Engine()
          : base("Creeper_Engine", "CE",
              "Engine Module Test",
              "Culebra_GH", "Testing")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Init Settings", "S", "Input the init settings output of Init component", GH_ParamAccess.list);
            pManager.AddGenericParameter("Move Settings", "S", "Input the move settings output from the Move component", GH_ParamAccess.list);
            pManager.AddGenericParameter("Behavioral Settings", "BS", "Input the behavior settings output from the Controller component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Visual Settings", "VS", "Input the visual settings output of Viz component", GH_ParamAccess.list);
            pManager.AddGenericParameter("Reset", "R", "Input a button to reset the sim and reset all fields", GH_ParamAccess.item);

            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Creepers", "C", "Outputs the heads of the creepers", GH_ParamAccess.list);
            pManager.AddGenericParameter("Trails", "T", "Outputs data trees for each Creeper with its trail polyline", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Connectivity", "CN", "Outputs curves connecting from creeper heads which indicate their search rad", GH_ParamAccess.list);
            pManager.AddGenericParameter("BoundingBox", "BB", "Outputs the working bounds of the sim", GH_ParamAccess.item);
            pManager.AddGenericParameter("Settings Data Test", "SDT", "The igh object data test", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("culebra"));
            ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("IKVM.OpenJDK.Core"));

            bool reset = new bool();

            List<object> init_Settings = new List<object>();
            List<object> move_Settings = new List<object>();
            List<object> visual_Settings = new List<object>();
            object behavioral_Settings = null;

            if (!DA.GetDataList(0, init_Settings) || init_Settings.Count == 0 || init_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Init Settings Detected, please connect Init Settings to enable the component");
                return;
            }
            if (!DA.GetDataList(1, move_Settings) || move_Settings.Count == 0 || move_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Move Settings Detected, please connect Move Settings to enable the component");
                return;
            }
            DA.GetDataList(3, visual_Settings);
            /*
            if (!DA.GetDataList(3, visual_Settings) || visual_Settings.Count == 0 || visual_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Visual Settings Detected, please connect Visual Settings to enable the component");
                return;
            }
            */
            if (!DA.GetData(4, ref reset)) return;

            Random rnd = new Random();
            if (!DA.GetData(2, ref behavioral_Settings) || behavioral_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input Object is Null");
                return;
            }
            string objtype = behavioral_Settings.GetType().Name.ToString();
            if (!(behavioral_Settings.GetType() == typeof(IGH_BehaviorData)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You did not input a Behavior Data Object, please ensure input is Behavior Data Object and not " + objtype);
                return;
            }
            else
            {
                //------------------------Init Settings--------------------------
                if (init_Settings.Count != 0)
                {
                    String init_Convert;
                    GH_Convert.ToString(init_Settings[0], out init_Convert, GH_Conversion.Primary);

                    if (init_Convert == "Box")
                    {
                        this.spawnData = "box";
                        GH_Convert.ToBox_Primary(init_Settings[3], ref this.box);
                        GH_Convert.ToInt32(init_Settings[4], out this.spawnType, GH_Conversion.Primary);
                        GH_Convert.ToInt32(init_Settings[5], out this.pointCount, GH_Conversion.Primary);
                        GH_Convert.ToInt32(init_Settings[1], out this.dimensions, GH_Conversion.Primary);

                    }
                    else if (init_Convert == "Points")
                    {
                        this.spawnData = "Points";
                        var wrapperToGoo = GH_Convert.ToGoo(init_Settings[3]);
                        wrapperToGoo.CastTo<List<Point3d>>(out this.ptList);
                        GH_Convert.ToInt32(init_Settings[1], out this.dimensions, GH_Conversion.Primary);
                    }
                    GH_Convert.ToBoolean(init_Settings[2], out this.bounds, GH_Conversion.Primary);
                }
                //------------------------Move Settings--------------------------
                if (move_Settings.Count != 0)
                {
                    GH_Convert.ToDouble(move_Settings[0], out this.initialSpeed, GH_Conversion.Primary);
                    GH_Convert.ToDouble(move_Settings[1], out this.maxSpeed, GH_Conversion.Primary);
                    GH_Convert.ToDouble(move_Settings[2], out this.maxForce, GH_Conversion.Primary);
                    GH_Convert.ToDouble(move_Settings[3], out this.velMultiplier, GH_Conversion.Primary);
                }


                this.bb = new BoundingBox();

                int loopCount = new int();
                bool create = new bool();
                if (this.spawnData == "box")
                {
                    this.bb = this.box.BoundingBox;
                    loopCount = this.pointCount;
                    create = true;
                }
                else if (this.spawnData == "Points")
                {
                    loopCount = this.ptList.Count;
                    create = false;
                    this.bounds = false;
                }
                //------------------------RESET STARTS HERE--------------------------
                if (reset)
                { //we are using the reset to reinitialize all the variables and positions to pass to the class once we are running

                    this.moveList = new List<Vector3d>();
                    this.startList = new List<Vector3d>();
                    this.posTree = new DataTree<Point3d>();
                    creepList = new List<CulebraObject>();
                    currentPosList = new List<Point3d>();
                    networkList = new List<Line>();

                    for (int i = 0; i < loopCount; i++)
                    {
                        if (this.dimensions == 0)
                        { //IF WE WANT 2D
                            if (create)
                            {
                                if (this.spawnType == 0 || this.spawnType == 2)
                                {
                                    this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area

                                }
                                else if (this.spawnType == 1 || this.spawnType == 3)
                                {
                                    this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area

                                }
                                //this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only   
                                this.moveVec = new Vector3d(rnd.Next(-1, 2) * 0.5, rnd.Next(-1, 2) * 0.5, 0); //move randomly in any direction 2d 
                            }
                            else
                            {
                                this.startPos = (Vector3d)this.ptList[i];
                                this.bb.Union(this.ptList[i]);
                                //this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only   
                                this.moveVec = new Vector3d(rnd.Next(-1, 2) * 0.5, rnd.Next(-1, 2) * 0.5, 0); //move randomly in any direction 2d                        
                            }
                            this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                            this.creepList.Add(this.creep);
                        }

                        this.startList.Add(this.startPos); //add the initial starting positions to the list to pass once we start running
                        this.moveList.Add(this.moveVec); //add the initial move vectors to the list to pass once we start running
                        /*
                        if (this.dimensions == 0)
                        { //IF WE WANT 2D
                            this.moveVec = new Vector3d(moveValue, 0, 0); //move to the right only
                            this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area
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
                        */
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

                        c.attributes.SetMoveAttributes((float)maxSpeed, (float)maxForce, (float)velMultiplier);

                        List<string> stringList = new List<string>();
                        IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)behavioral_Settings;
                        foreach (string s in igh_Behavior.Value.dataOrder)
                        {
                            if (s == "Flocking")
                            {
                                stringList.Add("Flocking Alignment Value = " + igh_Behavior.Value.flockData.alignment_Value.ToString());
                                stringList.Add("Flocking Separation Value = " + igh_Behavior.Value.flockData.separation_Value.ToString());

                                c.behaviors.Flock2D((float)igh_Behavior.Value.flockData.searchRadius, (float)igh_Behavior.Value.flockData.cohesion_Value, (float)igh_Behavior.Value.flockData.separation_Value, (float)igh_Behavior.Value.flockData.alignment_Value, (float)igh_Behavior.Value.flockData.viewAngle, this.creepList, igh_Behavior.Value.flockData.network);

                            }
                            else if (s == "Wandering")
                            {
                                stringList.Add("Wandering Radius Value = " + igh_Behavior.Value.wanderData.wanderingRadius.ToString());
                                stringList.Add("Wandering Distance Value = " + igh_Behavior.Value.wanderData.wanderingDistance.ToString());

                                c.behaviors.Wander2D(igh_Behavior.Value.wanderData.randomize, igh_Behavior.Value.wanderData.addHeading, (float)igh_Behavior.Value.wanderData.change, (float)igh_Behavior.Value.wanderData.wanderingRadius, (float)igh_Behavior.Value.wanderData.wanderingDistance);
                            }
                            else
                            {
                                stringList.Add("We got a behavior problem");
                            }
                        }
                        //DA.SetDataList(4, stringList);
                      
                        GH_Path path = new GH_Path(counter);
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

                        c.actions.Move(0, 400);
                        if (this.bounds)
                        {
                            c.actions.Bounce(bb);
                        }

                        currentPosList.Add(c.attributes.GetLocation());
                        trailTree.AddRange(c.attributes.GetTrailPoints(), path);

                        counter++;
                    }
                    DA.SetDataList(0, currentPosList);
                    DA.SetDataTree(1, trailTree);                 
                    DA.SetDataTree(2, networkTree);
                }
                /*
                //------------------------------------------------DATA RETURN TEST----------------------------------------------------
                List<string> stringList = new List<string>();
                IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)behavioral_Settings;
                foreach(string s in igh_Behavior.Value.dataOrder)
                {
                    if(s == "Flocking")
                    {
                        stringList.Add("Flocking Alignment Value = " + igh_Behavior.Value.flockData.alignment_Value.ToString());
                        stringList.Add("Flocking Separation Value = " + igh_Behavior.Value.flockData.separation_Value.ToString());
                    }
                    else if(s == "Wandering")
                    {
                        stringList.Add("Wandering Radius Value = " + igh_Behavior.Value.wanderData.wanderingRadius.ToString());
                        stringList.Add("Wandering Distance Value = " + igh_Behavior.Value.wanderData.wanderingDistance.ToString());
                    }
                    else
                    {
                        stringList.Add("We got a behavior problem");
                    }
                }
                DA.SetDataList(0, stringList); 
                //-------------------------------------------------------------------------------------------------------------------
                */
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
            get { return new Guid("af68d5a9-fd6b-4df1-adfc-b9fb225edb51"); }
        }
    }
}
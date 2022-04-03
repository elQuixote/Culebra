using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Objects;
using Grasshopper;
using System.Reflection;
using Grasshopper.Kernel.Types;
using System.Drawing;
using CulebraData.Objects;
using CulebraData.Utilities;
using CulebraData.Drawing;
using Culebra_GH.Data_Structures;
using Culebra_GH.Utilities;

namespace Culebra_GH.Engine
{
    public class Creeper_Engine_Dual : GH_Component
    {
        #region Globals
        //----------------Creeper Fields-------------------------
        private Creeper creep;
        private List<CulebraObject> creepList = new List<CulebraObject>();
        private List<Point3d> currentPosList = new List<Point3d>();
        private List<Line> networkList = new List<Line>();
        private List<Vector3d> moveList;
        private List<Vector3d> startList;
        private List<Point3d> ptList;
        private Vector3d startPos = new Vector3d();
        private Vector3d moveVec;
        private double initialSpeed, maxSpeed, maxForce, velMultiplier;
        private double child_initialSpeed, child_maxSpeed, child_maxForce, child_velMultiplier;
        private BoundingBox bb;
        private Box box;
        private int dimensions;
        private int bounds;
        private string spawnData;
        private int spawnType;
        private int pointCount;
        //----------------Graphics/Trail Fields-------------------------
        private int minthick;
        private int maxthick;
        private bool trail;
        private int displayMode;
        private int trailStep;
        private int maxTrailSize;
        private string particleTexture = "";
        private int alphaValue = new int();
        private double[] redValues = new double[2];
        private double[] greenValues = new double[2];
        private double[] blueValues = new double[2];
        private Color polylineColor;
        private bool dotted;
        private string graphicType;
        private bool useTexture;
        //----------------Child Graphics/Trail Fields---------------------
        private int child_minthick;
        private int child_maxthick;
        private bool child_trail;
        private int child_displayMode;
        private int child_trailStep;
        private int child_maxTrailSize;
        private string child_particleTexture = "";
        private int child_alphaValue = new int();
        private double[] child_redValues = new double[2];
        private double[] child_greenValues = new double[2];
        private double[] child_blueValues = new double[2];
        private Color child_polylineColor;
        private bool child_dotted;
        private string child_graphicType;
        private bool child_useTexture;
        //----------------SelfTail Chasing Fields-------------------------
        private List<Vector3d> flattenedTrails = new List<Vector3d>();
        //----------------Child Creeper Fields----------------------------
        private List<Vector3d> childSpawners = new List<Vector3d>();
        private List<int> childSpawnType = new List<int>();
        //------------------Graphics Globals------------------------------
        private List<Point3d> particleList = new List<Point3d>();
        private DataTree<Point3d> particleSet = new DataTree<Point3d>();
        private DataTree<Point3d> particleBabyASet = new DataTree<Point3d>();
        private DataTree<Point3d> particleBabyBSet = new DataTree<Point3d>();
        private Random randomGen;
        private BoundingBox _clippingBox;
        private Vizualization viz = new Vizualization();
        //-------------GH DOC Data---------------------------------------
        private bool myBool = true;

        private Engine_Global globalEngine;
        //-------------Timer Data-----------------------------------------
        private int cycles;
        private Timer timer = new Timer();
        //-------------Trail & Network Data-------------------------------
        private DataTree<Point3d> trailTree;
        private DataTree<Line> networkTree;
        //-------------Child Tracker Data---------------------------------
        private DataTree<Point3d> trailTree_ChildA;
        private DataTree<Point3d> trailTree_ChildB;
        #endregion     
        /// <summary>
        /// Initializes a new instance of the Creeper_Engine class.
        /// </summary>
        public Creeper_Engine_Dual()
          : base("Creeper_Engine_Dual", "CED",
              "Culebra Multi Object Engine",
              "Culebra_GH", "05 | Engine")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
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
            pManager.AddGenericParameter("Visual Settings", "VS", "Input the visual settings output of Viz component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Child Move Settings", "S", "Input the child move settings output from the Move component", GH_ParamAccess.list);
            pManager.AddGenericParameter("Child Behavioral Settings", "BS", "Input the child behavior settings output from the Controller component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Child Visual Settings", "VS", "Input the child visual settings output of Viz component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Reset", "R", "Input a button to reset the sim and reset all fields", GH_ParamAccess.item);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Creepers", "C", "Outputs the heads of the creepers", GH_ParamAccess.list);
            pManager.AddGenericParameter("Trails", "T", "Outputs data trees for each Creeper with its trail polyline", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Connectivity", "CN", "Outputs curves connecting from creeper heads which indicate their search rad", GH_ParamAccess.list);
            pManager.AddPointParameter("Child Creepers", "CC", "Outputs the heads of the child creepers", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Child A Trails", "CT", "Outputs data trees for Child Creeper Type A with its trail polyline", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Child B Trails", "CBT", "Outputs data trees for Child Creeper Type B with its trail polyline", GH_ParamAccess.tree);
        }
        #region Solution Code
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (GH_Document.IsEscapeKeyDown())
            {
                GH_Document GHDocument = OnPingDocument();
                GHDocument.RequestAbortSolution();
                return;
            }

            ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("culebra"));
            ikvm.runtime.Startup.addBootClassPathAssemby(Assembly.Load("IKVM.OpenJDK.Core"));

            bool reset = new bool();
            List<object> init_Settings = new List<object>();
            List<object> move_Settings = new List<object>();
            List<object> child_move_Settings = new List<object>();
            IGH_VisualData visual_Settings = null;
            IGH_VisualData child_visual_Settings = null;

            object behavioral_Settings = null;
            object child_behavioral_Settings = null;

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
            if (!DA.GetData(3, ref visual_Settings) || visual_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Visual Settings Detected, please connect Visual Settings to enable the component");
                return;
            }
            if (!DA.GetDataList(4, child_move_Settings) || child_move_Settings.Count == 0 || child_move_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Child Move Settings Detected, please connect Move Settings to enable the component");
                return;
            }
            if (!DA.GetData(7, ref reset)) return;
            Random rnd = new Random();
            if (!DA.GetData(2, ref behavioral_Settings) || behavioral_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Behavioral Settings Input Object is Null");
                return;
            }
            if (!DA.GetData(5, ref child_behavioral_Settings) || child_behavioral_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Child Behavioral Settings Input Object is Null");
                return;
            }
            if (!DA.GetData(6, ref child_visual_Settings) || child_visual_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Child Visual Settings Detected, please connect Visual Settings to enable the component");
                return;
            }
            //-----------Test if behavioral settings is of IGH_BehaviorData------
            string objtype = behavioral_Settings.GetType().Name.ToString();
            if (!(behavioral_Settings.GetType() == typeof(IGH_BehaviorData)) || !(child_behavioral_Settings.GetType() == typeof(IGH_BehaviorData)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You did not input a Behavior Data Object, please ensure input is Behavior Data Object and not " + objtype);
                return;
            }
            else
            {
                #region Initialize / Data Parse
                //------------------------Init Settings--------------------------
                if (init_Settings.Count != 0)
                {
                    String init_Convert = "";
                    //GH_Convert.ToString(init_Settings[0], out init_Convert, GH_Conversion.Primary);
                    if (init_Settings[0].GetType() == typeof(GH_String))
                    {
                        GH_String value = (GH_String)init_Settings[0];
                        init_Convert = value.Value;
                    }
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
                        GH_Convert.ToBox_Primary(init_Settings[4], ref this.box);
                    }
                    GH_Convert.ToInt32(init_Settings[2], out this.bounds, GH_Conversion.Primary);
                }
                //------------------------Move Settings--------------------------
                Vector3d initialVector = new Vector3d();
                if (move_Settings.Count != 0)
                {
                    if (move_Settings[0].GetType() == typeof(GH_Vector))
                    {
                        GH_Vector value = (GH_Vector)move_Settings[0];
                        initialVector = value.Value;
                    }
                    else if (move_Settings[0].GetType() == typeof(GH_Number))
                    {
                        GH_Number value = (GH_Number)move_Settings[0];
                        this.initialSpeed = value.Value;
                    }
                    GH_Convert.ToDouble(move_Settings[1], out this.maxSpeed, GH_Conversion.Primary);
                    GH_Convert.ToDouble(move_Settings[2], out this.maxForce, GH_Conversion.Primary);
                    GH_Convert.ToDouble(move_Settings[3], out this.velMultiplier, GH_Conversion.Primary);
                }
                //------------------------Child Move Settings--------------------------
                Vector3d child_initialVector = new Vector3d();
                if (child_move_Settings.Count != 0)
                {
                    if (child_move_Settings[0].GetType() == typeof(GH_Vector))
                    {
                        GH_Vector value = (GH_Vector)child_move_Settings[0];
                        child_initialVector = value.Value;
                    }
                    else if (child_move_Settings[0].GetType() == typeof(GH_Number))
                    {
                        GH_Number value = (GH_Number)child_move_Settings[0];
                        this.child_initialSpeed = value.Value;
                    }
                    GH_Convert.ToDouble(child_move_Settings[1], out this.child_maxSpeed, GH_Conversion.Primary);
                    GH_Convert.ToDouble(child_move_Settings[2], out this.child_maxForce, GH_Conversion.Primary);
                    GH_Convert.ToDouble(child_move_Settings[3], out this.child_velMultiplier, GH_Conversion.Primary);
                }
                //------------------------Visual Settings--------------------------
                TrailData td = visual_Settings.Value.trailData;
                ColorData cd = visual_Settings.Value.colorData;
                this.trail = td.createTrail;
                this.displayMode = visual_Settings.Value.displayMode;
                this.trailStep = td.trailStep;
                this.maxTrailSize = td.maxTrailSize;
                this.particleTexture = cd.ParticleTexture;
                this.graphicType = cd.ColorDataType;
                this.useTexture = visual_Settings.Value.useTexture;
                if (cd.ColorDataType == "Gradient")
                {
                    this.maxthick = cd.MaxThickness;
                    this.minthick = cd.MinThickness;
                    this.alphaValue = cd.Alpha;
                    this.redValues[0] = cd.RedChannel[0];
                    this.redValues[1] = cd.RedChannel[1];
                    this.greenValues[0] = cd.GreenChannel[0];
                    this.greenValues[1] = cd.GreenChannel[1];
                    this.blueValues[0] = cd.BlueChannel[0];
                    this.blueValues[1] = cd.BlueChannel[1];
                }
                else if (cd.ColorDataType == "GraphicPolyline")
                {
                    this.polylineColor = cd.Color;
                    this.dotted = cd.Dotted;
                    this.maxthick = cd.MaxThickness;
                }
                else if (cd.ColorDataType == "Disco")
                {
                    this.maxthick = cd.MaxThickness;
                    this.minthick = cd.MinThickness;
                }else if(cd.ColorDataType == "Base")
                {
                    this.maxthick = 3;
                    this.minthick = 1;
                }
                //------------------------Child Visual Settings--------------------------
                TrailData ctd = child_visual_Settings.Value.trailData;
                ColorData ccd = child_visual_Settings.Value.colorData;
                this.child_trail = ctd.createTrail;
                this.child_displayMode = child_visual_Settings.Value.displayMode;
                this.child_trailStep = ctd.trailStep;
                this.child_maxTrailSize = ctd.maxTrailSize;
                this.child_particleTexture = ccd.ParticleTexture;
                this.child_graphicType = ccd.ColorDataType;
                this.child_useTexture = child_visual_Settings.Value.useTexture;
                if (ccd.ColorDataType == "Gradient")
                {
                    this.child_maxthick = ccd.MaxThickness;
                    this.child_minthick = ccd.MinThickness;
                    this.child_alphaValue = ccd.Alpha;
                    this.child_redValues[0] = ccd.RedChannel[0];
                    this.child_redValues[1] = ccd.RedChannel[1];
                    this.child_greenValues[0] = ccd.GreenChannel[0];
                    this.child_greenValues[1] = ccd.GreenChannel[1];
                    this.child_blueValues[0] = ccd.BlueChannel[0];
                    this.child_blueValues[1] = ccd.BlueChannel[1];
                }
                else if (ccd.ColorDataType == "GraphicPolyline")
                {
                    this.child_polylineColor = ccd.Color;
                    this.child_dotted = ccd.Dotted;
                    this.child_maxthick = ccd.MaxThickness;
                }
                else if (ccd.ColorDataType == "Disco")
                {
                    this.child_maxthick = ccd.MaxThickness;
                    this.child_minthick = ccd.MinThickness;
                }
                else if (ccd.ColorDataType == "Base")
                {
                    this.child_maxthick = 3;
                    this.child_minthick = 1;
                }
                //-----------------------------------------------------------------
                IGH_PreviewObject comp = (IGH_PreviewObject)this;
                if (comp.Hidden && (this.displayMode == 0 || this.child_displayMode == 0))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Component preview must be enabled to see Graphic Mode on Canvas, right click on component and set preview on");
                }
                #endregion
                #region Pre Simulation Code
                //------------------------RESET STARTS HERE--------------------------
                if (reset)
                { //we are using the reset to reinitialize all the variables and positions to pass to the class once we are running
                    //-----------------------------------------------------------------
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
                        this.bb = this.box.BoundingBox;
                    }
                    //-----------------------------------------------------------------
                    this.globalEngine = new Engine_Global();
                    this.cycles = 0;

                    this.moveList = new List<Vector3d>();
                    this.startList = new List<Vector3d>();
                    this.creepList = new List<CulebraObject>();
                    this.currentPosList = new List<Point3d>();
                    this.networkList = new List<Line>();
                    //----------Children Data------------
                    this.childSpawners = new List<Vector3d>();
                    this.childSpawnType = new List<int>();
                    //-------Self Tail Chase Data--------
                    flattenedTrails = new List<Vector3d>();

                    for (int i = 0; i < loopCount; i++)
                    {
                        if (this.dimensions == 0)
                        { //If we want 2D
                            General.setViewport("Top", "Shaded");
                            if (create)
                            { //If are creating random points inside bbox
                                if (this.spawnType == 0 || this.spawnType == 2)
                                {
                                    this.startPos = new Vector3d((int)bb.Min[0], rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn along the y axis of the bounding area
                                }
                                else if (this.spawnType == 1 || this.spawnType == 3)
                                {
                                    this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), 0); //spawn randomly inside the bounding area
                                }
                                if (initialVector.Length > 0)
                                {
                                    this.moveVec = initialVector; //move in the user specified direction 
                                }
                                else
                                {
                                    this.moveVec = new Vector3d(rnd.Next(-1, 2) * initialSpeed, rnd.Next(-1, 2) * initialSpeed, 0); //move randomly in any direction 2d 
                                }
                            }
                            else
                            { //If we are using user defined points  
                                this.startPos = (Vector3d)this.ptList[i];
                                if (initialVector.Length > 0)
                                {
                                    this.moveVec = initialVector; //move in the user specified direction 
                                }
                                else
                                {
                                    this.moveVec = new Vector3d(rnd.Next(-1, 2) * initialSpeed, rnd.Next(-1, 2) * initialSpeed, 0); //move randomly in any direction 2d 
                                }
                            }
                            this.creep = new Creeper(this.startPos, this.moveVec, true, false);
                            this.creepList.Add(this.creep);
                        }
                        else
                        { //If we want 3D
                            General.setViewport("Perspective", "Shaded");
                            if (create)
                            { //If are creating random points inside bbox
                                if (this.spawnType == 0 || this.spawnType == 2)
                                {
                                    this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), (int)bb.Min[2]); //start randomly on the lowest plane of the 3d bounds
                                    if (initialVector.Length > 0)
                                    {
                                        this.moveVec = initialVector; //move in the user specified direction 
                                    }
                                    else
                                    {
                                        this.moveVec = new Vector3d(rnd.Next(-2, 2) * initialSpeed, rnd.Next(-2, 2) * initialSpeed, 1 * initialSpeed); //move randomly in the xy axis and up in the z axis
                                    }
                                }
                                else if (this.spawnType == 1 || this.spawnType == 3)
                                {
                                    this.startPos = new Vector3d(rnd.Next((int)bb.Min[0], (int)bb.Max[0]), rnd.Next((int)bb.Min[1], (int)bb.Max[1]), rnd.Next((int)bb.Min[2], (int)bb.Max[2])); //start randomly inside the 3d bounds
                                    if (initialVector.Length > 0)
                                    {
                                        this.moveVec = initialVector; //move in the user specified direction 
                                    }
                                    else
                                    {
                                        this.moveVec = new Vector3d(rnd.Next(-2, 2) * initialSpeed, rnd.Next(-2, 2) * initialSpeed, rnd.Next(-2, 2) * initialSpeed); //move randomly in any direction 3d
                                    }
                                }
                            }
                            else
                            { //If we are using user defined points  
                                this.startPos = (Vector3d)this.ptList[i];
                                if (initialVector.Length > 0)
                                {
                                    this.moveVec = initialVector; //move in the user specified direction 
                                }
                                else
                                {
                                    this.moveVec = new Vector3d(rnd.Next(-2, 2) * initialSpeed, rnd.Next(-2, 2) * initialSpeed, rnd.Next(-2, 2) * initialSpeed); //move randomly in any direction 3d
                                }
                            }
                            this.creep = new Creeper(this.startPos, this.moveVec, true, true);
                            this.creepList.Add(this.creep);
                        }
                        this.startList.Add(this.startPos); //add the initial starting positions to the list to pass once we start running
                        this.moveList.Add(this.moveVec); //add the initial move vectors to the list to pass once we start running
                    }
                    DA.SetDataList(0, this.startList);
                }
                #endregion
                #region Simulation Code
                else
                {                                  
                    this.particleSet = new DataTree<Point3d>();
                    this.particleBabyASet = new DataTree<Point3d>();
                    this.particleBabyBSet = new DataTree<Point3d>();
                    this.currentPosList = new List<Point3d>();
                    this.trailTree = new DataTree<Point3d>();
                    this.networkTree = new DataTree<Line>();
                    //-------------Child Tracker Data----------
                    this.trailTree_ChildA = new DataTree<Point3d>();
                    this.trailTree_ChildB = new DataTree<Point3d>();                  
                    //-----------------------------------------
                    if (this.moveList == null) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Please Reset the CreepyCrawlers Component"); return; }
                    try
                    {
                        globalEngine.Action(this.creepList, this.dimensions, behavioral_Settings, this.displayMode, this.networkList,
                            this.maxSpeed, this.maxForce, this.velMultiplier, this.flattenedTrails, this.particleList, this.particleSet, networkTree, trailStep, maxTrailSize, bounds, bb, currentPosList, trail, trailTree,
                            child_maxSpeed, child_maxForce, child_velMultiplier, child_behavioral_Settings, child_displayMode, child_trail, child_trailStep,
                            child_maxTrailSize, particleBabyASet, particleBabyBSet, trailTree_ChildA, trailTree_ChildB);
                    }catch(Exception e)
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message.ToString());
                        return;
                    }
                    #region Set all outputs
                    DA.SetDataList(0, this.currentPosList);
                    DA.SetDataTree(2, networkTree);
                    if (this.displayMode == 1 && this.trail)
                    {                     
                        DA.SetDataTree(1, trailTree); 
                    }
                    if (this.child_displayMode == 1 && this.child_trail)
                    {
                        DA.SetDataTree(4, trailTree_ChildA);
                        DA.SetDataTree(5, trailTree_ChildB);                      
                    }
                    this.flattenedTrails.Clear();
                    this.flattenedTrails.TrimExcess();
                    #endregion
                    this.cycles++;                 
                }
                #endregion
            }
            timer.DisplayMessage(this, "Double", this.cycles, this.myBool);
        }
        #endregion
        #region Visualization
        protected override void BeforeSolveInstance()
        {
            if (this.displayMode == 0)
            {
                this.particleList.Clear();
                _clippingBox = BoundingBox.Empty;
            }
            if (this.child_displayMode == 0)
            {
                this.particleList.Clear();
                _clippingBox = BoundingBox.Empty;
            }
        }
        protected override void AfterSolveInstance()
        {
            if (this.displayMode == 0)
            {
                _clippingBox = new BoundingBox(particleList);
            }
            if (this.child_displayMode == 0)
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
            if (this.displayMode == 0)
            {
                if (this.useTexture)
                {
                    if (this.particleTexture == string.Empty)
                    {
                        this.particleTexture = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Grasshopper\Libraries\Culebra_GH\textures\texture.png";
                    }
                    viz.DrawSprites(args, particleTexture, particleList);
                }
                if (this.trail)
                {
                    if (this.graphicType == "Gradient")
                    {
                        viz.DrawGradientTrails(args, particleSet, this.alphaValue, (float)this.redValues[0], (float)this.redValues[1], (float)this.greenValues[0], (float)this.greenValues[1], (float)this.blueValues[0], (float)this.blueValues[1], this.minthick, this.maxthick);
                    }
                    else if (this.graphicType == "GraphicPolyline")
                    {
                        viz.DrawPolylineTrails(args, particleSet, this.dotted, this.maxthick, this.polylineColor);
                    }
                    else if (this.graphicType == "Disco")
                    {
                        this.randomGen = new Random();
                        viz.DrawDiscoTrails(args, particleSet, randomGen, this.minthick, this.maxthick);
                    }else if (this.graphicType == "Base")
                    {
                        viz.DrawGradientTrails(args, particleSet, 0, this.alphaValue, this.minthick, this.maxthick);
                    }
                }
            }
            if (this.child_displayMode == 0)
            {
                if (this.child_trail)
                {
                    if (this.child_graphicType == "Gradient")
                    {
                        viz.DrawGradientTrails(args, particleBabyASet, this.child_alphaValue, (float)this.child_redValues[1], (float)this.child_redValues[0], (float)this.child_greenValues[0], (float)this.child_greenValues[1], (float)this.child_blueValues[0], (float)this.child_blueValues[1], this.child_minthick, this.child_maxthick);
                        viz.DrawGradientTrails(args, particleBabyBSet, this.child_alphaValue, (float)this.child_redValues[0], (float)this.child_redValues[1], (float)this.child_greenValues[0], (float)this.child_greenValues[1], (float)this.child_blueValues[0], (float)this.child_blueValues[1], this.child_minthick, this.child_maxthick);
                    }
                    else if (this.child_graphicType == "GraphicPolyline")
                    {
                        viz.DrawPolylineTrails(args, particleBabyASet, this.child_dotted, this.child_maxthick, this.child_polylineColor);
                        viz.DrawPolylineTrails(args, particleBabyBSet, this.child_dotted, this.child_maxthick, this.child_polylineColor);
                    }
                    else if (this.child_graphicType == "Disco")
                    {
                        this.randomGen = new Random();
                        viz.DrawDiscoTrails(args, particleBabyASet, randomGen, this.child_minthick, this.child_maxthick);
                        this.randomGen = new Random();
                        viz.DrawDiscoTrails(args, particleBabyBSet, randomGen, this.child_minthick, this.child_maxthick);
                    }else if(this.child_graphicType == "Base")
                    {
                        viz.DrawGradientTrails(args, this.particleBabyASet, 1, this.child_alphaValue, this.child_minthick, this.child_maxthick);
                        viz.DrawGradientTrails(args, this.particleBabyBSet, 2, this.child_alphaValue, this.child_minthick, this.child_maxthick);
                    }
                }
            }
        }
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Display Component Data", Menu_DoClick);
        }
        private void Menu_DoClick(object sender, EventArgs e)
        {
            myBool = !myBool;
        }
        public override void CreateAttributes()
        {
            base.m_attributes = new Utilities.CustomAttributes(this, 0);
        }
        #endregion
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Engine_CreepyCrawlers_A;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("dfb426a8-13c4-40c7-be21-5179dd3ff1e5"); }
        }
    }
}
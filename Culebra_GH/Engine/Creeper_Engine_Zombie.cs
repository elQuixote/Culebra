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
    public class Creeper_Engine_Zombie : GH_Component
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
        private BoundingBox bb;
        private Box box;
        private int dimensions;
        private int bounds;
        private string spawnData;
        private int spawnType;
        private int pointCount;
        //----------------Graphics/Trail Fields-------------------------
        private int minthick = new int();
        private int maxthick = new int();
        private bool trail = new bool();
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
        //----------------SelfTail Chasing Fields-------------------------
        private List<Vector3d> flattenedTrails = new List<Vector3d>();
        private Engine_Global globalEngine;
        //----------------------Timer-------------------------------------
        private int cycles;
        //------------------Graphics Globals------------------------------
        public List<Point3d> particleList = new List<Point3d>();
        public DataTree<Point3d> particleSet = new DataTree<Point3d>();
        DataTree<Point3d> trailTree = new DataTree<Point3d>();
        DataTree<Line> networkTree = new DataTree<Line>();
        public Random randomGen;
        public Color randomColorAction = new Color();
        private BoundingBox _clippingBox;
        public Vizualization viz = new Vizualization();

        public bool myBool = true;
        #endregion
        /// <summary>
        /// Initializes a new instance of the Creeper_Engine class.
        /// </summary>
        public Creeper_Engine_Zombie()
          : base("Creeper_ZombieEngine", "CZE",
              "Culebra Object Zombie Engine",
              "Culebra_GH", "05 | Engine")
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
            pManager.AddGenericParameter("Visual Settings", "VS", "Input the visual settings output of Viz component", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Iterations", "I", "Input the number of steps you would like to run", GH_ParamAccess.item);
            pManager.AddGenericParameter("Run", "R", "Input a button to reset the sim and reset all fields", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddPointParameter("Creepers", "C", "Outputs the heads of the creepers", GH_ParamAccess.list);
            pManager.AddGenericParameter("Trails", "T", "Outputs data trees for each Creeper with its trail polyline", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Connectivity", "CN", "Outputs curves connecting from creeper heads which indicate their search rad", GH_ParamAccess.list);
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
            int iterations = new int();
            List<object> init_Settings = new List<object>();
            List<object> move_Settings = new List<object>();
            IGH_VisualData visual_Settings = null;

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
            if (!DA.GetData(3, ref visual_Settings) || visual_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Visual Settings Detected, please connect Visual Settings to enable the component");
                return;
            }
            if (!DA.GetData(4, ref iterations)) return;
            if (!DA.GetData(5, ref reset)) return;
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
                #region Initialize / Data Parse
                //------------------------Init Settings--------------------------
                if (init_Settings.Count != 0)
                {
                    String init_Convert = "";
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
                }
                else if (cd.ColorDataType == "Base")
                {
                    this.maxthick = 3;
                    this.minthick = 1;
                }
                //-----------------------------------------------------------------
                IGH_PreviewObject comp = (IGH_PreviewObject)this;
                if (comp.Hidden && (this.displayMode == 0))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Component preview must be enabled to see Graphic Mode on Canvas, right click on component and set preview on");
                }
                #endregion
                #region Pre Simulation Code
                //------------------------RESET STARTS HERE--------------------------
                if (reset)
                { //We are using the reset to reinitialize all the variables and positions
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
                    this.moveList = new List<Vector3d>();
                    this.startList = new List<Vector3d>();
                    this.creepList = new List<CulebraObject>();
                    this.currentPosList = new List<Point3d>();
                    this.networkList = new List<Line>();
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
                    #endregion
                    #region Simulation Code
                    this.trailTree = new DataTree<Point3d>();
                    this.globalEngine = new Engine_Global();
                    for (int z = 0; z < iterations; z++)
                    {
                        this.particleSet = new DataTree<Point3d>();
                        this.currentPosList = new List<Point3d>();

                        this.trailTree.Clear();
                        this.networkTree.Clear();
                        this.trailTree.TrimExcess();
                        this.networkTree.TrimExcess();

                        if (this.moveList == null) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Please Reset the CreepyCrawlers Component"); return; }
                        try
                        {
                            globalEngine.Action(this.creepList, this.dimensions, behavioral_Settings, this.displayMode, this.networkList,
                            this.maxSpeed, this.maxForce, this.velMultiplier, this.flattenedTrails, this.particleList, this.particleSet, networkTree, trailStep, maxTrailSize, bounds, bb, currentPosList, trail, trailTree);
                        }
                        catch (Exception e)
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message.ToString());
                            return;
                        }

                        this.flattenedTrails.Clear();
                        this.flattenedTrails.TrimExcess();

                        #endregion
                    }
                    DA.SetDataList(0, this.currentPosList);
                    DA.SetDataTree(2, networkTree);
                    if (this.displayMode == 1 && this.trail)
                    {
                        DA.SetDataTree(1, trailTree);
                    }
                }
            }
        }
        #endregion
        #region Visualization
        protected override void BeforeSolveInstance()
        {
            if (this.displayMode == 0)
            {
                this.particleList.Clear();
                this.particleSet.Clear();
                _clippingBox = BoundingBox.Empty;
            }
        }
        protected override void AfterSolveInstance()
        {
            if (this.displayMode == 0)
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
                    }
                    else if (this.graphicType == "Base")
                    {
                        viz.DrawGradientTrails(args, particleSet, 0, this.alphaValue, this.minthick, this.maxthick);
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
                return Culebra_GH.Properties.Resources.zombie;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e2342f89-1643-4b88-ac07-dd7488fc6e45"); }
        }
    }
}
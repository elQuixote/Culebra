using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Objects;
using Grasshopper;
using System.Drawing;
using CulebraData.Drawing;
using Culebra_GH.Data_Structures;
using CulebraData.Behavior.Types;
using Culebra_GH.Utilities;

namespace Culebra_GH.Engine
{
    public class Bundling_Engine : GH_Component
    {
        #region Globals
        private SelfOrganize selfOrg = new SelfOrganize();
        private List<Curve> crvList = new List<Curve>();
        private int ptCount;
        private bool rebuild;
        private double thresh;
        private double ratio;
        private int weldCount;
        private Mesh dMesh;
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
        //------------------Graphics Globals------------------------------
        public List<Point3d> particleList = new List<Point3d>();
        public DataTree<Point3d> particleSet = new DataTree<Point3d>();
        DataTree<Point3d> trailTree = new DataTree<Point3d>();
        DataTree<Line> networkTree = new DataTree<Line>();
        public Random randomGen;
        public Color randomColorAction = new Color();
        private BoundingBox _clippingBox;
        public Vizualization viz = new Vizualization();
        //------------------Component Message Globals--------------------
        private Timer timer = new Timer();
        private int cycles;
        public bool myBool = true;

        #endregion
        /// <summary>
        /// Initializes a new instance of the Bundling_Engine class.
        /// </summary>
        public Bundling_Engine()
          : base("Bundling_Engine", "Nickname",
              "Engine for Self Organization of Curve Networks",
              "Culebra_GH", "05 | Engine")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Input curves to bundle", GH_ParamAccess.list);
            pManager.AddGenericParameter("Behavioral Settings", "BS", "Input the behavior settings output from the Controller component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Visual Settings", "VS", "Input the visual settings output of Viz component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Reset", "R", "Input a button to reset the sim and reset all fields", GH_ParamAccess.item);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometry", "G", "Self_Organized Set of curves", GH_ParamAccess.list);
        }
        #region Solution Code
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object behavioral_Settings = null;
            object visual_Settings = null;
            bool reset = new bool();
            List<Curve> curves = new List<Curve>();
            
            if(!DA.GetDataList(0, curves))return;
            if (!DA.GetData(1, ref behavioral_Settings) || behavioral_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Behavior Input Object is Null");
                return;
            }
            string objtype = behavioral_Settings.GetType().Name.ToString();
            if (!(behavioral_Settings.GetType() == typeof(IGH_BehaviorData)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You did not input a Behavior Data Object, please ensure input is Behavior Data Object and not " + objtype);
                return;
            }
            if(!DA.GetData(2, ref visual_Settings) || visual_Settings == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Visual Input Object is Null");
                return;
            }
            string objtype2 = visual_Settings.GetType().Name.ToString();
            if (!(visual_Settings.GetType() == typeof(IGH_VisualData)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You did not input a Visual Data Object, please ensure input is Visual Data Object and not " + objtype2);
                return;
            }
            if (!DA.GetData(3, ref reset)) return;
            //------------------------Visual Settings--------------------------
            IGH_VisualData igh_Visual = (IGH_VisualData)visual_Settings;
            TrailData td = igh_Visual.Value.trailData;
            ColorData cd = igh_Visual.Value.colorData;
            this.trail = td.createTrail;
            this.displayMode = igh_Visual.Value.displayMode;
            this.trailStep = td.trailStep;
            this.maxTrailSize = td.maxTrailSize;
            this.particleTexture = cd.ParticleTexture;
            this.graphicType = cd.ColorDataType;
            this.useTexture = igh_Visual.Value.useTexture;
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
            }
            else if (cd.ColorDataType == "Base")
            {
                this.maxthick = 3;
                this.minthick = 1;
            }
            //-----------------------------------------------------------------
            if (reset)
            {
                this.cycles = 0;
                this.crvList = new List<Curve>();
                this.crvList = curves;
                DA.SetDataList(0, crvList);
            }
            else
            {
                this.particleSet.Clear();
                this.particleSet.TrimExcess();

                IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)behavioral_Settings;

                this.thresh = igh_Behavior.Value.BundlingData.Threshold;
                this.ratio = igh_Behavior.Value.BundlingData.Ratio;
                this.ptCount = igh_Behavior.Value.BundlingData.PointCount;
                this.rebuild = igh_Behavior.Value.BundlingData.Rebuild;
                this.weldCount = igh_Behavior.Value.BundlingData.WeldCount;
                //-----------------------------------------------------------------
                foreach (string s in igh_Behavior.Value.DataOrder)
                {
                    if (s == "Bundling")
                    {
                        try
                        {
                            if(igh_Behavior.Value.BundlingData.ColorMesh != null)
                            {
                                this.crvList = this.selfOrg.Bundling(particleList, particleSet, this.crvList, this.thresh, this.ratio, this.weldCount, this.rebuild, this.ptCount,
                                    igh_Behavior.Value.BundlingData.ColorMesh, igh_Behavior.Value.BundlingData.UseColor);
                            }
                            else
                            {
                                this.crvList = this.selfOrg.Bundling(particleList, particleSet, this.crvList, this.thresh, this.ratio, this.weldCount, this.rebuild, this.ptCount);
                            }
                        }
                        catch(Exception e)
                        {
                            AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message.ToString());
                            return;
                        }
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Currently we can only run bundling behavior on the Bundling Engine, please remove other behaviors");
                        return;
                    }
                }
                if (this.displayMode == 1)
                {
                    DA.SetDataList(0, this.crvList);
                }
                this.cycles++;
            }
            timer.DisplayMessage(this, "Bundling", this.cycles, this.myBool);
        }
        #endregion
        #region Visuals
        protected override void BeforeSolveInstance()
        {
            this.particleList.Clear();
            this.particleSet.Clear();

            if (this.displayMode == 0)
            {
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
                return Culebra_GH.Properties.Resources.Engine_Bundling;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("356e9e06-8a54-4c45-9a63-39f4a1d1b88d"); }
        }
    }
}
using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Culebra_GH.Data_Structures;
using Culebra_GH.Objects;

namespace Culebra_GH.Settings_Visual
{
    public class Visual_Settings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Visual_Settings class.
        /// </summary>
        public Visual_Settings()
            : base("Visual Settings", "VS",
                "Controls the visual settings for the Creeper Engine Outputs",
                "Culebra_GH", "06 | Display")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
            }
        }
        public override void CreateAttributes()
        {
            base.m_attributes = new Utilities.CustomAttributes(this, 2);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Trail Data", "TD", "Input the Trail Data output from the Trail Data Component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Color Data", "CD", "Input the Trail Color Data output from the Gradient Color Component", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Apply Texture", "AT", "Input boolean specifying the application of texture as particle - WARNING VERY UNSTABLE", GH_ParamAccess.item, false);
            pManager.AddGenericParameter("Display Mode", "DM", "Input an integer specifying the Display Mode (0 = Graphic | 1 = Geometry)", GH_ParamAccess.item);

            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Visual Settings", "VS", "Outputs the Visual Settings for the Creeper Engine", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_Goo trailData = null;
            IGH_Goo colorData = null;
            bool texture = new bool();
            object displayMode = null;

            VisualData visualData = new VisualData();

            if (!DA.GetData(0, ref trailData) || trailData == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No Trail Data Detected, please connect Trail Data to enable the component");
                return;
            }
            DA.GetData(1, ref colorData);
            string dataType = trailData.GetType().Name.ToString();
            if (trailData.ToString() == "Culebra_GH.Data_Structures.TrailData")
            {
                TrailData td;
                bool worked = trailData.CastTo(out td);
                if (worked)
                {
                    visualData.trailData = td;
                }else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not convert trail data, ensure you have the correct inputs");
                    return;
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input Trail Data Output value for Trail Data not a " + dataType);
                return;
            }
            if(colorData != null)
            {
                string colorType = colorData.GetType().Name.ToString();
                if (colorData.ToString() == "Culebra_GH.Data_Structures.ColorData")
                {
                    ColorData cd;
                    bool worked = colorData.CastTo(out cd);
                    if (worked)
                    {
                        visualData.colorData = cd;
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Could not convert color data, ensure you have the correct inputs");
                        return;
                    }
                }
                else
                {

                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input Trail Data Output value for Trail Data not a " + colorType);
                    return;
                }
            }
            else
            {
                ColorData color = new ColorData();
                color.ColorDataType = "Base";
                visualData.colorData = color;
            }
            if (!DA.GetData(2, ref texture)) return;
            if(texture) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "WARNING USING TEXTURE CAN MAKE THE SIMULATION VERY UNSTABLE AND MAY CRASH WITHOUT WARNING, I RECOMMEND USING THE GH BUILT IN CLOUD DISPLAY FOR THE CREEPERS OUTPUT"); }
            visualData.useTexture = texture;

            if (!DA.GetData(3, ref displayMode)) return;
            string type2 = displayMode.GetType().Name.ToString();
            if (displayMode.GetType() != typeof(GH_Integer) && displayMode.GetType() != typeof(GH_Number))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input a integer/float value for Display Mode not a " + type2);
                return;
            }else
            {
                if (displayMode.GetType() == typeof(GH_Integer))
                {
                    GH_Integer ghInt = (GH_Integer)displayMode;
                    visualData.displayMode = ghInt.Value;
                }
                else
                {
                    GH_Number ghNum = (GH_Number)displayMode;
                    visualData.displayMode = Convert.ToInt32(ghNum.Value);
                }           
            }
            IGH_VisualData igh_Viz = new IGH_VisualData(visualData);
            DA.SetData(0, igh_Viz);          
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.VisualSettings;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("55b57ce1-1eb7-4eca-b107-4952c9cca589"); }
        }
    }
}
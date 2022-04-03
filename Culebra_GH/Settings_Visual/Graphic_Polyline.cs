using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;


namespace Culebra_GH.Settings_Visual
{
    public class Graphic_Polyline : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Graphic_Polyline class.
        /// </summary>
        public Graphic_Polyline()
          : base("Graphic_Polyline", "GP",
              "Controls the Graphic Polyline Color trail options for the Visual Settings Component",
              "Culebra_GH", "06 | Display")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
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
            pManager.AddTextParameter("Particle Texture Path", "PT", "Input path to particle texture, a png file", GH_ParamAccess.item);
            pManager.AddColourParameter("Color", "C", "Input a color to use for the graphical polyline drawing", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Dotted", "D", "Input a boolean value to specify polyline dotting (True = Solid | False = Dotted)", GH_ParamAccess.item, true);
            pManager.AddIntegerParameter("Thickness", "T", "Input the polyline thickness", GH_ParamAccess.item, 1);

            pManager[0].Optional = true;
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Trail Color Data", "TCD", "Outputs the Trail Color Data for the Visual Settings Component", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = "";
            System.Drawing.Color color = new System.Drawing.Color();
            bool dotted = new bool();
            int thickness = new int();

            DA.GetData(0, ref path);
            if (!DA.GetData(1, ref color)) return;
            if (!DA.GetData(2, ref dotted)) return;
            if (!DA.GetData(3, ref thickness)) return;

            if (thickness < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Thickness has to be greater than 0, please increase"); return; }
            if (thickness > 4) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "There is a performance decrease if the thickness is greater than 3, keep an eye on your value"); return; }

            ColorData colorData = new ColorData();
            colorData.Color = color;
            colorData.ParticleTexture = path;
            colorData.Dotted = dotted;
            colorData.MaxThickness = thickness;

            colorData.ColorDataType = "GraphicPolyline";

            DA.SetData(0, colorData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.GraphicPolyline;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1e598153-4123-4cc6-9767-93ba89ddc2f8"); }
        }
    }
}
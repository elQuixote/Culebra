using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Settings_Visual
{
    public class Gradient_Color : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Gradient_Color class.
        /// </summary>
        public Gradient_Color()
          : base("Gradient_Color", "GC",
              "Controls the Gradient Color trail options for the Visual Settings Component",
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
            pManager.AddIntegerParameter("Alpha Channel", "AC", "Input the polyline alpha channel (0-255)", GH_ParamAccess.item, 255);
            pManager.AddIntervalParameter("Red Channel", "RC", "Input a domain component specifying the minimum and maximum floating point values for the red channel", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Green Channel", "GC", "Input a domain component specifying the minimum and maximum floating point values for the red channel", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Blue Channel", "BC", "Input a domain component specifying the minimum and maximum floating point values for the red channel", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Minimum Thickness", "NT", "Input the polyline minimum thickness", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Maximum Thickness", "MT", "Input the polyline maximum thickness", GH_ParamAccess.item, 3);

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
            int alphaChannel = new int();
            Interval redInterval = new Interval();
            Interval greenInterval = new Interval();
            Interval blueInterval = new Interval();

            int minThickness = new int();
            int maxThickness = new int();

            DA.GetData(0, ref path);
            if (!DA.GetData(1, ref alphaChannel)) return;
            if (!DA.GetData(2, ref redInterval)) return;
            if (!DA.GetData(3, ref greenInterval)) return;
            if (!DA.GetData(4, ref blueInterval)) return;
            if (!DA.GetData(5, ref minThickness)) return;
            if (!DA.GetData(6, ref maxThickness)) return;

            if (minThickness < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Min thickness has to be greater than 0, please increase"); return; }
            if (maxThickness > 4) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "There is a performance decrease if the max thickness is greater than 3, keep an eye on your value"); return; }

            ColorData colorData = new ColorData(path, alphaChannel, redInterval, greenInterval, blueInterval, minThickness, maxThickness);
            colorData.ColorDataType = "Gradient";
            DA.SetData(0, colorData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.GradientTrail;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2f57fc33-cb63-4662-bc04-716b7bea41a8"); }
        }
    }
}
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
            pManager.AddIntervalParameter("Red Channel", "RC", "Input a domain component specifying the minimum and maximum floating point values for the red channel", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Green Channel", "GC", "Input a domain component specifying the minimum and maximum floating point values for the red channel", GH_ParamAccess.item);
            pManager.AddIntervalParameter("Blue Channel", "BC", "Input a domain component specifying the minimum and maximum floating point values for the red channel", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Minimum Thickness", "NT", "Input the polyline minimum thickness", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Maximum Thickness", "MT", "Input the polyline maximum thickness", GH_ParamAccess.item);

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
            Interval redInterval = new Interval();
            Interval greenInterval = new Interval();
            Interval blueInterval = new Interval();

            int minThickness = new int();
            int maxThickness = new int();

            DA.GetData(0, ref path);
            if (!DA.GetData(1, ref redInterval)) return;
            if (!DA.GetData(2, ref greenInterval)) return;
            if (!DA.GetData(3, ref blueInterval)) return;
            if (!DA.GetData(4, ref minThickness)) return;
            if (!DA.GetData(5, ref maxThickness)) return;

            ColorData colorData = new ColorData(path, redInterval, greenInterval, blueInterval, minThickness, maxThickness);
            colorData.colorDataType = "Gradient";
            DA.SetData(0, colorData);
            /*
            object redInterval = null;
            object greenInterval = null;
            object blueInterval = null;

            object minThickness = null;
            object maxThickness = null;

            if (!DA.GetData(0, ref redInterval)) return;
            if (!DA.GetData(1, ref greenInterval)) return;
            if (!DA.GetData(2, ref blueInterval)) return;
            if (!DA.GetData(3, ref minThickness)) return;
            if (!DA.GetData(4, ref maxThickness)) return;

            ArrayList myAL = new ArrayList();

            string intervalType = redInterval.GetType().Name.ToString();
            if(redInterval.GetType() != typeof(GH_Interval))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Red Channel Input must be an interval (domain) component not a " + intervalType); return;
            }else
            {
                GH_Interval ghInt = (GH_Interval)redInterval;
                myAL.Add(ghInt);
            }

            string intervalType2 = greenInterval.GetType().Name.ToString();
            if (greenInterval.GetType() != typeof(GH_Interval))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Red Channel Input must be an interval (domain) component not a " + intervalType2); return;
            }
            else
            {
                GH_Interval ghInt = (GH_Interval)greenInterval;
                myAL.Add(ghInt);
            }

            string intervalType3 = blueInterval.GetType().Name.ToString();
            if (blueInterval.GetType() != typeof(GH_Interval))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Red Channel Input must be an interval (domain) component not a " + intervalType3); return;
            }
            else
            {
                GH_Interval ghInt = (GH_Interval)blueInterval;
                myAL.Add(ghInt);
            }

            string type = minThickness.GetType().Name.ToString();
            if (minThickness.GetType() != typeof(GH_Integer) && minThickness.GetType() != typeof(GH_Number))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input a integer or float value for minimum thickness not a " + type); return;
            }
            else
            {
                if (minThickness.GetType() == typeof(GH_Integer))
                {
                    GH_Integer ghInt = (GH_Integer)minThickness;
                    myAL.Add(ghInt);
                }
                else
                {
                    GH_Number ghNum = (GH_Number)minThickness;
                    myAL.Add(Convert.ToInt32(ghNum.Value));
                }
            }

            string type2 = maxThickness.GetType().Name.ToString();
            if (maxThickness.GetType() != typeof(GH_Integer) && maxThickness.GetType() != typeof(GH_Number))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input a integer or float value for maximum thickness not a " + type2); return;
            }
            else
            {
                if (maxThickness.GetType() == typeof(GH_Integer))
                {
                    GH_Integer ghInt = (GH_Integer)maxThickness;
                    myAL.Add(ghInt);
                }
                else
                {
                    GH_Number ghNum = (GH_Number)maxThickness;
                    myAL.Add(Convert.ToInt32(ghNum.Value));
                }
            }
            DA.SetDataList(0, myAL);
            */
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
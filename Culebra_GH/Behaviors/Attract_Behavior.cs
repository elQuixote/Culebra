using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;
using System.Linq;

namespace Culebra_GH.Behaviors
{
    public class Attract_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Attract_Behavior class.
        /// </summary>
        public Attract_Behavior()
          : base("Attraction Force", "AF",
              "Attracts a object towards a set of targets. Differs from Seek",
              "Culebra_GH", "04 | Forces")
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
            base.m_attributes = new Utilities.CustomAttributes(this, 0);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Targets", "T", "Input a list of target points to attract to", GH_ParamAccess.list);
            pManager.AddNumberParameter("Thresholds", "TH", "Input a list of threshold values, one for each target", GH_ParamAccess.list);
            pManager.AddNumberParameter("Attraction Value", "AV", "Input a value specifying attraction, this is the magnitude", GH_ParamAccess.item, 0.97);
            pManager.AddNumberParameter("Max Attraction", "MA", "Input the maximum attraction value", GH_ParamAccess.item, 0.06);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Attraction Force", "AB", "The attraction behavior data structure", GH_ParamAccess.item);
            pManager.AddCircleParameter("Attraction Threshold Viz", "V", "Visual representation of attaction targets and thresholds", GH_ParamAccess.list);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> targets = new List<Point3d>();
            List<double> thresholds = new List<double>();
            double av = new double();
            double ma = new double();

            if (!DA.GetDataList(0, targets)) return;
            if (!DA.GetDataList(1, thresholds)) return;
            if (!DA.GetData(2, ref av)) return;
            if (!DA.GetData(3, ref ma)) return;
            if(targets.Count != thresholds.Count) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "List lengths for targets and thresholds must match, please check your inputs"); return; }

            List<Circle> circles = new List<Circle>();
            int count = 0;
            foreach(Point3d p in targets)
            {
                circles.Add(new Circle(p, thresholds[count]));
                count++;
            }
            List<float> floatThresholds = thresholds.Select<double, float>(i => (float)i).ToList();
            ForceData forceData = new ForceData(targets, floatThresholds, (float)av, (float)ma);
            forceData.ForceType = "Attract";

            DA.SetData(0, forceData);
            DA.SetDataList(1, circles);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Attract_A;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ead2ded8-0510-46cb-a1f3-891a01d72788"); }
        }
    }
}
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Tracking_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Tracking_Behavior class.
        /// </summary>
        public Tracking_Behavior()
          : base("Multi Path Tracking", "T",
              "Multi Path Following Algorithm",
              "Culebra_GH", "03 | Behaviors")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quinary;
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
            pManager.AddCurveParameter("Polylines", "P", "Input a list of polylines you want to follow, POLYLINE RESOLUTION IS IMPORTANT, KEEP AS LOW AS POSSIBLE", GH_ParamAccess.list);
            pManager.AddNumberParameter("Polyline Threshold","PT","Input the distance threshold enabling agents to see shapes", GH_ParamAccess.item, 500.0);
            pManager.AddNumberParameter("Projection Distance", "PD", "Input the projection distance of point ahead on the path to seek", GH_ParamAccess.item, 50.0);
            pManager.AddNumberParameter("Polyline Radius", "PR", "Input the radius of the shapes", GH_ParamAccess.item, 15.0);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Tracking Behavior", "TB", "The tracking behavior data structure", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> crvList = new List<Curve>();
            double threshold = new double();
            double projectionDistance = new double();
            double radius = new double();

            if (!DA.GetDataList(0, crvList)) return;
            if (!DA.GetData(1, ref threshold)) return;
            if (!DA.GetData(2, ref projectionDistance)) return;
            if (!DA.GetData(3, ref radius)) return;

            List<Polyline> polylineList = new List<Polyline>();
            foreach (Curve crv in crvList)
            {
                Polyline polyline = new Polyline();
                bool convert = crv.TryGetPolyline(out polyline);
                if (!convert) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not convert curve to polyline, please ensure that you do not input a 3 degree nurbs curve"); }
                polylineList.Add(polyline);
            }
            if(polylineList.Count == 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "None of the curves converted to polylines properly, please check your input curves or polylines"); return; }

            java.util.List jData = CulebraData.Utilities.Convert.PolylinesToMultiShapes(polylineList);
            TrackingData trackingData = new TrackingData(jData, (float)threshold, (float)projectionDistance, (float)radius);

            DA.SetData(0, trackingData);
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
                return Culebra_GH.Properties.Resources.Tracking;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ceda61d2-2e40-4bf4-8195-b1f653d47658"); }
        }
    }
}
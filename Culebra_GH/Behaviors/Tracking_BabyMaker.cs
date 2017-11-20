using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Tracking_BabyMaker : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Tracking_Behavior class.
        /// </summary>
        public Tracking_BabyMaker()
          : base("Tracking II", "TT",
              "Description",
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
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Polylines", "P", "Input a list of polylines you want to follow", GH_ParamAccess.list);
            pManager.AddNumberParameter("Polyline Threshold", "PT", "Input the distance threshold enabling agents to see shapes", GH_ParamAccess.item);
            pManager.AddNumberParameter("Projection Distance", "PD", "Input the projection distance of point ahead on the path to seek", GH_ParamAccess.item);
            pManager.AddNumberParameter("Polyline Radius", "PR", "Input the radius of the shapes", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Trigger Spawn", "TS", "Input value specifying if creeper is now allowed to spawn any children objects stored", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Max Children", "MC", "Input value specifying the maximum number of children each creeper can have", GH_ParamAccess.item);
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
            bool trigger = new bool();
            int maxChildren = new int();

            if (!DA.GetDataList(0, crvList)) return;
            if (!DA.GetData(1, ref threshold)) return;
            if (!DA.GetData(2, ref projectionDistance)) return;
            if (!DA.GetData(3, ref radius)) return;
            if (!DA.GetData(4, ref trigger)) return;
            if (!DA.GetData(5, ref maxChildren)) return;

            List<Polyline> polylineList = new List<Polyline>();
            foreach (Curve crv in crvList)
            {
                Polyline polyline = new Polyline();
                bool convert = crv.TryGetPolyline(out polyline);
                if (!convert) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not convert curve to polyline, please ensure that you do not input a 3 degree nurbs curve"); }
                polylineList.Add(polyline);
            }
            if (polylineList.Count == 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "None of the curves converted to polylines properly, please check your input curves or polylines"); return; }

            TrackingData trackingData = new TrackingData(polylineList, (float)threshold, (float)projectionDistance, (float)radius, trigger, maxChildren);

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1d4d2db9-e615-4d31-ab15-7718414491d9"); }
        }
    }
}
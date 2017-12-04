using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Tracking_Mapped_BabyMaker : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Tracking_Behavior class.
        /// </summary>
        public Tracking_Mapped_BabyMaker()
          : base("Multi Path Tracking II Mapped", "TT",
              "MultiShape Path Following Algorithm capable of spawning children with image color sampling override for any path attributes and remaping of color values - see example files",
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
            pManager.AddNumberParameter("Polyline Threshold", "PT", "Input the distance threshold enabling agents to see shapes", GH_ParamAccess.item, 500.0);
            pManager.AddNumberParameter("Projection Distance", "PD", "Input the projection distance of point ahead on the path to seek", GH_ParamAccess.item, 50.0);
            pManager.AddNumberParameter("Polyline Radius", "PR", "Input the radius of the shapes", GH_ParamAccess.item, 15.0);
            pManager.AddBooleanParameter("Trigger Spawn", "TS", "Input value specifying if creeper is now allowed to spawn any children objects stored", GH_ParamAccess.item, true);
            pManager.AddIntegerParameter("Max Children", "MC", "Input value specifying the maximum number of children each creeper can have", GH_ParamAccess.item, 2);
            pManager.AddMeshParameter("Colored Mesh", "CM", "Input a color mesh to drive the path parameters", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Map Threshold", "MT", "Input value specifying if you want the path threshold value to be color driven", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Map Projection", "MP", "Input value specifying if you want the projection value to be color driven", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Map Radius", "MR", "Input value specifying if you want the path radius value to be color driven", GH_ParamAccess.item, true);
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
            Mesh mesh = null;
            bool mapThreshold = new bool();
            bool mapProjection = new bool();
            bool mapRadius = new bool();

            if (!DA.GetDataList(0, crvList)) return;
            if (!DA.GetData(1, ref threshold)) return;
            if (!DA.GetData(2, ref projectionDistance)) return;
            if (!DA.GetData(3, ref radius)) return;
            if (!DA.GetData(4, ref trigger)) return;
            if (!DA.GetData(5, ref maxChildren)) return;
            if (!DA.GetData(6, ref mesh)) return;
            if (mesh == null || mesh.VertexColors.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input mesh must have vertex colors, please check your input");
                return;
            }
            if (!DA.GetData(7, ref mapThreshold)) return;
            if (!DA.GetData(8, ref mapProjection)) return;
            if (!DA.GetData(9, ref mapRadius)) return;

            List<Polyline> polylineList = new List<Polyline>();
            foreach (Curve crv in crvList)
            {
                Polyline polyline = new Polyline();
                bool convert = crv.TryGetPolyline(out polyline);
                if (!convert) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not convert curve to polyline, please ensure that you do not input a 3 degree nurbs curve"); }
                polylineList.Add(polyline);
            }
            if (polylineList.Count == 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "None of the curves converted to polylines properly, please check your input curves or polylines"); return; }

            java.util.List jData = CulebraData.Utilities.Convert.PolylinesToMultiShapes(polylineList);
            TrackingData trackingData = new TrackingData(jData, (float)threshold, (float)projectionDistance, (float)radius, trigger, maxChildren, mesh, mapThreshold, mapProjection, mapRadius);

            DA.SetData(0, trackingData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Tracking_BabyMapped;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8ccf4d7f-60f6-439b-b4f7-51b2b0b4c4a2"); }
        }
    }
}
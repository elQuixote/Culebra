using System;
using Culebra_GH.Data_Structures;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Culebra_GH.Behaviors
{
    public class MeshCrawl_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MeshCrawl_Behavior class.
        /// </summary>
        public MeshCrawl_Behavior()
          : base("Mesh Crawl", "MC",
              "Mesh Crawling allows agent to move along a mesh object",
              "Culebra_GH", "03 | Behaviors")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.senary;
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
            pManager.AddMeshParameter("Mesh", "M", "The mesh object to crawl on", GH_ParamAccess.item);
            pManager.AddNumberParameter("Mesh Threshold", "MT", "Input the distance threshold, the min distance current position needs to be from mesh in order to move towards it", GH_ParamAccess.item, 1.5);
            pManager.AddNumberParameter("Mesh Projection Distance", "MPD", "Input the amount to project the current location along the current speed", GH_ParamAccess.item, 2.0);
            pManager.AddNumberParameter("Multiplier", "M", "Input the multiplier value", GH_ParamAccess.item, 1.5);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Mesh Crawling Behavior", "MCB", "The mesh crawling behavior data structure", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Mesh mesh = null;
            double threshold = new double();
            double amplitude = new double();
            double multiplier = new double();

            if (!DA.GetData(0, ref mesh)) return;
            if (!DA.GetData(1, ref threshold)) return;
            if (!DA.GetData(2, ref amplitude)) return;
            if (!DA.GetData(3, ref multiplier)) return;

            MeshCrawlData mcd = new MeshCrawlData(mesh, (float)threshold, (float)amplitude, (float)multiplier);
            DA.SetData(0, mcd);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.MeshCrawl_B;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("ff2f8456-ec6d-4938-88af-ca19a76b4364"); }
        }
    }
}
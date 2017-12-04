using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Noise_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Noise_Behavior class.
        /// </summary>
        public Noise_Behavior()
          : base("Noise", "N",
              "2D/3D Improved Perlin Noise",
              "Culebra_GH", "03 | Behaviors")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.quarternary;
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
            pManager.AddNumberParameter("Scale", "S", "Input value specifying the noise scale", GH_ParamAccess.item, 300.0);
            pManager.AddNumberParameter("Strength", "ST", "Input value specifying the noise strength", GH_ParamAccess.item, 7.0);
            pManager.AddNumberParameter("Multiplier", "M", "Input value to add a jitter type of movement", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Velocity", "V", "Input value specifying the noise velocity multiplier", GH_ParamAccess.item, 0.5);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Noise Behavior", "SB", "The noise behavior data structure", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double scale = new double();
            double strength = new double();
            double multiplier = new double();
            double velocity = new double();

            if (!DA.GetData(0, ref scale)) return;
            if (!DA.GetData(1, ref strength)) return;
            if (!DA.GetData(2, ref multiplier)) return;
            if (!DA.GetData(3, ref velocity)) return;

            NoiseData noiseData = new NoiseData((float)scale, (float)strength, (float)multiplier, (float)velocity);
            DA.SetData(0, noiseData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.PerlinNoise;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("c6d25a5a-5379-443b-9021-15da22939ffb"); }
        }
    }
}
using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;
using Rhino.Geometry;

namespace Culebra_GH.Behaviors
{
    public class Noise_Mapped_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Noise_Behavior class.
        /// </summary>
        public Noise_Mapped_Behavior()
          : base("Noise Mapped", "NM",
              "2D Improved Perlin Noise with image color sampling override for any behavior attribute",
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
            pManager.AddMeshParameter("Colored Mesh", "CM", "Input a color mesh to drive the flocking parameters", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Map Scale", "MS", "Input value specifying if you want the scale value to be color driven", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Map Strength", "MST", "Input value specifying if you want the strength value to be color driven", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Map Multiplier", "MM", "Input value specifying if you want the multiplier value to be color driven", GH_ParamAccess.item, false);
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
            Mesh mesh = null;
            bool mapScale = new bool();
            bool mapStrength = new bool();
            bool mapMultiplier = new bool();

            if (!DA.GetData(0, ref scale)) return;
            if (!DA.GetData(1, ref strength)) return;
            if (!DA.GetData(2, ref multiplier)) return;
            if (!DA.GetData(3, ref velocity)) return;
            if (!DA.GetData(4, ref mesh)) return;

            if (mesh == null || mesh.VertexColors.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input mesh must have vertex colors, please check your input");
                return;
            }

            if (!DA.GetData(5, ref mapScale)) return;
            if (!DA.GetData(6, ref mapStrength)) return;
            if (!DA.GetData(7, ref mapMultiplier)) return;

            NoiseData noiseData = new NoiseData((float)scale, (float)strength, (float)multiplier, (float)velocity, mesh, mapScale, mapStrength, mapMultiplier);
            DA.SetData(0, noiseData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.PerlinNoise_Mapped;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("61e8507c-b396-4c05-b28b-c364236f252b"); }
        }
    }
}
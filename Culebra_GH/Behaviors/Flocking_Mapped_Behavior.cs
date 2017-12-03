using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;
using Rhino.Geometry;

namespace Culebra_GH.Behaviors
{
    public class Flocking_Mapped_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Flocking_Behavior class.
        /// </summary>
        public Flocking_Mapped_Behavior()
          : base("Flocking Mapped", "FM",
              "Flocking Algorithm with image color sampling override for any flocking attributes and remaping of color values",
              "Culebra_GH", "03 | Behaviors")
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
            pManager.AddBooleanParameter("Connect", "C", "Input a boolean toggle - True = Connect the heads (visualizes their search radius, DRAMATICALLY REDUCES PERFORMANCE) | False = Do not draw connectivity", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("View Angle", "VA", "Input a float value specifying the view angle each agent can see", GH_ParamAccess.item, 180);
            pManager.AddNumberParameter("Search Radius", "SR", "Input a float value specifying the distance each creeper can see", GH_ParamAccess.item, 70);
            pManager.AddNumberParameter("Align Value", "AV", "Input a float value specifying alignment (Steer towards the average heading of local flockmates) vector scale value", GH_ParamAccess.item, 0.24);
            pManager.AddNumberParameter("Separation Value", "SV", "Input a float value specifying separation (Steer to avoid crowding local flockmates) vector scale value", GH_ParamAccess.item, 0.09);
            pManager.AddNumberParameter("Cohesion Value", "CV", "Input a float value specifying cohesion (Steer to move toward the average position of local flockmates) vector scale value", GH_ParamAccess.item, 0.045);
            pManager.AddMeshParameter("Colored Mesh", "CM", "Input a color mesh to drive the flocking parameters", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Map Alignment", "MA", "Input value specifying if you want the alignment value to be color driven", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Map Separation", "MS", "Input value specifying if you want the separation value to be color driven", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Map Cohesion", "MC", "Input value specifying if you want the cohesion value to be color driven", GH_ParamAccess.item);

        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Flocking Behavior", "FB", "The flocking behavior data structure", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool connect = new bool();
            double viewAngle = new double();
            double searchRadius = new double();
            double alignValue = new double();
            double separateValue = new double();
            double cohesionValue = new double();

            Mesh mesh = null;
            bool mapAlign = new bool();
            bool mapSep = new bool();
            bool mapCoh = new bool();

            if (!DA.GetData(0, ref connect)) return;
            if (!DA.GetData(1, ref viewAngle)) return;
            if (!DA.GetData(2, ref searchRadius)) return;
            if (!DA.GetData(3, ref alignValue)) return;
            if (!DA.GetData(4, ref separateValue)) return;
            if (!DA.GetData(5, ref cohesionValue)) return;
            if (!DA.GetData(6, ref mesh)) return;

            if (viewAngle > 360)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Angle cannot be higher than 360, please reduce value");
                return;
            }
            if (viewAngle <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Angle cannot be less than or equal to 0, please increase value");
                return;
            }
            if (mesh == null || mesh.VertexColors.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input mesh must have vertex colors, please check your input");
                return;
            }
            if (connect)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Use this feature to visualize your search radius, however it drastically affects performance so use wisely");
            }
            if (!DA.GetData(7, ref mapAlign)) return;
            if (!DA.GetData(8, ref mapSep)) return;
            if (!DA.GetData(9, ref mapCoh)) return;

            FlockingData flockData = new FlockingData((float)alignValue, (float)separateValue, (float)cohesionValue, (float)searchRadius, (float)viewAngle, connect,
                mesh, mapAlign, mapSep, mapCoh);
            DA.SetData(0, flockData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Flocking_Map;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a8003d3a-f9d2-4f4d-9e7d-7a3fc2caf83a"); }
        }
    }
}
using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Flocking_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Flocking_Behavior class.
        /// </summary>
        public Flocking_Behavior()
          : base("Flocking", "FL",
              "Flocking Algorithm",
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

            if (!DA.GetData(0, ref connect)) return;
            if (!DA.GetData(1, ref viewAngle)) return;
            if (!DA.GetData(2, ref searchRadius)) return;
            if (!DA.GetData(3, ref alignValue)) return;
            if (!DA.GetData(4, ref separateValue)) return;
            if (!DA.GetData(5, ref cohesionValue)) return;

            if(viewAngle > 360)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Angle cannot be higher than 360, please reduce value");
                return;
            }
            if(viewAngle <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Angle cannot be less than or equal to 0, please increase value");
                return;
            }
            if (connect)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Use this feature to visualize your search radius, however it drastically affects performance so use wisely");
            }
            FlockingData flockData = new FlockingData((float)alignValue, (float)separateValue, (float)cohesionValue, (float)searchRadius, (float)viewAngle, connect);
            DA.SetData(0, flockData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Flocking;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("cc85ce0e-ac5e-4415-ad97-4167e426d0c1"); }
        }
    }
}
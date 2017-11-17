using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Flocking_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Flocking_Behavior class.
        /// </summary>
        public Flocking_Behavior()
          : base("Flocking", "Nickname",
              "Description",
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
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Connect", "C", "Input a boolean toggle - True = Connect the heads (visualizes their search radius) | False = Do not draw connectivity", GH_ParamAccess.item);
            pManager.AddNumberParameter("View Angle", "VA", "Input a float value specifying the view angle each agent can see", GH_ParamAccess.item);
            pManager.AddNumberParameter("Search Radius", "SR", "Input a float value specifying the distance each creeper can see", GH_ParamAccess.item);
            pManager.AddNumberParameter("Align Value", "AV", "Input a float value specifying alignment vector scale value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Separation Value", "SV", "Input a float value specifying separation vector scale value", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cohesion Value", "CV", "Input a float value specifying cohesion vector scale value", GH_ParamAccess.item);
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

            FlockingData flockData = new FlockingData(alignValue, separateValue, cohesionValue, searchRadius, viewAngle, connect);
            DA.SetData(0, flockData);
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
            get { return new Guid("cc85ce0e-ac5e-4415-ad97-4167e426d0c1"); }
        }
    }
}
using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Wandering_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Wandering_Behavior class.
        /// </summary>
        public Wandering_Behavior()
          : base("Wandering", "Nickname",
              "Description",
              "Culebra_GH", "Behaviors")
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
            pManager.AddBooleanParameter("Randomize", "R", "R", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Add Heading", "AH", "AH", GH_ParamAccess.item);
            pManager.AddNumberParameter("Change", "C", "C", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wander Radius", "WR", "WR", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wander Distance", "WD", "WD", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rotation Trigger", "RT", "RT", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Wandering Behavior", "WB", "The Wandering Behavior Data Structure", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool randomizeIt = new bool();
            bool addHeadingTrajectory = new bool();
            double change = new double();
            double wanderRad = new double();
            double wanderDist = new double();
            double rotationTrigger = new double();

            if (!DA.GetData(0, ref randomizeIt)) return;
            if (!DA.GetData(1, ref addHeadingTrajectory)) return;
            if (!DA.GetData(2, ref change)) return;
            if (!DA.GetData(3, ref wanderRad)) return;
            if (!DA.GetData(4, ref wanderDist)) return;
            if (!DA.GetData(5, ref rotationTrigger)) return;

            WanderingData wanderData = new WanderingData(change, wanderRad, wanderDist, rotationTrigger, randomizeIt, addHeadingTrajectory);
            DA.SetData(0, wanderData);
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
            get { return new Guid("6da52bc9-dc35-4168-8d99-9bc75fd8b607"); }
        }
    }
}
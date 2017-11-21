using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class SuperWandering_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Wandering_Behavior class.
        /// </summary>
        public SuperWandering_Behavior()
          : base("Weaving Wandering", "Nickname",
              "Description",
              "Culebra_GH", "03 | Behaviors")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
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
            pManager.AddNumberParameter("Change", "C", "C", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wander Radius", "WR", "WR", GH_ParamAccess.item);
            pManager.AddNumberParameter("Wander Distance", "WD", "WD", GH_ParamAccess.item);
            pManager.AddNumberParameter("Rotation Trigger", "RT", "WD", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Type", "T", "Input value specifying the type of Wandering (0 = Type A | 1 = Type B | 2 = Type C", GH_ParamAccess.item);
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
            double change = new double();
            double wanderRad = new double();
            double wanderDist = new double();
            double rotationTrigger = new double();
            int type = new int();

            if (!DA.GetData(0, ref change)) return;
            if (!DA.GetData(1, ref wanderRad)) return;
            if (!DA.GetData(2, ref wanderDist)) return;
            if (!DA.GetData(3, ref rotationTrigger)) return;
            if (!DA.GetData(4, ref type)) return;

            WanderingData wanderData = new WanderingData((float)change, (float)wanderRad, (float)wanderDist, (float)rotationTrigger);
            if (type == 0){wanderData.wanderingType = "SuperWander";}
            if (type == 1) { wanderData.wanderingType = "SuperWander_B"; }
            if (type == 2) { wanderData.wanderingType = "SuperWander_C"; }
            DA.SetData(0, wanderData);
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("26ce7eda-52f3-4bfa-bb98-47a375271c88"); }
        }
    }
}
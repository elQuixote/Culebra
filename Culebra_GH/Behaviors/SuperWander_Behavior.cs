using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class SuperWandering_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Wandering_Behavior class.
        /// </summary>
        public SuperWandering_Behavior()
          : base("Weaving Wandering", "SW",
              "Expanded 2D Wandering Algorithm using step triggers to create a weaving type movement 2D Wandering Algorithm, Wandering is a type of random steering which has some long term order. Force Values from Move Settings have a strong effect on behavior",
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
            pManager.AddNumberParameter("Change", "C", "Input value specifying the incremented change value used to get the polar coordinates.", GH_ParamAccess.item, 100.0);
            pManager.AddNumberParameter("Radius", "WR", "Input value specifying the radius for the wandering circle", GH_ParamAccess.item, 60.0);
            pManager.AddNumberParameter("Distance", "WD", "Input the distance for the wander circle, this is a projection value in the direction of the objects speed vector.", GH_ParamAccess.item, 60.0);
            pManager.AddNumberParameter("Rotation Trigger", "RT", "This value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value", GH_ParamAccess.item, 6.0);
            pManager.AddIntegerParameter("Type", "T", "Input value specifying the type of Wandering (0 = Type A | 1 = Type B | 2 = Type C", GH_ParamAccess.item, 0);
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
            if(type > 2) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Type value cannot be greater than 2, please input correct number"); }
            DA.SetData(0, wanderData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Wandering_Weave;
            }
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
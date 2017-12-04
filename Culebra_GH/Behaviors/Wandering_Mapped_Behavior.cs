using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;
using Rhino.Geometry;

namespace Culebra_GH.Behaviors
{
    public class Wandering_Mapped_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Wandering_Behavior class.
        /// </summary>
        public Wandering_Mapped_Behavior()
          : base("Wandering Mapped", "WM",
              "2D Wandering Algorithm with image color sampling override for any wandering attributes and remaping of color values, Wandering is a type of random steering which has some long term order. Force Values from Move Settings have a strong effect on behavior",
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
            pManager.AddBooleanParameter("Randomize", "R", "Input value specifying if change value will be randomly selected from-change value to change value each frame", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Add Heading", "AH", "Input value specifying if we want to add the heading", GH_ParamAccess.item, true);
            pManager.AddNumberParameter("Change", "C", "Input value specifying the incremented change value used to get the polar coordinates", GH_ParamAccess.item, 100.0);
            pManager.AddNumberParameter("Radius", "WR", "Input value specifying the radius for the wandering circle", GH_ParamAccess.item, 20.0);
            pManager.AddNumberParameter("Distance", "WD", "Input the distance for the wander circle, this is a projection value in the direction of the objects speed vector", GH_ParamAccess.item, 80.0);
            pManager.AddMeshParameter("Colored Mesh", "CM", "Input a color mesh to drive the wandering parameters", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Map Change", "MC", "Input value specifying if you want the change value to be color driven", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Map Radius", "MR", "Input value specifying if you want the radius value to be color driven", GH_ParamAccess.item, true);
            pManager.AddBooleanParameter("Map Distance", "MD", "Input value specifying if you want the distance value to be color driven", GH_ParamAccess.item, true);
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
            Mesh mesh = null;
            bool mapChange = new bool();
            bool mapRadius = new bool();
            bool mapDistance = new bool();

            if (!DA.GetData(0, ref randomizeIt)) return;
            if (!DA.GetData(1, ref addHeadingTrajectory)) return;
            if (!DA.GetData(2, ref change)) return;
            if (!DA.GetData(3, ref wanderRad)) return;
            if (!DA.GetData(4, ref wanderDist)) return;
            if (!DA.GetData(5, ref mesh)) return;
            if (mesh == null || mesh.VertexColors.Count == 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input mesh must have vertex colors, please check your input");
                return;
            }
            if (!DA.GetData(6, ref mapChange)) return;
            if (!DA.GetData(7, ref mapRadius)) return;
            if (!DA.GetData(8, ref mapDistance)) return;

            WanderingData wanderData = new WanderingData((float)change, (float)wanderRad, (float)wanderDist, (float)rotationTrigger, randomizeIt, addHeadingTrajectory,
                "Wander", mesh, mapChange, mapRadius, mapDistance);

            DA.SetData(0, wanderData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Wandering_Mapped;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("46515778-a1e3-40b2-a541-4eb9d17396a6"); }
        }
    }
}
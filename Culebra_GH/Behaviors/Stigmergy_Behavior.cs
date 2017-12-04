using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Stigmergy_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Stigmergy_Behavior class.
        /// </summary>
        public Stigmergy_Behavior()
          : base("Stigmergy", "ST",
              "2D/3D Trail Chasing Algorithm - Agents will chase agents trails. When using this algorithm in your main sketch use the overloaded move method, recommended values are move(6,100), in GH Trail Data TrailSet(6) Max Trails(100)",
              "Culebra_GH", "03 | Behaviors")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.tertiary;
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
            pManager.AddNumberParameter("View Angle", "VA", "Input float value for allowable vision angle in degrees", GH_ParamAccess.item, 60.0);
            pManager.AddNumberParameter("Cohesion Mag", "CM", "Input float value for cohesion value to steers towards average positions", GH_ParamAccess.item, 1.5);
            pManager.AddNumberParameter("Cohesion Range", "CR", "Input float value for cohesion threshold, value within range will enable tailCohMag", GH_ParamAccess.item, 80.0);
            pManager.AddNumberParameter("Separation Mag", "SM", "Input float value for separation value to avoids crowding on trail", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Separation Range", "SR", "Input float value for separation threshold, value within range will enable tailSepMag", GH_ParamAccess.item, 5.0);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stigmergy Behavior", "SB", "The stigmergy behavior data structure", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double va = new double();
            double cm = new double();
            double cr = new double();
            double sm = new double();
            double sr = new double();

            if (!DA.GetData(0, ref va)) return;
            if (!DA.GetData(1, ref cm)) return;
            if (!DA.GetData(2, ref cr)) return;
            if (!DA.GetData(3, ref sm)) return;
            if (!DA.GetData(4, ref sr)) return;

            if (va > 360)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Angle cannot be higher than 360, please reduce value");
                return;
            }
            if (va <= 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Angle cannot be less than or equal to 0, please increase value");
                return;
            }

            StigmergyData stigmergy = new StigmergyData((float)va, (float)cm, (float)cr, (float)sm, (float)sr);
            DA.SetData(0, stigmergy);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Engine;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("206a2559-9371-49fc-9434-8fb7e881aafb"); }
        }
    }
}
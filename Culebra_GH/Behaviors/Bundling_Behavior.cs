using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Behaviors
{
    public class Bundling_Behavior : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Bundling_Behavior class.
        /// </summary>
        public Bundling_Behavior()
          : base("Bundling", "B",
              "Settings for Self Organization of Curve Networks",
              "Culebra_GH", "04 | Forces")
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
            pManager.AddNumberParameter("Threshold", "T", "Input float value specifying the distance threshold for self org", GH_ParamAccess.item, 100.0);
            pManager.AddNumberParameter("Ratio", "R", "Input a float value specifying the distance each point can move per cycle", GH_ParamAccess.item, 0.5);
            pManager.AddBooleanParameter("Rebuild", "RB", "Input a boolean toggle - True = Rebuild | False = Do not rebuild input curve", GH_ParamAccess.item, true);
            pManager.AddIntegerParameter("Point Count", "PC", "Input integer specifying rebuild curve point value", GH_ParamAccess.item, 40);
            pManager.AddIntegerParameter("Weld Count", "WC", "Input integer value specifying how many points on each side of the curve you would like to weld", GH_ParamAccess.item, 2);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bundling Behavior", "BB", "The bundling behavior data structure", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double thresh = new double();
            double ratio = new double();
            bool rebuild = new bool();
            int pointCount = new int();
            int weldCount = new int();

            if (!DA.GetData(0, ref thresh)) return;
            if (!DA.GetData(1, ref ratio)) return;
            if (!DA.GetData(2, ref rebuild)) return;
            if (!DA.GetData(3, ref pointCount)) return;
            if (!DA.GetData(4, ref weldCount)) return;

            if (pointCount <= (weldCount / 2))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Your point count cant be less than half of the weldcount, please increase point count or decrease weld count");
                return;
            }

            BundlingData bundling = new BundlingData(thresh, ratio, rebuild, pointCount, weldCount);
            DA.SetData(0, bundling);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Bundling;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("067b02f6-3830-4429-82b5-e1576d9f2a18"); }
        }
    }
}
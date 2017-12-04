using System;
using Grasshopper.Kernel;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Settings
{
    public class Trail_Data : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Settings_Visual class.
        /// </summary>
        public Trail_Data()
            : base("Trail Data", "TD",
                "Controls the Trail Data for the Visual Settings Component",
                "Culebra_GH", "06 | Display")
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
            base.m_attributes = new Utilities.CustomAttributes(this, 2);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Trail", "T", "Input a boolean toggle specifying trail visibility (True = On | False = Off)", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Trail Step", "TS", "Input an integer value specifying the minimum amount of steps that must be taken before we store a trail vector (higher values increase performance)", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Max Trail Size", "MT", "Input an integer value specifying maximum amount of allowable trail vectors per object (lower values increase performance)", GH_ParamAccess.item, 5000);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Trail Data", "TD", "Outputs the Trail Data for the Visual Settings Component", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool trail = new bool();
            int trailStep = new int();
            int maxTrail = new int();

            if (!DA.GetData(0, ref trail)) return;
            if (!DA.GetData(1, ref trailStep)) return;
            if (!DA.GetData(2, ref maxTrail)) return;

            if(trailStep < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Trail Step has to be greater than 0, please increase"); return; }
            if (maxTrail < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "MaxTrail has to be greater than 0, please increase"); return; }

            TrailData trailData = new TrailData(trail, trailStep, maxTrail);
            DA.SetData(0, trailData);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.TrailData;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("25be7947-f47d-4138-9a6a-c53a82c70113"); }
        }
    }
}
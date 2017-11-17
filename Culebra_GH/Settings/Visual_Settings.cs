using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections;
using Grasshopper.Kernel.Types;

namespace Culebra_GH.Settings
{
    public class Visual_Settings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Settings_Visual class.
        /// </summary>
        public Visual_Settings()
            : base("Visual Settings", "VS",
                "Controls the visual settings for the Creeper Engine Outputs",
                "Culebra_GH", "Initialize")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Trail", "T", "Input a boolean toggle specifying trail visibility (True = On | False = Off)", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Trail Type", "TT", "Input an integer value list specifying the trail type (0 = Trail Curves | 1 = Trail Points", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Trail Resolution", "TR", "Input an integer value specifying the resolution of the trail (Recommend 0-20 - High Values will result in lower resolution output which can be adjusted live and dramatically increases the speed", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Clear Trail", "CT", "Input a button (True = Clears Trails | False = Keeps Trails)", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Visual Settings", "VS", "Outputs the Visual Settings for the Creeper Engine", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            bool Trail = new bool();
            bool Clear_Trail = new bool();
            int TrailRes = new int();
            int Trail_Type = new int();

            if (!DA.GetData(0, ref Trail)) return;
            if (!DA.GetData(1, ref Trail_Type)) return;
            if (!DA.GetData(2, ref TrailRes)) return;
            if (!DA.GetData(3, ref Clear_Trail)) return;

            ArrayList myAL = new ArrayList();

            myAL.Add(Trail);
            myAL.Add(Clear_Trail);
            myAL.Add(TrailRes);
            myAL.Add(Trail_Type);

            DA.SetDataList(0, myAL);
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
            get { return new Guid("25be7947-f47d-4138-9a6a-c53a82c70113"); }
        }
    }
}
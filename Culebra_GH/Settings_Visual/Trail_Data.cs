using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections;
using Grasshopper.Kernel.Types;
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
            pManager.AddIntegerParameter("Trail Step", "TS", "Input an integer value specifying the minimum amount of steps that must be taken before we store a trail vector (higher values increase performance)", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Max Trail Size", "MT", "Input an integer value specifying maximum amount of allowable trail vectors per object (lower values increase performance)", GH_ParamAccess.item);
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

            TrailData trailData = new TrailData(trail, trailStep, maxTrail);
            DA.SetData(0, trailData);

            /*
            bool trail = new bool();
            bool clearTrail = new bool();
            object trailType = null;
            int trailStep = new int();
            int maxTrail = new int();

            if (!DA.GetData(0, ref trail)) return;
            if (!DA.GetData(1, ref trailType)) return;
            if (!DA.GetData(2, ref trailStep)) return;
            if (!DA.GetData(3, ref maxTrail)) return;
            if (!DA.GetData(4, ref clearTrail)) return;

            string type = trail.GetType().Name.ToString();
            if(trail.GetType() != typeof(Boolean))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input a boolean value for Trail Input not a " + type);
                return;
            }

            ArrayList myAL = new ArrayList();

            myAL.Add(trail);
            myAL.Add(clearTrail);
            myAL.Add(trailStep);
            myAL.Add(maxTrail);

            string type2 = trailType.GetType().Name.ToString();
            if (trailType.GetType() != typeof(GH_Integer) && trailType.GetType() != typeof(GH_Number))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input a integer value for Trail Type not a " + type2);
                return;
            } else
            {
                if (trailType.GetType() == typeof(GH_Integer))
                {
                    GH_Integer ghInt = (GH_Integer)trailType;
                    myAL.Add(ghInt);
                }
                else
                {
                    GH_Number ghNum = (GH_Number)trailType;                  
                    myAL.Add(Convert.ToInt32(ghNum.Value));
                }       
            }
            DA.SetDataList(0, myAL);
            */
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
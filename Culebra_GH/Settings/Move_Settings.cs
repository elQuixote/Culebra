using System;
using Grasshopper.Kernel;
using System.Collections;
using Grasshopper.Kernel.Types;

namespace Culebra_GH.Move
{
    public class Move_Settings : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Settings_Init class.
        /// </summary>
        public Move_Settings()
            : base("Move Settings", "MS",
                "Sends the move settings to the Creeper Engine.",
                "Culebra_GH", "02 | Initialize")
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
            base.m_attributes = new Utilities.CustomAttributes(this, 1);
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Initial Speed||Vector", "IS", "Input the initial speed vector or float value", GH_ParamAccess.item);
            pManager.AddGenericParameter("Max Speed", "MS", "Input the max speed value allowed", GH_ParamAccess.item);
            pManager.AddGenericParameter("Max Force", "MF", "Input the max force value allowed", GH_ParamAccess.item);
            pManager.AddGenericParameter("Velocity Multiplier", "VM", "Input the velocity mutiplier", GH_ParamAccess.item);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Move Settings", "MS", "Outputs the move settings for the Creeper Engine", GH_ParamAccess.list);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object initialSpeed = null;
            double maxSpeed = new double();
            double maxForce = new double();
            double velocityMultiplier = new double();

            if (!DA.GetData(0, ref initialSpeed)) return;
            if (!DA.GetData(1, ref maxSpeed)) return;
            if (!DA.GetData(2, ref maxForce)) return;
            if (!DA.GetData(3, ref velocityMultiplier)) return;

            ArrayList myAL = new ArrayList();
            string objectType = initialSpeed.GetType().Name.ToString();
            if(!(initialSpeed.GetType() == typeof(GH_Vector)) && !(initialSpeed.GetType() == typeof(GH_Number)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Initial Speed input must either be a vector or a double value and not " + objectType);
            }else
            {          
                if(initialSpeed.GetType() == typeof(GH_Vector))
                {
                    GH_Vector moveVector = (GH_Vector)initialSpeed;
                    myAL.Add(initialSpeed);
                }
                else
                {
                    GH_Number moveValue = (GH_Number)initialSpeed;
                    myAL.Add(moveValue);
                }
            }
            myAL.Add(maxSpeed);
            myAL.Add(maxForce);
            myAL.Add(velocityMultiplier);

            DA.SetDataList(0, myAL);
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.MoveSettings;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("d22a39f7-599d-4c79-be2a-fc840c6bfac2"); }
        }
    }
}
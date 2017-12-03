using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Collections;

namespace Culebra_GH.Initialize
{
    public class Settings_Init : GH_Component
    {
        private string text;
        /// <summary>
        /// Initializes a new instance of the Settings_Init class.
        /// </summary>
        public Settings_Init()
            : base("Init Settings", "IS",
                "Sends the init settings to the Creeper Engine.",
                "Culebra_GH", "02 | Initialize")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.primary;
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
            pManager.AddGenericParameter("Spawn Settings", "ST", "Input the Spawn Settings output of any of the Spawn Types", GH_ParamAccess.list);
            pManager.AddGenericParameter("Dimension", "D", "Input an integer specifying which dimension you want to live in (0 = 2D | 1 = 3D", GH_ParamAccess.item);
            pManager.AddGenericParameter("Boundary", "B", "Input an integer specifying the bounds condition ( 0 = Bounce | 1 = Respawn | 2 = Do Nothing", GH_ParamAccess.item);
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Init Settings", "IS", "Outputs the init settings for the Creeper Engine", GH_ParamAccess.list);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Object> Spawn_Type = new List<Object>();
            int Dimension = new int();
            int Bounds = new int();

            if (!DA.GetDataList(0, Spawn_Type)) return;
            if (!DA.GetData(1, ref Dimension)) return;
            if (!DA.GetData(2, ref Bounds)) return;

            if (Dimension > 1 || Dimension < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Dimension value cannot be less than 0 and higher than 1, please adjust"); return; }
            if (Bounds > 2 || Bounds < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Boundary value cannot be less than 0 and higher than 2, please adjust"); return; }

            GH_Convert.ToString(Spawn_Type[0], out this.text, GH_Conversion.Both);
            ArrayList myAL = new ArrayList();
            myAL.Add(Spawn_Type[0]);
            myAL.Add(Dimension);
            myAL.Add(Bounds);
            if (this.text == "Box")
            {
                myAL.Add(Spawn_Type[1]);
                myAL.Add(Spawn_Type[2]);
                myAL.Add(Spawn_Type[3]);
                DA.SetDataList(0, myAL);
            }
            else if (this.text == "Points")
            {
                myAL.Add(Spawn_Type[1]);
                myAL.Add(Spawn_Type[2]);
                DA.SetDataList(0, myAL);
            }
            else if (this.text == "Mesh")
            {
                myAL.Add(Spawn_Type[1]);
                myAL.Add(Spawn_Type[2]);
                myAL.Add(Spawn_Type[3]);
                DA.SetDataList(0, myAL);
            }
            else if (this.text == "Curve")
            {
                myAL.Add(Spawn_Type[1]);
                myAL.Add(Spawn_Type[2]);
                myAL.Add(Spawn_Type[3]);
                DA.SetDataList(0, myAL);
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Please input a valid spawn type");
                return;
            }
        }
        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Settings_Init;
            }
        }
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{13E9265B-9E8A-4189-AC4A-BCA67DE0B906}"); }
        }
    }
}
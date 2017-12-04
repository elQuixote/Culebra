using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections;

namespace Culebra_GH.SpawnTypes
{
    public class Spawn_Box : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Spawn_Box class.
        /// </summary>
        public Spawn_Box()
            : base("Box Spawn", "BS",
                "Uses a box to contain the creepers spawn area in 2D or 3D",
                "Culebra_GH", "01 | Spawn Types")
        {
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
            pManager.AddBoxParameter("Box", "B", "Add a box specifying the spawn area in 2D or 3D", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Spawn Location", "S", "Input a value list specifying the spawn sets (0 = 2D Aligned or 3D Random on Ground | 1 = 2D Random Scattered on ground or 3D Random Scattered in box", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("Spawn Count", "CC", "Input an integer specifying the creeper count at start", GH_ParamAccess.item, 500);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Spawn Settings", "SS", "Outputs the spawn settings", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Box box = new Box();
            int spawn_location = new int();
            int spawn_count = new int();

            if (!DA.GetData(0, ref box)) return;
            if (!DA.GetData(1, ref spawn_location)) return;
            if (!DA.GetData(2, ref spawn_count)) return;

            if (spawn_location < 0 || spawn_location > 1) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spawn Location value must be greater than 0 and less than or equal to 1, please adjust"); return; }
            if(spawn_count < 0) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Spawn count must be greater than 0, please increase"); return; }

            ArrayList myAL = new ArrayList();

            myAL.Add("Box");
            myAL.Add(box);
            myAL.Add(spawn_location);
            myAL.Add(spawn_count);

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
                return Culebra_GH.Properties.Resources.Spawn_Box;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{4455948F-34BC-4FB8-BF3C-93DB8C3BAA03}"); }
        }
    }
}
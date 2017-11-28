using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Collections;

namespace Culebra_GH.SpawnTypes
{
    public class Spawn_Point : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Spawn_Point class.
        /// </summary>
        public Spawn_Point()
            : base("Point Spawn", "PS",
                "Uses a list of points as starting positions for the creepers in 2D or 3D",
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
            pManager.AddPointParameter("Points", "P", "Add a list of points which will become the creepers spawn location", GH_ParamAccess.list);
            pManager.AddBoxParameter("Box", "B", "Add a box specifying the bounding box to bounce from", GH_ParamAccess.item);
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
            List<Point3d> ptList = new List<Point3d>();
            Box box = new Box();

            if (!DA.GetDataList(0, ptList)) return;
            if (!DA.GetData(1, ref box)) return;

            ArrayList myAL = new ArrayList();
            myAL.Add("Points");
            myAL.Add(ptList);
            myAL.Add(box);
            DA.SetDataList(0, myAL);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Spawn_Point;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("7173e98d-7626-4a15-b229-994064315f79"); }
        }
    }
}
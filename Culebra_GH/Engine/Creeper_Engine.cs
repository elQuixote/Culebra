using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Objects;

namespace Culebra_GH.Engine
{
    public class Creeper_Engine : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Creeper_Engine class.
        /// </summary>
        public Creeper_Engine()
          : base("Creeper_Engine", "CE",
              "Engine Module Test",
              "Culebra_GH", "Testing")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.secondary;
            }
        }
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Behavior Data", "BD", "The behavior data", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Settings Data Test", "SDT", "The igh object data test", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            object obj = null;

            if (!DA.GetData(0, ref obj) || obj == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input Object is Null");
                return;
            }
            string objtype = obj.GetType().Name.ToString();
            if (!(obj.GetType() == typeof(IGH_BehaviorData)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "You did not input a Behavior Data Object, please ensure input is Behavior Data Object and not " + objtype);
                return;
            }
            else
            {
                List<string> stringList = new List<string>();
                IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)obj;
                foreach(string s in igh_Behavior.Value.dataOrder)
                {
                    if(s == "Flocking")
                    {
                        stringList.Add("Flocking Alignment Value = " + igh_Behavior.Value.flockData.alignment_Value.ToString());
                        stringList.Add("Flocking Separation Value = " + igh_Behavior.Value.flockData.separation_Value.ToString());
                    }
                    else if(s == "Wandering")
                    {
                        stringList.Add("Wandering Radius Value = " + igh_Behavior.Value.wanderData.wanderingRadius.ToString());
                        stringList.Add("Wandering Distance Value = " + igh_Behavior.Value.wanderData.wanderingDistance.ToString());
                    }
                    else
                    {
                        stringList.Add("We got a behavior problem");
                    }
                }
                DA.SetDataList(0, stringList); 
            }
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
            get { return new Guid("af68d5a9-fd6b-4df1-adfc-b9fb225edb51"); }
        }
    }
}
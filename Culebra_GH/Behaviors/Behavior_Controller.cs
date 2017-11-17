
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using System.Linq;
using Grasshopper.Kernel.Parameters;

using Rhino;
using Rhino.DocObjects;
using Rhino.Collections;

using GH_IO;
using GH_IO.Serialization;

using System.IO;

using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using Culebra_GH.Data_Structures;
using Culebra_GH.Objects;

namespace Culebra_GH.Behaviors
{
    public class Behavior_Controller : GH_Component, IGH_VariableParameterComponent
    {
        GH_Document GrasshopperDocument;
        IGH_Component Component;

        public Behavior_Controller()
          : base("Controller", "BC",
              "Behavior Merging Controller",
              "Culebra_GH", "03 | Behaviors")
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
            //pManager.AddGenericParameter("Behavior A", "B", "Connect Desired Behavior Here", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Behavior Settings", "B", "Behavior Settings - These connect to the engine component", GH_ParamAccess.list);
            pManager.AddGenericParameter("Behavior Data Object", "BDO", "The behaviorData object", GH_ParamAccess.item);
        }
        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Component = this;
            GrasshopperDocument = this.OnPingDocument();

            int inputCount = Component.Params.Input.Count;
            BehaviorData behaviorData = new BehaviorData();
            List<string> stringlist = new List<string>();
            List<string> behaviorNames = new List<string>();
            int hitCounter = 0;
            for (int i = 0; i < inputCount; i++)
            {
                IGH_DocumentObject connectedComponent = Component.Params.Input[i].Sources[0].Attributes.GetTopLevel.DocObject;
                string name = connectedComponent.Name;
                Component.Params.Input[i].NickName = name;

                foreach (IGH_Goo a in Component.Params.Input[i].VolatileData.get_Branch(0))
                {
                    //if (a.GetType() == typeof(FlockingData))
                    if(a.ToString() == "Culebra_GH.Data_Structures.FlockingData")
                    {
                        hitCounter++;
                        //Rhino.RhinoApp.WriteLine("INSIDE FLOCKING DATA");
                        FlockingData fd;
                        bool worked = a.CastTo(out fd);
                        //Rhino.RhinoApp.WriteLine(worked.ToString());
                        //Rhino.RhinoApp.WriteLine(fd.alignment_Value.ToString());
                        behaviorData.flockData = fd;
                        behaviorNames.Add("Flocking");

                        stringlist.Add("Alignment Value = " + fd.alignment_Value.ToString());
                        stringlist.Add("Separation Value = " + fd.separation_Value.ToString());
                    }
                    //else if (a.GetType() == typeof(WanderingData))
                    else if (a.ToString() == "Culebra_GH.Data_Structures.WanderingData")
                    {
                        hitCounter++;
                        //Rhino.RhinoApp.WriteLine("INSIDE WANDERING DATA");

                        WanderingData wd;
                        bool worked = a.CastTo(out wd);
                        //Rhino.RhinoApp.WriteLine(worked.ToString());
                        //Rhino.RhinoApp.WriteLine(wd.wanderingRadius.ToString());
                        behaviorData.wanderData = wd;
                        behaviorNames.Add("Wandering");

                        stringlist.Add("Wandering Radius Value = " + wd.wanderingRadius.ToString());
                        stringlist.Add("Wandering Distance Value = " + wd.wanderingDistance.ToString());
                    }
                    else
                    {
                        AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not convert incoming data");
                    }   
                }                     
            }
            if(hitCounter > 0)
            {
                behaviorData.dataOrder = behaviorNames;
                IGH_BehaviorData igh_Behavior = new IGH_BehaviorData(behaviorData);
                DA.SetData(1, igh_Behavior);
            }
            DA.SetDataList(0, stringlist);
        }
        //-------------------------------------------
        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            if (side == GH_ParameterSide.Input && Params.Input.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            Param_GenericObject param = new Param_GenericObject();
            param.Name = GH_ComponentParamServer.InventUniqueNickname("NewComponentMagicalName", Params.Input);
            param.NickName = param.Name;
            param.Description = "Param" + (Params.Input.Count + 1);
            param.Access = GH_ParamAccess.item;

            return param;
        }
        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return true;
        }
        public void VariableParameterMaintenance()
        {
        }
        private void ParamSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            if (((e.ParameterSide == 0) && (e.ParameterIndex == (Component.Params.Input.Count - 1))) && (e.Parameter.SourceCount > 0))
            {
                IGH_Param param = this.CreateParameter(0, Component.Params.Input.Count);
                Component.Params.RegisterInputParam(param);
                Component.Params.OnParametersChanged();
            }
        }
        //-------------------------------------------
        //-------------------------------------------
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a096aa9c-1948-4da6-9e07-c01fc21285ad"); }
        }
    }
}
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Parameters;
using Culebra_GH.Data_Structures;
using Culebra_GH.Objects;

namespace Culebra_GH.Behaviors
{
    public class Behavior_Controller : GH_Component, IGH_VariableParameterComponent
    {
        private GH_Document GrasshopperDocument;
        private IGH_Component Component;
    
        public Behavior_Controller()
          : base("Controller", "BC",
              "Behavior Merging Controller, you can add/remove/rearrange behaviors. The input order will be the behavior execution stack",
              "Culebra_GH", "03 | Behaviors")
        {
        }
        public override GH_Exposure Exposure
        {
            get
            {
                return GH_Exposure.septenary;
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
        }
        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
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
            List<ForceData> forceDataList = new List<ForceData>();
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
                    if(a.ToString() == "Culebra_GH.Data_Structures.FlockingData")
                    {
                        hitCounter++;
                        FlockingData fd;
                        bool worked = a.CastTo(out fd);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Flocking data structure, please check your inputs"); return; }
                        behaviorData.FlockData = fd;
                        behaviorNames.Add("Flocking");

                        stringlist.Add("Alignment Value = " + fd.Alignment_Value.ToString());
                        stringlist.Add("Separation Value = " + fd.Separation_Value.ToString());
                    }
                    else if (a.ToString() == "Culebra_GH.Data_Structures.WanderingData")
                    {
                        hitCounter++;
                        WanderingData wd;
                        bool worked = a.CastTo(out wd);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Wandering data structure, please check your inputs"); return; }
                        behaviorData.WanderData = wd;
                        behaviorNames.Add("Wandering");

                        stringlist.Add("Wandering Radius Value = " + wd.wanderingRadius.ToString());
                        stringlist.Add("Wandering Distance Value = " + wd.wanderingDistance.ToString());
                    }else if(a.ToString() == "Culebra_GH.Data_Structures.TrackingData")
                    {
                        hitCounter++;
                        TrackingData td;
                        bool worked = a.CastTo(out td);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Tracking data structure, please check your inputs"); return; }
                        behaviorData.TrackingData = td;
                        behaviorNames.Add("Tracking");
                    }else if(a.ToString() == "Culebra_GH.Data_Structures.StigmergyData")
                    {
                        hitCounter++;
                        StigmergyData sd;
                        bool worked = a.CastTo(out sd);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Stigmergy data structure, please check your inputs"); return; }
                        behaviorData.StigmergyData = sd;
                        behaviorNames.Add("Stigmergy");
                    }else if (a.ToString() == "Culebra_GH.Data_Structures.NoiseData")
                    {
                        hitCounter++;
                        NoiseData nd;
                        bool worked = a.CastTo(out nd);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Noise data structure, please check your inputs"); return; }
                        behaviorData.NoiseData = nd;
                        behaviorNames.Add("Noise");
                    }else if (a.ToString() == "Culebra_GH.Data_Structures.ForceData")
                    {
                        hitCounter++;
                        ForceData data;
                        bool worked = a.CastTo(out data);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Forces data structure, please check your inputs"); return; }
                        forceDataList.Add(data);
                        behaviorData.ForceData = forceDataList;
                        behaviorNames.Add("Force");
                    }else if (a.ToString() == "Culebra_GH.Data_Structures.SeparationData")
                    {
                        hitCounter++;
                        SeparationData data;
                        bool worked = a.CastTo(out data);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Separation data structure, please check your inputs"); return; }
                        behaviorData.SeparationData = data;
                        behaviorNames.Add("Separation");
                    }else if (a.ToString() == "Culebra_GH.Data_Structures.MeshCrawlData")
                    {
                        hitCounter++;
                        MeshCrawlData data;
                        bool worked = a.CastTo(out data);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Mesh Crawl data structure, please check your inputs"); return; }
                        behaviorData.MeshCrawlData = data;
                        behaviorNames.Add("Crawl");
                    }else if (a.ToString() == "Culebra_GH.Data_Structures.BundlingData")
                    {
                        hitCounter++;
                        BundlingData data;
                        bool worked = a.CastTo(out data);
                        if (!worked) { AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "We could not cast to Bundling data structure, please check your inputs"); return; }
                        behaviorData.BundlingData = data;
                        behaviorNames.Add("Bundling");
                    }
                    else { AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Could not convert incoming data"); return; }   
                }                     
            }
            if(hitCounter > 0)
            {
                behaviorData.DataOrder = behaviorNames;
                IGH_BehaviorData igh_Behavior = new IGH_BehaviorData(behaviorData);
                DA.SetData(0, igh_Behavior);
            }
        }
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
        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Culebra_GH.Properties.Resources.Controller;
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
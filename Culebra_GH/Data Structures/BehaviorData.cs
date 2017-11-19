using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Culebra_GH.Data_Structures
{
    public struct BehaviorData
    {
        public FlockingData flockData { get; set; }
        public WanderingData wanderData { get; set; }
        public TrackingData trackingData { get; set; }
        public StigmergyData stigmergyData { get; set; }
        public NoiseData noiseData { get; set; }
        public List<ForceData> forceData { get; set; }
        public SeparationData separationData { get; set; }
        public List<string> dataOrder { get; set; }

        public BehaviorData(FlockingData flock_Data, WanderingData wander_Data, TrackingData tracking_data, StigmergyData stigmergy_Data, NoiseData noise_Data, List<ForceData> force_Data, SeparationData separation_Data, List<string> order_Data)
        {
            this.flockData = flock_Data;
            this.wanderData = wander_Data;
            this.trackingData = tracking_data;
            this.stigmergyData = stigmergy_Data;
            this.noiseData = noise_Data;
            this.forceData = force_Data;
            this.separationData = separation_Data;
            this.dataOrder = order_Data;
        }
    }
}

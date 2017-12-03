using System.Collections.Generic;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Behavior Data Structure
    /// </summary>
    public struct BehaviorData
    {
        public FlockingData flockData { get; set; }
        public WanderingData wanderData { get; set; }
        public TrackingData trackingData { get; set; }
        public StigmergyData stigmergyData { get; set; }
        public NoiseData noiseData { get; set; }
        public List<ForceData> forceData { get; set; }
        public SeparationData separationData { get; set; }
        public MeshCrawlData meshCrawlData { get; set; }
        public BundlingData bundlingData { get; set; }
        public List<string> dataOrder { get; set; }

        public BehaviorData(FlockingData flock_Data, WanderingData wander_Data, TrackingData tracking_data, StigmergyData stigmergy_Data, NoiseData noise_Data, 
            List<ForceData> force_Data, SeparationData separation_Data, MeshCrawlData meshCrawl_Data, List<string> order_Data, BundlingData bundling_Data)
        {
            this.flockData = flock_Data;
            this.wanderData = wander_Data;
            this.trackingData = tracking_data;
            this.stigmergyData = stigmergy_Data;
            this.noiseData = noise_Data;
            this.forceData = force_Data;
            this.separationData = separation_Data;
            this.meshCrawlData = meshCrawl_Data;
            this.bundlingData = bundling_Data;
            this.dataOrder = order_Data;
        }
    }
}

using System.Collections.Generic;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Behavior Data Structure
    /// </summary>
    public struct BehaviorData
    {
        public FlockingData FlockData { get; set; }
        public WanderingData WanderData { get; set; }
        public TrackingData TrackingData { get; set; }
        public StigmergyData StigmergyData { get; set; }
        public NoiseData NoiseData { get; set; }
        public List<ForceData> ForceData { get; set; }
        public SeparationData SeparationData { get; set; }
        public MeshCrawlData MeshCrawlData { get; set; }
        public BundlingData BundlingData { get; set; }
        public List<string> DataOrder { get; set; }

        public BehaviorData(FlockingData flock_Data, WanderingData wander_Data, TrackingData tracking_data, StigmergyData stigmergy_Data, NoiseData noise_Data, 
            List<ForceData> force_Data, SeparationData separation_Data, MeshCrawlData meshCrawl_Data, List<string> order_Data, BundlingData bundling_Data)
        {
            this.FlockData = flock_Data;
            this.WanderData = wander_Data;
            this.TrackingData = tracking_data;
            this.StigmergyData = stigmergy_Data;
            this.NoiseData = noise_Data;
            this.ForceData = force_Data;
            this.SeparationData = separation_Data;
            this.MeshCrawlData = meshCrawl_Data;
            this.BundlingData = bundling_Data;
            this.DataOrder = order_Data;
        }
    }
}

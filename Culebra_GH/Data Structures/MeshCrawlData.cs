using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    public struct MeshCrawlData
    {
        public Mesh mesh { get; set; }
        public float meshThreshold { get; set; }
        public float meshProjectionDistance { get; set; }
        public float multiplier { get; set; }
        public bool triggerBabies { get; set; }
        public int maxChildren { get; set; }

        public MeshCrawlData(Mesh mesh, float mesh_Threshold, float mesh_ProjectionDistance, float multiplier, bool trigger_Babies = false, int max_Children = 2)
        {
            this.mesh = mesh;
            this.meshThreshold = mesh_Threshold;
            this.meshProjectionDistance = mesh_ProjectionDistance;
            this.multiplier = multiplier;
            this.triggerBabies = trigger_Babies;
            this.maxChildren = max_Children;
        }
    }
}

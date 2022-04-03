using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Mesh Crawling Data Structure
    /// </summary>
    public struct MeshCrawlData
    {
        public Mesh Mesh { get; set; }
        public float MeshThreshold { get; set; }
        public float MeshProjectionDistance { get; set; }
        public float Multiplier { get; set; }
        public bool TriggerBabies { get; set; }
        public int MaxChildren { get; set; }

        public MeshCrawlData(Mesh mesh, float mesh_Threshold, float mesh_ProjectionDistance, float multiplier, bool trigger_Babies = false, int max_Children = 2)
        {
            this.Mesh = mesh;
            this.MeshThreshold = mesh_Threshold;
            this.MeshProjectionDistance = mesh_ProjectionDistance;
            this.Multiplier = multiplier;
            this.TriggerBabies = trigger_Babies;
            this.MaxChildren = max_Children;
        }
    }
}

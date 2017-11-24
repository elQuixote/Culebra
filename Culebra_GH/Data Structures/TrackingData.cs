using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    public struct TrackingData
    {
        public java.util.List polylines { get; set; }
        public float pathThreshold { get; set; }
        public float projectionDistance { get; set; }
        public float pathRadius { get; set; }
        public bool triggerBabies { get; set; }
        public int maxChildren { get; set; }

        public Mesh colorMesh { get; set; }
        public bool mapThreshold { get; set; }
        public bool mapProjection { get; set; }
        public bool mapRadius { get; set; }
         
        public TrackingData(java.util.List polylines, float path_Threshold, float projection_Distance, float path_Radius, bool trigger_Babies = false, int max_Children = 2,
            Mesh color_Mesh = null, bool map_Threshold = false, bool map_Projection = false, bool map_Radius = false)
        {
            this.polylines = polylines;
            this.pathThreshold = path_Threshold;
            this.projectionDistance = projection_Distance;
            this.pathRadius = path_Radius;
            this.triggerBabies = trigger_Babies;
            this.maxChildren = max_Children;

            this.colorMesh = color_Mesh;
            this.mapThreshold = map_Threshold;
            this.mapProjection = map_Projection;
            this.mapRadius = map_Radius;
        }
    }
}

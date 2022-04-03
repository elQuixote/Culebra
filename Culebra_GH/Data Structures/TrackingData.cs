using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Polyline Tracking Data Structure
    /// </summary>
    public struct TrackingData
    {
        public java.util.List Polylines { get; set; }
        public float PathThreshold { get; set; }
        public float ProjectionDistance { get; set; }
        public float PathRadius { get; set; }
        public bool TriggerBabies { get; set; }
        public int MaxChildren { get; set; }

        public Mesh ColorMesh { get; set; }
        public bool MapThreshold { get; set; }
        public bool MapProjection { get; set; }
        public bool MapRadius { get; set; }
         
        public TrackingData(java.util.List polylines, float path_Threshold, float projection_Distance, float path_Radius, bool trigger_Babies = false, int max_Children = 2,
            Mesh color_Mesh = null, bool map_Threshold = false, bool map_Projection = false, bool map_Radius = false)
        {
            this.Polylines = polylines;
            this.PathThreshold = path_Threshold;
            this.ProjectionDistance = projection_Distance;
            this.PathRadius = path_Radius;
            this.TriggerBabies = trigger_Babies;
            this.MaxChildren = max_Children;

            this.ColorMesh = color_Mesh;
            this.MapThreshold = map_Threshold;
            this.MapProjection = map_Projection;
            this.MapRadius = map_Radius;
        }
    }
}

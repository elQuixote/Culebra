using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
         
        public TrackingData(java.util.List polylines, float path_Threshold, float projection_Distance, float path_Radius, bool trigger_Babies = false, int max_Children = 2)
        {
            this.polylines = polylines;
            this.pathThreshold = path_Threshold;
            this.projectionDistance = projection_Distance;
            this.pathRadius = path_Radius;
            this.triggerBabies = trigger_Babies;
            this.maxChildren = max_Children;
        }
    }
}

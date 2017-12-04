using Rhino.Geometry;
using System.Collections.Generic;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Forces Data Structure
    /// </summary>
    public struct ForceData
    {
        public List<Point3d> targets { get; set; }
        public List<float> thresholds { get; set; }
        public float attractionValue { get; set; }
        public float maxAttraction { get; set; }
        public float repelValue { get; set; }
        public float maxRepel { get; set; }
        public string forceType { get; set; }

        public ForceData(List<Point3d> targets, List<float> thresholds, float attraction_Value = 0.0f, float max_Attraction = 0.0f, float repel_Value = 0.0f, float max_Repel = 0.0f, string forceType = "")
        {
            this.targets = targets;
            this.thresholds = thresholds;
            this.attractionValue = attraction_Value;
            this.maxAttraction = max_Attraction;
            this.repelValue = repel_Value;
            this.maxRepel = max_Repel;
            this.forceType = forceType;
        }
    }
}

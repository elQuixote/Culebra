using Rhino.Geometry;
using System.Collections.Generic;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Forces Data Structure
    /// </summary>
    public struct ForceData
    {
        public List<Point3d> Targets { get; set; }
        public List<float> Thresholds { get; set; }
        public float AttractionValue { get; set; }
        public float MaxAttraction { get; set; }
        public float RepelValue { get; set; }
        public float MaxRepel { get; set; }
        public string ForceType { get; set; }

        public ForceData(List<Point3d> targets, List<float> thresholds, float attraction_Value = 0.0f, float max_Attraction = 0.0f, float repel_Value = 0.0f, float max_Repel = 0.0f, string forceType = "")
        {
            this.Targets = targets;
            this.Thresholds = thresholds;
            this.AttractionValue = attraction_Value;
            this.MaxAttraction = max_Attraction;
            this.RepelValue = repel_Value;
            this.MaxRepel = max_Repel;
            this.ForceType = forceType;
        }
    }
}

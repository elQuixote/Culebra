using Rhino.Geometry;
using System.Collections.Generic;

namespace Culebra_GH.Data_Structures
{
    public struct StigmergyData
    {
        public float viewAngle { get; set; }
        public float cohesionMagnitude { get; set; }
        public float cohesionRange { get; set; }
        public float separationMagnitude { get; set; }
        public float separationRange { get; set; }
        public List<Vector3d> trails { get; set; }

        public StigmergyData(float view_Angle, float cohesion_Magnitude, float cohesion_Range, float separation_Magnitude, float separation_Range, List<Vector3d> flattenedTrails = null)
        {
            this.viewAngle = view_Angle;
            this.cohesionMagnitude = cohesion_Magnitude;
            this.cohesionRange = cohesion_Range;
            this.separationMagnitude = separation_Magnitude;
            this.separationRange = separation_Range;
            this.trails = flattenedTrails;
        }

    }
}

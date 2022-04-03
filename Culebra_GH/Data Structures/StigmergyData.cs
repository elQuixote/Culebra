using Rhino.Geometry;
using System.Collections.Generic;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Stigmergy Data Structure
    /// </summary>
    public struct StigmergyData
    {
        public float ViewAngle { get; set; }
        public float CohesionMagnitude { get; set; }
        public float CohesionRange { get; set; }
        public float SeparationMagnitude { get; set; }
        public float SeparationRange { get; set; }

        public StigmergyData(float view_Angle, float cohesion_Magnitude, float cohesion_Range, float separation_Magnitude, float separation_Range)
        {
            this.ViewAngle = view_Angle;
            this.CohesionMagnitude = cohesion_Magnitude;
            this.CohesionRange = cohesion_Range;
            this.SeparationMagnitude = separation_Magnitude;
            this.SeparationRange = separation_Range;
        }
    }
}

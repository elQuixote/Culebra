using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Culebra_GH.Data_Structures
{
    public struct FlockingData
    {
        public bool network { get; set; }
        public double alignment_Value { get; set; }
        public double separation_Value { get; set; }
        public double cohesion_Value { get; set; }
        public double searchRadius { get; set; }
        public double viewAngle { get; set; }

        public FlockingData(double alignmentValue, double separationValue, double cohesionValue, double searchRadius, double viewAngle, bool drawSearchConnectivity)
        {
            this.alignment_Value = alignmentValue;
            this.separation_Value = separationValue;
            this.cohesion_Value = cohesionValue;
            this.network = drawSearchConnectivity;
            this.searchRadius = searchRadius;
            this.viewAngle = viewAngle;
        }
    }
}

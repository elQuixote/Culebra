namespace Culebra_GH.Data_Structures
{
    public struct FlockingData
    {
        public bool network { get; set; }
        public float alignment_Value { get; set; }
        public float separation_Value { get; set; }
        public float cohesion_Value { get; set; }
        public float searchRadius { get; set; }
        public float viewAngle { get; set; }

        public FlockingData(float alignmentValue, float separationValue, float cohesionValue, float searchRadius, float viewAngle = 60.0f, bool drawSearchConnectivity = false)
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

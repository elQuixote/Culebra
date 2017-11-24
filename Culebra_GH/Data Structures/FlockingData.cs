using Rhino.Geometry;

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
        
        public Mesh colorMesh { get; set; }
        public bool mapAlignment { get; set; }
        public bool mapSeparation { get; set; }
        public bool mapCohesion { get; set; }

        public FlockingData(float alignmentValue, float separationValue, float cohesionValue, float searchRadius, float viewAngle = 60.0f, bool drawSearchConnectivity = false,
            Mesh color_Mesh = null, bool map_Alignment = false, bool map_Separation = false, bool map_Cohesion = false)
        {
            this.alignment_Value = alignmentValue;
            this.separation_Value = separationValue;
            this.cohesion_Value = cohesionValue;
            this.network = drawSearchConnectivity;
            this.searchRadius = searchRadius;
            this.viewAngle = viewAngle;

            this.colorMesh = color_Mesh;
            this.mapAlignment = map_Alignment;
            this.mapSeparation = map_Separation;
            this.mapCohesion = map_Cohesion;
        }
    }
}

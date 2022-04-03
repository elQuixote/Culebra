using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Flocking Data Structure
    /// </summary>
    public struct FlockingData
    {
        public bool Network { get; set; }
        public float Alignment_Value { get; set; }
        public float Separation_Value { get; set; }
        public float Cohesion_Value { get; set; }
        public float SearchRadius { get; set; }
        public float ViewAngle { get; set; }
        
        public Mesh ColorMesh { get; set; }
        public bool MapAlignment { get; set; }
        public bool MapSeparation { get; set; }
        public bool MapCohesion { get; set; }

        public FlockingData(float alignmentValue, float separationValue, float cohesionValue, float searchRadius, float viewAngle = 60.0f, bool drawSearchConnectivity = false,
            Mesh color_Mesh = null, bool map_Alignment = false, bool map_Separation = false, bool map_Cohesion = false)
        {
            this.Alignment_Value = alignmentValue;
            this.Separation_Value = separationValue;
            this.Cohesion_Value = cohesionValue;
            this.Network = drawSearchConnectivity;
            this.SearchRadius = searchRadius;
            this.ViewAngle = viewAngle;

            this.ColorMesh = color_Mesh;
            this.MapAlignment = map_Alignment;
            this.MapSeparation = map_Separation;
            this.MapCohesion = map_Cohesion;
        }
    }
}

using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Noise Data Structure
    /// </summary>
    public struct NoiseData
    {
        public float Scale { get; set; }
        public float Strength { get; set; }
        public float Multiplier { get; set; }
        public float Velocity { get; set; }

        public Mesh ColorMesh { get; set; }
        public bool MapScale { get; set; }
        public bool MapStrength { get; set; }
        public bool MapMultiplier { get; set; }

        public NoiseData(float scale, float strength, float multiplier, float velocity, Mesh color_Mesh = null, bool map_Scale = false, bool map_Strength = false, bool map_Multiplier = false)
        {
            this.Scale = scale;
            this.Strength = strength;
            this.Multiplier = multiplier;
            this.Velocity = velocity;

            this.ColorMesh = color_Mesh;
            this.MapScale = map_Scale;
            this.MapStrength = map_Strength;
            this.MapMultiplier = map_Multiplier;
        }
    }
}

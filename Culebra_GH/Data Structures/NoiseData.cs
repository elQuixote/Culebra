using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    public struct NoiseData
    {
        public float scale { get; set; }
        public float strength { get; set; }
        public float multiplier { get; set; }
        public float velocity { get; set; }

        public Mesh colorMesh { get; set; }
        public bool mapScale { get; set; }
        public bool mapStrength { get; set; }
        public bool mapMultiplier { get; set; }

        public NoiseData(float scale, float strength, float multiplier, float velocity, Mesh color_Mesh = null, bool map_Scale = false, bool map_Strength = false, bool map_Multiplier = false)
        {
            this.scale = scale;
            this.strength = strength;
            this.multiplier = multiplier;
            this.velocity = velocity;

            this.colorMesh = color_Mesh;
            this.mapScale = map_Scale;
            this.mapStrength = map_Strength;
            this.mapMultiplier = map_Multiplier;
        }
    }
}

namespace Culebra_GH.Data_Structures
{
    public struct NoiseData
    {
        public float scale { get; set; }
        public float strength { get; set; }
        public float multiplier { get; set; }
        public float velocity { get; set; }

        public NoiseData(float scale, float strength, float multiplier, float velocity)
        {
            this.scale = scale;
            this.strength = strength;
            this.multiplier = multiplier;
            this.velocity = velocity;
        }
    }
}

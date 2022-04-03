using Rhino.Geometry;
using System.Drawing;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Graphics Visual Data Structure
    /// </summary>
    public struct ColorData
    {
        public string ColorDataType { get; set; }
        public string ParticleTexture { get; set; }
        public Interval RedChannel { get; set; }
        public Interval GreenChannel { get; set; }
        public Interval BlueChannel { get; set; }
        public int Alpha { get; set; }
        public int MinThickness { get; set; }
        public int MaxThickness { get; set; }
        public bool Dotted { get; set; }
        public Color Color { get; set; }

        public ColorData(string particle_Texture, int alpha_Channel, Interval red_Channel, Interval green_Channel, Interval blue_Channel, int min_Thickness, int max_Thickness, bool dotted = false, Color color = new System.Drawing.Color(), string colorDataType = "")
        {
            this.ParticleTexture = particle_Texture;
            this.Alpha = alpha_Channel;
            this.RedChannel = red_Channel;
            this.GreenChannel = green_Channel;
            this.BlueChannel = blue_Channel;
            this.MinThickness = min_Thickness;
            this.MaxThickness = max_Thickness;
            this.Dotted = dotted;
            this.Color = color;
            this.ColorDataType = colorDataType;
        }
    }
}

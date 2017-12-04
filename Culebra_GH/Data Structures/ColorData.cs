using Rhino.Geometry;
using System.Drawing;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Graphics Visual Data Structure
    /// </summary>
    public struct ColorData
    {
        public string colorDataType { get; set; }
        public string particleTexture { get; set; }
        public Interval redChannel { get; set; }
        public Interval greenChannel { get; set; }
        public Interval blueChannel { get; set; }
        public int minThickness { get; set; }
        public int maxThickness { get; set; }
        public bool dotted { get; set; }
        public Color color { get; set; }

        public ColorData(string particle_Texture, Interval red_Channel, Interval green_Channel, Interval blue_Channel, int min_Thickness, int max_Thickness, bool dotted = false, Color color = new System.Drawing.Color(), string colorDataType = "")
        {
            this.particleTexture = particle_Texture;
            this.redChannel = red_Channel;
            this.greenChannel = green_Channel;
            this.blueChannel = blue_Channel;
            this.minThickness = min_Thickness;
            this.maxThickness = max_Thickness;
            this.dotted = dotted;
            this.color = color;
            this.colorDataType = colorDataType;
        }
    }
}

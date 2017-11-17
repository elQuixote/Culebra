using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Culebra_GH.Data_Structures
{
    public struct ColorData
    {
        public string particleTexture { get; set; }
        public Interval redChannel { get; set; }
        public Interval greenChannel { get; set; }
        public Interval blueChannel { get; set; }
        public int minThickness { get; set; }
        public int maxThickness { get; set; }

        public ColorData(string particle_Texture, Interval red_Channel, Interval green_Channel, Interval blue_Channel, int min_Thickness, int max_Thickness)
        {
            this.particleTexture = particle_Texture;
            this.redChannel = red_Channel;
            this.greenChannel = green_Channel;
            this.blueChannel = blue_Channel;
            this.minThickness = min_Thickness;
            this.maxThickness = max_Thickness;
        }
    }
}

using System.Collections.Generic;
using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    public struct BundlingData
    {
        public Mesh colorMesh { get; set; }
        public bool useColor { get; set; }
        public double threshold { get; set; }
        public double ratio { get; set; }
        public bool rebuild { get; set; }
        public int pointCount { get; set; }
        public int weldCount { get; set; }

        public BundlingData(double threshold, double ratio, bool rebuild, int point_Count, int weld_Count, Mesh color_Mesh = null, bool use_Color = false)
        {
            this.colorMesh = color_Mesh;
            this.useColor = use_Color;
            this.threshold = threshold;
            this.ratio = ratio;
            this.rebuild = rebuild;
            this.pointCount = point_Count;
            this.weldCount = weld_Count;
        }
    }
}

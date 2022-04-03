using System.Collections.Generic;
using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Bundling Data Structure
    /// </summary>
    public struct BundlingData
    {
        public Mesh ColorMesh { get; set; }
        public bool UseColor { get; set; }
        public double Threshold { get; set; }
        public double Ratio { get; set; }
        public bool Rebuild { get; set; }
        public int PointCount { get; set; }
        public int WeldCount { get; set; }

        public BundlingData(double threshold, double ratio, bool rebuild, int point_Count, int weld_Count, Mesh color_Mesh = null, bool use_Color = false)
        {
            this.ColorMesh = color_Mesh;
            this.UseColor = use_Color;
            this.Threshold = threshold;
            this.Ratio = ratio;
            this.Rebuild = rebuild;
            this.PointCount = point_Count;
            this.WeldCount = weld_Count;
        }
    }
}

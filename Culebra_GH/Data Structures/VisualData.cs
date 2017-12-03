namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// VisualData Structure
    /// </summary>
    public struct VisualData
    {
        public TrailData trailData { get; set; }
        public ColorData colorData { get; set; }
        public int displayMode { get; set; }
        public bool useTexture { get; set; }

        public VisualData(TrailData trail_Data, ColorData color_Data, int display_Mode, bool applyTexture = false)
        {
            this.trailData = trail_Data;
            this.colorData = color_Data;
            this.displayMode = display_Mode;
            this.useTexture = applyTexture;
        }
    }
}

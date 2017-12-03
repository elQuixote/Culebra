namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Trail Data Structure
    /// </summary>
    public struct TrailData
    {
        public bool createTrail { get; set; }
        public int trailStep { get; set; }
        public int maxTrailSize { get; set; }

        public TrailData(bool create_Trail, int trail_Step, int trail_MaxSize)
        {
            this.createTrail = create_Trail;
            this.trailStep = trail_Step;
            this.maxTrailSize = trail_MaxSize;
        }
    }
}

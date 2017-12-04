namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Separation Data Structure
    /// </summary>
    public struct SeparationData
    {
        public float maxSeparation { get; set; }

        public SeparationData(float max_Separation)
        {
            this.maxSeparation = max_Separation;
        }
    }
}

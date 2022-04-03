namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Separation Data Structure
    /// </summary>
    public struct SeparationData
    {
        public float MaxSeparation { get; set; }

        public SeparationData(float max_Separation)
        {
            this.MaxSeparation = max_Separation;
        }
    }
}

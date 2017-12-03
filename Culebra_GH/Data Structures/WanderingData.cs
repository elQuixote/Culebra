using Rhino.Geometry;

namespace Culebra_GH.Data_Structures
{
    /// <summary>
    /// Wandering Data Structure
    /// </summary>
    public struct WanderingData
    {
        public bool randomize { get; set; }
        public bool addHeading { get; set; }
        public float change { get; set; }
        public float wanderingRadius { get; set; }
        public float wanderingDistance { get; set; }
        public float rotationTrigger { get; set; }
        public string wanderingType { get; set; }

        public Mesh colorMesh { get; set; }
        public bool mapChange { get; set; }
        public bool mapRadius { get; set; }
        public bool mapDistance { get; set; }

        public WanderingData(float change, float wandering_Radius, float wandering_Distance, float rotationTrigger = 6.0f, bool randomize = true, bool add_Heading = true, 
            string wandering_Type = "", Mesh mesh = null, bool map_Change = false, bool map_Radius = false, bool map_Distance = false)
        {
            this.change = change;
            this.wanderingRadius = wandering_Radius;
            this.wanderingDistance = wandering_Distance;
            this.rotationTrigger = rotationTrigger;
            this.randomize = randomize;
            this.addHeading = add_Heading;
            this.wanderingType = wandering_Type;

            this.colorMesh = mesh;
            this.mapChange = map_Change;
            this.mapRadius = map_Radius;
            this.mapDistance = map_Distance;
        }
    }
}

namespace Culebra_GH.Data_Structures
{
    public struct WanderingData
    {
        public bool randomize { get; set; }
        public bool addHeading { get; set; }
        public float change { get; set; }
        public float wanderingRadius { get; set; }
        public float wanderingDistance { get; set; }
        public float rotationTrigger { get; set; }
        public string wanderingType { get; set; }

        public WanderingData(float change, float wandering_Radius, float wandering_Distance, float rotationTrigger = 6.0f, bool randomize = true, bool add_Heading = true, string wandering_Type = "")
        {
            this.change = change;
            this.wanderingRadius = wandering_Radius;
            this.wanderingDistance = wandering_Distance;
            this.rotationTrigger = rotationTrigger;
            this.randomize = randomize;
            this.addHeading = add_Heading;
            this.wanderingType = wandering_Type;
        }

    }
}

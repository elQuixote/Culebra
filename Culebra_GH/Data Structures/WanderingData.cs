using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Culebra_GH.Data_Structures
{
    public struct WanderingData
    {
        public bool randomize { get; set; }
        public bool addHeading { get; set; }
        public double change { get; set; }
        public double wanderingRadius { get; set; }
        public double wanderingDistance { get; set; }
        public double rotationTrigger { get; set; }

        public WanderingData(double change, double wandering_Radius, double wandering_Distance, double rotationTrigger, bool randomize, bool add_Heading)
        {
            this.change = change;
            this.wanderingRadius = wandering_Radius;
            this.wanderingDistance = wandering_Distance;
            this.rotationTrigger = rotationTrigger;
            this.randomize = randomize;
            this.addHeading = add_Heading;
        }

    }
}

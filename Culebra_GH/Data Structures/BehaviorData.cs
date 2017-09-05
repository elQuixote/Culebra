using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Culebra_GH.Data_Structures
{
    public struct BehaviorData
    {

        public FlockingData flockData { get; set; }
        public WanderingData wanderData { get; set; }

        public BehaviorData(FlockingData flock_Data, WanderingData wander_Data)
        {
            this.flockData = flock_Data;
            this.wanderData = wander_Data;
        }
    }
}

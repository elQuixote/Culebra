using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Culebra_GH.Data_Structures
{
    public struct SeparationData
    {
        public float maxSeparation { get; set; }

        public SeparationData(float max_Separation)
        {
            this.maxSeparation = max_Separation;
        }
    }
}

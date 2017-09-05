using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Culebra_GH.Data_Structures;
using Grasshopper;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Geometry;

namespace Culebra_GH.Objects
{
    public class IGH_BehaviorData : GH_Goo<BehaviorData>
    {
        private IGH_BehaviorData iGH_behData;

        public IGH_BehaviorData(BehaviorData behData)
        {
            this.Value = behData;
        }
        public IGH_BehaviorData(IGH_BehaviorData iGH_behData)
        {
            this.iGH_behData = iGH_behData;
        }
        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
        public override string TypeDescription
        {
            get
            {
                return "IGH Version of BehaviorData Object";
            }
        }

        public override string TypeName
        {
            get
            {
                return "BehaviorData";
            }
        }
        public override IGH_Goo Duplicate()
        {
            return new IGH_BehaviorData(this);
        }

        public override string ToString()
        {
            return this.Value.GetType().Name.ToString();
        }
    }
}

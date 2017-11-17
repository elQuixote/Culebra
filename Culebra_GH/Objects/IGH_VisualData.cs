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
    public class IGH_VisualData : GH_Goo<VisualData>
    {
        private IGH_VisualData iGH_vizData;

        public IGH_VisualData(VisualData vizData)
        {
            this.Value = vizData;
        }
        public IGH_VisualData(IGH_VisualData iGH_vizData)
        {
            this.iGH_vizData = iGH_vizData;
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
                return "IGH Version of VisualData Object";
            }
        }

        public override string TypeName
        {
            get
            {
                return "VisualData";
            }
        }
        public override IGH_Goo Duplicate()
        {
            return new IGH_VisualData(this);
        }

        public override string ToString()
        {
            return this.Value.GetType().Name.ToString();
        }
    }
}

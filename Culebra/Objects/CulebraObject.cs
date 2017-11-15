using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CulebraData.Objects
{
    public abstract class CulebraObject
    {
        public CulebraData.Behavior.Controller behaviors;
        public CulebraData.Attributes.Attributes attributes;
        public CulebraData.Operations.Actions actions;

        protected abstract internal culebra.objects.Object GetObject();
    }
}

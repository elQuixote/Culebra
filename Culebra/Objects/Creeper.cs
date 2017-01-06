using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using ikvm;
using processing.core;
using culebra.behaviors;
using Rhino;
using Rhino.Geometry;
using CulebraData.Utilities;

namespace CulebraData
{
    namespace Objects
    {
        public class Creeper
        {
            private culebra.objects.Creeper creeperObject;
            public CulebraData.Behavior.Controller behaviors;
            public CulebraData.Attributes.Attributes attributes;
            public CulebraData.Operations.Actions actions;
            
            public Creeper(Vector3d loc, Vector3d speed, bool instanceable, bool In3D)
            {
                this.behaviors = new CulebraData.Behavior.Controller(this);
                this.attributes = new CulebraData.Attributes.Attributes(this);
                this.actions = new CulebraData.Operations.Actions(this);

                creeperObject = new culebra.objects.Creeper(Utility.toPVec(loc), Utility.toPVec(speed), Utility.toJavaBool(instanceable), Utility.toJavaBool(In3D), Utility.toPApplet());
            }
            protected internal culebra.objects.Creeper getCreeperObject()
            {
                return this.creeperObject;
            }

        }
    }
}

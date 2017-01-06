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

namespace CulebraData.Objects
{
    public class BabyCreeper : Creeper
    {
        private culebra.objects.BabyCreeper babycreeperObject;

        public BabyCreeper(Vector3d loc, Vector3d speed, bool instanceable, string babyType, bool In3D) : base(loc, speed, instanceable, In3D)
        {
            this.behaviors = new CulebraData.Behavior.Controller(this);
            this.attributes = new CulebraData.Attributes.Attributes(this);
            this.actions = new CulebraData.Operations.Actions(this);

            babycreeperObject = new culebra.objects.BabyCreeper(Utility.toPVec(loc), Utility.toPVec(speed), Utility.toJavaBool(instanceable), babyType, Utility.toJavaBool(In3D), Utility.toPApplet());
        }
        protected internal culebra.objects.BabyCreeper getBabyCreeperObject()
        {
            return this.babycreeperObject;
        }
        /*
        new protected internal culebra.objects.BabyCreeper getCreeperObject()
        {
            return this.babycreeperObject;
        }
        */

    }
}

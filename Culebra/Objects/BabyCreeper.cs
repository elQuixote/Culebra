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
    /// <summary>
    /// Baby Creeper class which inherits from the Creeper Class - Object is meant to be used as a child of the Creeper Object 
    /// </summary>
    public class BabyCreeper : Creeper
    {
        private String babyType = null;
        private culebra.objects.BabyCreeper babycreeperObject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">the location of the creeper object</param>
        /// <param name="speed">speed of the creeper object</param>
        /// <param name="instanceable">is the creeper instanceable</param>
        /// <param name="babyType">specifies which type of baby is created. Use either "a" or "b"</param>
        /// <param name="In3D">specifies if we are in 2D or 3D</param>
        public BabyCreeper(Vector3d location, Vector3d speed, bool instanceable, string babyType, bool In3D) : base(location, speed, instanceable, In3D)
        {
            this.behaviors = new CulebraData.Behavior.Controller(this);
            this.attributes = new CulebraData.Attributes.Attributes(this);
            this.actions = new CulebraData.Operations.Actions(this);

            babycreeperObject = new culebra.objects.BabyCreeper(Utilities.Convert.ToPVec(location), Utilities.Convert.ToPVec(speed), Utilities.Convert.ToJavaBool(instanceable), babyType, Utilities.Convert.ToJavaBool(In3D), Utilities.Convert.ToPApplet());
        }
        /// <summary>
        /// Getter Method for retrieving the culebra java baby creeper object
        /// </summary>
        /// <returns>the culebra java baby creeper object</returns>
        protected internal culebra.objects.BabyCreeper GetBabyCreeperObject()
        {
            return this.babycreeperObject;
        }
    }
}

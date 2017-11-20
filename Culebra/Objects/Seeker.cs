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
using culebra.objects;

namespace CulebraData.Objects
{
    /// <summary>
    /// </summary>
    public class Seeker : CulebraObject
    {
        private culebra.objects.Seeker seekerObject;
        //public CulebraData.Behavior.Controller behaviors;
        //public CulebraData.Attributes.Attributes attributes;
        //public CulebraData.Operations.Actions actions;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">the location of the creeper object</param>
        /// <param name="speed">speed of the creeper object</param>
        /// <param name="instanceable">is the seeker instanceable</param>
        /// <param name="In3D">specifies if we are in 2D or 3D</param> 
        public Seeker(Vector3d location, Vector3d speed, bool instanceable, bool In3D)
        {
            this.behaviors = new CulebraData.Behavior.Controller(this);
            this.attributes = new CulebraData.Attributes.Attributes(this);
            this.actions = new CulebraData.Operations.Actions(this);

            seekerObject = new culebra.objects.Seeker(Utilities.Convert.ToPVec(location), Utilities.Convert.ToPVec(speed), instanceable, Utilities.Convert.ToJavaBool(In3D), Utilities.Convert.ToPApplet());
        }
        /// <summary>
        /// Getter Method for retrieving the culebra java seeker object
        /// </summary>
        /// <returns>the culebra java seeker object</returns>
        protected internal override culebra.objects.Object GetObject()
        {
            return this.seekerObject;
        }
    }
}

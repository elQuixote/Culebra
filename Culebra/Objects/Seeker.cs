using Rhino.Geometry;

namespace CulebraData.Objects
{
    /// <summary>
    /// </summary>
    public class Seeker : CulebraObject
    {
        private culebra.objects.Seeker seekerObject;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">the location of the creeper object</param>
        /// <param name="speed">speed of the creeper object</param>
        /// <param name="instanceable">is the seeker instanceable</param>
        /// <param name="In3D">specifies if we are in 2D or 3D</param> 
        public Seeker(Vector3d location, Vector3d speed, bool instanceable, bool In3D)
        {
            this.behaviors = new Behavior.Controller(this);
            this.attributes = new Attributes.Attributes(this);
            this.actions = new Operations.Actions(this);

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

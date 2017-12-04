using Rhino.Geometry;

namespace CulebraData.Objects
{
    /// <summary>
    /// Seeker Objects are the technically also main implementation of the abstract Objects, like Creeper Objects, they poses the same capabilities. They are able to implement any type of behavior but are meant to be used as Objects which "Seek" other objects.They should be used in conjunction with behaviors.trailFollowing Methods. I have not restricted their behavior implementations at this stage but might do so in the future to make certain behaviors more Object type specific.
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

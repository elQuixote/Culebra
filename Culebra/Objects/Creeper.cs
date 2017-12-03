using Rhino.Geometry;

namespace CulebraData.Objects
{
    /// <summary>
    /// Creeper Objects are the main implementation of the abstract Objects. They are able to implement any type of behavior and are meant as a do all type of object.
    /// </summary>
    public class Creeper : CulebraObject
    {
        private culebra.objects.Creeper creeperObject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">the location of the creeper object</param>
        /// <param name="speed">speed of the creeper object</param>
        /// <param name="instanceable">is the creeper instanceable</param>
        /// <param name="In3D">specifies if we are in 2D or 3D</param> 
        public Creeper(Vector3d location, Vector3d speed, bool instanceable, bool In3D)
        {
            this.behaviors = new Behavior.Controller(this);
            this.attributes = new Attributes.Attributes(this);
            this.actions = new Operations.Actions(this);

            creeperObject = new culebra.objects.Creeper(Utilities.Convert.ToPVec(location), Utilities.Convert.ToPVec(speed), Utilities.Convert.ToJavaBool(instanceable), Utilities.Convert.ToJavaBool(In3D), Utilities.Convert.ToPApplet());
        }
        /// <summary>
        /// Getter Method for retrieving the culebra java creeper object
        /// </summary>
        /// <returns>the culebra java creeper object</returns>
        protected internal override culebra.objects.Object GetObject()
        {
            return this.creeperObject;
        }
    }    
}

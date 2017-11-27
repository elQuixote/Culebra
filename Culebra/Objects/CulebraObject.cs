namespace CulebraData.Objects
{
    /// <summary>
    /// CulebraObject Class - Abstract class of Object from which other objects can inherit from. This class defines the basic abstract methods required by any object attempting to interface with the system.
    /// </summary>
    public abstract class CulebraObject
    {
        /// <summary>
        /// Controller Instance
        /// </summary>
        public Behavior.Controller behaviors;
        /// <summary>
        /// Attributes Instance
        /// </summary>
        public Attributes.Attributes attributes;
        /// <summary>
        /// Actions Instance
        /// </summary>
        public Operations.Actions actions;
        /// <summary>
        /// Gets the culebra java object
        /// </summary>
        /// <returns></returns>
        protected abstract internal culebra.objects.Object GetObject();
    }
}

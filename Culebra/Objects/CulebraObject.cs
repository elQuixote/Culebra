namespace CulebraData.Objects
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CulebraObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Behavior.Controller behaviors;
        /// <summary>
        /// 
        /// </summary>
        public Attributes.Attributes attributes;
        /// <summary>
        /// 
        /// </summary>
        public Operations.Actions actions;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract internal culebra.objects.Object GetObject();
    }
}

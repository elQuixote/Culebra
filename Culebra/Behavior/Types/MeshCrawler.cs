using processing.core;
using Rhino.Geometry;
using System.Collections.Generic;
using toxi.geom;

namespace CulebraData.Behavior.Types
{
    /// <summary>
    /// The <see cref="Types"/> namespace contains all additions to the Culebra Objects Behavior Types
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }
    /// <summary>
    /// Mesh Crawler Class - Used with a Rhino Mesh to navigate along a mesh, navigation is not topology dependent
    /// </summary>
    public class MeshCrawler
    {
        private bool sepActive = false;
        private bool instanceable;
        private int instanceTriggerCount;
        private int maxChildren;
        private List<Vector3d> childSpawners;
        private List<int> childSpawnType;
        private PVector predict = new PVector();
        private PVector nextPosPrev = new PVector();
        private PVector newVec = new PVector();
        private PVector zero = new PVector();
        private Vec3D tvec = new Vec3D();
        private Vec3D cp = new Vec3D();
        private Vec3D delta = new Vec3D();
        /// <summary>
        /// Constructor
        /// </summary>
        public MeshCrawler()
        {
            this.instanceTriggerCount = 0;
            this.childSpawners = new List<Vector3d>();
            this.childSpawnType = new List<int>();
        }
        /// <summary>
        /// Mesh Walking allows agent to move along a mesh object
        /// </summary>
        /// <param name="mesh">the mesh object to crawl on</param>
        /// <param name="meshThreshold">min distance the object needs to be from mesh in order to move to it</param>
        /// <param name="location">the current object location</param>
        /// <param name="speed">the current objects speed</param>
        /// <param name="amplitude">the amount to project the current location along the current speed to get the predicted next location</param>
        /// <param name="multiplier">the multiplier value for the output vector</param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="instanceable">if the child is instanceable it can reproduce. Only objects which inherit from the culebra.objects.Object class are instanceable. Child objects cannot produce more children</param>
        /// <param name="maxChildren">the max number of children each agent can create</param>
        /// <param name="childSpawner">list of stored children to spawn next. use (current object).behaviors.getChildStartPositions() to get them</param>
        /// <param name="childSpawnType">list of values used to alter types of children. use (current object).behaviors.getChildSpawnType() to get it.</param>
        /// <returns>The vector to apply</returns>
        public Vector3d MeshWalk(Mesh mesh, float meshThreshold, PVector location, PVector speed, float amplitude, float multiplier, bool triggerBabies, bool instanceable, int maxChildren, List<Vector3d> childSpawner = null, List<int> childSpawnType = null)
        {
            this.maxChildren = maxChildren;
            this.instanceable = instanceable;
            this.childSpawners = childSpawner;
            this.childSpawnType = childSpawnType;

            PVector predict = speed.copy();
            predict.normalize();
            predict.mult(amplitude);

            PVector nextPosPrev = PVector.add(location, predict);
            Vec3D tvec = new Vec3D(nextPosPrev.x, nextPosPrev.y, nextPosPrev.z);
            MeshPoint mp = mesh.ClosestMeshPoint(Utilities.Convert.ToPoint3d(nextPosPrev), 0.0);

            Vec3D cp = new Vec3D((float)mp.Point.X, (float)mp.Point.Y, (float)mp.Point.Z);
            Vec3D delta = cp.sub(tvec);

            PVector newVec = new PVector(delta.x(), delta.y(), delta.z());
            if (newVec.magSq() < meshThreshold)
            {
                this.sepActive = true;
                if (triggerBabies && this.instanceable && this.instanceTriggerCount < this.maxChildren)
                {
                    childSpawners.Add(new Vector3d(location.x, location.y, location.z));
                    childSpawnType.Add(this.instanceTriggerCount);
                    this.instanceTriggerCount++;
                }
                PVector zero = new PVector(0, 0, 0);
                zero.mult(3);
                return Utilities.Convert.ToVector3d(zero);
            }
            newVec.normalize();
            newVec.mult(multiplier);
            return Utilities.Convert.ToVector3d(newVec);
        }
        /// <summary>
        /// Gets the child start positions if any
        /// </summary>
        /// <returns>The child start positions</returns>
        public List<Vector3d> GetChildStartPositions()
        {
            List<Vector3d> childList = this.childSpawners;
            this.childSpawners = new List<Vector3d>();
            return childList;
        }
        /// <summary>
        /// Gets the child spawn types if any
        /// </summary>
        /// <returns>The child spawn types</returns>
        public List<int> GetChildSpawnType()
        {
            List<int> returnedList = this.childSpawnType;
            this.childSpawnType = new List<int>();
            return returnedList;
        }
        /// <summary>
        /// Resets the child start position list
        /// </summary>
        public void ResetChildStartPositions()
        {
            this.childSpawners = new List<Vector3d>();
        }
        /// <summary>
        /// Resets the child spawn types list
        /// </summary>
        public void ResetChildSpawnType()
        {
            this.childSpawnType = new List<int>();
        }
        /// <summary>
        /// Checks if the separate feature is active
        /// </summary>
        /// <returns>If the separate is active</returns>
        public bool IsSeparateActive()
        {
            return this.sepActive;
        }
        /// <summary>
        /// Sets the separate feature inactive
        /// </summary>
        public void SetSeparateInactive()
        {
            this.sepActive = false;
        }
    }
}

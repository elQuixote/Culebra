using processing.core;
using Rhino.Geometry;
using System.Collections.Generic;
using toxi.geom;

namespace CulebraData.Geometry
{
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
        /// <param name="multiplier"></param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="instanceable"></param>
        /// <param name="maxChildren"></param>
        /// <param name="childSpawner"></param>
        /// <param name="childSpawnType"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public List<Vector3d> GetChildStartPositions()
        {
            List<Vector3d> childList = this.childSpawners;
            this.childSpawners = new List<Vector3d>();
            return childList;
        }
        /// <summary>
        /// Gets the child spawn types if any
        /// </summary>
        /// <returns></returns>
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
        /// <returns></returns>
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

using processing.core;
using Rhino.Geometry;
using System.Collections.Generic;
using toxi.geom;

namespace CulebraData.Geometry
{
    /// <summary>
    /// 
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
        /// 
        /// </summary>
        public MeshCrawler()
        {
            this.instanceTriggerCount = 0;
            this.childSpawners = new List<Vector3d>();
            this.childSpawnType = new List<int>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mesh"></param>
        /// <param name="meshThreshold"></param>
        /// <param name="location"></param>
        /// <param name="speed"></param>
        /// <param name="amplitude"></param>
        /// <param name="multiplier"></param>
        /// <param name="triggerBabies"></param>
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
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Vector3d> GetChildStartPositions()
        {
            List<Vector3d> childList = this.childSpawners;
            this.childSpawners = new List<Vector3d>();
            return childList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<int> GetChildSpawnType()
        {
            List<int> returnedList = this.childSpawnType;
            this.childSpawnType = new List<int>();
            return returnedList;
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetChildStartPositions()
        {
            this.childSpawners = new List<Vector3d>();
        }
        /// <summary>
        /// 
        /// </summary>
        public void ResetChildSpawnType()
        {
            this.childSpawnType = new List<int>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSeparateActive()
        {
            return this.sepActive;
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetSeparateInactive()
        {
            this.sepActive = false;
        }
    }
}

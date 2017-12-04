using System;
using Rhino.Geometry;
using CulebraData.Objects;

namespace CulebraData.Operations
{
    /// <summary>
    /// The <see cref="Operations"/> namespace contains all Culebra Objects Operations Classes
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// Actions Class - Used to access Creeper Object's Actions
    /// </summary>
    public class Actions
    {
        private CulebraObject culebraObject;
        private Random r;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The Culebra Object whose actions you want to access</param>
        public Actions(CulebraObject obj)
        {
            this.culebraObject = obj;
        }
        /// <summary>
        /// 2D bounce method for boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        public void Bounce(BoundingBox bbox)
        {
            if (Utilities.Convert.ToBoolean(this.culebraObject.GetObject().behavior.getD3()))
            {
                if (this.culebraObject.GetObject().getLocation().x >= (int)bbox.Max[0] || this.culebraObject.GetObject().getLocation().x <= (int)bbox.Min[0])
                {
                    this.culebraObject.GetObject().getSpeed().x = this.culebraObject.GetObject().getSpeed().x * -1;
                }
                if (this.culebraObject.GetObject().getLocation().y >= (int)bbox.Max[1] || this.culebraObject.GetObject().getLocation().y <= (int)bbox.Min[1])
                {
                    this.culebraObject.GetObject().getSpeed().y = this.culebraObject.GetObject().getSpeed().y * -1;
                }
                if (this.culebraObject.GetObject().getLocation().z >= (int)bbox.Max[2] || this.culebraObject.GetObject().getLocation().z <= (int)bbox.Min[2])
                {
                    this.culebraObject.GetObject().getSpeed().z = this.culebraObject.GetObject().getSpeed().z * -1;
                } 
            }
            else
            {
                if (this.culebraObject.GetObject().getLocation().x >= (int)bbox.Max[0] || this.culebraObject.GetObject().getLocation().x <= (int)bbox.Min[0])
                {
                    this.culebraObject.GetObject().getSpeed().x = this.culebraObject.GetObject().getSpeed().x * -1;
                }
                if (this.culebraObject.GetObject().getLocation().y >= (int)bbox.Max[1] || this.culebraObject.GetObject().getLocation().y <= (int)bbox.Min[1])
                {
                    this.culebraObject.GetObject().getSpeed().y = this.culebraObject.GetObject().getSpeed().y * -1;
                }   
            }
        }
        /// <summary>
        /// 3D bounce method for boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        public void Bounce3D(BoundingBox bbox)
        {
            if (this.culebraObject.GetObject().getLocation().x >= (int)bbox.Max[0] || this.culebraObject.GetObject().getLocation().x <= (int)bbox.Min[0])
            {
                this.culebraObject.GetObject().getSpeed().x = this.culebraObject.GetObject().getSpeed().x * -1;
            }
            if (this.culebraObject.GetObject().getLocation().y >= (int)bbox.Max[1] || this.culebraObject.GetObject().getLocation().y <= (int)bbox.Min[1])
            {
                this.culebraObject.GetObject().getSpeed().y = this.culebraObject.GetObject().getSpeed().y * -1;
            }
            if (this.culebraObject.GetObject().getLocation().z >= (int)bbox.Max[2] || this.culebraObject.GetObject().getLocation().z <= (int)bbox.Min[2])
            {
                this.culebraObject.GetObject().getSpeed().z = this.culebraObject.GetObject().getSpeed().z * -1;
            }   
        }
        /// <summary>
        /// 2D Respawn method for when objects reach the sketch or any defined boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        public void Respawn(BoundingBox bbox)
        {
            this.r = new Random();
            if (!bbox.Contains(Utilities.Convert.ToPoint3d(this.culebraObject.GetObject().getLocation())))
            {
                if (Utilities.Convert.ToBoolean(this.culebraObject.GetObject().behavior.getD3()))
                {
                    this.culebraObject.GetObject().behavior.setLoc(Utilities.Convert.ToPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), r.Next((int)bbox.Min[2], (int)bbox.Max[2]))));
                }
                else
                {
                    this.culebraObject.GetObject().behavior.setLoc(Utilities.Convert.ToPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), 0)));
                }
                this.culebraObject.GetObject().behavior.setCreeperTrails(new java.util.ArrayList());
                this.culebraObject.GetObject().behavior.resetConnections();
            }
        }
        /// <summary>
        /// 3D Respawn method for when object reached the sketch or any defined boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        /// <param name="spawnOnGround">do you want all the new respawns to spawn on the ground?</param>
        /// <param name="spawnRandomly">do you want all the new respawns to spawn randomly in space?</param>
        public void Respawn(BoundingBox bbox, bool spawnOnGround, bool spawnRandomly)
        {
            this.r = new Random();
            if (!bbox.Contains(Utilities.Convert.ToPoint3d(this.culebraObject.GetObject().getLocation())))
            {
                if (Utilities.Convert.ToBoolean(this.culebraObject.GetObject().behavior.getD3()))
                {
                    if (spawnRandomly)
                    {
                        this.culebraObject.GetObject().behavior.setLoc(Utilities.Convert.ToPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), r.Next((int)bbox.Min[2], (int)bbox.Max[2]))));
                    }
                    else if (spawnOnGround)
                    {
                        this.culebraObject.GetObject().behavior.setLoc(Utilities.Convert.ToPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), 0)));
                    }
                }
                else
                {
                    this.culebraObject.GetObject().behavior.setLoc(Utilities.Convert.ToPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), 0)));
                }
                this.culebraObject.GetObject().behavior.setCreeperTrails(new java.util.ArrayList());
                this.culebraObject.GetObject().behavior.resetConnections();
            }
        }
        /// <summary>
        /// Reverses the current objects speed
        /// </summary>
        public void ReverseSpeed()
        {
            this.culebraObject.GetObject().reverseSpeed();
        }
        /// <summary>
        /// Move method for moving the object.
        /// </summary>
        public void Move()
        {
            this.culebraObject.GetObject().move(0, 1000);
        }
        /// <summary>
        /// Overloaded move method for moving the object. This method allows for a minimum amount of steps to be taken before adding and storing a trail point. You can also specify the max amount of trail points stored. This will certainly help with performance over time. This method is also the best to use with the behaviors.selfTrailChasing methods
        /// </summary>
        /// <param name="minStepAmount">minimum amount of steps that must be taken before we store a trail PVector</param>
        /// <param name="maxPositions_Stored">maximum amount of allowable trail PVectors per object.</param>
        public void Move(int minStepAmount, int maxPositions_Stored)
        {
            this.culebraObject.GetObject().move(minStepAmount, maxPositions_Stored);         
        }
    }
}

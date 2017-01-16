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
using CulebraData.Objects;
namespace CulebraData.Operations
{
    public class Actions
    {
        private Creeper creeper;
        private Random r;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The creeper object</param>
        public Actions(Creeper obj)
        {
            this.creeper = obj;
        }
        /// <summary>
        /// 2D bounce method for boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        public void bounce(BoundingBox bbox)
        {
            if (Utilities.Convert.toBoolean(this.creeper.getCreeperObject().behavior.getD3()))
            {
                if (this.creeper.getCreeperObject().getLocation().x >= (int)bbox.Max[0] || this.creeper.getCreeperObject().getLocation().x <= (int)bbox.Min[0])
                {
                    this.creeper.getCreeperObject().getSpeed().x = this.creeper.getCreeperObject().getSpeed().x * -1;
                }
                if (this.creeper.getCreeperObject().getLocation().y >= (int)bbox.Max[1] || this.creeper.getCreeperObject().getLocation().y <= (int)bbox.Min[1])
                {
                    this.creeper.getCreeperObject().getSpeed().y = this.creeper.getCreeperObject().getSpeed().y * -1;
                }
                if (this.creeper.getCreeperObject().getLocation().z >= (int)bbox.Max[2] || this.creeper.getCreeperObject().getLocation().z <= (int)bbox.Min[2])
                {
                    this.creeper.getCreeperObject().getSpeed().z = this.creeper.getCreeperObject().getSpeed().z * -1;
                } 
            }
            else
            {
                if (this.creeper.getCreeperObject().getLocation().x >= (int)bbox.Max[0] || this.creeper.getCreeperObject().getLocation().x <= (int)bbox.Min[0])
                {
                    this.creeper.getCreeperObject().getSpeed().x = this.creeper.getCreeperObject().getSpeed().x * -1;
                }
                if (this.creeper.getCreeperObject().getLocation().y >= (int)bbox.Max[1] || this.creeper.getCreeperObject().getLocation().y <= (int)bbox.Min[1])
                {
                    this.creeper.getCreeperObject().getSpeed().y = this.creeper.getCreeperObject().getSpeed().y * -1;
                }   
            }
        }
        /// <summary>
        /// 3D bounce method for boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        public void bounce3D(BoundingBox bbox)
        {
            if (this.creeper.getCreeperObject().getLocation().x >= (int)bbox.Max[0] || this.creeper.getCreeperObject().getLocation().x <= (int)bbox.Min[0])
            {
                this.creeper.getCreeperObject().getSpeed().x = this.creeper.getCreeperObject().getSpeed().x * -1;
            }
            if (this.creeper.getCreeperObject().getLocation().y >= (int)bbox.Max[1] || this.creeper.getCreeperObject().getLocation().y <= (int)bbox.Min[1])
            {
                this.creeper.getCreeperObject().getSpeed().y = this.creeper.getCreeperObject().getSpeed().y * -1;
            }
            if (this.creeper.getCreeperObject().getLocation().z >= (int)bbox.Max[2] || this.creeper.getCreeperObject().getLocation().z <= (int)bbox.Min[2])
            {
                this.creeper.getCreeperObject().getSpeed().z = this.creeper.getCreeperObject().getSpeed().z * -1;
            }   
        }
        /// <summary>
        /// 2D Respawn method for when objects reach the sketch or any defined boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        public void respawn(BoundingBox bbox)
        {
            this.r = new Random();
            if (!bbox.Contains(Utilities.Convert.toPoint3d(this.creeper.getCreeperObject().getLocation())))
            {
                if (Utilities.Convert.toBoolean(this.creeper.getCreeperObject().behavior.getD3()))
                {
                    this.creeper.getCreeperObject().behavior.setLoc(Utilities.Convert.toPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), r.Next((int)bbox.Min[2], (int)bbox.Max[2]))));
                }
                else
                {
                    this.creeper.getCreeperObject().behavior.setLoc(Utilities.Convert.toPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), 0)));
                }
                this.creeper.getCreeperObject().behavior.setCreeperTrails(new java.util.ArrayList());
                this.creeper.getCreeperObject().behavior.resetConnections();
            }
        }
        /// <summary>
        /// 3D Respawn method for when object reached the sketch or any defined boundary
        /// </summary>
        /// <param name="bbox">the bounding box to use as container</param>
        /// <param name="spawnOnGround">do you want all the new respawns to spawn on the ground?</param>
        /// <param name="spawnRandomly">do you want all the new respawns to spawn randomly in space?</param>
        public void respawn(BoundingBox bbox, bool spawnOnGround, bool spawnRandomly)
        {
            this.r = new Random();
            if (!bbox.Contains(Utilities.Convert.toPoint3d(this.creeper.getCreeperObject().getLocation())))
            {
                if (Utilities.Convert.toBoolean(this.creeper.getCreeperObject().behavior.getD3()))
                {
                    if (spawnRandomly)
                    {
                        this.creeper.getCreeperObject().behavior.setLoc(Utilities.Convert.toPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), r.Next((int)bbox.Min[2], (int)bbox.Max[2]))));
                    }
                    else if (spawnOnGround)
                    {
                        this.creeper.getCreeperObject().behavior.setLoc(Utilities.Convert.toPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), 0)));
                    }
                }
                else
                {
                    this.creeper.getCreeperObject().behavior.setLoc(Utilities.Convert.toPVec(new Vector3d(r.Next((int)bbox.Min[0], (int)bbox.Max[0]), r.Next((int)bbox.Min[1], (int)bbox.Max[1]), 0)));
                }
                this.creeper.getCreeperObject().behavior.setCreeperTrails(new java.util.ArrayList());
                this.creeper.getCreeperObject().behavior.resetConnections();
            }
        }
        /// <summary>
        /// Reverses the current objects speed
        /// </summary>
        public void reverseSpeed()
        {
            this.creeper.getCreeperObject().reverseSpeed();
        }
        /// <summary>
        /// Move method for moving the object.
        /// </summary>
        public void move()
        {
            this.creeper.getCreeperObject().move();
        }
        /// <summary>
        /// Overloaded move method for moving the object. This method allows for a minimum amount of steps to be taken before adding and storing a trail point. You can also specify the max amount of trail points stored. This will certainly help with performance over time. This method is also the best to use with the behaviors.selfTrailChasing methods
        /// </summary>
        /// <param name="minStepAmount">minimum amount of steps that must be taken before we store a trail PVector</param>
        /// <param name="maxPositions_Stored">maximum amount of allowable trail PVectors per object.</param>
        public void move(int minStepAmount, int maxPositions_Stored)
        {
            this.creeper.getCreeperObject().move(minStepAmount, maxPositions_Stored);         
        }
    }
}

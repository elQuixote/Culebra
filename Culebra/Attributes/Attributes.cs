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

namespace CulebraData.Attributes
{
    /// <summary>
    /// Attributes Class - Used to access Creeper Object's Attributes
    /// </summary>
    public class Attributes
    {
        private Creeper creeper;
        private BabyCreeper babyCreeper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The Creeper Object</param>
        public Attributes(Creeper obj)
        {
            this.creeper = obj;
        }     
        /// <summary>
        /// Sets the move attributes for each object. This can be set up during the creation of the object in the sketch setup() method or it can be modified per object in the draw() method.
        /// </summary>
        /// <param name="maxSpeed">the maximum speed each object can have</param>
        /// <param name="maxForce">the maximum force each object can have</param>
        /// <param name="velocityMult">a velocity multiplier to increase the speed</param>
        public void SetMoveAttributes(float maxSpeed, float maxForce, float velocityMult)
        {
           this.creeper.GetCreeperObject().setMoveAttributes(maxSpeed, maxForce, velocityMult);         
        }
        /// <summary>
        /// Retrieves the objects trail PVectors and converts to List of Points
        /// </summary>
        /// <returns>the list of trail converted PVectors</returns>
        public List<Point3d> GetTrailPoints()
        {
            List<Point3d> transfer = Utilities.Convert.ToPointList(this.creeper.GetCreeperObject().getTrailPoints());    
            return transfer;
        }
        /// <summary>
        /// Retrieves the objects trail PVectors and converts to List of Vector3D
        /// </summary>
        /// <returns>the list of trail converted PVectors</returns>
        public List<Vector3d> GetTrailVectors()
        {
            List<Vector3d> transfer = Utilities.Convert.ToVec3DList(this.creeper.GetCreeperObject().getTrailPoints());
            return transfer;
        }
        /// <summary>
        /// Getter method for retrieving object location as Point3d
        /// </summary>
        /// <returns>the objects location puller from the controller</returns>
        public Point3d GetLocation()
        {
            Point3d loc = Utilities.Convert.ToPoint3d(this.creeper.GetCreeperObject().getLocation());         
            return loc;
        }
        /// <summary>
        /// Getter method for retrieving object location as Vector3D
        /// </summary>
        /// <returns>the objects location puller from the controller</returns>
        public Vector3d GetVecLocation()
        {
            Vector3d loc = Utilities.Convert.ToVector3d(this.creeper.GetCreeperObject().getLocation());
            return loc;
        }
        public void SetLocation(Vector3d newLocation)
        {
            this.creeper.GetCreeperObject().behavior.setLoc(Utilities.Convert.ToPVec(newLocation));
        }
        /// <summary>
        /// Getter for retrieving the object speed
        /// </summary>
        /// <returns>the objects speed puller from the controller</returns>
        public Vector3d GetSpeed()
        {
            Vector3d speed = Utilities.Convert.ToVector3d(this.creeper.GetCreeperObject().getSpeed());
            return speed;
        }
        /// <summary>
        /// Setter for objects speed
        /// </summary>
        /// <param name="newSpeed">the desired new speed</param>
        public void SetSpeed(Vector3d newSpeed)
        {
            this.creeper.GetCreeperObject().behavior.setSpeed(Utilities.Convert.ToPVec(newSpeed));
        }
        /// <summary>
        /// Getter for retrieving the objects behavior type
        /// </summary>
        /// <returns>the objects behavior type</returns>
        public string GetBehaviorType()
        {
            return this.creeper.GetCreeperObject().behavior.getBehaviorType();
        }
        /// <summary>
        /// Gets the superclass of the object
        /// </summary>
        /// <returns>the superclass as string</returns>
        public string GetSuperClass()
        {
            string superclass = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                this.creeper.GetCreeperObject().behavior.setSuperClass(this.babyCreeper.GetBabyCreeperObject().getSuperClass());
                superclass = this.babyCreeper.GetBabyCreeperObject().getSuperClass();
            }
            else
            {
                superclass = this.creeper.GetCreeperObject().getSuperClass();
            }
            return superclass;
        }
        /// <summary>
        /// Getter for retrieving the objectType uses the java getClass().getName() method
        /// </summary>
        /// <returns>the object type specified from java getClass().getName() method</returns>
        public string GetObjType()
        {
            string objType = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                this.creeper.GetCreeperObject().behavior.setObjType(this.babyCreeper.GetBabyCreeperObject().getObjectType());
                objType = this.babyCreeper.GetBabyCreeperObject().getObjectType();        
            }
            else
            {
                objType = this.creeper.GetCreeperObject().getObjectType();            
            }
            return objType;
        }
        /// <summary>
        /// Gets the type of baby
        /// </summary>
        /// <returns>the baby type as a string </returns>
        public string GetChildType()
        {
            string childType = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                childType = this.babyCreeper.GetBabyCreeperObject().getType();
            }
            return childType;
        }
        /// <summary>
        /// Gets the connectivity between each object and any objects within its search radius
        /// </summary>
        /// <returns>network data which visualizes the search radius between agents</returns>
        public List<Vector3d> GetNetwork()
        {
            List<Vector3d> networkList = new List<Vector3d>();
            if (this.creeper.GetCreeperObject().behavior.getDrawConnections() != null)
            {
                if (Utilities.Convert.ToBoolean(this.creeper.GetCreeperObject().behavior.getDrawConnections()))
                {
                    networkList = Utilities.Convert.ToVec3DList(this.creeper.GetCreeperObject().behavior.getNetworkData());
                    this.creeper.GetCreeperObject().behavior.setDrawConnections(Utilities.Convert.ToJavaBool(false));
                }
            }
            return networkList;
        }
    }
}

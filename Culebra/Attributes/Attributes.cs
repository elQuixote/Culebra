using System.Collections.Generic;
using Rhino.Geometry;
using CulebraData.Objects;

namespace CulebraData.Attributes
{
    /// <summary>
    /// The <see cref="Attributes"/> namespace contains all Culebra Objects Attributes
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// Attributes Class - Used to access Creeper Object's Attributes
    /// </summary>
    public class Attributes
    {
        private BabyCreeper babyCreeper;
        private CulebraObject culebraObject;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The Culebra Object whose attributes you want to access</param>
        public Attributes(CulebraObject obj)
        {
            this.culebraObject = obj;
        } 
        /// <summary>
        /// Sets the move attributes for each object. This can be set up during the creation of the object in the sketch setup() method or it can be modified per object in the draw() method.
        /// </summary>
        /// <param name="maxSpeed">the maximum speed each object can have</param>
        /// <param name="maxForce">the maximum force each object can have</param>
        /// <param name="velocityMult">a velocity multiplier to increase the speed</param>
        public void SetMoveAttributes(float maxSpeed, float maxForce, float velocityMult)
        {
            this.culebraObject.GetObject().setMoveAttributes(maxSpeed, maxForce, velocityMult);
        }
        /// <summary>
        /// Retrieves the objects trail PVectors and converts to List of Points
        /// </summary>
        /// <returns>the list of trail converted PVectors</returns>
        public List<Point3d> GetTrailPoints()
        {
            List<Point3d> transfer = Utilities.Convert.ToPointList(this.culebraObject.GetObject().getTrailPoints());
            return transfer;
        }
        /// <summary>
        /// Retrieves the objects trail PVectors and converts to List of Vector3D
        /// </summary>
        /// <returns>the list of trail converted PVectors</returns>
        public List<Vector3d> GetTrailVectors()
        {
            List<Vector3d> transfer = Utilities.Convert.ToVec3DList(this.culebraObject.GetObject().getTrailPoints());
            return transfer;
        }
        /// <summary>
        /// Getter method for retrieving object location as Point3d
        /// </summary>
        /// <returns>the objects location puller from the controller</returns>
        public Point3d GetLocation()
        {
            Point3d loc = Utilities.Convert.ToPoint3d(this.culebraObject.GetObject().getLocation());         
            return loc;
        }
        /// <summary>
        /// Getter method for retrieving object location as Vector3D
        /// </summary>
        /// <returns>the objects location puller from the controller</returns>
        public Vector3d GetVecLocation()
        {
            Vector3d loc = Utilities.Convert.ToVector3d(this.culebraObject.GetObject().getLocation());
            return loc;
        }
        /// <summary>
        /// Setter method for setting the new object location
        /// </summary>
        /// <param name="newLocation">Set the new location</param>
        public void SetLocation(Vector3d newLocation)
        {
            this.culebraObject.GetObject().behavior.setLoc(Utilities.Convert.ToPVec(newLocation));
        }
        /// <summary>
        /// Getter for retrieving the object speed
        /// </summary>
        /// <returns>the objects speed puller from the controller</returns>
        public Vector3d GetSpeed()
        {
            Vector3d speed = Utilities.Convert.ToVector3d(this.culebraObject.GetObject().getSpeed());
            return speed;
        }
        /// <summary>
        /// Setter for objects speed
        /// </summary>
        /// <param name="newSpeed">the desired new speed</param>
        public void SetSpeed(Vector3d newSpeed)
        {
            this.culebraObject.GetObject().behavior.setSpeed(Utilities.Convert.ToPVec(newSpeed));
        }
        /// <summary>
        /// Getter for retrieving the objects behavior type
        /// </summary>
        /// <returns>the objects behavior type</returns>
        public string GetBehaviorType()
        {
            return this.culebraObject.GetObject().behavior.getBehaviorType();
        }
        /// <summary>
        /// Gets the superclass of the object
        /// </summary>
        /// <returns>the superclass as string</returns>
        public string GetSuperClass()
        {
            string superclass = "";
            if (this.culebraObject is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.culebraObject;
                this.culebraObject.GetObject().behavior.setSuperClass(this.babyCreeper.GetBabyCreeperObject().getSuperClass());
                superclass = this.babyCreeper.GetBabyCreeperObject().getSuperClass();
            }
            else
            {
                superclass = this.culebraObject.GetObject().getSuperClass();
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
            if (this.culebraObject is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.culebraObject;
                this.culebraObject.GetObject().behavior.setObjType(this.babyCreeper.GetBabyCreeperObject().getObjectType());
                objType = this.babyCreeper.GetBabyCreeperObject().getObjectType();        
            }
            else
            {
                objType = this.culebraObject.GetObject().getObjectType();            
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
            if (this.culebraObject is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.culebraObject;
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
            if (this.culebraObject.GetObject().behavior.getDrawConnections() != null)
            {
                if (Utilities.Convert.ToBoolean(this.culebraObject.GetObject().behavior.getDrawConnections()))
                {
                    networkList = Utilities.Convert.ToVec3DList(this.culebraObject.GetObject().behavior.getNetworkData());
                    this.culebraObject.GetObject().behavior.setDrawConnections(Utilities.Convert.ToJavaBool(false));
                }
            }
            return networkList;
        }
    }
}

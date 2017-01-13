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
        public void setMoveAttributes(float maxSpeed, float maxForce, float velocityMult)
        {
           this.creeper.getCreeperObject().setMoveAttributes(maxSpeed, maxForce, velocityMult);         
        }
        /// <summary>
        /// Retrieves the objects trail PVectors and converts to List of Points
        /// </summary>
        /// <returns>the list of trail converted PVectors</returns>
        public List<Point3d> getTrailPoints()
        {
            List<Point3d> transfer = Utilities.Convert.toPointList(this.creeper.getCreeperObject().getTrailPoints());    
            return transfer;
        }
        /// <summary>
        /// Retrieves the objects trail PVectors and converts to List of Vector3D
        /// </summary>
        /// <returns>the list of trail converted PVectors</returns>
        public List<Vector3d> getTrailVectors()
        {
            List<Vector3d> transfer = Utilities.Convert.toVec3DList(this.creeper.getCreeperObject().getTrailPoints());
            return transfer;
        }
        /// <summary>
        /// Getter method for retrieving object location as Point3d
        /// </summary>
        /// <returns>the objects location puller from the controller</returns>
        public Point3d getLocation()
        {
            Point3d loc = Utilities.Convert.toPoint3d(this.creeper.getCreeperObject().getLocation());         
            return loc;
        }
        /// <summary>
        /// Getter method for retrieving object location as Vector3D
        /// </summary>
        /// <returns>the objects location puller from the controller</returns>
        public Vector3d getVecLocation()
        {
            Vector3d loc = Utilities.Convert.toVector3d(this.creeper.getCreeperObject().getLocation());
            return loc;
        }
        /// <summary>
        /// Getter for retrieving the object speed
        /// </summary>
        /// <returns>the objects speed puller from the controller</returns>
        public Vector3d getSpeed()
        {
            Vector3d speed = Utilities.Convert.toVector3d(this.creeper.getCreeperObject().getSpeed());
            return speed;
        }
        /// <summary>
        /// Gets the superclass of the object
        /// </summary>
        /// <returns>the superclass as string</returns>
        public string getSuperClass()
        {
            string superclass = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                this.creeper.getCreeperObject().behavior.setSuperClass(this.babyCreeper.getBabyCreeperObject().getSuperClass());
                superclass = this.babyCreeper.getBabyCreeperObject().getSuperClass();
            }
            else
            {
                superclass = this.creeper.getCreeperObject().getSuperClass();
            }
            return superclass;
        }
        /// <summary>
        /// Getter for retrieving the objectType uses the java getClass().getName() method
        /// </summary>
        /// <returns>the object type specified from java getClass().getName() method</returns>
        public string getObjType()
        {
            string objType = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                this.creeper.getCreeperObject().behavior.setObjType(this.babyCreeper.getBabyCreeperObject().getObjectType());
                objType = this.babyCreeper.getBabyCreeperObject().getObjectType();        
            }
            else
            {
                objType = this.creeper.getCreeperObject().getObjectType();            
            }
            return objType;
        }
        /// <summary>
        /// Gets the type of baby
        /// </summary>
        /// <returns>the baby type as a string </returns>
        public string getChildType()
        {
            string childType = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                childType = this.babyCreeper.getBabyCreeperObject().getType();
            }
            return childType;
        }
        /// <summary>
        /// Gets the connectivity between each object and any objects within its search radius
        /// </summary>
        /// <returns></returns>
        public List<Vector3d> getNetwork()
        {
            List<Vector3d> networkList = new List<Vector3d>();
            if (this.creeper.getCreeperObject().behavior.getDrawConnections() != null)
            {
                if (Utilities.Convert.toBoolean(this.creeper.getCreeperObject().behavior.getDrawConnections()))
                {
                    networkList = Utilities.Convert.toVec3DList(this.creeper.getCreeperObject().behavior.getNetworkData());
                    this.creeper.getCreeperObject().behavior.setDrawConnections(Utilities.Convert.toJavaBool(false));
                }
            }
            return networkList;
        }
    }
}

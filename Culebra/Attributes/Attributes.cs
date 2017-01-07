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

        public Attributes(Creeper obj)
        {
            this.creeper = obj;
        }     
        public void setMoveAttributes(float maxSpeed, float maxForce, float velocityMult)
        {
           this.creeper.getCreeperObject().setMoveAttributes(maxSpeed, maxForce, velocityMult);         
        }
        public List<Point3d> getTrailPoints()
        {
            List<Point3d> transfer = Utilities.Convert.toPointList(this.creeper.getCreeperObject().getTrailPoints());    
            return transfer;
        }
        public List<Vector3d> getTrailVectors()
        {
            List<Vector3d> transfer = Utilities.Convert.toVec3DList(this.creeper.getCreeperObject().getTrailPoints());
            return transfer;
        }
        public Point3d getLocation()
        {
            Point3d loc = Utilities.Convert.toPoint3d(this.creeper.getCreeperObject().getLocation());         
            return loc;
        }
        public Vector3d getVecLocation()
        {
            Vector3d loc = Utilities.Convert.toVector3d(this.creeper.getCreeperObject().getLocation());
            return loc;
        }
        public Vector3d getSpeed()
        {
            Vector3d speed = Utilities.Convert.toVector3d(this.creeper.getCreeperObject().getSpeed());
            return speed;
        }
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

    }
}

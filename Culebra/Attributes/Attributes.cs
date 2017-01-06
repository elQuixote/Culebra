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
            List<Point3d> transfer = Utility.toPointList(this.creeper.getCreeperObject().getTrailPoints());    
            return transfer;
        }
        public List<Vector3d> getTrailVectors()
        {
            List<Vector3d> transfer = Utility.toVec3DList(this.creeper.getCreeperObject().getTrailPoints());
            return transfer;
        }
        public Point3d getLocation()
        {
            Point3d loc = Utility.toPoint3d(this.creeper.getCreeperObject().getLocation());         
            return loc;
        }
        public string getSuperClass()
        {
            string superclass = "";
            if (this.creeper is BabyCreeper)
            {
                this.babyCreeper = (BabyCreeper)this.creeper;
                superclass = this.babyCreeper.getBabyCreeperObject().getSuperClass();
            }
            else
            {
                superclass = this.creeper.getCreeperObject().getSuperClass();
            }
            return superclass;
        }
    }
}

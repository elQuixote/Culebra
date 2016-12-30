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

namespace CulebraData
{
    namespace Objects
    {
        public class Creeper
        {
            private culebra.objects.Creeper creeperObject;
            
            public Creeper(Vector3d loc, Vector3d speed, bool instanceable, bool In3D)
            {
                creeperObject = new culebra.objects.Creeper(Utility.toPVec(loc), Utility.toPVec(speed), Utility.toJavaBool(instanceable), Utility.toJavaBool(In3D), Utility.toPApplet());
            }
            protected internal culebra.objects.Creeper getCreeperObject()
            {
                return this.creeperObject;
            }
            public void bounce(BoundingBox bbox)
            {
                if (creeperObject.getLocation().x >= (int)bbox.Max[0] || creeperObject.getLocation().x <= (int)bbox.Min[0])
                {
                    creeperObject.getSpeed().x = creeperObject.getSpeed().x * -1;
                }
                if (creeperObject.getLocation().y >= (int)bbox.Max[1] || creeperObject.getLocation().y <= (int)bbox.Min[1])
                {
                    creeperObject.getSpeed().y = creeperObject.getSpeed().y * -1;
                }
            }
            public void setMoveAtt(float maxSpeed, float maxForce, float velocityMult)
            {
                creeperObject.setMoveAttributes(maxSpeed, maxForce, velocityMult);
            }
            public void applyBehavior(float sr, float cv, float sv, float av, float va, List<Creeper> cl, bool connect)
            {
                creeperObject.behavior.flock2D(sr, cv, sv, av, va, Utility.toJavaList(cl), Utility.toJavaBool(connect));
            }
            public void applyWandBehavior()
            {
                creeperObject.behavior.wander2D(new java.lang.Boolean(true), new java.lang.Boolean(false), 2.0f, 80.0f, 26.0f);
            }
            public void applyMove()
            {
                creeperObject.move();
            }
            public List<Point3d> getTrails()
            {
                List<Point3d> transfer = Utility.toPointList(creeperObject.getTrailPoints());
                return transfer;
            }
            public Point3d getObjectLocation()
            {
                Point3d loc = Utility.toPoint3d(creeperObject.getLocation());
                return loc;
            }
            public string getSuperClass()
            {
                return creeperObject.getSuperClass();
            }

        }
    }
}

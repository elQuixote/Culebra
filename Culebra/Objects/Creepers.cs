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
    public class Creepers : culebra.objects.Creeper
    {

        public Creepers(Vector3d pv1, Vector3d pv2, bool b, bool c)
            : base(Utilities.Convert.toPVec(pv1), Utilities.Convert.toPVec(pv2), Utilities.Convert.toJavaBool(b), Utilities.Convert.toJavaBool(c), Utilities.Convert.toPApplet())
        {
        }

        public void bounce(BoundingBox bbox)
        {            
            if (this.getLocation().x >= (int)bbox.Max[0] || this.getLocation().x <= (int)bbox.Min[0])
            {
                this.getSpeed().x = this.getSpeed().x * -1;
            }
            if (this.getLocation().y >= (int)bbox.Max[1] || this.getLocation().y <= (int)bbox.Min[1])
            {
                this.getSpeed().y = this.getSpeed().y * -1;
            }
        }
        public List<Point3d> gh_getTrails()
        {

            List<Point3d> transfer = Utilities.Convert.toPointList(this.getTrailPoints());
            return transfer;
        }
        
        public Point3d gh_getObjectLocation()
        {
            Point3d loc = Utilities.Convert.toPoint3d(this.getLocation());
            return loc;
        }
    }
}

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

        public Creepers(PVector pv1, PVector pv2, java.lang.Boolean b, java.lang.Boolean c, PApplet app) : base(pv1,pv2,b,c,app)
        {
        }

        public void gh_Bounce(BoundingBox bbox)
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
            List<Point3d> transfer = Utility.toPointList(this.getTrailPoints());
            return transfer;
        }
        public Point3d gh_getObjectLocation()
        {
            Point3d loc = Utility.toPoint3d(this.getLocation());
            return loc;
        }
    }
}

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
using CulebraData.Objects;

namespace CulebraData
{
    namespace Utilities
    {
        public static class Utility
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="pvec"></param>
            /// <returns></returns>
            public static Vector3d toVector3d(PVector pvec)
            {
                Vector3d vector = new Vector3d(pvec.x, pvec.y, pvec.z);
                return vector;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="pvec"></param>
            /// <returns></returns>
            public static Point3d toPoint3d(PVector pvec)
            {
                Point3d newPt = new Point3d(pvec.x, pvec.y, pvec.z);
                return newPt;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="pt"></param>
            /// <returns></returns>
            public static PVector toPVec(Point3d pt)
            {
                PVector newPt = new PVector((float)pt.X, (float)pt.Y, (float)pt.Z);
                return newPt;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="vec"></param>
            /// <returns></returns>
            public static PVector toPVec(Vector3d vec)
            {
                PVector newVec = new PVector((float)vec.X, (float)vec.Y, (float)vec.Z);
                return newVec;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static java.lang.Boolean toJavaBool(bool value)
            {
                java.lang.Boolean boolean = new java.lang.Boolean(value);
                return boolean;
            }
            public static bool toBoolean(java.lang.Boolean value)
            {
                bool boolean = value.booleanValue();
                return boolean;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static PApplet toPApplet()
            {
                PApplet app = new PApplet();
                return app;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="genericList"></param>
            /// <returns></returns>
            public static java.util.List toJavaList(List<CulebraData.Objects.Creeper> genericList)
            {
                java.util.List javalist = new java.util.ArrayList();
                foreach (CulebraData.Objects.Creeper c in genericList)
                {
                    javalist.add(c.getCreeperObject());
                }
                return javalist;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="genericList"></param>
            /// <returns></returns>
            public static java.util.List toJavaList(List<Creepers> genericList)
            {
                java.util.List javalist = new java.util.ArrayList();
                foreach (Creepers c in genericList)
                {
                    javalist.add(c);
                }
                return javalist;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="arrayList"></param>
            /// <returns></returns>
            public static List<Point3d> toPointList(java.util.ArrayList arrayList)
            {
                List<Point3d> ptList = new List<Point3d>();
                foreach (PVector p in arrayList)
                {
                    ptList.Add(toPoint3d(p));
                }
                return ptList;
            }
        }
    }

}

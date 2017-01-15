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
using System.Drawing;


namespace CulebraData.Utilities
{
    public static class Convert
    {
        /// <summary>
        /// Converts PVector to Vector3D
        /// </summary>
        /// <param name="pvec">the PVector to convert</param>
        /// <returns>the converted PVector to Vector3d</returns>
        public static Vector3d toVector3d(PVector pvec)
        {
            Vector3d vector = new Vector3d(pvec.x, pvec.y, pvec.z);
            return vector;
        }
        /// <summary>
        /// Converts PVector to Point3D
        /// </summary>
        /// <param name="pvec">the PVector to convert</param>
        /// <returns>the converted PVector to Vector3d</returns>
        public static Point3d toPoint3d(PVector pvec)
        {
            Point3d newPt = new Point3d(pvec.x, pvec.y, pvec.z);
            return newPt;
        }
        /// <summary>
        /// Converts Point3D to PVector
        /// </summary>
        /// <param name="pt">the point to convert</param>
        /// <returns>the converted Point3d to PVector</returns>
        public static PVector toPVec(Point3d pt)
        {
            PVector newPt = new PVector((float)pt.X, (float)pt.Y, (float)pt.Z);
            return newPt;
        }
        /// <summary>
        /// Converts Vector3D to PVector
        /// </summary>
        /// <param name="vec">the PVector to convert</param>
        /// <returns>the converted Vector3d to PVector</returns>
        public static PVector toPVec(Vector3d vec)
        {
            PVector newVec = new PVector((float)vec.X, (float)vec.Y, (float)vec.Z);
            return newVec;
        }
        /// <summary>
        /// Converts System Boolean to java.lang.Boolean
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the converted boolean</returns>
        public static java.lang.Boolean toJavaBool(bool value)
        {
            java.lang.Boolean boolean = new java.lang.Boolean(value);
            return boolean;
        }
        /// <summary>
        /// Converts java.lang.Boolean to System.Boolean
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the converted boolean</returns>
        public static bool toBoolean(java.lang.Boolean value)
        {
            bool boolean = value.booleanValue();
            return boolean;
        }
        /// <summary>
        /// Returns an instance of processing.core.PApplet
        /// </summary>
        /// <returns>the PApplet</returns>
        public static PApplet toPApplet()
        {
            PApplet app = new PApplet();
            return app;
        }
        /// <summary>
        /// Converts List of Creeper objects to a java.util.List of java creepers 
        /// </summary>
        /// <param name="genericList">the list to convert</param>
        /// <returns>the java.util.List of java creepers</returns>
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
        /// Converts an arraylist of PVectors to a list of Point3D
        /// </summary>
        /// <param name="arrayList">the arraylist to convert</param>
        /// <returns>the list of Points</returns>
        public static List<Point3d> toPointList(java.util.ArrayList arrayList)
        {
            List<Point3d> ptList = new List<Point3d>();
            foreach (PVector p in arrayList)
            {
                ptList.Add(toPoint3d(p));
            }
            return ptList;
        }
        /// <summary>
        /// Converts an arraylist of PVectors to a list of Vector3D
        /// </summary>
        /// <param name="arrayList">the arraylist to convert</param>
        /// <returns>the list of Vectors</returns>
        public static List<Vector3d> toVec3DList(java.util.ArrayList arrayList)
        {
            List<Vector3d> vecList = new List<Vector3d>();
            foreach (PVector p in arrayList)
            {
                vecList.Add(toVector3d(p));
            }
            return vecList;
        }
        /// <summary>
        /// Converts a java.util.List of PVectors to a list of Vector3D
        /// </summary>
        /// <param name="arrayList">the java.util.List to convert</param>
        /// <returns>the list of Vectors</returns>
        public static List<Vector3d> toVec3DList(java.util.List arrayList)
        {
            List<Vector3d> vecList = new List<Vector3d>();
            for (int i = 0; i < arrayList.size(); i ++)
            //foreach (PVector p in arrayList)
            {
                vecList.Add(toVector3d((PVector)arrayList.get(i)));
            }
            return vecList;
        }
        /// <summary>
        /// Converts a list of Vector3D to an arraylist of PVectors
        /// </summary>
        /// <param name="vectorList">the list to convert</param>
        /// <returns>the arraylist of PVectors</returns>
        public static java.util.ArrayList toPVecList(List<Vector3d> vectorList)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();
            foreach (Vector3d v in vectorList)
            {
                javalist.add(toPVec(v));
            }
            return javalist;
        }
        /// <summary>
        /// Converts a list of Polylines to a list of shapes for Culebra Java Tracking Behaviors
        /// </summary>
        /// <param name="plineList">the list of polylines to convert</param>
        /// <returns>the arraylist of shapes</returns>
        public static java.util.ArrayList polylinesToMultiShapes(List<Polyline> plineList)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();           
            foreach (Polyline p in plineList)
            {
                java.util.ArrayList javalist2 = new java.util.ArrayList();
                for (int i = 0; i < p.SegmentCount + 1; i++)
                {
                    javalist2.add(toPVec(p.PointAt(i)));
                }
                javalist.add(javalist2);
            }
            return javalist;
        }
        /// <summary>
        /// Converts a single polyline to a shape for Culebra Java Tracking Behaviors
        /// </summary>
        /// <param name="pline">the polyline to convert</param>
        /// <returns>the arraylist of shapes</returns>
        public static java.util.ArrayList polylineToShape(Polyline pline)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();
            for (int i = 0; i < pline.SegmentCount + 1; i++)
            {
                javalist.add(toPVec(pline.PointAt(i)));
            }             
            return javalist;
        }
        /// <summary>
        /// Converts arraylist of java.lang.Integer to a list of integers
        /// </summary>
        /// <param name="javaIntList">arraylist to convert</param>
        /// <returns>the list of integers</returns>
        public static List<int> toIntList(java.util.ArrayList javaIntList)
        {
            List<int> intList = new List<int>();

            for (int i = 0; i < javaIntList.size(); i++)
            {
                int convertBabySequencer = 0;
                Int32.TryParse(javaIntList.get(i).ToString(), out convertBabySequencer);
                intList.Add(convertBabySequencer);
            }
            return intList;
        }
        /// <summary>
        /// Converts list of integer to arraylist of java integers
        /// </summary>
        /// <param name="intList">the list of integer to convert</param>
        /// <returns>the converted list of integers as arraylist of java integers</returns>
        public static java.util.ArrayList toJavaIntList(List<int> intList)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();
            foreach (int i in intList)
            {
                javalist.Add(toJavaInt(i));
            }
            return javalist;
        }
        /// <summary>
        /// Converts 32 bit integer to java.lang.Integer
        /// </summary>
        /// <param name="integer">the integer to convert</param>
        /// <returns>the java.lang.Integer</returns>
        public static java.lang.Integer toJavaInt(int integer)
        {
            java.lang.Integer javaInt = new java.lang.Integer(integer);
            return javaInt;
        }
        public static float map(float value, float istart, float istop, float ostart, float ostop) 
        {
		    return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	    }
        public static Color GetRandomColor(Random randomGen)
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }
    } 
}

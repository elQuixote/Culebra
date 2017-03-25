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
    /// <summary>
    /// Static Class to Convert Java & Processing Objects to .Net Geometry Objects
    /// </summary>
    public static class Convert
    {
        /// <summary>
        /// Converts PVector to Vector3D
        /// </summary>
        /// <param name="pvec">the PVector to convert</param>
        /// <returns>the converted PVector to Vector3d</returns>
        public static Vector3d ToVector3d(PVector pvec)
        {
            Vector3d vector = new Vector3d(pvec.x, pvec.y, pvec.z);
            return vector;
        }
        /// <summary>
        /// Converts PVector to Point3D
        /// </summary>
        /// <param name="pvec">the PVector to convert</param>
        /// <returns>the converted PVector to Vector3d</returns>
        public static Point3d ToPoint3d(PVector pvec)
        {
            Point3d newPt = new Point3d(pvec.x, pvec.y, pvec.z);
            return newPt;
        }
        /// <summary>
        /// Converts Point3D to PVector
        /// </summary>
        /// <param name="pt">the point to convert</param>
        /// <returns>the converted Point3d to PVector</returns>
        public static PVector ToPVec(Point3d pt)
        {
            PVector newPt = new PVector((float)pt.X, (float)pt.Y, (float)pt.Z);
            return newPt;
        }
        /// <summary>
        /// Converts Vector3D to PVector
        /// </summary>
        /// <param name="vec">the PVector to convert</param>
        /// <returns>the converted Vector3d to PVector</returns>
        public static PVector ToPVec(Vector3d vec)
        {
            PVector newVec = new PVector((float)vec.X, (float)vec.Y, (float)vec.Z);
            return newVec;
        }
        /// <summary>
        /// Converts System Boolean to java.lang.Boolean
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the converted boolean</returns>
        public static java.lang.Boolean ToJavaBool(bool value)
        {
            java.lang.Boolean boolean = new java.lang.Boolean(value);
            return boolean;
        }
        /// <summary>
        /// Converts java.lang.Boolean to System.Boolean
        /// </summary>
        /// <param name="value">the value</param>
        /// <returns>the converted boolean</returns>
        public static bool ToBoolean(java.lang.Boolean value)
        {
            bool boolean = value.booleanValue();
            return boolean;
        }
        /// <summary>
        /// Returns an instance of processing.core.PApplet
        /// </summary>
        /// <returns>the PApplet</returns>
        public static PApplet ToPApplet()
        {
            PApplet app = new PApplet();
            return app;
        }
        /// <summary>
        /// Converts List of Creeper objects to a java.util.List of java creepers 
        /// </summary>
        /// <param name="genericList">the list to convert</param>
        /// <returns>the java.util.List of java creepers</returns>
        public static java.util.List ToJavaList(List<CulebraData.Objects.Creeper> genericList)
        {
            java.util.List javalist = new java.util.ArrayList();
            foreach (CulebraData.Objects.Creeper c in genericList)
            {
                javalist.add(c.GetCreeperObject());
            }
            return javalist;
        }
        /// <summary>
        /// Converts an arraylist of PVectors to a list of Point3D
        /// </summary>
        /// <param name="arrayList">the arraylist to convert</param>
        /// <returns>the list of Points</returns>
        public static List<Point3d> ToPointList(java.util.ArrayList arrayList)
        {
            List<Point3d> ptList = new List<Point3d>();
            foreach (PVector p in arrayList)
            {
                ptList.Add(ToPoint3d(p));
            }
            return ptList;
        }
        /// <summary>
        /// Converts an arraylist of PVectors to a list of Vector3D
        /// </summary>
        /// <param name="arrayList">the arraylist to convert</param>
        /// <returns>the list of Vectors</returns>
        public static List<Vector3d> ToVec3DList(java.util.ArrayList arrayList)
        {
            List<Vector3d> vecList = new List<Vector3d>();
            foreach (PVector p in arrayList)
            {
                vecList.Add(ToVector3d(p));
            }
            return vecList;
        }
        /// <summary>
        /// Converts a java.util.List of PVectors to a list of Vector3D
        /// </summary>
        /// <param name="arrayList">the java.util.List to convert</param>
        /// <returns>the list of Vectors</returns>
        public static List<Vector3d> ToVec3DList(java.util.List arrayList)
        {
            List<Vector3d> vecList = new List<Vector3d>();
            for (int i = 0; i < arrayList.size(); i ++)
            //foreach (PVector p in arrayList)
            {
                vecList.Add(ToVector3d((PVector)arrayList.get(i)));
            }
            return vecList;
        }
        /// <summary>
        /// Converts a list of Vector3D to an arraylist of PVectors
        /// </summary>
        /// <param name="vectorList">the list to convert</param>
        /// <returns>the arraylist of PVectors</returns>
        public static java.util.ArrayList ToPVecList(List<Vector3d> vectorList)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();
            foreach (Vector3d v in vectorList)
            {
                javalist.add(ToPVec(v));
            }
            return javalist;
        }
        /// <summary>
        /// Converts a list of Polylines to a list of shapes for Culebra Java Tracking Behaviors
        /// </summary>
        /// <param name="plineList">the list of polylines to convert</param>
        /// <returns>the arraylist of shapes</returns>
        public static java.util.ArrayList PolylinesToMultiShapes(List<Polyline> plineList)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();           
            foreach (Polyline p in plineList)
            {
                java.util.ArrayList javalist2 = new java.util.ArrayList();
                for (int i = 0; i < p.SegmentCount + 1; i++)
                {
                    javalist2.add(ToPVec(p.PointAt(i)));
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
        public static java.util.ArrayList PolylineToShape(Polyline pline)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();
            for (int i = 0; i < pline.SegmentCount + 1; i++)
            {
                javalist.add(ToPVec(pline.PointAt(i)));
            }             
            return javalist;
        }
        /// <summary>
        /// Converts arraylist of java.lang.Integer to a list of integers
        /// </summary>
        /// <param name="javaIntList">arraylist to convert</param>
        /// <returns>the list of integers</returns>
        public static List<int> ToIntList(java.util.ArrayList javaIntList)
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
        public static java.util.ArrayList ToJavaIntList(List<int> intList)
        {
            java.util.ArrayList javalist = new java.util.ArrayList();
            foreach (int i in intList)
            {
                javalist.Add(ToJavaInt(i));
            }
            return javalist;
        }
        /// <summary>
        /// Converts 32 bit integer to java.lang.Integer
        /// </summary>
        /// <param name="integer">the integer to convert</param>
        /// <returns>the java.lang.Integer</returns>
        public static java.lang.Integer ToJavaInt(int integer)
        {
            java.lang.Integer javaInt = new java.lang.Integer(integer);
            return javaInt;
        }
        /// <summary>
        /// Remaps a value 
        /// </summary>
        /// <param name="value">value to map</param>
        /// <param name="istart">source min value</param>
        /// <param name="istop">source max value</param>
        /// <param name="ostart">target min value</param>
        /// <param name="ostop">target max value</param>
        /// <returns></returns>
        public static float Map(float value, float istart, float istop, float ostart, float ostop) 
        {
		    return ostart + (ostop - ostart) * ((value - istart) / (istop - istart));
	    }
        /// <summary>
        /// Reparametrizes Values
        /// </summary>
        /// <param name="dataToMap">data to map</param> 
        /// <param name="sourceMax">source max value</param> 
        /// <returns>the list of mapped values</returns> 
        public static List<double> RemapValues(List<double> dataToMap, double sourceMax)
        {
            double oldMin = 0.0;
            double NewMax = 1.0;
            double NewMin = 0.0;
            for (int i = 0; i < dataToMap.Count; i++)
            {
                double oldValue = dataToMap[i];
                double oldRange = (sourceMax - oldMin);
                double newRange = (NewMax - NewMin);
                dataToMap[i] = (((oldValue - oldMin) * newRange) / oldRange) + NewMin;
            }
            return dataToMap;
        }
        /// <summary>
        /// Generates a random color
        /// </summary>
        /// <param name="randomGen">the random object instance</param>
        /// <returns></returns>
        public static Color GetRandomColor(Random randomGen)
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }
    }
    /// <summary>
    /// Provides a set of color utilities for mesh editing
    /// </summary>
    public static class ColorUtility
    {
        /// <summary>
        /// Gets the HLS of a mesh at a specified location
        /// </summary>
        /// <param name="sp">Sample Point</param> 
        /// <param name="coloredMesh">colors mesh to sample</param> 
        /// <returns>The Hue Saturation and Luminance at location</returns> 
        public static Rhino.Display.ColorHSL GetHueSatLum(Point3d sp, Mesh coloredMesh)
        {
            MeshPoint mp = coloredMesh.ClosestMeshPoint(sp, 0);
            if (mp != null)
            {
                MeshFace face = coloredMesh.Faces[mp.FaceIndex];
                Color colorA = coloredMesh.VertexColors[face.A];
                Color colorB = coloredMesh.VertexColors[face.B];
                Color colorC = coloredMesh.VertexColors[face.C];

                double colorSampleA = colorA.A * mp.T[0] + colorB.A * mp.T[1] + colorC.A * mp.T[2];
                double colorSampleR = colorA.R * mp.T[0] + colorB.R * mp.T[1] + colorC.R * mp.T[2];
                double colorSampleG = colorA.G * mp.T[0] + colorB.G * mp.T[1] + colorC.G * mp.T[2];
                double colorSampleB = colorA.B * mp.T[0] + colorB.B * mp.T[1] + colorC.B * mp.T[2];

                int Alpha = (int)colorSampleA;
                int Red = (int)colorSampleR;
                int Green = (int)colorSampleG;
                int Blue = (int)colorSampleB;

                Color colour = Color.FromArgb(Alpha, Red, Green, Blue);

                Rhino.Display.ColorHSL HSL = new Rhino.Display.ColorHSL(colour);
                double H = HSL.H;
                double lum = HSL.L;
                double S = HSL.S;
                return HSL;
            }
            else
            {
                Rhino.Display.ColorHSL HSL = new Rhino.Display.ColorHSL();
                return HSL;
            }
        }
        /// <summary>
        /// Gets the color of a mesh at a specified point
        /// </summary>
        /// <param name="sp">The sample point</param> 
        /// <param name="coloredMesh">The colored mesh to sample</param> 
        /// <returns>The color at that location</returns> 
        public static Color GetColor(Point3d sp, Mesh coloredMesh)
        {
            coloredMesh.Faces.ConvertQuadsToTriangles();
            MeshPoint mp = coloredMesh.ClosestMeshPoint(sp, 0);
            if (mp != null)
            {
                MeshFace face = coloredMesh.Faces[mp.FaceIndex];
                Color colorA = coloredMesh.VertexColors[face.A];
                Color colorB = coloredMesh.VertexColors[face.B];
                Color colorC = coloredMesh.VertexColors[face.C];

                double colorSampleA = colorA.A * mp.T[0] + colorB.A * mp.T[1] + colorC.A * mp.T[2];
                double colorSampleR = colorA.R * mp.T[0] + colorB.R * mp.T[1] + colorC.R * mp.T[2];
                double colorSampleG = colorA.G * mp.T[0] + colorB.G * mp.T[1] + colorC.G * mp.T[2];
                double colorSampleB = colorA.B * mp.T[0] + colorB.B * mp.T[1] + colorC.B * mp.T[2];

                int Alpha = (int)colorSampleA;
                int Red = (int)colorSampleR;
                int Green = (int)colorSampleG;
                int Blue = (int)colorSampleB;

                Color colour = Color.FromArgb(Alpha, Red, Green, Blue);

                Rhino.Display.ColorHSL HSL = new Rhino.Display.ColorHSL(colour);
                double H = HSL.H;
                double lum = HSL.L;
                double S = HSL.S;
                return HSL;
            }
            else
            {
                Rhino.Display.ColorHSL HSL = new Rhino.Display.ColorHSL();
                return HSL;
            }
        }

    }
}

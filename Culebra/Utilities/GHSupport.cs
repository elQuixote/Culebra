using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace CulebraData.Utilities
{
    /// <summary>
    /// GHSupport class is meant to ease the pain of dealing with DataTrees(for scripting components) and GH_Structure(for GHA Dev),
    /// it allows you to create nested lists that cannot be used in grasshopper, for example getAllFaceVertices returns a (List List Point3d ), 
    /// this data cannot be returned in grasshopper as points in space, so using GHSupport.convertToGHDataTree() we can convert that nested list into a 
    /// data tree structure and we can now use nested lists of whatever
    /// </summary>
    public static class GHSupport
    {
        /// <summary>
        /// Converts nested list of points into GH_Structure (for gha dev)
        /// </summary>
        /// <param name="conversionData">the nested list to convert</param> 
        /// <returns>The data structure</returns> 
        public static GH_Structure<IGH_Goo> ConvertToGHDataStructure(List<List<Point3d>> conversionData)
        {
            GH_Structure<IGH_Goo> ghStructure = new GH_Structure<IGH_Goo>();
            for (int i = 0; i < conversionData.Count; i++)
            {
                List<GH_Point> ghPtList = ConvertTOGHPTList(conversionData[i]);
                ghStructure.AppendRange(ghPtList, new GH_Path(i));
            }
            return ghStructure;
        }
        /// <summary>
        /// Converts nested list of lines into GH_Structure (for gha dev)
        /// </summary>
        /// <param name="conversionData">the nested list to convert</param> 
        /// <returns>The data structure</returns> 
        public static GH_Structure<IGH_Goo> ConvertToGHDataStructure(List<List<Line>> conversionData)
        {
            GH_Structure<IGH_Goo> ghStructure = new GH_Structure<IGH_Goo>();
            for (int i = 0; i < conversionData.Count; i++)
            {
                List<GH_Line> ghLineList = ConvertTOGHLineList(conversionData[i]);
                ghStructure.AppendRange(ghLineList, new GH_Path(i));
            }
            return ghStructure;
        }
        /// <summary>
        /// Converts nested list of polylines into GH_Structure (for gha dev)
        /// </summary>
        /// <param name="conversionData">the nested list to convert</param> 
        /// <returns>The data structure</returns> 
        public static GH_Structure<IGH_Goo> ConvertToGHDataStructure(List<List<Polyline>> conversionData)
        {
            GH_Structure<IGH_Goo> ghStructure = new GH_Structure<IGH_Goo>();
            for (int i = 0; i < conversionData.Count; i++)
            {
                List<GH_Curve> ghCurveList = ConvertTOGHCurveList(conversionData[i]);
                ghStructure.AppendRange(ghCurveList, new GH_Path(i));
            }
            return ghStructure;
        }
        /// <summary>
        /// Converts nested list of points into data tree (for scripting components inside gh)
        /// </summary>
        /// <param name="conversionData">the nested list to convert</param> 
        /// <returns>The data structure</returns> 
        public static DataTree<Point3d> ConvertToGHDataTree(List<List<Point3d>> conversionData)
        {
            DataTree<Point3d> ghTree = new DataTree<Point3d>();
            for (int i = 0; i < conversionData.Count; i++)
            {
                ghTree.AddRange(conversionData[i], new GH_Path(i));
            }
            return ghTree;
        }
        /// <summary>
        /// Converts nested list of lines into data tree (for scripting components inside gh)
        /// </summary>
        /// <param name="conversionData">the nested list to convert</param> 
        /// <returns>The data structure</returns> 
        public static DataTree<Line> ConvertToGHDataTree(List<List<Line>> conversionData)
        {
            DataTree<Line> ghTree = new DataTree<Line>();
            for (int i = 0; i < conversionData.Count; i++)
            {
                ghTree.AddRange(conversionData[i], new GH_Path(i));
            }
            return ghTree;
        }
        /// <summary>
        /// Converts a point into a GH_Point
        /// </summary>
        /// <param name="pt">The point to convert</param> 
        /// <returns>The GH_Point</returns> 
        public static GH_Point ConvertToGHPoint(Point3d pt)
        {
            GH_Point ghp = new GH_Point();
            bool c = GH_Convert.ToGHPoint(pt, GH_Conversion.Both, ref ghp);
            return ghp;
        }
        /// <summary>
        /// Converts a line into a GH_Line
        /// </summary>
        /// <param name="line">The line to convert</param> 
        /// <returns>The GH_Line</returns> 
        public static GH_Line ConvertToGHLine(Line line)
        {
            GH_Line ghl = new GH_Line();
            bool c = GH_Convert.ToGHLine(line, GH_Conversion.Both, ref ghl);
            return ghl;
        }
        /// <summary>
        /// Converts a polyline into a GH_Curve
        /// </summary>
        /// <param name="pline">The polyline to convert</param> 
        /// <returns>The GH_Curve</returns> 
        public static GH_Curve ConvertToGHCurve(Polyline pline)
        {
            GH_Curve ghl = new GH_Curve();
            bool c = GH_Convert.ToGHCurve(pline, GH_Conversion.Both, ref ghl);
            return ghl;
        }
        /// <summary>
        /// Converts a point list into a list of GH_Points
        /// </summary>
        /// <param name="ptList">The list to convert</param> 
        /// <returns>The list of GH_Points</returns> 
        public static List<GH_Point> ConvertTOGHPTList(List<Point3d> ptList)
        {
            List<GH_Point> ghPtList = new List<GH_Point>();
            foreach (Point3d p in ptList)
            {
                ghPtList.Add(ConvertToGHPoint(p));
            }
            return ghPtList;
        }
        /// <summary>
        /// Converts a list of lines into a list of GH_Lines
        /// </summary>
        /// <param name="edgeList">The list to convert</param> 
        /// <returns>The list of GH_Lines</returns> 
        public static List<GH_Line> ConvertTOGHLineList(List<Line> edgeList)
        {
            List<GH_Line> ghLineList = new List<GH_Line>();
            foreach (Line p in edgeList)
            {
                ghLineList.Add(ConvertToGHLine(p));
            }
            return ghLineList;
        }
        /// <summary>
        /// Converts a list of polylines into a list of GH_Curves
        /// </summary>
        /// <param name="edgeList">The list to convert</param> 
        /// <returns>The list of GH_Curves</returns> 
        public static List<GH_Curve> ConvertTOGHCurveList(List<Polyline> edgeList)
        {
            List<GH_Curve> ghCurveList = new List<GH_Curve>();
            foreach (Polyline p in edgeList)
            {
                ghCurveList.Add(ConvertToGHCurve(p));
            }
            return ghCurveList;
        }
    }
}

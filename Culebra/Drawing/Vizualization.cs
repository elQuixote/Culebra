using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel;
using System.Drawing;

namespace CulebraData.Drawing
{
    /// <summary>
    /// The <see cref="Drawing"/> namespace contains all Culebra Objects Visualization Classes
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// Visualization Class - Used to access Creeper Object's Viz properties
    /// </summary>
    public class Vizualization
    {
        /// <summary>
        /// Draws a Point Graphic through the display pipeline
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="particleList">the list of points representing the particle positions</param>
        public void DrawPointGraphic(IGH_PreviewArgs args, List<Point3d> particleList)
        {
            foreach (Point3d p in particleList)
                args.Display.DrawPoint(p, System.Drawing.Color.Blue);
        }
        /// <summary>
        /// Draws a Particle through the display pipeline
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="file">the texture file</param>
        /// <param name="particleSystem">the particle system</param>
        public void DrawParticles(IGH_PreviewArgs args, string file, ParticleSystem particleSystem)
        {
            Bitmap bm = new Bitmap(file);
            Rhino.Display.DisplayBitmap dbm = new Rhino.Display.DisplayBitmap(bm);
            args.Display.DrawParticles(particleSystem, dbm);
        }
        /// <summary>
        /// Draws Sprites through the display pipeline
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="file">the texture file</param>
        /// <param name="particleList">the list of points representing the particle positions</param>
        public void DrawSprites(IGH_PreviewArgs args, string file, List<Point3d> particleList)
        {
            Bitmap bm = new Bitmap(file);
            Rhino.Display.DisplayBitmap dbm = new Rhino.Display.DisplayBitmap(bm);
            Rhino.Display.DisplayBitmapDrawList ddl = new Rhino.Display.DisplayBitmapDrawList();         
            ddl.SetPoints(particleList, Color.FromArgb(255, 255, 255, 255));
            args.Display.DrawSprites(dbm, ddl, 2.0f, new Vector3d(0, 0, 1), true);
        }
        /// <summary>
        /// Draws a gradient trail through the display pipeline
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="particleSet">The data tree containing the points list for each object you want to draw a gradient for</param>
        /// <param name="colorType">the color type</param>
        /// <param name="alpha">the trail alpha value</param>
        /// <param name="minTrailThickness">the minimum trail thickness</param>
        /// <param name="maxTrailThickness">the maximum trail thickness</param>
        public void DrawGradientTrails(IGH_PreviewArgs args, DataTree<Point3d> particleSet, int colorType, int alpha, float minTrailThickness, float maxTrailThickness)
        {
            for (int i = 0; i < particleSet.BranchCount; i++)
            {
                List<Point3d> ptlist = particleSet.Branch(i);
                //-------DRAW TRAILS AS SEGMENTS WITH CUSTOM STROKE WIDTH---------
                if (ptlist.Count > 0)
                {
                    for (int x = 0; x < ptlist.Count; x++)
                    {
                        if (x != 0)
                        {
                            float stroke = Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, minTrailThickness, maxTrailThickness);
                            float colorValue = Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, 0f, 255.0f);
                            if(colorType == 0)
                            {
                                args.Display.DrawLine(ptlist[x - 1], ptlist[x], Color.FromArgb(alpha, (int)colorValue, 0, 100), (int)stroke);
                            }
                            else if(colorType == 1)
                            {
                                args.Display.DrawLine(ptlist[x - 1], ptlist[x], Color.FromArgb(alpha, 0, 255, (int)colorValue), (int)stroke);
                            }
                            else
                            {
                                args.Display.DrawLine(ptlist[x - 1], ptlist[x], Color.FromArgb(alpha, 255, 255, (int)colorValue), (int)stroke);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws a gradient trail through the display pipeline
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="particleSet">The data tree containing the points list for each object you want to draw a gradient for</param>
        /// <param name="alpha">the trail alpha value</param>
        /// <param name="r_colorA">the target min color value for the r channel</param>
        /// <param name="r_colorB">the target max color value for the r channel</param>
        /// <param name="g_colorA">the targer min color value for the g channel</param>
        /// <param name="g_colorB">the target max color value for the g channel</param>
        /// <param name="b_colorA">the target min color value for the b channel</param>
        /// <param name="b_colorB">the target max color value for the b channel</param>
        /// <param name="minTrailThickness">the minimum trail thickness</param>
        /// <param name="maxTrailThickness">the maximum trail thickness</param>
        public void DrawGradientTrails(IGH_PreviewArgs args, DataTree<Point3d> particleSet, int alpha, float r_colorA, float r_colorB, float g_colorA, float g_colorB, 
            float b_colorA, float b_colorB, float minTrailThickness, float maxTrailThickness)
        {
            for (int i = 0; i < particleSet.BranchCount; i++)
            {
                List<Point3d> ptlist = particleSet.Branch(i);
                //-------DRAW TRAILS AS SEGMENTS WITH CUSTOM STROKE WIDTH---------
                if (ptlist.Count > 0)
                {
                    for (int x = 0; x < ptlist.Count; x++)
                    {
                        if (x != 0)
                        {
                            float stroke = Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, minTrailThickness, maxTrailThickness);
                            float colorValueR = Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, r_colorA, r_colorB);
                            float colorValueG = Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, g_colorA, g_colorB);
                            float colorValueB = Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, b_colorA, b_colorB);
                            args.Display.DrawLine(ptlist[x - 1], ptlist[x], Color.FromArgb(alpha, (int)colorValueR, (int)colorValueG, (int)colorValueB), (int)stroke);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Draws a polyline trail through the display pipeline
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="particleSet">The data tree containing the points list for each object you want to draw a gradient for</param>
        /// <param name="dottedPolyline">do you want a dotted polyline</param>
        /// <param name="thickness">the thickness of the trail</param>
        /// <param name="color">the color of the trail</param>
        public void DrawPolylineTrails(IGH_PreviewArgs args, DataTree<Point3d> particleSet, bool dottedPolyline, int thickness, System.Drawing.Color color)
        {
            for (int i = 0; i < particleSet.BranchCount; i++)
            {
                List<Point3d> ptlist = particleSet.Branch(i);
                if (dottedPolyline)
                {
                    args.Display.DrawDottedPolyline(ptlist, color, false);
                }
                else
                {
                    args.Display.DrawPolyline(ptlist, color, thickness);
                }
            }
        }
        /// <summary>
        /// Draws a disco trail through the display pipeline. Trails flash different colors throughout the simulation
        /// </summary>
        /// <param name="args">preview Display Args for IGH_PreviewObjects</param>
        /// <param name="particleSet">The data tree containing the points list for each object you want to draw a gradient for</param>
        /// <param name="randomGen">an instance of the random class</param>
        /// <param name="minTrailThickness">the minimum trail thickness</param>
        /// <param name="maxTrailThickness">the maximum trail thickness</param>
        public void DrawDiscoTrails(IGH_PreviewArgs args, DataTree<Point3d> particleSet, Random randomGen, float minTrailThickness, float maxTrailThickness)
        {
            Color color = args.WireColour;
            for (int i = 0; i < particleSet.BranchCount; i++)
            {
                List<Point3d> ptlist = particleSet.Branch(i);
                //-------DRAW TRAILS AS SEGMENTS WITH CUSTOM STROKE WIDTH---------
                Color randomColorAction = CulebraData.Utilities.ColorUtility.GetRandomColor(randomGen);
                if (ptlist.Count > 0)
                {
                    for (int x = 0; x < ptlist.Count; x++)
                    {
                        if (x != 0)
                        {
                            float stroke = CulebraData.Utilities.Mapping.Map(x / (1.0f * ptlist.Count), 0.0f, 1.0f, minTrailThickness, maxTrailThickness);
                            args.Display.DrawLine(ptlist[x - 1], ptlist[x], randomColorAction, (int)stroke);
                        }
                    }
                }
            }
        }
    }
}

using Rhino.Geometry;
using System.Drawing;
using System;

namespace CulebraData.Utilities
{
    /// <summary>
    /// The <see cref="Utilities"/> namespace contains all Culebra Utilities Classes
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// Provides a static set of color utilities
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
                return colour;
            }
            else
            {
                return new Color();
            }
        }
        /// <summary>
        /// Generates a random color
        /// </summary>
        /// <param name="randomGen">the random object instance</param>
        /// <returns>The random color</returns>
        public static Color GetRandomColor(Random randomGen)
        {
            KnownColor[] names = (KnownColor[])Enum.GetValues(typeof(KnownColor));
            KnownColor randomColorName = names[randomGen.Next(names.Length)];
            Color randomColor = Color.FromKnownColor(randomColorName);

            return randomColor;
        }
    }
}

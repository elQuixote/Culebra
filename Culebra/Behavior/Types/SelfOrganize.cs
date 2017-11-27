using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Geometry;
using Grasshopper;
using Grasshopper.Kernel.Data;

namespace CulebraData.Behavior.Types
{
    public class SelfOrganize
    {
        public List<Curve> Bundling(List<Point3d> particleList, DataTree<Point3d> particleSet, List<Curve> crvList, double thresh, double ratio, int weldCount, bool rebuild, int ptCount, Mesh dMesh = null, bool useColor = false)
        {
            bool color_Override = false;
            double multiplier = new double();
            //-----------SelfOrg---------------------
            for (int j = 0; j < crvList.Count; j++)
            { //go through each curve

                GH_Path path = new GH_Path(j);

                List<Point3d> arrNewPos = new List<Point3d>();

                if (rebuild || (!rebuild && crvList[j].Degree == 3))
                { //rebuild curve
                    crvList[j] = crvList[j].Rebuild(ptCount, 1, false);
                }

                Rhino.Geometry.Polyline pline = new Rhino.Geometry.Polyline();
                bool conv = crvList[j].TryGetPolyline(out pline); //get polyline from curve
                if (!conv)
                {
                    throw new Exception("Could not convert to a polyline, had to abort");
                }

                List<Point3d> ptList = new List<Point3d>();
                for (int z = 0; z < pline.SegmentCount + 1; z++)
                {
                    ptList.Add(pline.PointAt((float)z));
                }
                //-----------Weld Points Setup---------------------
                List<Point3d> subList = new List<Point3d>();
                subList = ptList.GetRange(weldCount, (ptList.Count) - ((weldCount) * 2));
                List<Point3d> frontList = new List<Point3d>();
                frontList = ptList.GetRange(0, weldCount);
                List<Point3d> endList = new List<Point3d>();
                endList = ptList.GetRange((weldCount) + subList.Count, weldCount);
                //-----------Go through each point of the current curve---------------------
                for (int k = 0; k < subList.Count; k++)
                {
                    //-----------Color---------------------
                    if (useColor)
                    {
                        multiplier = Utilities.ColorUtility.GetHueSatLum(subList[k], dMesh).L;
                        if (multiplier == 0)
                        {
                            color_Override = true;
                        }
                        else
                        {
                            color_Override = false;
                            multiplier *= thresh;
                        }
                    }             
                    else
                    {
                        color_Override = false;
                        multiplier = thresh;
                    }
                    if (!color_Override)
                    {
                        List<Point3d> arrCrvPts = new List<Point3d>();
                        //-----------Go through each other curve and get the closest point on each curve---------------------
                        for (int m = 0; m < crvList.Count; m++)
                        {
                            if (j != m)
                            {
                                double tVal = new double();
                                crvList[m].ClosestPoint(subList[k], out tVal, thresh);
                                arrCrvPts.Add(crvList[m].PointAt(tVal));
                            }
                        }
                        Point3d newcrvPt = arrCrvPts[Rhino.Collections.Point3dList.ClosestIndexInList(arrCrvPts, subList[k])]; //get the closest point of the closest points from each curve
                        arrCrvPts.Clear();
                        double dist = newcrvPt.DistanceTo(subList[k]);
                        if (dist < multiplier)
                        {
                            Vector3d subVec = Rhino.Geometry.Vector3d.Subtract(new Vector3d(newcrvPt), new Vector3d(subList[k]));
                            Vector3d newVec = Rhino.Geometry.Vector3d.Multiply(subVec, ratio);
                            arrNewPos.Add(Rhino.Geometry.Point3d.Add(newVec, subList[k]));
                        }
                        else
                        {
                            arrNewPos.Add(subList[k]);
                        }
                    }
                    else
                    {
                        arrNewPos.Add(subList[k]);
                    }
                }
                //-----------Re insert the og weld point positions and create the new curve---------------------
                arrNewPos.InsertRange(0, frontList);
                arrNewPos.InsertRange(arrNewPos.Count, endList);
                crvList[j] = Curve.CreateInterpolatedCurve(arrNewPos, 1);

                particleSet.AddRange(arrNewPos, path);
                particleList.AddRange(arrNewPos);

                arrNewPos.Clear();
                ptList.Clear();
                subList.Clear();
                frontList.Clear();
                endList.Clear();
            }
            return crvList;
        }
    }
}

using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Culebra_GH.Objects;
using Grasshopper;
using Grasshopper.Kernel.Data;
using CulebraData.Objects;

namespace Culebra_GH.Engine
{
    /// <summary>
    /// 
    /// </summary>
    public class Engine_Global
    {
        private List<Vector3d> childSpawners = new List<Vector3d>();
        private List<int> childSpawnType = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Vector3d> GetChildSpawners() { return this.childSpawners; } 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<int> GetChildSpawnTypes() { return this.childSpawnType; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="creepList"></param>
        private void SpawnChild(int dimensions, List<CulebraObject> creepList)
        {
            Random rnd = new Random();
            int babyCount = 0;
            foreach (Vector3d px in this.childSpawners)
            {
                Vector3d speed;
                if (dimensions == 0)
                {
                    speed = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, 0);
                }
                else
                {
                    speed = new Vector3d(rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5, rnd.Next(-2, 2) * 0.5);
                }
                Creeper a;
                bool three_d = new bool();
                if (dimensions == 0) { three_d = false; }
                else { three_d = true; }
                if (this.childSpawnType[babyCount] % 2 == 0)
                {
                    a = new BabyCreeper(new Vector3d(px.X, px.Y, px.Z), speed, false, "a", three_d);
                    creepList.Add(a);
                }
                else
                {
                    a = new BabyCreeper(new Vector3d(px.X, px.Y, px.Z), speed, false, "b", three_d);
                    creepList.Add(a);
                }
                babyCount++;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="creepList"></param>
        /// <param name="dimensions"></param>
        /// <param name="behavioral_Settings"></param>
        /// <param name="displayMode"></param>
        /// <param name="networkList"></param>
        /// <param name="maxSpeed"></param>
        /// <param name="maxForce"></param>
        /// <param name="velMultiplier"></param>
        /// <param name="totTail"></param>
        /// <param name="particleList"></param>
        /// <param name="particleSet"></param>
        /// <param name="networkTree"></param>
        /// <param name="trailStep"></param>
        /// <param name="maxTrailSize"></param>
        /// <param name="bounds"></param>
        /// <param name="bb"></param>
        /// <param name="currentPosList"></param>
        /// <param name="trail"></param>
        /// <param name="trailTree"></param>
        /// <param name="child_maxSpeed"></param>
        /// <param name="child_maxForce"></param>
        /// <param name="child_velMultiplier"></param>
        /// <param name="child_behavioral_Settings"></param>
        /// <param name="child_displayMode"></param>
        /// <param name="child_trail"></param>
        /// <param name="child_trailStep"></param>
        /// <param name="child_maxTrailSize"></param>
        /// <param name="particleBabyASet"></param>
        /// <param name="particleBabyBSet"></param>
        /// <param name="trailTree_ChildA"></param>
        /// <param name="trailTree_ChildB"></param>
        public void Action(List<CulebraObject> creepList, int dimensions, object behavioral_Settings, int displayMode, List<Line> networkList, double maxSpeed, double maxForce, double velMultiplier,
            List<Vector3d> totTail, List<Point3d> particleList, DataTree<Point3d> particleSet, DataTree<Line> networkTree, int trailStep, int maxTrailSize, bool bounds, BoundingBox bb, List<Point3d> currentPosList,
            bool trail, DataTree<Point3d> trailTree, double child_maxSpeed = 0.0, double child_maxForce = 0.0, double child_velMultiplier = 0.0, object child_behavioral_Settings = null, int child_displayMode = 0, bool child_trail = false, int child_trailStep = 0,
            int child_maxTrailSize = 0, DataTree<Point3d>particleBabyASet = null, DataTree<Point3d>particleBabyBSet = null, DataTree<Point3d>trailTree_ChildA = null, DataTree<Point3d>trailTree_ChildB = null)
        {
            int counter = 0;
            foreach (Creeper c in creepList)
            {
                networkList = new List<Line>();
                GH_Path path = new GH_Path(counter);
                if (c is BabyCreeper)
                {
                    c.attributes.SetMoveAttributes((float)child_maxSpeed, (float)child_maxForce, (float)child_velMultiplier);
                    try { FilterBehaviors(c, creepList, child_behavioral_Settings, dimensions, childSpawners, childSpawnType, totTail); }
                    catch (Exception e) { throw new Exception(e.Message); }
                    if (c.attributes.GetChildType() == "a")
                    {
                        //-------ADD POINTS TO GRAPHIC POINTS LIST---------
                        if (child_displayMode == 0)
                        {
                            particleList.Add(c.attributes.GetLocation());
                            particleBabyASet.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        else if (child_displayMode == 1 && child_trail)
                        {
                            trailTree_ChildA.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        currentPosList.Add(c.attributes.GetLocation());
                    }
                    else
                    {
                        //-------ADD POINTS TO GRAPHIC POINTS LIST---------
                        if (child_displayMode == 0)
                        {
                            particleList.Add(c.attributes.GetLocation());
                            particleBabyBSet.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        else if (child_displayMode == 1 && child_trail)
                        {
                            trailTree_ChildB.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        currentPosList.Add(c.attributes.GetLocation());
                    }
                    c.actions.Move(child_trailStep, child_maxTrailSize);
                }
                else
                {
                    c.attributes.SetMoveAttributes((float)maxSpeed, (float)maxForce, (float)velMultiplier);

                    try { FilterBehaviors(c, creepList, behavioral_Settings, dimensions, this.childSpawners, this.childSpawnType, totTail); }
                    catch (Exception e) { throw new Exception(e.Message); }

                    if (displayMode == 0)
                    {
                        particleList.Add(c.attributes.GetLocation());
                        particleSet.AddRange(c.attributes.GetTrailPoints(), path);
                    }
                    currentPosList.Add(c.attributes.GetLocation());

                    if (displayMode == 1 || displayMode == 0)
                    {
                        List<Vector3d> testList = c.attributes.GetNetwork();
                        if (testList.Count > 0)
                        {
                            foreach (Vector3d v in testList)
                            {
                                Line l = new Line(c.attributes.GetLocation(), (Point3d)v);
                                networkList.Add(l);
                            }
                            networkTree.AddRange(networkList, path);
                        }
                    }
                    if (displayMode == 1 && trail)
                    {
                        trailTree.AddRange(c.attributes.GetTrailPoints(), path);
                    }
                    c.actions.Move(trailStep, maxTrailSize);
                }
                if (bounds)
                {
                    if (dimensions == 0) { c.actions.Bounce(bb); }
                    else if (dimensions == 1) { c.actions.Bounce3D(bb); }
                }
                counter++;
            }
            if (this.childSpawners.Count > 0)
            {
                SpawnChild(dimensions, creepList);
                this.childSpawners = new List<Vector3d>();
                this.childSpawnType = new List<int>();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="creepList"></param>
        /// <param name="behavioral_Settings"></param>
        /// <param name="dimensions"></param>
        /// <param name="childSpawners"></param>
        /// <param name="childSpawnType"></param>
        /// <param name="totTail"></param>
        private void FilterBehaviors(Creeper c, List<CulebraObject> creepList, object behavioral_Settings, int dimensions, List<Vector3d> childSpawners, List<int> childSpawnType, List<Vector3d> totTail)
        {
            IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)behavioral_Settings;
            foreach (string s in igh_Behavior.Value.dataOrder)
            {
                if (s == "Flocking")
                {
                    if (dimensions == 0)
                    {
                        if(igh_Behavior.Value.flockData.colorMesh != null)
                        {
                            c.behaviors.Flock2D(igh_Behavior.Value.flockData.searchRadius, igh_Behavior.Value.flockData.cohesion_Value, igh_Behavior.Value.flockData.separation_Value, igh_Behavior.Value.flockData.alignment_Value, igh_Behavior.Value.flockData.viewAngle, creepList, 
                                igh_Behavior.Value.flockData.network, igh_Behavior.Value.flockData.mapAlignment, igh_Behavior.Value.flockData.mapSeparation, igh_Behavior.Value.flockData.mapCohesion, igh_Behavior.Value.flockData.colorMesh);
                        }
                        else
                        {
                            c.behaviors.Flock2D(igh_Behavior.Value.flockData.searchRadius, igh_Behavior.Value.flockData.cohesion_Value, igh_Behavior.Value.flockData.separation_Value, igh_Behavior.Value.flockData.alignment_Value, igh_Behavior.Value.flockData.viewAngle, creepList, igh_Behavior.Value.flockData.network);
                        }
                    }
                    else if (dimensions == 1)
                    {
                        if (igh_Behavior.Value.flockData.colorMesh != null)
                        {
                            c.behaviors.Flock3D(igh_Behavior.Value.flockData.searchRadius, igh_Behavior.Value.flockData.cohesion_Value, igh_Behavior.Value.flockData.separation_Value, igh_Behavior.Value.flockData.alignment_Value, igh_Behavior.Value.flockData.viewAngle, creepList,
                                igh_Behavior.Value.flockData.network, igh_Behavior.Value.flockData.mapAlignment, igh_Behavior.Value.flockData.mapSeparation, igh_Behavior.Value.flockData.mapCohesion, igh_Behavior.Value.flockData.colorMesh);
                        }
                        else
                        {
                            c.behaviors.Flock3D(igh_Behavior.Value.flockData.searchRadius, igh_Behavior.Value.flockData.cohesion_Value, igh_Behavior.Value.flockData.separation_Value, igh_Behavior.Value.flockData.alignment_Value, igh_Behavior.Value.flockData.viewAngle, creepList, igh_Behavior.Value.flockData.network);
                        }
                    }
                }
                else if (s == "Wandering")
                {
                    if (dimensions == 0)
                    {
                        if (igh_Behavior.Value.wanderData.wanderingType == "Wander")
                        {
                            if(igh_Behavior.Value.wanderData.colorMesh != null)
                            {
                                c.behaviors.Wander2D(igh_Behavior.Value.wanderData.randomize, igh_Behavior.Value.wanderData.addHeading, igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance,
                                    igh_Behavior.Value.wanderData.mapChange, igh_Behavior.Value.wanderData.mapRadius, igh_Behavior.Value.wanderData.mapDistance, igh_Behavior.Value.wanderData.colorMesh);
                            }
                            else
                            {
                                c.behaviors.Wander2D(igh_Behavior.Value.wanderData.randomize, igh_Behavior.Value.wanderData.addHeading, igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance);
                            }
                        }
                        else
                        {
                            c.behaviors.SuperWander2D(igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance, igh_Behavior.Value.wanderData.rotationTrigger);
                        }
                    }
                    else if (dimensions == 1)
                    {
                        if (igh_Behavior.Value.wanderData.wanderingType == "SuperWander_B")
                        {
                            c.behaviors.Wander3D_subA(igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance, igh_Behavior.Value.wanderData.rotationTrigger);
                        }
                        else if (igh_Behavior.Value.wanderData.wanderingType == "SuperWander_C")
                        {
                            c.behaviors.Wander3D_subB(igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance, igh_Behavior.Value.wanderData.rotationTrigger);
                        }
                        else
                        {
                            if(igh_Behavior.Value.wanderData.colorMesh != null)
                            {
                                c.behaviors.Wander3D(igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance, igh_Behavior.Value.wanderData.rotationTrigger,
                                    igh_Behavior.Value.wanderData.mapChange, igh_Behavior.Value.wanderData.mapRadius, igh_Behavior.Value.wanderData.mapDistance, false, igh_Behavior.Value.wanderData.colorMesh);
                            }
                            else
                            {
                                c.behaviors.Wander3D(igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance, igh_Behavior.Value.wanderData.rotationTrigger);
                            }
                        }
                    }
                }
                else if (s == "Tracking")
                {
                    if (igh_Behavior.Value.trackingData.triggerBabies)
                    {
                        if(igh_Behavior.Value.trackingData.colorMesh != null)
                        {
                            c.behaviors.MultiPolylineTrackerBabyMaker(igh_Behavior.Value.trackingData.polylines, igh_Behavior.Value.trackingData.pathThreshold, igh_Behavior.Value.trackingData.projectionDistance, igh_Behavior.Value.trackingData.pathRadius,
                                igh_Behavior.Value.trackingData.triggerBabies, igh_Behavior.Value.trackingData.maxChildren, true, childSpawners, childSpawnType,
                                igh_Behavior.Value.trackingData.mapThreshold, igh_Behavior.Value.trackingData.mapProjection, igh_Behavior.Value.trackingData.mapRadius, igh_Behavior.Value.trackingData.colorMesh);
                            this.childSpawners = c.behaviors.GetChildStartPositions();
                            this.childSpawnType = c.behaviors.GetChildSpawnTypes();
                        }
                        else
                        {
                            c.behaviors.MultiPolylineTrackerBabyMaker(igh_Behavior.Value.trackingData.polylines, igh_Behavior.Value.trackingData.pathThreshold, igh_Behavior.Value.trackingData.projectionDistance, igh_Behavior.Value.trackingData.pathRadius,
                                igh_Behavior.Value.trackingData.triggerBabies, igh_Behavior.Value.trackingData.maxChildren, true, childSpawners, childSpawnType);
                            this.childSpawners = c.behaviors.GetChildStartPositions();
                            this.childSpawnType = c.behaviors.GetChildSpawnTypes();
                        }
                    }
                    else
                    {
                        if(igh_Behavior.Value.trackingData.colorMesh != null)
                        {
                            c.behaviors.MultiPolylineTracker(igh_Behavior.Value.trackingData.polylines, igh_Behavior.Value.trackingData.pathThreshold, igh_Behavior.Value.trackingData.projectionDistance, igh_Behavior.Value.trackingData.pathRadius,
                                igh_Behavior.Value.trackingData.mapThreshold, igh_Behavior.Value.trackingData.mapProjection, igh_Behavior.Value.trackingData.mapRadius, igh_Behavior.Value.trackingData.colorMesh);
                        }
                        else
                        {
                            c.behaviors.MultiPolylineTracker(igh_Behavior.Value.trackingData.polylines, igh_Behavior.Value.trackingData.pathThreshold, igh_Behavior.Value.trackingData.projectionDistance, igh_Behavior.Value.trackingData.pathRadius);
                        }
                    }
                }
                else if(s == "Crawl")
                {
                    if (igh_Behavior.Value.meshCrawlData.triggerBabies)
                    {
                        c.behaviors.MeshWalk(igh_Behavior.Value.meshCrawlData.mesh, igh_Behavior.Value.meshCrawlData.meshThreshold, igh_Behavior.Value.meshCrawlData.meshProjectionDistance, igh_Behavior.Value.meshCrawlData.multiplier, igh_Behavior.Value.meshCrawlData.triggerBabies, true, igh_Behavior.Value.meshCrawlData.maxChildren, childSpawners, childSpawnType);
                        this.childSpawners = c.behaviors.GetMeshCrawler().GetChildStartPositions();
                        this.childSpawnType = c.behaviors.GetMeshCrawler().GetChildSpawnType();
                    }
                    else
                    {
                        c.behaviors.MeshWalk(igh_Behavior.Value.meshCrawlData.mesh, igh_Behavior.Value.meshCrawlData.meshThreshold, igh_Behavior.Value.meshCrawlData.meshProjectionDistance, igh_Behavior.Value.meshCrawlData.multiplier);
                    }
                }
                else if (s == "Stigmergy")
                {
                    totTail.AddRange(c.attributes.GetTrailVectors());
                    c.behaviors.SelfTailChase(igh_Behavior.Value.stigmergyData.viewAngle, igh_Behavior.Value.stigmergyData.cohesionMagnitude, igh_Behavior.Value.stigmergyData.cohesionRange, igh_Behavior.Value.stigmergyData.separationMagnitude, igh_Behavior.Value.stigmergyData.separationRange, totTail);
                }
                else if (s == "Noise")
                {
                    if(igh_Behavior.Value.noiseData.colorMesh != null)
                    {
                        c.behaviors.Perlin2DMap(igh_Behavior.Value.noiseData.scale, igh_Behavior.Value.noiseData.strength, igh_Behavior.Value.noiseData.multiplier, igh_Behavior.Value.noiseData.velocity,
                            igh_Behavior.Value.noiseData.mapStrength, igh_Behavior.Value.noiseData.mapStrength, igh_Behavior.Value.noiseData.mapMultiplier, igh_Behavior.Value.noiseData.colorMesh);
                    }
                    else
                    {
                        c.behaviors.Perlin(igh_Behavior.Value.noiseData.scale, igh_Behavior.Value.noiseData.strength, igh_Behavior.Value.noiseData.multiplier, igh_Behavior.Value.noiseData.velocity);
                    }
                }
                else if (s == "Separation")
                {
                    c.behaviors.Separate(igh_Behavior.Value.separationData.maxSeparation, creepList);
                }
                else if (s == "Force")
                {
                    int forceAmount = igh_Behavior.Value.forceData.Count;
                    for (int i = 0; i < forceAmount; i++)
                    {
                        if (igh_Behavior.Value.forceData[i].forceType == "Attract")
                        {
                            int attCount = 0;
                            foreach (Point3d p in igh_Behavior.Value.forceData[i].targets)
                            {
                                c.behaviors.Attract((Vector3d)p, igh_Behavior.Value.forceData[i].thresholds[attCount], igh_Behavior.Value.forceData[i].attractionValue, igh_Behavior.Value.forceData[i].maxAttraction);
                                attCount++;
                            }
                        }
                        else if (igh_Behavior.Value.forceData[i].forceType == "Repel")
                        {
                            int attCount = 0;
                            foreach (Point3d p in igh_Behavior.Value.forceData[i].targets)
                            {
                                c.behaviors.Repel((Vector3d)p, igh_Behavior.Value.forceData[i].thresholds[attCount], igh_Behavior.Value.forceData[i].repelValue, igh_Behavior.Value.forceData[i].maxRepel);
                                attCount++;
                            }
                        }
                    }
                }
                else { throw new Exception("Houston we have a problem, no behavior data read"); }
            }
        }
    }
}

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
    /// Engine Global Class - Filters behaviors and does the actual work.
    /// </summary>
    public class Engine_Global
    {
        private List<Vector3d> childSpawners = new List<Vector3d>();
        private List<int> childSpawnType = new List<int>();
        /// <summary>
        /// Gets the child spawner positions
        /// </summary>
        /// <returns>The list of child start positions</returns>
        public List<Vector3d> GetChildSpawners() { return this.childSpawners; } 
        /// <summary>
        /// Gets the child spawn types
        /// </summary>
        /// <returns>The list of child spawn types</returns>
        public List<int> GetChildSpawnTypes() { return this.childSpawnType; }
        /// <summary>
        /// Spawns the creeper child objects
        /// </summary>
        /// <param name="dimensions">If we are in 2D or 3D</param>
        /// <param name="creepList">The list of culebra objects to add the children to</param>
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
        /// Performs all the actions
        /// </summary>
        public void Action(List<CulebraObject> creepList, int dimensions, object behavioral_Settings, int displayMode, List<Line> networkList, double maxSpeed, double maxForce, double velMultiplier,
            List<Vector3d> totTail, List<Point3d> particleList, DataTree<Point3d> particleSet, DataTree<Line> networkTree, int trailStep, int maxTrailSize, int bounds, BoundingBox bb, List<Point3d> currentPosList,
            bool trail, DataTree<Point3d> trailTree, double child_maxSpeed = 0.0, double child_maxForce = 0.0, double child_velMultiplier = 0.0, object child_behavioral_Settings = null, int child_displayMode = 0, bool child_trail = false, int child_trailStep = 0,
            int child_maxTrailSize = 0, DataTree<Point3d>particleBabyASet = null, DataTree<Point3d>particleBabyBSet = null, DataTree<Point3d>trailTree_ChildA = null, DataTree<Point3d>trailTree_ChildB = null)
        {
            int counter = 0;
            foreach (Creeper c in creepList)
            {
                networkList = new List<Line>();
                GH_Path path = new GH_Path(counter);
                if (c is BabyCreeper)
                { //--If we are a baby object
                    c.attributes.SetMoveAttributes((float)child_maxSpeed, (float)child_maxForce, (float)child_velMultiplier);
                    try { FilterBehaviors(c, creepList, child_behavioral_Settings, dimensions, childSpawners, childSpawnType, totTail); }
                    catch (Exception e) { throw new Exception(e.Message); }
                    if (c.attributes.GetChildType() == "a")
                    {
                        if (child_displayMode == 0) //--if graphics mode we will add the relevant data to the graphics lists/datatrees for child type a
                        {
                            particleList.Add(c.attributes.GetLocation());
                            particleBabyASet.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        else if (child_displayMode == 1 && child_trail) //--if geometry mode we will add the relevant data to the geometry lists/datatrees for child type a
                        {
                            trailTree_ChildA.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        currentPosList.Add(c.attributes.GetLocation());
                    }
                    else
                    {
                        if (child_displayMode == 0) //--if graphics mode we will add the relevant data to the graphics lists/datatrees for child type b
                        {
                            particleList.Add(c.attributes.GetLocation());
                            particleBabyBSet.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        else if (child_displayMode == 1 && child_trail) //--if geometry mode we will add the relevant data to the geometry lists/datatrees for child type b
                        {
                            trailTree_ChildB.AddRange(c.attributes.GetTrailPoints(), path);
                        }
                        currentPosList.Add(c.attributes.GetLocation());
                    }
                    c.actions.Move(child_trailStep, child_maxTrailSize);
                }
                else
                { //--If we are not a baby object
                    c.attributes.SetMoveAttributes((float)maxSpeed, (float)maxForce, (float)velMultiplier);
                    try { FilterBehaviors(c, creepList, behavioral_Settings, dimensions, this.childSpawners, this.childSpawnType, totTail); }
                    catch (Exception e) { throw new Exception(e.Message); }     
                              
                    if (displayMode == 0)
                    {
                        particleList.Add(c.attributes.GetLocation());
                        particleSet.AddRange(c.attributes.GetTrailPoints(), path);
                    }
                    currentPosList.Add(c.attributes.GetLocation());

                    if (displayMode == 1 || displayMode == 0) //--if we have connectivity network data to show then add it to the network tree 
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
                    if (displayMode == 1 && trail) //--if trail is enabled and geometry mode is enabled
                    {
                        trailTree.AddRange(c.attributes.GetTrailPoints(), path);
                    }
                    c.actions.Move(trailStep, maxTrailSize);
                }
                if (bounds == 0) //--if bounds is set to bounce
                {
                    if (dimensions == 0) { c.actions.Bounce(bb); }
                    else if (dimensions == 1) { c.actions.Bounce3D(bb); }
                }else if(bounds == 1) //--if bounds is set to respawn  
                {
                    if (dimensions == 0) { c.actions.Respawn(bb); }
                    else if (dimensions == 1) { c.actions.Respawn(bb,false, true); }
                }
                counter++;
            }
            if (this.childSpawners.Count > 0) //--if we have data to spawn children with then spawn them
            {
                SpawnChild(dimensions, creepList);
                this.childSpawners = new List<Vector3d>();
                this.childSpawnType = new List<int>();
            }
        }
        /// <summary>
        /// Filters through the Controller Data
        /// </summary>
        private void FilterBehaviors(Creeper c, List<CulebraObject> creepList, object behavioral_Settings, int dimensions, List<Vector3d> childSpawners, List<int> childSpawnType, List<Vector3d> totTail)
        {
            IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)behavioral_Settings;
            foreach (string s in igh_Behavior.Value.DataOrder)
            {
                if (s == "Flocking")
                {
                    if (dimensions == 0)
                    {
                        if(igh_Behavior.Value.FlockData.ColorMesh != null)
                        {
                            c.behaviors.Flock2D(igh_Behavior.Value.FlockData.SearchRadius, igh_Behavior.Value.FlockData.Cohesion_Value, igh_Behavior.Value.FlockData.Separation_Value, igh_Behavior.Value.FlockData.Alignment_Value, igh_Behavior.Value.FlockData.ViewAngle, creepList, 
                                igh_Behavior.Value.FlockData.Network, igh_Behavior.Value.FlockData.MapAlignment, igh_Behavior.Value.FlockData.MapSeparation, igh_Behavior.Value.FlockData.MapCohesion, igh_Behavior.Value.FlockData.ColorMesh);
                        }
                        else
                        {
                            c.behaviors.Flock2D(igh_Behavior.Value.FlockData.SearchRadius, igh_Behavior.Value.FlockData.Cohesion_Value, igh_Behavior.Value.FlockData.Separation_Value, igh_Behavior.Value.FlockData.Alignment_Value, igh_Behavior.Value.FlockData.ViewAngle, creepList, igh_Behavior.Value.FlockData.Network);
                        }
                    }
                    else if (dimensions == 1)
                    {
                        if (igh_Behavior.Value.FlockData.ColorMesh != null)
                        {
                            c.behaviors.Flock3D(igh_Behavior.Value.FlockData.SearchRadius, igh_Behavior.Value.FlockData.Cohesion_Value, igh_Behavior.Value.FlockData.Separation_Value, igh_Behavior.Value.FlockData.Alignment_Value, igh_Behavior.Value.FlockData.ViewAngle, creepList,
                                igh_Behavior.Value.FlockData.Network, igh_Behavior.Value.FlockData.MapAlignment, igh_Behavior.Value.FlockData.MapSeparation, igh_Behavior.Value.FlockData.MapCohesion, igh_Behavior.Value.FlockData.ColorMesh);
                        }
                        else
                        {
                            c.behaviors.Flock3D(igh_Behavior.Value.FlockData.SearchRadius, igh_Behavior.Value.FlockData.Cohesion_Value, igh_Behavior.Value.FlockData.Separation_Value, igh_Behavior.Value.FlockData.Alignment_Value, igh_Behavior.Value.FlockData.ViewAngle, creepList, igh_Behavior.Value.FlockData.Network);
                        }
                    }
                }
                else if( s == "Bundling")
                {
                    throw new Exception("You have input bundling behavior, Creepers engine cannot run bundling behavior, please use Bundling Engine");
                }
                else if (s == "Wandering")
                {
                    if (dimensions == 0)
                    {
                        if (igh_Behavior.Value.WanderData.wanderingType == "Wander")
                        {
                            if(igh_Behavior.Value.WanderData.colorMesh != null)
                            {
                                c.behaviors.Wander2D(igh_Behavior.Value.WanderData.randomize, igh_Behavior.Value.WanderData.addHeading, igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance,
                                    igh_Behavior.Value.WanderData.mapChange, igh_Behavior.Value.WanderData.mapRadius, igh_Behavior.Value.WanderData.mapDistance, igh_Behavior.Value.WanderData.colorMesh);
                            }
                            else
                            {
                                c.behaviors.Wander2D(igh_Behavior.Value.WanderData.randomize, igh_Behavior.Value.WanderData.addHeading, igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance);
                            }
                        }
                        else
                        {
                            c.behaviors.SuperWander2D(igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance, igh_Behavior.Value.WanderData.rotationTrigger);
                        }
                    }
                    else if (dimensions == 1)
                    {
                        if (igh_Behavior.Value.WanderData.wanderingType == "SuperWander_B")
                        {
                            c.behaviors.Wander3D_subA(igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance, igh_Behavior.Value.WanderData.rotationTrigger);
                        }
                        else if (igh_Behavior.Value.WanderData.wanderingType == "SuperWander_C")
                        {
                            c.behaviors.Wander3D_subB(igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance, igh_Behavior.Value.WanderData.rotationTrigger);
                        }
                        else
                        {
                            if(igh_Behavior.Value.WanderData.colorMesh != null)
                            {
                                c.behaviors.Wander3D(igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance, igh_Behavior.Value.WanderData.rotationTrigger,
                                    igh_Behavior.Value.WanderData.mapChange, igh_Behavior.Value.WanderData.mapRadius, igh_Behavior.Value.WanderData.mapDistance, false, igh_Behavior.Value.WanderData.colorMesh);
                            }
                            else
                            {
                                c.behaviors.Wander3D(igh_Behavior.Value.WanderData.change, igh_Behavior.Value.WanderData.wanderingRadius, igh_Behavior.Value.WanderData.wanderingDistance, igh_Behavior.Value.WanderData.rotationTrigger);
                            }
                        }
                    }
                }
                else if (s == "Tracking")
                {
                    if (igh_Behavior.Value.TrackingData.TriggerBabies)
                    {
                        if(igh_Behavior.Value.TrackingData.ColorMesh != null)
                        {
                            c.behaviors.MultiPolylineTrackerBabyMaker(igh_Behavior.Value.TrackingData.Polylines, igh_Behavior.Value.TrackingData.PathThreshold, igh_Behavior.Value.TrackingData.ProjectionDistance, igh_Behavior.Value.TrackingData.PathRadius,
                                igh_Behavior.Value.TrackingData.TriggerBabies, igh_Behavior.Value.TrackingData.MaxChildren, true, childSpawners, childSpawnType,
                                igh_Behavior.Value.TrackingData.MapThreshold, igh_Behavior.Value.TrackingData.MapProjection, igh_Behavior.Value.TrackingData.MapRadius, igh_Behavior.Value.TrackingData.ColorMesh);
                            this.childSpawners = c.behaviors.GetChildStartPositions();
                            this.childSpawnType = c.behaviors.GetChildSpawnTypes();
                        }
                        else
                        {
                            c.behaviors.MultiPolylineTrackerBabyMaker(igh_Behavior.Value.TrackingData.Polylines, igh_Behavior.Value.TrackingData.PathThreshold, igh_Behavior.Value.TrackingData.ProjectionDistance, igh_Behavior.Value.TrackingData.PathRadius,
                                igh_Behavior.Value.TrackingData.TriggerBabies, igh_Behavior.Value.TrackingData.MaxChildren, true, childSpawners, childSpawnType);
                            this.childSpawners = c.behaviors.GetChildStartPositions();
                            this.childSpawnType = c.behaviors.GetChildSpawnTypes();
                        }
                    }
                    else
                    {
                        if(igh_Behavior.Value.TrackingData.ColorMesh != null)
                        {
                            c.behaviors.MultiPolylineTracker(igh_Behavior.Value.TrackingData.Polylines, igh_Behavior.Value.TrackingData.PathThreshold, igh_Behavior.Value.TrackingData.ProjectionDistance, igh_Behavior.Value.TrackingData.PathRadius,
                                igh_Behavior.Value.TrackingData.MapThreshold, igh_Behavior.Value.TrackingData.MapProjection, igh_Behavior.Value.TrackingData.MapRadius, igh_Behavior.Value.TrackingData.ColorMesh);
                        }
                        else
                        {
                            c.behaviors.MultiPolylineTracker(igh_Behavior.Value.TrackingData.Polylines, igh_Behavior.Value.TrackingData.PathThreshold, igh_Behavior.Value.TrackingData.ProjectionDistance, igh_Behavior.Value.TrackingData.PathRadius);
                        }
                    }
                }
                else if(s == "Crawl")
                {
                    if (igh_Behavior.Value.MeshCrawlData.TriggerBabies)
                    {
                        c.behaviors.MeshWalk(igh_Behavior.Value.MeshCrawlData.Mesh, igh_Behavior.Value.MeshCrawlData.MeshThreshold, igh_Behavior.Value.MeshCrawlData.MeshProjectionDistance, igh_Behavior.Value.MeshCrawlData.Multiplier, igh_Behavior.Value.MeshCrawlData.TriggerBabies, true, igh_Behavior.Value.MeshCrawlData.MaxChildren, childSpawners, childSpawnType);
                        this.childSpawners = c.behaviors.GetMeshCrawler().GetChildStartPositions();
                        this.childSpawnType = c.behaviors.GetMeshCrawler().GetChildSpawnType();
                    }
                    else
                    {
                        c.behaviors.MeshWalk(igh_Behavior.Value.MeshCrawlData.Mesh, igh_Behavior.Value.MeshCrawlData.MeshThreshold, igh_Behavior.Value.MeshCrawlData.MeshProjectionDistance, igh_Behavior.Value.MeshCrawlData.Multiplier);
                    }
                }
                else if (s == "Stigmergy")
                {
                    totTail.AddRange(c.attributes.GetTrailVectors());
                    c.behaviors.SelfTailChase(igh_Behavior.Value.StigmergyData.ViewAngle, igh_Behavior.Value.StigmergyData.CohesionMagnitude, igh_Behavior.Value.StigmergyData.CohesionRange, igh_Behavior.Value.StigmergyData.SeparationMagnitude, igh_Behavior.Value.StigmergyData.SeparationRange, totTail);
                }
                else if (s == "Noise")
                {
                    if(igh_Behavior.Value.NoiseData.ColorMesh != null)
                    {
                        c.behaviors.Perlin2DMap(igh_Behavior.Value.NoiseData.Scale, igh_Behavior.Value.NoiseData.Strength, igh_Behavior.Value.NoiseData.Multiplier, igh_Behavior.Value.NoiseData.Velocity,
                            igh_Behavior.Value.NoiseData.MapStrength, igh_Behavior.Value.NoiseData.MapStrength, igh_Behavior.Value.NoiseData.MapMultiplier, igh_Behavior.Value.NoiseData.ColorMesh);
                    }
                    else
                    {
                        c.behaviors.Perlin(igh_Behavior.Value.NoiseData.Scale, igh_Behavior.Value.NoiseData.Strength, igh_Behavior.Value.NoiseData.Multiplier, igh_Behavior.Value.NoiseData.Velocity);
                    }
                }
                else if (s == "Separation")
                {
                    c.behaviors.Separate(igh_Behavior.Value.SeparationData.MaxSeparation, creepList);
                }
                else if (s == "Force")
                {
                    int forceAmount = igh_Behavior.Value.ForceData.Count;
                    for (int i = 0; i < forceAmount; i++)
                    {
                        if (igh_Behavior.Value.ForceData[i].ForceType == "Attract")
                        {
                            int attCount = 0;
                            foreach (Point3d p in igh_Behavior.Value.ForceData[i].Targets)
                            {
                                c.behaviors.Attract((Vector3d)p, igh_Behavior.Value.ForceData[i].Thresholds[attCount], igh_Behavior.Value.ForceData[i].AttractionValue, igh_Behavior.Value.ForceData[i].MaxAttraction);
                                attCount++;
                            }
                        }
                        else if (igh_Behavior.Value.ForceData[i].ForceType == "Repel")
                        {
                            int attCount = 0;
                            foreach (Point3d p in igh_Behavior.Value.ForceData[i].Targets)
                            {
                                c.behaviors.Repel((Vector3d)p, igh_Behavior.Value.ForceData[i].Thresholds[attCount], igh_Behavior.Value.ForceData[i].RepelValue, igh_Behavior.Value.ForceData[i].MaxRepel);
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

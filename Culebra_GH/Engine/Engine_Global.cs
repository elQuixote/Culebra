using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Culebra_GH.Objects;
using Grasshopper;
using System.Reflection;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Drawing;
using ikvm;
using processing.core;
using culebra.behaviors;
using CulebraData;
using CulebraData.Objects;
using CulebraData.Utilities;
using CulebraData.Drawing;
using System.Collections;
using System.Linq;
using Culebra_GH.Data_Structures;

namespace Culebra_GH.Engine
{
    public class Engine_Global
    {
        public List<Vector3d> childSpawners = new List<Vector3d>();
        public List<int> childSpawnType = new List<int>();

        public void SpawnChild(int dimensions, List<CulebraObject> creepList)
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

        public void FilterBehaviors(Creeper c, List<CulebraObject> creepList, object behavioral_Settings, int dimensions, List<Vector3d> childSpawners, List<int> childSpawnType, List<Vector3d> totTail)
        {
            IGH_BehaviorData igh_Behavior = (IGH_BehaviorData)behavioral_Settings;
            foreach (string s in igh_Behavior.Value.dataOrder)
            {
                if (s == "Flocking")
                {
                    if (dimensions == 0)
                    {
                        c.behaviors.Flock2D(igh_Behavior.Value.flockData.searchRadius, igh_Behavior.Value.flockData.cohesion_Value, igh_Behavior.Value.flockData.separation_Value, igh_Behavior.Value.flockData.alignment_Value, igh_Behavior.Value.flockData.viewAngle, creepList, igh_Behavior.Value.flockData.network);
                    }
                    else if (dimensions == 1)
                    {
                        c.behaviors.Flock3D(igh_Behavior.Value.flockData.searchRadius, igh_Behavior.Value.flockData.cohesion_Value, igh_Behavior.Value.flockData.separation_Value, igh_Behavior.Value.flockData.alignment_Value, igh_Behavior.Value.flockData.viewAngle, creepList, igh_Behavior.Value.flockData.network);
                    }
                }
                else if (s == "Wandering")
                {
                    if (dimensions == 0)
                    {
                        if (igh_Behavior.Value.wanderData.wanderingType == "Wander")
                        {
                            c.behaviors.Wander2D(igh_Behavior.Value.wanderData.randomize, igh_Behavior.Value.wanderData.addHeading, igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance);
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
                            c.behaviors.Wander3D(igh_Behavior.Value.wanderData.change, igh_Behavior.Value.wanderData.wanderingRadius, igh_Behavior.Value.wanderData.wanderingDistance, igh_Behavior.Value.wanderData.rotationTrigger);
                        }
                    }
                }
                else if (s == "Tracking")
                {
                    if (igh_Behavior.Value.trackingData.triggerBabies)
                    {
                        c.behaviors.MultiPolylineTrackerBabyMaker(igh_Behavior.Value.trackingData.polylines, igh_Behavior.Value.trackingData.pathThreshold, igh_Behavior.Value.trackingData.projectionDistance, igh_Behavior.Value.trackingData.pathRadius,
                            igh_Behavior.Value.trackingData.triggerBabies, igh_Behavior.Value.trackingData.maxChildren, true, childSpawners, childSpawnType);
                        this.childSpawners = c.behaviors.GetChildStartPositions();
                        this.childSpawnType = c.behaviors.GetChildSpawnTypes();
                    }
                    else
                    {
                        c.behaviors.MultiPolylineTracker(igh_Behavior.Value.trackingData.polylines, igh_Behavior.Value.trackingData.pathThreshold, igh_Behavior.Value.trackingData.projectionDistance, igh_Behavior.Value.trackingData.pathRadius);
                    }
                }
                else if (s == "Stigmergy")
                {
                    totTail.AddRange(c.attributes.GetTrailVectors());
                    c.behaviors.SelfTailChase(igh_Behavior.Value.stigmergyData.viewAngle, igh_Behavior.Value.stigmergyData.cohesionMagnitude, igh_Behavior.Value.stigmergyData.cohesionRange, igh_Behavior.Value.stigmergyData.separationMagnitude, igh_Behavior.Value.stigmergyData.separationRange, totTail);
                }
                else if (s == "Noise")
                {
                    c.behaviors.Perlin(igh_Behavior.Value.noiseData.scale, igh_Behavior.Value.noiseData.strength, igh_Behavior.Value.noiseData.multiplier, igh_Behavior.Value.noiseData.velocity);
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

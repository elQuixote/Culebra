using System;
using System.Collections.Generic;
using Rhino.Geometry;
using CulebraData.Objects;
using Rhino.Display;
using CulebraData.Behavior.Types;

namespace CulebraData.Behavior
{
    /// <summary>
    /// The <see cref="Behavior"/> namespace contains all Culebra Objects Behaviors
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGenerated]
    class NamespaceDoc
    {
    }

    /// <summary>
    /// Controller Class - Used to access Creeper Object's Behaviors. This class wraps the Culebra Java Objects behavior controller
    /// </summary>
    public class Controller
    {
        private MeshCrawler meshCrawler;
        private CulebraObject culebraObject;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The Culebra Object whose behaviors you want to access</param>
        public Controller(CulebraObject obj)
        {
            this.culebraObject = obj;
            this.meshCrawler = new MeshCrawler();
        }
        /// <summary>
        /// Gets the Mesh Crawler 
        /// </summary>
        /// <returns>The meshcrawler class instance </returns>
        public MeshCrawler GetMeshCrawler() { return this.meshCrawler; }
        #region MeshCrawling Behavior Methods
        /// <summary>
        /// Mesh Crawling allows agent to move along a mesh object
        /// </summary>
        /// <param name="mesh">the mesh object</param>
        /// <param name="meshThreshold">min distance current position needs to be from mesh in order to move to it</param>
        /// <param name="amplitude">the amount to project the current location along the current speed to get the predicted next location</param>
        /// <param name="multiplier"></param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="instanceable">if the object is instanceable it can reproduce. Only objects which inherit from the culebra.objects.Object class are instanceable.Child objects cannot produce more children</param>
        /// <param name="maxChildren">the max number of children each agent can create</param>
        /// <param name="childList"></param>
        /// <param name="childTypeList"></param>
        public void MeshWalk(Mesh mesh, float meshThreshold, float amplitude, float multiplier, bool triggerBabies = false, bool instanceable = false, int maxChildren = 0, List<Vector3d> childList = null, List<int> childTypeList = null)
        {
            this.culebraObject.attributes.GetObjType();
            this.culebraObject.GetObject().behavior.setBehaviorType("Crawler");
            Vector3d direction;       
            if ((this.culebraObject.GetObject().behavior.getObjType() != "culebra.objects.BabyCreeper" && this.culebraObject.GetObject().behavior.getObjType() != "culebra.objects.BabySeeker")
                    || (this.culebraObject.GetObject().behavior.getSuperClass() != "culebra.objects.BabyCreeper"
                            && this.culebraObject.GetObject().behavior.getSuperClass() != "culebra.objects.BabySeeker"))
            {
                direction = this.meshCrawler.MeshWalk(mesh, meshThreshold, Utilities.Convert.ToPVec(this.culebraObject.attributes.GetLocation()), Utilities.Convert.ToPVec(this.culebraObject.attributes.GetSpeed()), amplitude, multiplier, triggerBabies, instanceable, maxChildren, childList, childTypeList);
            }
            else
            {
                direction = this.meshCrawler.MeshWalk(mesh, meshThreshold, Utilities.Convert.ToPVec(this.culebraObject.attributes.GetLocation()), Utilities.Convert.ToPVec(this.culebraObject.attributes.GetSpeed()), amplitude, multiplier, triggerBabies, false, maxChildren);
            }
            this.culebraObject.behaviors.ApplyForce(direction);
        }
        #endregion
        #region Flocking Behavior Methods
        /// <summary>
        /// Alignment Behavior steers towards average heading of neighbors for use with culebra.objects.Object type
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="alignValue">steers towards average heading of neighbors. Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void Align(float searchRadius, float alignValue, List<CulebraObject> collection)
        {
            this.culebraObject.GetObject().behavior.align(searchRadius, alignValue, Utilities.Convert.ToJavaList(collection));
        }
        /// <summary>
        /// Cohesion Behavior steers towards average positions of neighbors (long range attraction) for use with culebra.objects.Object type
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="cohesionValue">steers towards average positions of neighbors (long range attraction). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void Cohesion(float searchRadius, float cohesionValue, List<CulebraObject> collection)
        {
            this.culebraObject.GetObject().behavior.cohesion(searchRadius, cohesionValue, Utilities.Convert.ToJavaList(collection));
        }
        /// <summary>
        /// Separation Behavior for use with culebra.objects.Object type - avoids crowding neighbors (short range repulsion)
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="separateValue">avoids crowding neighbors (short range repulsion). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void Separate(float searchRadius, float separateValue, List<CulebraObject> collection)
        {
            this.culebraObject.GetObject().behavior.separate(searchRadius, separateValue, Utilities.Convert.ToJavaList(collection));
        }
        /// <summary>
        /// Separation Behavior II for use with culebra.objects.Object type - avoids crowding neighbors (short range repulsion)
        /// </summary>
        /// <param name="maxSeparation">maxDistance threshold to enable separate</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void Separate(float maxSeparation, List<CulebraObject> collection)
        {
            this.culebraObject.GetObject().behavior.separate(maxSeparation, Utilities.Convert.ToJavaList(collection));
        }
        /// <summary>
        /// Overloaded 2D Separation Algorithm with image color sampling override for any behavior attribute with color value remapping for use with culebra.objects.Object type
        /// </summary>
        /// <param name="maxSeparation">maxDistance threshold to enable separate</param>
        /// <param name="collection">list of culebra.objects.Object</param>
        /// <param name="mapSeparation">uses mesh color data as multiplier for separation value</param>
        /// <param name="minVal">minimum value to remap color data</param>
        /// <param name="maxVal">maximum value to remap color data</param>
        /// <param name="coloredMesh">the colored Mesh to sample</param>
        public void Separate(float maxSeparation, List<CulebraObject> collection, bool mapSeparation, float minVal, float maxVal, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            float mappedValue = Utilities.Mapping.Map((float)hls.L, 0, 1.0f, minVal,maxVal);
            if (mapSeparation)
            {
                this.culebraObject.GetObject().behavior.separate(mappedValue, Utilities.Convert.ToJavaList(collection));
            }
            else
            {
                this.culebraObject.GetObject().behavior.separate(maxSeparation, Utilities.Convert.ToJavaList(collection));
            }          
        }
        /// <summary>
        /// 2D Separation Algorithm with image color sampling override for any behavior attribute for use with culebra.objects.Object type
        /// </summary>
        /// <param name="maxSeparation">maxDistance threshold to enable separate</param>
        /// <param name="collection">list of other culebra.objects.Object</param>
        /// <param name="mapSeparation">uses image color data as multiplier for separation value</param>
        /// <param name="coloredMesh">the colored Mesh to sample</param>
        public void Separate(float maxSeparation, List<CulebraObject> collection, bool mapSeparation, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapSeparation)
            {
                maxSeparation *= (float)hls.L;
            }
            this.culebraObject.GetObject().behavior.separate(maxSeparation, Utilities.Convert.ToJavaList(collection));
        }
        /// <summary>
        /// Overloaded 2D Flocking for use with culebra.objects.Object type - this example adds an angle parameter which allows agents to see only within the angle specified
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="cohesionValue">cohesion value steers towards average positions of neighbors (long range attraction). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="separateValue">separateValue separate value avoids crowding neighbors (short range repulsion). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="alignValue">align value steers towards average heading of neighbors. Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="viewAngle">allowable vision angle in degrees</param>
        /// <param name="creeperList">list of other Creeper Objects</param>
        /// <param name="drawSearchConnectivity">network visualizing search radius</param>
        public void Flock2D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<CulebraObject> creeperList, bool drawSearchConnectivity)
        {
            this.culebraObject.GetObject().behavior.flock2D(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.ToJavaList(creeperList), Utilities.Convert.ToJavaBool(drawSearchConnectivity));

        }
        /// <summary>
        /// Overloaded 2D Flocking for use with culebra.objects.Object type - this example adds an angle parameter which allows agents to see only within the angle specified
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="cohesionValue">cohesion value steers towards average positions of neighbors (long range attraction). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="separateValue">separateValue separate value avoids crowding neighbors (short range repulsion). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="alignValue">align value steers towards average heading of neighbors. Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="viewAngle">allowable vision angle in degrees</param>
        /// <param name="creeperList">list of other Creeper Objects</param>
        /// <param name="drawSearchConnectivity">network visualizing search radius</param>
        public void Flock3D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<CulebraObject> creeperList, bool drawSearchConnectivity)
        {
            this.culebraObject.GetObject().behavior.flock(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.ToJavaList(creeperList), drawSearchConnectivity);
        }
        /// <summary>
        /// 2D Flocking Algorithm with mesh color sampling override for any behavior attribute for use with culebra.objects.Object type
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="cohesionValue">cohesion value steers towards average positions of neighbors (long range attraction). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="separateValue">separateValue separate value avoids crowding neighbors (short range repulsion). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="alignValue">align value steers towards average heading of neighbors. Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="viewAngle">allowable vision angle in degrees</param>
        /// <param name="creeperList">list of other Creeper Objects</param>
        /// <param name="drawSearchConnectivity">network visualizing search radius</param>
        /// <param name="mapAlignment">uses mesh color data as multiplier for alignment value</param>
        /// <param name="mapSeparation">uses mesh color data as multiplier for separation value</param>
        /// <param name="mapCohesion">uses mesh color data as multiplier for cohesion value</param>
        /// <param name="coloredMesh">the colored Mesh to sample</param>
        public void Flock2D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<CulebraObject> creeperList, bool drawSearchConnectivity, bool mapAlignment, bool mapSeparation, bool mapCohesion, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapAlignment) alignValue *= (float)hls.L;
            if (mapSeparation) separateValue *= (float)hls.L;
            if (mapCohesion) cohesionValue *= (float)hls.L;

            this.culebraObject.GetObject().behavior.flock2D(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.ToJavaList(creeperList), Utilities.Convert.ToJavaBool(drawSearchConnectivity));
        }
        /// <summary>
        /// 3D Flocking Algorithm with mesh color sampling override for any behavior attribute for use with culebra.objects.Object type
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="cohesionValue">cohesion value steers towards average positions of neighbors (long range attraction). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="separateValue">separateValue separate value avoids crowding neighbors (short range repulsion). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="alignValue">align value steers towards average heading of neighbors. Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="viewAngle">allowable vision angle in degrees</param>
        /// <param name="creeperList">list of other Creeper Objects</param>
        /// <param name="drawSearchConnectivity">network visualizing search radius</param>
        /// <param name="mapAlignment">uses mesh color data as multiplier for alignment value</param>
        /// <param name="mapSeparation">uses mesh color data as multiplier for separation value</param>
        /// <param name="mapCohesion">uses mesh color data as multiplier for cohesion value</param>
        /// <param name="coloredMesh">the colored Mesh to sample</param>
        public void Flock3D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<CulebraObject> creeperList, bool drawSearchConnectivity, bool mapAlignment, bool mapSeparation, bool mapCohesion, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapAlignment) alignValue *= (float)hls.L;
            if (mapSeparation) separateValue *= (float)hls.L;
            if (mapCohesion) cohesionValue *= (float)hls.L;

            this.culebraObject.GetObject().behavior.flock(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.ToJavaList(creeperList), drawSearchConnectivity);
        }
        #endregion
        #region Wandering Behavior Methods
        /// <summary>
        /// 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="randomize">if true then the change value will be randomly selected from -change value to change value each frame</param>
        /// <param name="addHeading">if true adds the heading to the wandertheta</param>
        /// <param name="change">the incremented change value used to get the polar coordinates.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander2D(Boolean randomize, Boolean addHeading, float change, float wanderR, float wanderD)
        {
            this.culebraObject.GetObject().behavior.wander2D(new java.lang.Boolean(randomize), new java.lang.Boolean(addHeading), change, wanderR, wanderD);
        }
        /// <summary>
        /// Overloaded 2D Wander Algorithm - by default this one randmoizes the change value from -change to change and incorporates the heading 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">the incremented change value used to get the polar coordinates.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander2D(float change, float wanderR, float wanderD)
        {
            this.culebraObject.GetObject().behavior.wander2D(change, wanderR, wanderD);
        }
        /// <summary>
        /// Overloaded Mapped 2D Wandering Algorithm with image color sampling override for any behavior attribute.No remapping capabilities in this method, min and max image values are fixed 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation.And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="randomize">if true then the change value will be randomly selected from change value to change value each frame</param>
        /// <param name="addHeading">if true adds the heading to the wandertheta</param>
        /// <param name="change">the incremented change value used to get the polar coordinates.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="mapChange">uses mesh color data as multiplier for wander change value</param>
        /// <param name="mapWanderR">uses mesh color data as multiplier for wander circle radius value</param>
        /// <param name="mapWanderD">uses mesh color data as multiplier for wander circle distance value</param>
        /// <param name="coloredMesh">the colored Mesh to sample</param>
        public void Wander2D(bool randomize, bool addHeading, float change, float wanderR, float wanderD, bool mapChange, bool mapWanderR, bool mapWanderD, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapChange) change *= (float)hls.L;
            if (mapWanderR) wanderR *= (float)hls.L;
            if (mapWanderD) wanderD *= (float)hls.L;

            this.culebraObject.GetObject().behavior.wander2D(new java.lang.Boolean(randomize), new java.lang.Boolean(addHeading), change, wanderR, wanderD);
        }
        /// <summary>
        /// Overloaded Expanded 2D Wandering Algorithm using triggers to create a "weaving" type movement 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        public void SuperWander2D(float change, float wanderR, float wanderD, float rotationTrigger)
        {
            this.culebraObject.GetObject().behavior.superWander2D(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// Expanded 2D Wandering Algorithm using triggers to create a "weaving" type movement. USE WITH NON CREEPER DERIVED OBJECTS, if you create your own object use this and pass the objtype below. 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        /// <param name="objType">to use with generic objects not derrived from culebra.objects.Creeper. Input "Parent" for parent objects and "Child" for child objects</param>
        public void SuperWander2D(float change, float wanderR, float wanderD, float rotationTrigger, String objType)
        {
            this.culebraObject.GetObject().behavior.superWander2D(change, wanderR, wanderD, rotationTrigger, objType);
        }
        /// <summary>
        /// Expanded 3D Wandering Algorithm - Type "Primary" using triggers to create a "weaving" type movement. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        public void Wander3D(float change, float wanderR, float wanderD, float rotationTrigger)
        {
            this.culebraObject.GetObject().behavior.wander3D(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// Mapped 3D Wandering Algorithm with image color sampling override for any behavior attribute 3D Wandering Algorithm
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        /// <param name="mapChange">uses mesh color data as multiplier for change value</param>
        /// <param name="mapWanderR">uses mesh color data as multiplier for radius value</param>
        /// <param name="mapWanderD">uses mesh color data as multiplier for distance value</param>
        /// <param name="mapRotationTrigger">uses mesh color data as multiplier for rotation trigger value</param>
        /// <param name="coloredMesh">the colored mesh to sample</param>
        public void Wander3D(float change, float wanderR, float wanderD, float rotationTrigger, bool mapChange, bool mapWanderR, bool mapWanderD, bool mapRotationTrigger, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapChange) change *= (float)hls.L;
            if (mapWanderR) wanderR *= (float)hls.L;
            if (mapWanderD) wanderD *= (float)hls.L;
            if (mapRotationTrigger) rotationTrigger *= (float)hls.L;

            this.culebraObject.GetObject().behavior.wander3D(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// Expanded 3D Wandering Algorithm - Type "B" using triggers to create a "weaving" type movement. Type B uses a different assortment of Heading variations creating a differnt type of behavior. These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        public void Wander3D_subA(float change, float wanderR, float wanderD, float rotationTrigger)
        {
            this.culebraObject.GetObject().behavior.wander3D_subA(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// Expanded 3D Wandering Algorithm - Type "C" using triggers to create a "weaving" type movement. Type B uses a different assortment of Heading variations creating a differnt type of behavior. These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        public void Wander3D_subB(float change, float wanderR, float wanderD, float rotationTrigger)
        {
            this.culebraObject.GetObject().behavior.wander3D_subB(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// 3D Wandering Algorithm - Type "MOD" uses no Z value These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander3D_Mod(float change, float wanderR, float wanderD)
        {
            this.culebraObject.GetObject().behavior.wander3D_Mod(change, wanderR, wanderD);
        }
        /// <summary>
        /// 3D Wandering Algorithm - Type "MOD2" uses randomized sin and cos values with the wandertheta for the Z value These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander3D_Mod2(float change, float wanderR, float wanderD)
        {
            this.culebraObject.GetObject().behavior.wander3D_Mod2(change, wanderR, wanderD);
        }
        /// <summary>
        /// 3D Wandering Algorithm - Type "MOD3" uses randomized sin and cos values with the wandertheta for all PVector components These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander3D_Mod3(float change, float wanderR, float wanderD)
        {
            this.culebraObject.GetObject().behavior.wander3D_Mod3(change, wanderR, wanderD);
        }
        #endregion
        #region Trail Chasing - Polyline Tracking Methods
        /// <summary>
        /// 2D/3D Trail Chasing Algorithm - Agents will chase all other agents trails. When using this algorithm in your main sketch use the overloaded move method, recommended values are move(6,100)
        /// </summary>
        /// <param name="tailViewAngle">allowable vision angle in degrees.</param>
        /// <param name="tailCohMag">cohesion value steers towards average positions.</param>
        /// <param name="tailCohViewRange">cohesion threshold, value within range will enable tailCohMag</param>
        /// <param name="tailSepMag">separation value avoids crowding on trail.</param>
        /// <param name="tailSepViewRange">separation threshold, value within range will enable tailSepMag</param>
        /// <param name="trailsPts">list of all PVectors from all trails - see example file</param>
        public void SelfTailChase(float tailViewAngle, float tailCohMag, float tailCohViewRange, float tailSepMag, float tailSepViewRange, List<Vector3d> trailsPts)
        {
            this.culebraObject.GetObject().behavior.selfTailChaser(tailViewAngle, tailCohMag, tailCohViewRange, tailSepMag, tailSepViewRange, Utilities.Convert.ToPVecList(trailsPts));
        }
        /// <summary>
        /// Shape Following Algorithm - Requires a polyline to track against. - see example files
        /// </summary>
        /// <param name="polyline">A single polyline to track</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        public void PolylineTracker(Polyline polyline, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.culebraObject.GetObject().behavior.shapeTracker(Utilities.Convert.PolylineToShape(polyline), shapeThreshold, projectionDistance, shapeRadius);
        }
        /// <summary>
        /// Multi Shape Following Algorithm - Requires list of Polylines, the polylines get converted into Arraylist of PVectors defining a each shapes points. - see example files
        /// </summary>
        /// <param name="multiShapeList">A list of polylines to track against</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        public void MultiPolylineTracker(java.util.List multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.culebraObject.GetObject().behavior.multiShapeTracker(multiShapeList, shapeThreshold, projectionDistance, shapeRadius);
        }
        /// <summary>
        /// Multi Shape Following Algorithm with mesh color sampling override for any shape attributes - Requires list of Polylines, the polylines get converted into Arraylist of PVectors defining a each shapes points. - see example files
        /// </summary>
        /// <param name="multiShapeList">A list of polylines to track against</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        /// <param name="mapThreshold">uses mesh color data as multiplier for threshold value</param>
        /// <param name="mapDistance">uses mesh color data as multiplier for distance value</param>
        /// <param name="mapRadius">uses mesh color data as multiplier for radius value</param>
        /// <param name="coloredMesh">the colored mesh to sample</param>
        public void MultiPolylineTracker(java.util.List multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool mapThreshold, bool mapDistance, bool mapRadius, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapThreshold) shapeThreshold *= (float)hls.L;
            if (mapDistance) projectionDistance *= (float)hls.L;
            if (mapRadius) shapeRadius *= (float)hls.L;

            this.culebraObject.GetObject().behavior.multiShapeTracker(multiShapeList, shapeThreshold, projectionDistance, shapeRadius);
        }
        /// <summary>
        /// MultiShape Following Algorithm capable of spawning children - Requires list of Polylines, the polylines get converted into Arraylist of PVectors defining a each shapes points - see example files
        /// </summary>
        /// <param name="multiShapeList">A list of polylines to track against</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="maxChildren">the max number of children each agent can create</param>
        /// <param name="instanceable">if the child is instanceable it can reproduce. Only objects which inherit from the culebra.objects.Object class are instanceable. Child objects cannot produce more children</param>
        /// <param name="objTypeOverride">input type to override objtype in even of custom object. Use "Parent" as the string override for objects you want to be able to spawn children</param>
        /// <param name="childList">list of stored children to spawn next. use (current object).behaviors.getChildStartPositions() to get them</param>
        /// <param name="childTypeList">list of values used to alter types of children. use (current object).behaviors.getChildSpawnType() to get it.</param>
        public void MultiPolylineTrackerBabyMaker(java.util.List multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, String objTypeOverride, List<Vector3d> childList, List<int> childTypeList)
        {
            this.culebraObject.GetObject().behavior.multiShapeTrackerBabyMaker(multiShapeList, shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, objTypeOverride, Utilities.Convert.ToPVecList(childList), Utilities.Convert.ToJavaIntList(childTypeList));
        }
        /// <summary>
        /// MultiShape Following Algorithm capable of spawning children - Requires list of Polylines, the polylines get converted into Arraylist of PVectors defining a each shapes points - see example files
        /// </summary>
        /// <param name="multiShapeList">A list of polylines to track against</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="maxChildren">the max number of children each agent can create</param>
        /// <param name="instanceable">if the child is instanceable it can reproduce. Only objects which inherit from the culebra.objects.Object class are instanceable. Child objects cannot produce more children</param>
        /// <param name="childList">list of stored children to spawn next. use (current object).behaviors.getChildStartPositions() to get them</param>
        /// <param name="childTypeList">list of values used to alter types of children. use (current object).behaviors.getChildSpawnType() to get it.</param>
        public void MultiPolylineTrackerBabyMaker(java.util.List multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList)
        {
            this.culebraObject.attributes.GetObjType();
            this.culebraObject.GetObject().behavior.multiShapeTrackerBabyMaker(multiShapeList, shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.ToPVecList(childList), Utilities.Convert.ToJavaIntList(childTypeList));
        }
        /// <summary>
        /// MultiShape Following Algorithm capable of spawning children with mesh color sampling override for any shape attributes - Requires list of Polylines, the polylines get converted into Arraylist of PVectors defining a each shapes points - see example files
        /// </summary>
        /// <param name="multiShapeList">A list of polylines to track against</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="maxChildren">the max number of children each agent can create</param>
        /// <param name="instanceable">if the child is instanceable it can reproduce. Only objects which inherit from the culebra.objects.Object class are instanceable. Child objects cannot produce more children</param>
        /// <param name="childList">list of stored children to spawn next. use (current object).behaviors.getChildStartPositions() to get them</param>
        /// <param name="childTypeList">list of values used to alter types of children. use (current object).behaviors.getChildSpawnType() to get it.</param>
        /// <param name="mapThreshold">uses mesh color data as multiplier for threshold value</param>
        /// <param name="mapDistance">uses mesh color data as multiplier for distance value</param>
        /// <param name="mapRadius">uses mesh color data as multiplier for radius value</param>
        /// <param name="coloredMesh">the colored mesh to sample</param>
        public void MultiPolylineTrackerBabyMaker(java.util.List multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList, bool mapThreshold, bool mapDistance, bool mapRadius, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapThreshold) shapeThreshold *= (float)hls.L;
            if (mapDistance) projectionDistance *= (float)hls.L;
            if (mapRadius) shapeRadius *= (float)hls.L;

            this.culebraObject.attributes.GetObjType();
            this.culebraObject.GetObject().behavior.multiShapeTrackerBabyMaker(multiShapeList, shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.ToPVecList(childList), Utilities.Convert.ToJavaIntList(childTypeList));
        }
        /// <summary>
        /// Other Object Trails Following Algorithm - Meant for Seeker or sub Seeker types of objects. These objects will chase the trails of other objects - see example files
        /// </summary>
        /// <param name="trailsPts">list of all PVectors from all trails</param>
        /// <param name="trailThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="trailRadius">the radius of the shapes</param>
        public void TrailFollowers(List<List<Vector3d>> trailsPts, float trailThreshold, float projectionDistance, float trailRadius)
        {
            this.culebraObject.GetObject().behavior.trailFollower(Utilities.Convert.NestedList_To_NestedArrayList(trailsPts), trailThreshold, projectionDistance, trailRadius);
        }
        /// <summary>
        /// Other Object Trails Following Algorithm capable of spawning children - Meant for Seeker or sub Seeker types of objects. These objects will chase the trails of other objects
        /// </summary>
        /// <param name="trailsPts">list of all PVectors from all trails</param>
        /// <param name="trailThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="trailRadius">the radius of the shapes</param>
        /// <param name="triggerBabies">if true agent is now allowed to spawn any babies stored</param>
        /// <param name="maxChildren">the max number of children each agent can create</param>
        /// <param name="instanceable">if the child is instanceable it can reproduce. Only objects which inherit from the culebra.objects.Object class are instanceable. Child objects cannot produce more children</param>
        /// <param name="childList">list of stored children to spawn next. use (current object).behaviors.getChildStartPositions() to get them</param>
        /// <param name="childTypeList">list of values used to alter types of children. use (current object).behaviors.getChildSpawnType() to get it.</param>
        public void TrailFollowersBabyMakers(List<Vector3d> trailsPts, float trailThreshold, float projectionDistance, float trailRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList)
        {
            this.culebraObject.GetObject().behavior.trailFollowerBabyMaker(Utilities.Convert.ToPVecList(trailsPts), trailThreshold, projectionDistance, trailRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.ToPVecList(childList), Utilities.Convert.ToJavaIntList(childTypeList));
        }
        /// <summary>
        /// Gets the child start positions if any
        /// </summary>
        /// <returns>the child start positions</returns>
        public List<Vector3d> GetChildStartPositions() 
        {
            return Utilities.Convert.ToVec3DList(this.culebraObject.GetObject().behavior.getChildStartPositions());
        }
        /// <summary>
        /// Gets the child spawn types if any
        /// </summary>
        /// <returns>the child spawn type</returns>
        public List<int> GetChildSpawnTypes() 
        {
            return Utilities.Convert.ToIntList(this.culebraObject.GetObject().behavior.getChildSpawnType());
        }
        #endregion
        #region Forces Methods
        /// <summary>
        /// Calculates a steering force towards a target as defined by Daniel Shiffmans implementation of Craig Reynolds steering force.
        /// </summary>
        /// <param name="targetVector">the target to steer towards</param>
        public void Seek(Vector3d targetVector)
        {
            this.culebraObject.GetObject().behavior.seek(Utilities.Convert.ToPVec(targetVector));
        }
        /// <summary>
        /// Calculates a steering force towards a target as defined by Daniel Shiffmans implementation of Craig Reynolds steering force.
        /// </summary>
        /// <param name="targetVector">the target to steer towards</param>
        /// <param name="amplitude">amount to multiply the steer vectory by</param>
        public void Seek(Vector3d targetVector, float amplitude)
        {
            this.culebraObject.GetObject().behavior.seek(Utilities.Convert.ToPVec(targetVector), amplitude);
        }
        /// <summary>
        /// Calculates a steering force towards a target as defined by Daniel Shiffmans implementation of Craig Reynolds steering force.
        /// </summary>
        /// <param name="targetVector">the target to steer towards</param>
        /// <param name="normalize">option to normalize the desired parameter</param>
        public void Seek(Vector3d targetVector, bool normalize)
        {
            this.culebraObject.GetObject().behavior.seek(Utilities.Convert.ToPVec(targetVector), normalize);
        }
        /// <summary>
        /// Attracts a object towards a target. Differs from Seek
        /// </summary>
        /// <param name="target">target to attract towards</param>
        /// <param name="threshold">if target is within this threshold then attract towards it</param>
        /// <param name="attractionValue">value specifying attraction, this is the magnitude.</param>
        /// <param name="maxAttraction">maximum attraction value</param>
        public void Attract(Vector3d target, float threshold, float attractionValue, float maxAttraction)
        {
            this.culebraObject.GetObject().behavior.attract(Utilities.Convert.ToPVec(target), threshold, attractionValue, maxAttraction);
        }
        /// <summary>
        /// Repels a object away from a target.
        /// </summary>
        /// <param name="target">target to repel away from</param>
        /// <param name="threshold">if target is within this threshold then repel away from it</param>
        /// <param name="repelValue">value specifying repulsion, this is the magnitude.</param>
        /// <param name="maxRepel">maximum repulsion value</param>
        public void Repel(Vector3d target, float threshold, float repelValue, float maxRepel)
        {
            this.culebraObject.GetObject().behavior.repel(Utilities.Convert.ToPVec(target), threshold, repelValue, maxRepel);
        }
        /// <summary>
        /// Applies the force vector to the acceleration and adds it to the current speed.
        /// </summary>
        /// <param name="force">vector to add to acceleration</param>
        public void ApplyForce(Vector3d force)
        {
            this.culebraObject.GetObject().behavior.applyForce(Utilities.Convert.ToPVec(force));
        }
        #endregion
        #region Noise Methods
        /// <summary>
        /// 2D/3D Improved Perlin Noise
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">value adds a jitter type of movement</param>
        public void Perlin(float scale, float strength, float multiplier, float velocity)
        {
            this.culebraObject.GetObject().behavior.perlin(scale, strength, multiplier, velocity);
        }
        /// <summary>
        /// 2D Improved Perlin Noise with mesh color sampling override for any behavior attribute
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">value adds a jitter type of movement</param>
        /// <param name="mapScale">uses mesh color data as multiplier for scale value</param>
        /// <param name="mapStrength">uses mesh color data as multiplier for strength value</param>
        /// <param name="mapMultiplier">uses mesh color data as multiplier for multiplier value</param>
        /// <param name="coloredMesh">the colored mesh to sample</param>
        public void Perlin2DMap(float scale, float strength, float multiplier, float velocity, bool mapScale, bool mapStrength, bool mapMultiplier, Mesh coloredMesh)
        {
            ColorHSL hls = Utilities.ColorUtility.GetHueSatLum(this.culebraObject.attributes.GetLocation(), coloredMesh);
            if (mapScale) scale *= (float)hls.L;
            if (mapStrength) strength *= (float)hls.L;
            if (mapMultiplier) multiplier *= (float)hls.L;

            this.culebraObject.GetObject().behavior.perlin(scale, strength, multiplier, velocity);
        }
        /// <summary>
        /// 2D/3D Modified Improved Perlin Noise Type A. Randomized remapped Scale value adjustments.
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">multiplier value for velocity</param>
        /// <param name="modMultiplier">multiplier value multiplied to the scale</param>
        /// <param name="modScaleDivider">number to divide the result of the scale * modMultiplier</param>
        public void NoiseModified_A(float scale, float strength, float multiplier, float velocity, float modMultiplier,
            float modScaleDivider)
        {
            this.culebraObject.GetObject().behavior.noiseModified_A(scale, strength, multiplier, velocity, modMultiplier, modScaleDivider);
        }
        /// <summary>
        /// 2D/3D Modified Improved Perlin Noise Type B. Randomized remapped Scale value adjustments.
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">multiplier value for velocity</param>
        /// <param name="modMultiplier">multiplier value multiplied to the scale</param>
        public void NoiseModified_B(float scale, float strength, float multiplier, float velocity, float modMultiplier)
        {
            this.culebraObject.GetObject().behavior.noiseModified_B(scale, strength, multiplier, velocity, modMultiplier);
        }
        /// <summary>
        /// 2D/3D Modified Improved Perlin Noise Type C. Randomized remapped Strength value adjustments.
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">multiplier value for velocity</param>
        /// <param name="modMultiplier">multiplier value multiplied to the scale</param>
        public void NoiseModified_C(float scale, float strength, float multiplier, float velocity, float modMultiplier)
        {
            this.culebraObject.GetObject().behavior.noiseModified_C(scale, strength, multiplier, velocity, modMultiplier);
        }
        #endregion
    }
}

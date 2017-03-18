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
using CulebraData.Utilities;
using CulebraData.Objects;

namespace CulebraData.Behavior
{
    /// <summary>
    /// Controller Class - Used to acess Creeper Object's Behaviors
    /// </summary>
    public class Controller
    {
        private Creeper creeper;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">The Creeper Object whose behaviors you want to access</param>
        public Controller(Creeper obj)
        {
            this.creeper = obj;
        }
        /// <summary>
        /// Alignment Behavior steers towards average heading of neighbors for use with culebra.objects.Object type
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="alignValue">steers towards average heading of neighbors. Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void align(float searchRadius, float alignValue, List<Creeper> collection)
        {
            this.creeper.getCreeperObject().behavior.align(searchRadius, alignValue, Utilities.Convert.toJavaList(collection));
        }
        /// <summary>
        /// Cohesion Behavior steers towards average positions of neighbors (long range attraction) for use with culebra.objects.Object type
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="cohesionValue">steers towards average positions of neighbors (long range attraction). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void cohesion(float searchRadius, float cohesionValue, List<Creeper> collection)
        {
            this.creeper.getCreeperObject().behavior.cohesion(searchRadius, cohesionValue, Utilities.Convert.toJavaList(collection));
        }
        /// <summary>
        /// Separation Behavior for use with culebra.objects.Object type - avoids crowding neighbors (short range repulsion)
        /// </summary>
        /// <param name="searchRadius">distance each culebra.objects.Object can see</param>
        /// <param name="separateValue">avoids crowding neighbors (short range repulsion). Is only enabled for whatever agents are within the search radius.</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void separate(float searchRadius, float separateValue, List<Creeper> collection)
        {
            this.creeper.getCreeperObject().behavior.separate(searchRadius, separateValue, Utilities.Convert.toJavaList(collection));
        }
        /// <summary>
        /// Separation Behavior II for use with culebra.objects.Object type - avoids crowding neighbors (short range repulsion)
        /// </summary>
        /// <param name="maxSeparation">maxDistance threshold to enable separate</param>
        /// <param name="collection">list of other Creeper Objects</param>
        public void separate(float maxSeparation, List<Creeper> collection)
        {
            this.creeper.getCreeperObject().behavior.separate(maxSeparation, Utilities.Convert.toJavaList(collection));
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
        public void flock2D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<Creeper> creeperList, bool drawSearchConnectivity)
        {
            this.creeper.getCreeperObject().behavior.flock2D(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.toJavaList(creeperList), Utilities.Convert.toJavaBool(drawSearchConnectivity));
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
        public void flock3D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<Creeper> creeperList, bool drawSearchConnectivity)
        {
            this.creeper.getCreeperObject().behavior.flock(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.toJavaList(creeperList), drawSearchConnectivity);
        }
        /// <summary>
        /// 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="randomize">if true then the change value will be randomly selected from -change value to change value each frame</param>
        /// <param name="addHeading">if true adds the heading to the wandertheta</param>
        /// <param name="change">the incremented change value used to get the polar coordinates.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void wander2D(Boolean randomize, Boolean addHeading, float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander2D(new java.lang.Boolean(randomize), new java.lang.Boolean(addHeading), change, wanderR, wanderD);
        }
        /// <summary>
        /// Overloaded 2D Wander Algorithm - by default this one randmoizes the change value from -change to change and incorporates the heading 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">the incremented change value used to get the polar coordinates.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void wander2D(float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander2D(change, wanderR, wanderD);
        }
        /// <summary>
        /// Overloaded Expanded 2D Wandering Algorithm using triggers to create a "weaving" type movement 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        public void superWander2D(float change, float wanderR, float wanderD, float rotationTrigger)
        {
            this.creeper.getCreeperObject().behavior.superWander2D(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// Expanded 2D Wandering Algorithm using triggers to create a "weaving" type movement. USE WITH NON CREEPER DERIVED OBJECTS, if you create your own object use this and pass the objtype below. 2D Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        /// <param name="rotationTrigger">this value is compared against each movement step. If rotationTrigger value > iteration count then we will reverse the change value.</param>
        /// <param name="objType">to use with generic objects not derrived from culebra.objects.Creeper. Input "Parent" for parent objects and "Child" for child objects</param>
        public void superWander2D(float change, float wanderR, float wanderD, float rotationTrigger, String objType)
        {
            this.creeper.getCreeperObject().behavior.superWander2D(change, wanderR, wanderD, rotationTrigger, objType);
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
            this.creeper.getCreeperObject().behavior.wander3D(change, wanderR, wanderD, rotationTrigger);
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
            this.creeper.getCreeperObject().behavior.wander3D_subA(change, wanderR, wanderD, rotationTrigger);
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
            this.creeper.getCreeperObject().behavior.wander3D_subB(change, wanderR, wanderD, rotationTrigger);
        }
        /// <summary>
        /// 3D Wandering Algorithm - Type "MOD" uses no Z value These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander3D_Mod(float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander3D_Mod(change, wanderR, wanderD);
        }
        /// <summary>
        /// 3D Wandering Algorithm - Type "MOD2" uses randomized sin and cos values with the wandertheta for the Z value These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander3D_Mod2(float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander3D_Mod2(change, wanderR, wanderD);
        }
        /// <summary>
        /// 3D Wandering Algorithm - Type "MOD3" uses randomized sin and cos values with the wandertheta for all PVector components These variations are best used with tracking behaviors. Wandering Algorithm - "Agent predicts its future location as a fixed distance in front of it (in the direction of its velocity), draws a circle with radius r at that location, and picks a random point along the circumference of the circle. That random point moves randomly around the circle in each frame of animation. And that random point is the vehicles target, its desired vector pointing in that direction" - Daniel Shiffman on Craig Reynolds Wandering Behavior
        /// </summary>
        /// <param name="change">NON incremented change value used to get the polar coordinates. As opposed to other wander examples this one does not increment the theta value, we simply use whichever value is given and use the trigger to specify which direction the rotation will occur.</param>
        /// <param name="wanderR">the radius for the circle</param>
        /// <param name="wanderD">the distance for the wander circle, this is a projection value in the direction of the objects speed vector.</param>
        public void Wander3D_Mod3(float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander3D_Mod3(change, wanderR, wanderD);
        }
        /// <summary>
        /// 2D/3D Trail Chasing Algorithm - Agents will chase all other agents trails. When using this algorithm in your main sketch use the overloaded move method, recommended values are move(6,100)
        /// </summary>
        /// <param name="tailViewAngle">allowable vision angle in degrees.</param>
        /// <param name="tailCohMag">cohesion value steers towards average positions.</param>
        /// <param name="tailCohViewRange">cohesion threshold, value within range will enable tailCohMag</param>
        /// <param name="tailSepMag">separation value avoids crowding on trail.</param>
        /// <param name="tailSepViewRange">separation threshold, value within range will enable tailSepMag</param>
        /// <param name="trailsPts">list of all PVectors from all trails - see example file</param>
        public void selfTailChase(float tailViewAngle, float tailCohMag, float tailCohViewRange, float tailSepMag, float tailSepViewRange, List<Vector3d> trailsPts)
        {
            this.creeper.getCreeperObject().behavior.selfTailChaser(tailViewAngle, tailCohMag, tailCohViewRange, tailSepMag, tailSepViewRange, Utilities.Convert.toPVecList(trailsPts));
        }
        /// <summary>
        /// Shape Following Algorithm - Requires a polyline to track against. - see example files
        /// </summary>
        /// <param name="polyline">A single polyline to track</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        public void polylineTracker(Polyline polyline, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.creeper.getCreeperObject().behavior.shapeTracker(Utilities.Convert.polylineToShape(polyline), shapeThreshold, projectionDistance, shapeRadius);
        }
        /// <summary>
        /// Multi Shape Following Algorithm - Requires list of Polylines, the polylines get converted into Arraylist of PVectors defining a each shapes points. - see example files
        /// </summary>
        /// <param name="multiShapeList">A list of polylines to track against</param>
        /// <param name="shapeThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="shapeRadius">the radius of the shapes</param>
        public void multiPolylineTracker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.creeper.getCreeperObject().behavior.multiShapeTracker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius);
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
        public void multiPolylineTrackerBabyMaker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, String objTypeOverride, List<Vector3d> childList, List<int> childTypeList)
        {
            this.creeper.getCreeperObject().behavior.multiShapeTrackerBabyMaker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, objTypeOverride, Utilities.Convert.toPVecList(childList), Utilities.Convert.toJavaIntList(childTypeList));
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
        public void multiPolylineTrackerBabyMaker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList)
        {
            this.creeper.attributes.getObjType();
            this.creeper.getCreeperObject().behavior.multiShapeTrackerBabyMaker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.toPVecList(childList), Utilities.Convert.toJavaIntList(childTypeList));
        }
        /// <summary>
        /// Other Object Trails Following Algorithm - Meant for Seeker or sub Seeker types of objects. These objects will chase the trails of other objects - see example files
        /// </summary>
        /// <param name="trailsPts">list of all PVectors from all trails</param>
        /// <param name="trailThreshold">distance threshold enabling agents to see shapes</param>
        /// <param name="projectionDistance">Reynolds "point ahead on the path" to seek</param>
        /// <param name="trailRadius">the radius of the shapes</param>
        public void trailFollowers(List<Vector3d> trailsPts, float trailThreshold, float projectionDistance, float trailRadius)
        {
            this.creeper.getCreeperObject().behavior.trailFollower(Utilities.Convert.toPVecList(trailsPts), trailThreshold, projectionDistance, trailRadius);
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
        public void trailFollowersBabyMakers(List<Vector3d> trailsPts, float trailThreshold, float projectionDistance, float trailRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList)
        {
            this.creeper.getCreeperObject().behavior.trailFollowerBabyMaker(Utilities.Convert.toPVecList(trailsPts), trailThreshold, projectionDistance, trailRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.toPVecList(childList), Utilities.Convert.toJavaIntList(childTypeList));
        }
        /// <summary>
        /// Gets the child start positions if any
        /// </summary>
        /// <returns>the child start positions</returns>
        public List<Vector3d> getChildStartPositions() 
        {
            return Utilities.Convert.toVec3DList(this.creeper.getCreeperObject().behavior.getChildStartPositions());
        }
        /// <summary>
        /// Gets the child spawn types if any
        /// </summary>
        /// <returns>the child spawn type</returns>
        public List<int> getChildSpawnTypes() 
        {
            return Utilities.Convert.toIntList(this.creeper.getCreeperObject().behavior.getChildSpawnType());
        }
        /// <summary>
        /// Calculates a steering force towards a target as defined by Daniel Shiffmans implementation of Craig Reynolds steering force.
        /// </summary>
        /// <param name="targetVector">the target to steer towards</param>
        public void seek(Vector3d targetVector)
        {
            this.creeper.getCreeperObject().behavior.seek(Utilities.Convert.toPVec(targetVector));
        }
        /// <summary>
        /// Calculates a steering force towards a target as defined by Daniel Shiffmans implementation of Craig Reynolds steering force.
        /// </summary>
        /// <param name="targetVector">the target to steer towards</param>
        /// <param name="amplitude">amount to multiply the steer vectory by</param>
        public void seek(Vector3d targetVector, float amplitude)
        {
            this.creeper.getCreeperObject().behavior.seek(Utilities.Convert.toPVec(targetVector), amplitude);
        }
        /// <summary>
        /// Calculates a steering force towards a target as defined by Daniel Shiffmans implementation of Craig Reynolds steering force.
        /// </summary>
        /// <param name="targetVector">the target to steer towards</param>
        /// <param name="normalize">option to normalize the desired parameter</param>
        public void seek(Vector3d targetVector, bool normalize)
        {
            this.creeper.getCreeperObject().behavior.seek(Utilities.Convert.toPVec(targetVector), normalize);
        }
        /// <summary>
        /// Attracts a object towards a target. Differs from Seek
        /// </summary>
        /// <param name="target">target to attract towards</param>
        /// <param name="threshold">if target is within this threshold then attract towards it</param>
        /// <param name="attractionValue">value specifying attraction, this is the magnitude.</param>
        /// <param name="maxAttraction">maximum attraction value</param>
        public void attract(Vector3d target, float threshold, float attractionValue, float maxAttraction)
        {
            this.creeper.getCreeperObject().behavior.attract(Utilities.Convert.toPVec(target), threshold, attractionValue, maxAttraction);
        }
        /// <summary>
        /// Repels a object away from a target.
        /// </summary>
        /// <param name="target">target to repel away from</param>
        /// <param name="threshold">if target is within this threshold then repel away from it</param>
        /// <param name="repelValue">value specifying repulsion, this is the magnitude.</param>
        /// <param name="maxRepel">maximum repulsion value</param>
        public void repel(Vector3d target, float threshold, float repelValue, float maxRepel)
        {
            this.creeper.getCreeperObject().behavior.repel(Utilities.Convert.toPVec(target), threshold, repelValue, maxRepel);
        }
        /// <summary>
        /// Applies the force vector to the acceleration and adds it to the current speed.
        /// </summary>
        /// <param name="force">vector to add to acceleration</param>
        public void applyForce(Vector3d force)
        {
            this.creeper.getCreeperObject().behavior.applyForce(Utilities.Convert.toPVec(force));
        }
        /// <summary>
        /// 2D/3D Improved Perlin Noise
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">value adds a jitter type of movement</param>
        public void perlin(float scale, float strength, float multiplier, float velocity)
        {
            this.creeper.getCreeperObject().behavior.perlin(scale, strength, multiplier, velocity);
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
        public void noiseModified_A(float scale, float strength, float multiplier, float velocity, float modMultiplier,
            float modScaleDivider)
        {
            this.creeper.getCreeperObject().behavior.noiseModified_A(scale, strength, multiplier, velocity, modMultiplier, modScaleDivider);
        }
        /// <summary>
        /// 2D/3D Modified Improved Perlin Noise Type B. Randomized remapped Scale value adjustments.
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">multiplier value for velocity</param>
        /// <param name="modMultiplier">multiplier value multiplied to the scale</param>
        public void noiseModified_B(float scale, float strength, float multiplier, float velocity, float modMultiplier)
        {
            this.creeper.getCreeperObject().behavior.noiseModified_B(scale, strength, multiplier, velocity, modMultiplier);
        }
        /// <summary>
        /// 2D/3D Modified Improved Perlin Noise Type C. Randomized remapped Strength value adjustments.
        /// </summary>
        /// <param name="scale">overall scale of the noise</param>
        /// <param name="strength">overall strength of the noise</param>
        /// <param name="multiplier">value adds a jitter type of movement</param>
        /// <param name="velocity">multiplier value for velocity</param>
        /// <param name="modMultiplier">multiplier value multiplied to the scale</param>
        public void noiseModified_C(float scale, float strength, float multiplier, float velocity, float modMultiplier)
        {
            this.creeper.getCreeperObject().behavior.noiseModified_C(scale, strength, multiplier, velocity, modMultiplier);
        }
    }
}

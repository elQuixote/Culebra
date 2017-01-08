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
    public class Controller
    {
        private Creeper creeper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public Controller(Creeper obj)
        {
            this.creeper = obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchRadius"></param>
        /// <param name="cohesionValue"></param>
        /// <param name="separateValue"></param>
        /// <param name="alignValue"></param>
        /// <param name="viewAngle"></param>
        /// <param name="creeperList"></param>
        /// <param name="drawSearchConnectivity"></param>
        public void flock2D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<Creeper> creeperList, bool drawSearchConnectivity)
        {
            this.creeper.getCreeperObject().behavior.flock2D(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.toJavaList(creeperList), Utilities.Convert.toJavaBool(drawSearchConnectivity));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="randomize"></param>
        /// <param name="addHeading"></param>
        /// <param name="change"></param>
        /// <param name="wanderR"></param>
        /// <param name="wanderD"></param>
        public void wander2D(Boolean randomize, Boolean addHeading, float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander2D(new java.lang.Boolean(randomize), new java.lang.Boolean(addHeading), change, wanderR, wanderD);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tailViewAngle"></param>
        /// <param name="tailCohMag"></param>
        /// <param name="tailCohViewRange"></param>
        /// <param name="tailSepMag"></param>
        /// <param name="tailSepViewRange"></param>
        /// <param name="trailsPts"></param>
        public void selfTailChase(float tailViewAngle, float tailCohMag, float tailCohViewRange, float tailSepMag, float tailSepViewRange, List<Vector3d> trailsPts)
        {
            this.creeper.getCreeperObject().behavior.selfTailChaser(tailViewAngle, tailCohMag, tailCohViewRange, tailSepMag, tailSepViewRange, Utilities.Convert.toPVecList(trailsPts));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="polyline"></param>
        /// <param name="shapeThreshold"></param>
        /// <param name="projectionDistance"></param>
        /// <param name="shapeRadius"></param>
        public void polylineTracker(Polyline polyline, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.creeper.getCreeperObject().behavior.shapeTracker(Utilities.Convert.polylineToShape(polyline), shapeThreshold, projectionDistance, shapeRadius);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiShapeList"></param>
        /// <param name="shapeThreshold"></param>
        /// <param name="projectionDistance"></param>
        /// <param name="shapeRadius"></param>
        public void multiPolylineTracker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.creeper.getCreeperObject().behavior.multiShapeTracker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiShapeList"></param>
        /// <param name="shapeThreshold"></param>
        /// <param name="projectionDistance"></param>
        /// <param name="shapeRadius"></param>
        /// <param name="triggerBabies"></param>
        /// <param name="maxChildren"></param>
        /// <param name="instanceable"></param>
        /// <param name="objTypeOverride"></param>
        /// <param name="childList"></param>
        /// <param name="childTypeList"></param>
        public void multiPolylineTrackerBabyMaker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, String objTypeOverride, List<Vector3d> childList, List<int> childTypeList)
        {
            this.creeper.getCreeperObject().behavior.multiShapeTrackerBabyMaker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, objTypeOverride, Utilities.Convert.toPVecList(childList), Utilities.Convert.toJavaIntList(childTypeList));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="multiShapeList"></param>
        /// <param name="shapeThreshold"></param>
        /// <param name="projectionDistance"></param>
        /// <param name="shapeRadius"></param>
        /// <param name="triggerBabies"></param>
        /// <param name="maxChildren"></param>
        /// <param name="instanceable"></param>
        /// <param name="childList"></param>
        /// <param name="childTypeList"></param>
        public void multiPolylineTrackerBabyMaker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList)
        {
            this.creeper.attributes.getObjType();
            this.creeper.getCreeperObject().behavior.multiShapeTrackerBabyMaker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.toPVecList(childList), Utilities.Convert.toJavaIntList(childTypeList));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Vector3d> getChildStartPositions() 
        {
            return Utilities.Convert.toVec3DList(this.creeper.getCreeperObject().behavior.getChildStartPositions());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<int> getChildSpawnTypes() 
        {
            return Utilities.Convert.toIntList(this.creeper.getCreeperObject().behavior.getChildSpawnType());
        }
    }
}

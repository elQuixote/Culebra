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

        public Controller(Creeper obj)
        {
            this.creeper = obj;
        }

        public void flock2D(float searchRadius, float cohesionValue, float separateValue, float alignValue, float viewAngle, List<Creeper> creeperList, bool drawSearchConnectivity)
        {
            this.creeper.getCreeperObject().behavior.flock2D(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utilities.Convert.toJavaList(creeperList), Utilities.Convert.toJavaBool(drawSearchConnectivity));
        }
        public void wander2D(Boolean randomize, Boolean addHeading, float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander2D(new java.lang.Boolean(randomize), new java.lang.Boolean(addHeading), change, wanderR, wanderD);
        }
        public void selfTailChase(float tailViewAngle, float tailCohMag, float tailCohViewRange, float tailSepMag, float tailSepViewRange, List<Vector3d> trailsPts)
        {
            this.creeper.getCreeperObject().behavior.selfTailChaser(tailViewAngle, tailCohMag, tailCohViewRange, tailSepMag, tailSepViewRange, Utilities.Convert.toPVecList(trailsPts));
        }
        public void polylineTracker(Polyline polyline, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.creeper.getCreeperObject().behavior.shapeTracker(Utilities.Convert.polylineToShape(polyline), shapeThreshold, projectionDistance, shapeRadius);
        }
        public void multiPolylineTracker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius)
        {
            this.creeper.getCreeperObject().behavior.multiShapeTracker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius);
        }
        public void multiPolylineTrackerBabyMaker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, String objTypeOverride, List<Vector3d> childList, java.util.ArrayList childTypeList)
        {
            this.creeper.getCreeperObject().behavior.multiShapeTrackerBabyMaker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, objTypeOverride, Utilities.Convert.toPVecList(childList), childTypeList);
        }
        public void multiPolylineTrackerBabyMaker(List<Polyline> multiShapeList, float shapeThreshold, float projectionDistance, float shapeRadius, bool triggerBabies, int maxChildren, bool instanceable, List<Vector3d> childList, List<int> childTypeList)
        {
            this.creeper.attributes.getObjType();
            this.creeper.getCreeperObject().behavior.multiShapeTrackerBabyMaker(Utilities.Convert.polylinesToMultiShapes(multiShapeList), shapeThreshold, projectionDistance, shapeRadius, triggerBabies, maxChildren, instanceable, Utilities.Convert.toPVecList(childList), Utilities.Convert.toJavaIntList(childTypeList));
        }
        public List<Vector3d> getChildStartPositions() 
        {
            return Utilities.Convert.toVec3DList(this.creeper.getCreeperObject().behavior.getChildStartPositions());
        }
        public List<int> getChildSpawnTypes() 
        {
            return Utilities.Convert.toIntList(this.creeper.getCreeperObject().behavior.getChildSpawnType());
        }
    }
}

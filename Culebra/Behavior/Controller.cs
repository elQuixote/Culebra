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
            this.creeper.getCreeperObject().behavior.flock2D(searchRadius, cohesionValue, separateValue, alignValue, viewAngle, Utility.toJavaList(creeperList), Utility.toJavaBool(drawSearchConnectivity));
        }
        public void wander2D(Boolean randomize, Boolean addHeading, float change, float wanderR, float wanderD)
        {
            this.creeper.getCreeperObject().behavior.wander2D(new java.lang.Boolean(randomize), new java.lang.Boolean(addHeading), change, wanderR, wanderD);
        }
        public void selfTailChase(float tailViewAngle, float tailCohMag, float tailCohViewRange, float tailSepMag, float tailSepViewRange, List<Vector3d> trailsPts)
        {
            this.creeper.getCreeperObject().behavior.selfTailChaser(tailViewAngle, tailCohMag, tailCohViewRange, tailSepMag, tailSepViewRange, Utility.toPVecList(trailsPts));
        }
    }
}

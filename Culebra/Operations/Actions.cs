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
namespace CulebraData.Operations
{
    public class Actions
    {
        private Creeper creeper;

        public Actions(Creeper obj)
        {
            this.creeper = obj;
        }

        public void bounce(BoundingBox bbox)
        {
            if (this.creeper.getCreeperObject().getLocation().x >= (int)bbox.Max[0] || this.creeper.getCreeperObject().getLocation().x <= (int)bbox.Min[0])
            {
                this.creeper.getCreeperObject().getSpeed().x = this.creeper.getCreeperObject().getSpeed().x * -1;
            }
            if (this.creeper.getCreeperObject().getLocation().y >= (int)bbox.Max[1] || this.creeper.getCreeperObject().getLocation().y <= (int)bbox.Min[1])
            {
                this.creeper.getCreeperObject().getSpeed().y = this.creeper.getCreeperObject().getSpeed().y * -1;
            }        
        }

        public void applyMove(int minStepAmount, int maxPositions_Stored)
        {
            this.creeper.getCreeperObject().move(minStepAmount, maxPositions_Stored);         
        }
        public void applyMove()
        {
            this.creeper.getCreeperObject().move();
        }
    }
}

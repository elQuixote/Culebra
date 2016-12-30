
using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace Culebra_GH
{

  public class creepers{
    //-----------------Class Variables------------------------------
    private BoundingBox bbox;
    public Vector3d pos = new Vector3d();
    public Vector3d nextVec = new Vector3d();
    public List<Vector3d> posList = new List<Vector3d>();
    public List<Vector3d> posMoveList = new List<Vector3d>();
    public List<Vector3d> outputList = new List<Vector3d>();
    public List<Vector3d> outputMoveList = new List<Vector3d>();
    public List<Line> lineList = new List<Line>();
    public DataTree<Point3d> posTree;
    //-----------Swarm Value fields------------
    private bool flag;
    private int dim;
    private bool conn;
    private bool enableBounds;
    private bool swarmFlag;
    private bool trail;
    private double ct;
    private double sR;
    private double aValue;
    private double sValue;
    private double cValue;
    private double moveAmp;
    //-----------Swarm fields------------
    private List<Vector3d> otherPtsList;
    private List<Vector3d> otherMoveValues;
    private List<double> allDist;
    private Vector3d cummVec;
    //------Swarm behavior fields--------
    private Vector3d alignVector;
    private Vector3d separateVector;
    private Vector3d cohesionVector;
    //------------------Constructor----------------------------------
    public creepers(List<Vector3d> vecs, List<Vector3d> move, BoundingBox bb, int dimension, bool connect, double connThresh, bool Swarm, double lookArea, double alignVal, double sepVal, double cohVal, DataTree<Point3d> baseTree, double moveVecAmp, bool eb, bool trail){

      this.posList = vecs;
      this.posMoveList = move;
      this.bbox = bb;
      this.dim = dimension;
      this.conn = connect;
      this.ct = connThresh;
      this.swarmFlag = Swarm;
      this.sR = lookArea;
      this.aValue = alignVal;
      this.sValue = sepVal;
      this.cValue = cohVal;
      this.posTree = baseTree;
      this.moveAmp = moveVecAmp;
      this.enableBounds = eb;
      this.trail = trail;
    }
    //-----------Method Creates the Action----------------------------
    public void action(){

      for(int i = 0; i < this.posList.Count; i++){ //go through all the creepers

        this.pos = new Vector3d();
        this.nextVec = new Vector3d();
        this.pos = this.posList[i];

        if(this.enableBounds){ //check to see if we enabled the bounding area which causes the creeper to turn around if it hits it
          this.checkLoc(); //call the checkloc method

          if(this.flag){ //if the creeper is on the boundary turn reverse it
            Vector3d temp = this.posMoveList[i];
            if(this.dim == 0){
              if(this.pos.X >= (int) this.bbox.Max[0] || this.pos.X <= (int) this.bbox.Min[0]){
                temp.X *= -1;
              }
              if(this.pos.Y >= (int) this.bbox.Max[1] || this.pos.Y <= (int) this.bbox.Min[1]){
                temp.Y *= -1;
              }
            }else{
              if(this.pos.X >= (int) this.bbox.Max[0] || this.pos.X <= (int) this.bbox.Min[0]){
                temp.X *= -1;
              }
              if(this.pos.Y >= (int) this.bbox.Max[1] || this.pos.Y <= (int) this.bbox.Min[1]){
                temp.Y *= -1;
              }
              if(this.pos.Z >= (int) this.bbox.Max[2] || this.pos.Z <= (int) this.bbox.Min[2]){
                temp.Z *= -1;
              }
            }
            this.posMoveList[i] = temp;
          }
        }
        this.nextVec = this.posMoveList[i]; //set the move vector the current move vector from the list

        if(this.swarmFlag){ //if we want them to interact with each other then enable the feature in the component
          swarm();
        }

        this.pos += this.nextVec; //add the move vec to the current position
        GH_Path pth = new GH_Path(i); //set a new path for the data structure from the current count
        if(this.trail){
          this.posTree.Add((Point3d) this.pos, pth); //add the current position to the data tree at the specified path
        }
        this.outputMoveList.Add(this.nextVec); //add the next move vec to the output list to return replace the move list
        this.outputList.Add(this.pos); //add the new position to the output list to replace the previous current positions

        if(this.conn){ //enable link to see how far each creeper can see this will show you their interaction range
          link();
        }
      }
    }
    //-----------Method Computes the interaction----------------------------
    public void swarm(){

      this.otherPtsList = new List<Vector3d>();
      this.otherMoveValues = new List<Vector3d>();
      this.allDist = new List<double>();

      for(int i = 0; i < this.posList.Count; i++){

        Point3d othercreeper = (Point3d) this.posList[i];
        double dist = othercreeper.DistanceTo((Point3d) this.pos);

        if(dist > 0 && dist < this.sR){
          this.otherPtsList.Add(this.posList[i]);
          this.allDist.Add(dist);
          this.otherMoveValues.Add(this.posMoveList[i]);
        }
      }
      if(this.otherPtsList.Count > 0){

        this.cummVec = new Vector3d();
        //----------Align-----------------
        align();
        if(this.alignVector.Length > 0){
          this.alignVector.Unitize();
        }
        this.alignVector *= this.aValue;
        //----------Separate-----------------
        separate();
        if(this.separateVector.Length > 0){
          this.separateVector.Unitize();
        }
        this.separateVector *= this.sValue;
        //----------Cohesion-----------------
        cohesion();
        if(this.cohesionVector.Length > 0){
          this.cohesionVector.Unitize();
        }
        this.cohesionVector *= this.cValue;
        //-----------------------------------

        this.cummVec += this.alignVector;
        this.cummVec += this.separateVector;
        this.cummVec += this.cohesionVector;

        this.nextVec += this.cummVec;
        this.nextVec.Unitize();
        this.nextVec *= this.moveAmp;
      }
    }
    //-----------Method Computes the alignment----------------------------
    public void align(){

      this.alignVector = new Vector3d();

      for(int i = 0; i < this.otherPtsList.Count; i++){
        this.alignVector += (this.otherMoveValues[i] *= (this.sR / this.allDist[i]));
      }
    }
    //-----------Method Computes the separation----------------------------
    public void separate(){

      this.separateVector = new Vector3d();

      for(int i = 0; i < this.otherPtsList.Count; i++){
        this.separateVector += Vector3d.Multiply(Vector3d.Subtract(this.pos, this.otherPtsList[i]), this.sR / this.allDist[i]);
      }
    }
    //-----------Method Computes the cohesion-----------------------------
    public void cohesion(){

      this.cohesionVector = new Vector3d(0, 0, 0);

      for(int i = 0; i < this.otherPtsList.Count; i++){
        this.cohesionVector += this.otherPtsList[i];
      }
      Vector3d scaleVec = Vector3d.Multiply(this.cohesionVector, (1.00 / this.otherPtsList.Count));
      Point3d posPoint = (Point3d) this.pos;
      double dist = posPoint.DistanceTo((Point3d) scaleVec);

      this.cohesionVector = Vector3d.Multiply(Vector3d.Subtract(scaleVec, this.pos), this.sR / dist);
    }
    //-----Method Creates visual diagram of the creeper search space------
    public void link(){

      for(int i = 0; i < this.outputList.Count; i++){
        Point3d othercreeper = (Point3d) this.outputList[i];
        double dist = othercreeper.DistanceTo((Point3d) this.pos);

        if(dist > 0 && dist < this.ct){
          Line l = new Line(othercreeper, (Point3d) this.pos);
          lineList.Add(l);
        }
      }
    }
    //----Method Checks if the creeper is still in the bounding area-----
    public void checkLoc(){
      flag = new bool();
      if(!this.bbox.Contains((Point3d) this.pos)){
        //Random rnd = new Random(); //use this if you want the creeper to spawn in a new random area instead of turning it around.
        //this.pos = new Vector3d(rnd.Next(-250, 250), rnd.Next(-250, 250), 0); //use this if you want the creeper to spawn in a new random area instead of turning it around.
        this.flag = true;
      }else{
        this.flag = false;
      }
    }
  }
}

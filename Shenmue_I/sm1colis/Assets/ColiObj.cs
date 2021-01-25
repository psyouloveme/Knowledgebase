using System;
using System.Collections.Generic;
using System.Numerics;

namespace mapinforeader {
    public class ColiObj {
      public ColiObj(){ 
        ObjData = new List<float>();
      }
      public ColiObj(uint ct, uint? cst, uint cc) {
        ColiType = ct;
        ColiSubType = cst;
        ColiCount = cc;
        ObjData = new List<float>();
      }      
      public ColiObj(uint ct, uint? cst){
        ColiType = ct;
        ColiSubType = cst;
        ObjData = new List<float>();
      }
      public uint ColiType { get; set; }
      public uint? ColiSubType { get; set; }
      public uint ColiCount { get; set; }
      public List<float> ObjData { get ;set; }
      public virtual List<float> GetObjData() {
        return ObjData;
      }
    }

    public class ColiTypeZeroTwo : ColiObj {
      public ColiTypeZeroTwo() : base(0,2) {}
      public ColiTypeZeroTwo(uint size) : base(0,2,size){}
      public override List<float> GetObjData() {
        Console.WriteLine("In 20");
        return ObjData;
      }

      public List<Vector3> Points { 
        get {
          List<Vector3> ret = new List<Vector3>();
          for (int x = 0; x < this.ObjData.Count; x++) {
            ret.Add(new Vector3(this.ObjData[x], 0, this.ObjData[++x]));
          }
          return ret;
        } 
      }
    }
}

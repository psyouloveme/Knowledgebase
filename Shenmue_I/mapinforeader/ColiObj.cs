using System;
using System.Collections.Generic;
using System.Numerics;

namespace mapinforeader {
    public class ColiObj {
      public ColiObj(){ 
        ObjData = new List<float>();
      }
      public ColiObj(uint ct, uint? cst, uint? cc) {
        ColiType = ct;
        ColiSubType = cst;
        ColiCount = cc;
        ObjData = new List<float>();
      }      
      public ColiObj(uint ct, uint? cst){
        ColiType = ct;
        ColiSubType = cst;
        ColiCount = null;
        ObjData = new List<float>();
      }
      public uint ColiType { get; set; }
      public uint? ColiSubType { get; set; }
      public uint? ColiCount { get; set; }
      public List<float> ObjData { get ;set; }
      public virtual List<float> GetObjData() {
        return ObjData;
      }
    }

    public class Coli2d : ColiObj {
        public Coli2d(uint ct, uint? cst, uint? cc) : base(ct, cst, cc) { }
        public Coli2d(uint ct, uint? cst) : base(ct, cst) { } 
        public List<Vector3> Points
        {
            get
            {
                List<Vector3> ret = new List<Vector3>();
                if (ObjData.Count % 2 != 0)
                {
                    throw new Exception("Coli data was not in expected format.");
                }
                for (int x = 0; x < this.ObjData.Count; x++)
                {
                    ret.Add(new Vector3(this.ObjData[x], 0, this.ObjData[++x]));
                }
                return ret;
            }
        }
    }

    public class ColiTypeZeroTwo : Coli2d {
      public ColiTypeZeroTwo() : base(0,2) {}
      public ColiTypeZeroTwo(uint size) : base(0,2,size){}
      public override List<float> GetObjData() {
        Console.WriteLine("In 20");
        return ObjData;
      }
    }
    public class ColiTypeZeroThree : ColiObj
    {
        public ColiTypeZeroThree() : base(0, 3) { }
        public ColiTypeZeroThree(uint size) : base(0, 3, size) { }
        public override List<float> GetObjData()
        {
            Console.WriteLine("In 30");
            return ObjData;
        }

        public List<Vector3> Points
        {
            get
            {
                List<Vector3> ret = new List<Vector3>();
                if (this.ObjData.Count % 3 != 0){
                    throw new Exception("Coli data was not in expected format.");
                }
                for (int i = 0; i < this.ObjData.Count; i++)
                {
                    var y = this.ObjData[i];
                    var x = this.ObjData[++i];
                    var z = this.ObjData[++i];
                    ret.Add(new Vector3(x, y, z));
                }
                return ret;
            }
        }
    }
    public class ColiTypeZeroOne : Coli2d
    {
        public ColiTypeZeroOne() : base(0, 1) { }
        public ColiTypeZeroOne(uint size) : base(0, 1, size) { }
        public override List<float> GetObjData()
        {
            Console.WriteLine("In 10");
            return ObjData;
        }
    }
}

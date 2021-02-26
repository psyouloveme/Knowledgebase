    using System;
    using System.Collections.Generic;
    using System.Numerics;

    namespace mapinforeader.Old {
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

        public class ColiType0902 : Coli2d
        {
            public ColiType0902() : base(9, 2) { }
            public ColiType0902(uint size) : base(9, 2, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiType0902");
                return ObjData;
            }
        }

        public class ColiTypeC802 : Coli2d
        {
            public ColiTypeC802() : base(0xC8, 2) { }
            public ColiTypeC802(uint size) : base(0xC8, 2, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiTypeC802");
                return ObjData;
            }
        }

        public class ColiType0702 : Coli2d
        {
            public ColiType0702() : base(7, 2) { }
            public ColiType0702(uint size) : base(7, 2, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiType0702");
                return ObjData;
            }
        }

        public class ColiType0002 : Coli2d {
        public ColiType0002() : base(0,2) {}
        public ColiType0002(uint size) : base(0,2,size){}
        public override List<float> GetObjData() {
            Console.WriteLine("In 20");
            return ObjData;
        }
        }
        public class ColiType0003 : ColiObj
        {
            public ColiType0003() : base(0, 3) { }
            public ColiType0003(uint size) : base(0, 3, size) { }
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
        public class ColiType0005 : ColiObj
        {
            public ColiType0005() : base(0, 5) { }
            public ColiType0005(uint size) : base(0, 5, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In 00 05");
                return ObjData;
            }

            public List<Vector3> Points
            {
                get
                {
                    // all of the 00 05s are 6 words long
                    // discard everything after the first 2
                    // because idk what it represents
                    List<Vector3> ret = new List<Vector3>();
                    var x = this.ObjData[0];
                    var z = this.ObjData[1];
                    ret.Add(new Vector3(x, 0, z));
                    return ret;
                }
            }
        }
        public class ColiType0903 : ColiObj
        {
            public ColiType0903() : base(9, 3) { }
            public ColiType0903(uint size) : base(9, 3, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In 09 03");
                return ObjData;
            }

            public List<Vector3> Points
            {
                get
                {
                    // all of the 09 05s are 6 words long
                    // discard everything after the first 2
                    // because idk what it represents
                    List<Vector3> ret = new List<Vector3>();
                    var y = this.ObjData[0];
                    var x = this.ObjData[1];
                    var z = this.ObjData[2];
                    ret.Add(new Vector3(x, y, z));
                    return ret;
                }
            }
        }
        public class ColiType0905 : ColiObj
        {
            public ColiType0905() : base(9, 5) { }
            public ColiType0905(uint size) : base(9, 5, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In 09 05");
                return ObjData;
            }

            public List<Vector3> Points
            {
                get
                {
                    // all of the 09 05s are 6 words long
                    // discard everything after the first 2
                    // because idk what it represents
                    List<Vector3> ret = new List<Vector3>();
                    var x = this.ObjData[0];
                    var z = this.ObjData[1];
                    ret.Add(new Vector3(x, 0, z));
                    return ret;
                }
            }
        }

        public class ColiType0001 : Coli2d
        {
            public ColiType0001() : base(0, 1) { }
            public ColiType0001(uint size) : base(0, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In 10");
                return ObjData;
            }
        }
        public class ColiType6E01 : Coli2d
        {
            public ColiType6E01() : base(0x6E, 1) { }
            public ColiType6E01(uint size) : base(0x6E, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In 6E 01");
                return ObjData;
            }
        }
        public class ColiType6401 : Coli2d
        {
            public ColiType6401() : base(0x64, 1) { }
            public ColiType6401(uint size) : base(0x64, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In 64 01");
                return ObjData;
            }
        }
        public class ColiType0701 : Coli2d
        {
            public ColiType0701() : base(0x07, 1) { }
            public ColiType0701(uint size) : base(0x07, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiType0701");
                return ObjData;
            }
        }
        public class ColiType0901 : Coli2d
        {
            public ColiType0901() : base(0x09, 1) { }
            public ColiType0901(uint size) : base(0x09, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiType0901");
                return ObjData;
            }
        }
        public class ColiType0A01 : Coli2d
        {
            public ColiType0A01() : base(0x0A, 1) { }
            public ColiType0A01(uint size) : base(0x0A, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiType0A01");
                return ObjData;
            }
        }

        public class ColiType0801 : Coli2d
        {
            public ColiType0801() : base(0x08, 1) { }
            public ColiType0801(uint size) : base(0x08, 1, size) { }
            public override List<float> GetObjData()
            {
                Console.WriteLine("In ColiType0801");
                return ObjData;
            }
        }
    }

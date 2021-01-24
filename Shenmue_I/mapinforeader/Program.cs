using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace mapinforeader
{   
    class Program
    {
        private const string FILE_NAME = "/mnt/c/Users/Matthew/repos/smkb/Shenmue_I/mapinforeader/D000_MAPINFO.BIN.bytes";
        static void Main(string[] args)
        {
            if (!File.Exists(FILE_NAME))
            {
                Console.WriteLine($"{FILE_NAME} doesn't exist!");
                return;
            }
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs, Encoding.ASCII))
                {
                    Cols c = Cols.ReadCols(r);
                    Dictionary<string, int> typeStats = new Dictionary<string, int>(); 
                    c.Colis.ForEach(coli => {
                        coli.ColiObjs.ForEach(coliObj => {
                            string key = coliObj.ObjType.ToString("X2") + " ";
                            if (coliObj.ObjSubTypeOrSomething.HasValue) {
                                key += coliObj.ObjSubTypeOrSomething.Value.ToString("X2");
                            } else {
                                key += "--";
                            }
                            key += " " + coliObj.ObjCount.ToString("X2");
                            key += " " + coliObj.ObjData.Count;
                            if (typeStats.ContainsKey(key)) {
                                typeStats[key]++;
                            } else {
                                typeStats[key] = 1;
                            }
                        });
                    });
                    List<string> s = typeStats.Select(v => $"{v.Key} {v.Value}").ToList();
                    s.Sort();
                    FileStream f = File.Open("D000_COLS_COLI0_ColiObj_Types.txt", FileMode.Create);
                    using (StreamWriter sw = new StreamWriter(f)) {
                        s.ForEach(g => sw.WriteLine(g));
                    }
                    s.ForEach(g => Console.WriteLine(g));
                    Console.WriteLine("-----------------------------------");
                    FileStream l = File.Open("D000_COLS_COLI0_All_ColiObjs.txt", FileMode.Create);
                    using (StreamWriter sw = new StreamWriter(l)) {
                        c.Colis.ForEach(h => {
                            h.ColiObjs.ForEach(j => {
                                sw.Write(j.ObjType.ToString("X2") + " ");
                                if (j.ObjSubTypeOrSomething.HasValue) {
                                    sw.Write(j.ObjSubTypeOrSomething.Value.ToString("X2") + " ");
                                } else {
                                    sw.Write("-- ");
                                }
                                sw.Write(j.ObjCount + " ");
                                sw.Write(j.ObjData.Count + "\n");
                            });
                        });
                    }
                    c.Colis.ForEach(h => {
                        h.ColiObjs.ForEach(j => {
                            Console.Write(j.ObjType.ToString("X2") + " ");
                            if (j.ObjSubTypeOrSomething.HasValue) {
                                Console.Write(j.ObjSubTypeOrSomething.Value.ToString("X2") + " ");
                            } else {
                                Console.Write("-- ");
                            }
                            Console.Write(j.ObjCount + " ");
                            Console.Write(j.ObjData.Count + "\n");
                        });
                    });
                }
            }
        }
    }
}

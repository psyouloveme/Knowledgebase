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
            Cols c;
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs, Encoding.ASCII))
                {
                    c = Cols.ReadCols(r);
                }
            }
            Dictionary<string, int> typeStats = new Dictionary<string, int>(); 
            c.Colis.ForEach(coli => {
                coli.ColiObjs.ForEach(coliObj => {
                    string key = coliObj.ColiType.ToString("X2") + " ";
                    if (coliObj.ColiSubType.HasValue) {
                        key += coliObj.ColiSubType.Value.ToString("X2");
                    } else {
                        key += "--";
                    }
                    key += " " + coliObj.ColiCount.ToString("X2");
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
            Console.WriteLine("-----------------------------------");
            FileStream l = File.Open("D000_COLS_COLI0_All_ColiObjs.txt", FileMode.Create);
            using (StreamWriter sw = new StreamWriter(l)) {
                c.Colis.ForEach(h => {
                    h.ColiObjs.ForEach(j => {
                        sw.Write(j.ColiType.ToString("X2") + " ");
                        if (j.ColiSubType.HasValue) {
                            sw.Write(j.ColiSubType.Value.ToString("X2") + " ");
                        } else {
                            sw.Write("-- ");
                        }
                        sw.Write(j.ColiCount + " ");
                        sw.Write(j.ObjData.Count + "\n");
                    });
                });
            }
            Console.WriteLine("-----------------------------------");
            l = File.Open("D000_COLS_COLI0_All_ColiObjs_DATA.txt", FileMode.Create);
            using (StreamWriter sw = new StreamWriter(l)) {
                c.Colis.ForEach(h => {
                    h.ColiObjs.ForEach(j => {
                        sw.Write(j.ColiType.ToString("X2") + " ");
                        if (j.ColiSubType.HasValue) {
                            sw.Write(j.ColiSubType.Value.ToString("X2") + " ");
                        } else {
                            sw.Write("-- ");
                        }
                        sw.Write(j.ColiCount + " ");
                        sw.Write(j.ObjData.Count + "\n");
                        for(int i = 0; i < j.ObjData.Count; i++) {
                            if (j.ColiType == 0 && j.ColiSubType.HasValue && j.ColiSubType.Value == 2) {
                                sw.WriteLine($"({j.ObjData[i]}, {j.ObjData[++i]})");
                            } else {
                                sw.WriteLine(j.ObjData[i]);
                            }
                        }
                    });
                });
            }
            Console.WriteLine("-----------------------------------");
            c.Colis.Aggregate(float.MinValue,
                (max, next) => {
                    float colMax = next.ColiObjs.Aggregate(max,
                        (colmax, next) => {
                            float dataMax = next.ObjData.Aggregate(colmax,
                                (datmax, next) => {
                                    return next > datmax ? next : datmax; 
                                },
                                d => d
                            );
                            Console.WriteLine("Found max: " + dataMax);
                            return dataMax > colmax ? dataMax : colmax;
                        },
                        cm => cm
                    );
                    Console.WriteLine("Found COL max: " + colMax);
                    return colMax > max ? colMax : max;
                },
                cc => cc
            );
            c.Colis.Aggregate(float.MaxValue,
                (min, next) => {
                    float colMin = next.ColiObjs.Aggregate(min,
                        (colmin, next) => {
                            float dataMin = next.ObjData.Aggregate(colmin,
                                (datmin, next) => {
                                    if (next == float.NaN) {
                                        Console.WriteLine("FOUND A NAN");
                                    }
                                    return next < datmin ? next : datmin; 
                                },
                                d => d
                            );
                            Console.WriteLine("Found min: " + dataMin);
                            return dataMin < colmin ? dataMin : colmin;
                        },
                        cm => cm
                    );
                    Console.WriteLine("Found COL min: " + colMin);
                    return colMin < min ? colMin : min;
                },
                cc => cc
            );
        }
    }
}

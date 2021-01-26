using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace mapinforeader.Utils
{
    public static class Analysis {

        public static void DumpFormattedTypeCountsToFile(Cols c, string filename) {
            Dictionary<string, int> typeStats = new Dictionary<string, int>();
            c.Colis.ForEach(coli =>
            {
                coli.ColiObjs.ForEach(coliObj =>
                {
                    string key = coliObj.ColiType.ToString("X2") + " ";
                    if (coliObj.ColiSubType.HasValue)
                    {
                        key += coliObj.ColiSubType.Value.ToString("X2");
                    }
                    else
                    {
                        key += "--";
                    }
                    if (coliObj.ColiCount.HasValue)
                    {
                        // key += coliObj.ColiSubType.Value.ToString("X2");
                        key += " " + coliObj.ColiCount.Value.ToString("X2");
                    }
                    else
                    {
                        key += " --";
                    }
                    key += " " + coliObj.ObjData.Count;
                    if (typeStats.ContainsKey(key))
                    {
                        typeStats[key]++;
                    }
                    else
                    {
                        typeStats[key] = 1;
                    }
                });
            });
            List<string> s = typeStats.Select(v => $"{v.Key} {v.Value}").ToList();
            s.Sort();
            FileStream f = File.Open("D000_COLS_COLI0_ColiObj_Types.txt", FileMode.Create);
            using (StreamWriter sw = new StreamWriter(f))
            {
                s.ForEach(g => sw.WriteLine(g));
            }
            Console.WriteLine("-----------------------------------");
        }

        public static void DumpFormattedColsMetadataToFile(Cols c, string filename) {
            FileStream l = File.Open(filename, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(l))
            {
                c.Colis.ForEach(h =>
                {
                    h.ColiObjs.ForEach(j =>
                    {
                        sw.Write(j.ColiType.ToString("X2") + " ");
                        if (j.ColiSubType.HasValue)
                        {
                            sw.Write(j.ColiSubType.Value.ToString("X2") + " ");
                        }
                        else
                        {
                            sw.Write("-- ");
                        }
                        sw.Write(j.ColiCount + " ");
                        sw.Write(j.ObjData.Count + "\n");
                    });
                });
            }
        }
        public static void DumpFormattedColsToFile(Cols c, string filename) {
            FileStream l = File.Open(filename, FileMode.Create);
            using (StreamWriter sw = new StreamWriter(l))
            {
                c.Colis.ForEach(h =>
                {
                    h.ColiObjs.ForEach(j =>
                    {
                        sw.Write(j.ColiType.ToString("X2") + " ");
                        if (j.ColiSubType.HasValue)
                        {
                            sw.Write(j.ColiSubType.Value.ToString("X2") + " ");
                        }
                        else
                        {
                            sw.Write("-- ");
                        }
                        sw.Write(j.ColiCount + " ");
                        sw.Write(j.ObjData.Count + "\n");
                        for (int i = 0; i < j.ObjData.Count; i++)
                        {
                            float bx, by, bz;
                            if (j.ColiType == 0 && j.ColiSubType.HasValue) {
                                switch (j.ColiSubType) {
                                    case 0x02:
                                    case 0x01:
                                        bx = j.ObjData[i];
                                        bz = j.ObjData[++i];
                                        sw.WriteLine($"({bx}, {bz})");
                                        break;
                                    case 0x03:
                                        by = j.ObjData[i];
                                        bx = j.ObjData[++i];
                                        bz = j.ObjData[++i];
                                        sw.WriteLine($"({bx}, {by}, {bz})");
                                        break;
                                    default:
                                        sw.WriteLine(j.ObjData[i]);
                                        break;
                                }
                            }
                            else
                            {
                                sw.WriteLine(j.ObjData[i]);
                            }
                        }
                    });
                });
            }
        }
        public static float FindMaxZeroTwoCoord(Cols c) {
            return c.Colis.Aggregate(float.MinValue,
                (max, next) =>
                {
                    float colMax = next.ColiObjs.Aggregate(max,
                        (colmax, next) =>
                        {
                            float dataMax = next.ObjData.Aggregate(colmax,
                                (datmax, next) =>
                                {
                                    return next > datmax ? next : datmax;
                                },
                                d => d
                            );
                            // Console.WriteLine("Found max: " + dataMax);
                            return dataMax > colmax ? dataMax : colmax;
                        },
                        cm => cm
                    );
                    // Console.WriteLine("Found COL max: " + colMax);
                    return colMax > max ? colMax : max;
                },
                cc => cc
            );
        }
        public static float FindMinZeroTwoCoord(Cols c) {
            return c.Colis.Aggregate(float.MaxValue,
                (min, next) =>
                {
                    float colMin = next.ColiObjs.Aggregate(min,
                        (colmin, next) =>
                        {
                            float dataMin = next.ObjData.Aggregate(colmin,
                                (datmin, next) =>
                                {
                                    if (next == float.NaN)
                                    {
                                        Console.WriteLine("FOUND A NAN");
                                    }
                                    return next < datmin ? next : datmin;
                                },
                                d => d
                            );
                            // Console.WriteLine("Found min: " + dataMin);
                            return dataMin < colmin ? dataMin : colmin;
                        },
                        cm => cm
                    );
                    // Console.WriteLine("Found COL min: " + colMin);
                    return colMin < min ? colMin : min;
                },
                cc => cc
            );
        }
    }
}
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using mapinforeader.Utils;

namespace mapinforeader
{   
    class Program
    {
        private const string FILE_NAME = "/mnt/c/Users/Matthew/Desktop/SM_D1_SCENE/01/D000/MAPINFO.BIN";
        static void Main(string[] args)
        {
            if (!File.Exists(FILE_NAME))
            {
                Console.WriteLine($"{FILE_NAME} doesn't exist!");
                return;
            }
            // Cols c;
            List<Cols.ColiInfo> infos = new List<Cols.ColiInfo>();
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs, Encoding.ASCII))
                {
                    infos = Analysis.LocateColiOffsets(r);
                    // Console.WriteLine($"looked for infos got {infos.Count}");
                    for (int idx = 0; idx < infos.Count; idx++){
                        // Console.WriteLine($"Coli {idx}: Size: {infos[idx].Size} At: {infos[idx].HeaderOffset:X2}");
                    }
                    // c = Cols.ReadCols(r);
                }
            }
                foreach (var coli in infos)
                {
                    using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read)){
                    // Console.WriteLine("reading stream");
                    fs.Seek(coli.ContentOffset, SeekOrigin.Begin);
                    coli.ReadColiObjs(fs);
                }
                for(int idx = 0; idx < infos.Count; idx++) {
                    // Console.WriteLine($"Reading objs from coli {idx}");
                    foreach (var coliObj in infos[idx].ColiObjs)
                    {
                        // Console.Write($"Found coliObj {coliObj.ColiType:X2}");
                        if (coliObj.ColiSubType.HasValue)
                        {
                            // Console.Write($" {coliObj.ColiSubType.Value:X2}");
                        }
                        if (coliObj.ColiCount.HasValue)
                        {
                            // Console.Write($" {coliObj.ColiCount.Value:X2}");
                        }
                        // Console.Write("\n");
                    }
                }
                // Analysis.DumpFormattedTypeCountsToFile(infos, "New_Frequencies.md");
            }

            // Analysis.DumpFormatted0005FreqsToFile(infos, "0005ColFreqs.md");
            Analysis.DumpFormatted0005ColsToFile(infos, "0005ColData.md");
            // Analysis.DumpFormattedZeroThreeColsToFile(c, "0003ColLocs.md");
            // Console.WriteLine("Max 00 02 coord is " + Analysis.FindMaxZeroTwoCoord(c));
            // Console.WriteLine("Min 00 02 coord is " + Analysis.FindMinZeroTwoCoord(c));
        }
    }
}

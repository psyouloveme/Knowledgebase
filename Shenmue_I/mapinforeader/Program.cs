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
        private const string FILE_NAME = "/mnt/c/Users/Matthew/repos/ps-smkb/Shenmue_I/mapinforeader/D000_MAPINFO.BIN.bytes";
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
            Analysis.DumpFormattedColsToFile(c, "Colis_With_0003s.txt");
            Console.WriteLine("Max 00 02 coord is " + Analysis.FindMaxZeroTwoCoord(c));
            Console.WriteLine("Min 00 02 coord is " + Analysis.FindMinZeroTwoCoord(c));
        }
    }
}

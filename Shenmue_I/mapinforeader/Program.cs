using System;
using System.IO;
using System.Text;

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
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader r = new BinaryReader(fs, Encoding.ASCII))
                {
                    Cols c = Cols.ReadCols(r);
                }
            }
        }
    }
}

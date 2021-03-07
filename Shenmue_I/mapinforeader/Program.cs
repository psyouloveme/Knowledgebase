using System;
using System.IO;
using mapinforeader.Models.MapinfoSections;
using mapinforeader.Util;

namespace mapinforeader
{   
    class Program
    {
        private const string FILE_NAME = "./COLIMBUI.PKS";
        static void Main(string[] args)
        {
            if (!File.Exists(FILE_NAME))
            {
                Console.WriteLine($"{FILE_NAME} doesn't exist!");
                return;
            }
            Cols cols = null;
            using (FileStream fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read)) {
                using (ColsReader reader = new ColsReader(fs)) {
                    cols = reader.ReadCols();
                }
            }
            Analysis.DumpColsBySectionIdx(cols, "./Docs/ColiObj_Frequencies_Refactor2.md");
        }
    }
}

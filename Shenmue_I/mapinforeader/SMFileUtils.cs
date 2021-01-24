using System.IO;


namespace mapinforeader.Utils
{
    public static class SMFileUtils
    {
        public static void WriteBytesToFile(string filePath, byte[] content) {
            FileStream f = File.Open(filePath, FileMode.Create);
            using (BinaryWriter writer = new BinaryWriter(f)) {
               writer.Write(content);
            }
        }
    }
}
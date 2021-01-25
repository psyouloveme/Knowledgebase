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

        public static bool AllBytesMatch(byte[] buffer, byte match) {
            foreach(byte b in buffer) {
                if (b != match) {
                    return false;
                }
            }
            return true;
        }

        public static byte[] CreateByteArray(byte fill, int length) {
            byte[] b = new byte[length];
            for(int x = 0; x < length; x++) {
                b[x] = fill;
            }
            return b;
        }
    }
}
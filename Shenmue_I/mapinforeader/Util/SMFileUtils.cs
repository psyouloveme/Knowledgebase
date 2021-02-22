using System.IO;
using System.Text;
using System;

namespace mapinforeader.Util
{
    public static class SMFileUtils
    {
        public static long? FindNextString(BinaryReader reader, string target) {
            long? position = null;
            bool streamEnded = false;
            bool found = false;
            while (!streamEnded && !found) {
                int i;
                for (i = 0; i < target.Length && !streamEnded; i++){
                    byte b;
                    try {
                        b = reader.ReadByte();
                    } catch {
                        streamEnded = true;
                        break;
                    }
                    if (b < 0) {
                        streamEnded = true;
                    }
                    if (b != target[i]){
                        break;
                    }
                }
                if (i == target.Length) {
                    position = reader.BaseStream.Position - i;
                    found = true;
                }
            }
            return position;
        }

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

        public static string ConvertBytesToString(byte[] bytes) {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes) {
              sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        } 
    }
}
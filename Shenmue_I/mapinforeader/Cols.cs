using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace mapinforeader
{   
  public class Cols {
    public static class Headers {  
      public static readonly string COLS = "COLS";
      public static readonly string COLI = "COLI";
      public static readonly string HGHT = "HGHT";
      public static readonly string EVNT = "EVNT";
      public static readonly string UNDU = "UNDU";
      public static readonly string SOND = "SOND";
      public static readonly string PROP = "PROP";
      public static readonly string WALK = "WALK";
    }

    public class ColiInfo {
      public uint Size { get; set; }
      public byte[] Content { get; set; }
    }
    byte[] Content { get; set; }
    uint Size { get; set; }
    string Identifier { get; set; }
    IEnumerable<ColiInfo> Colis { get; set; }
    byte[] Hght { get; set; }
    byte[] Evnt { get; set; }
    byte[] Undu { get; set; }
    byte[] Sond { get; set; }
    byte[] Prop { get; set; }
    byte[] Walk { get; set; }

    public static IEnumerable<string> HeaderList {
      get {
        PropertyInfo[] props = typeof(Cols.Headers).GetProperties();
        List<string> headerList = new List<string>();
        foreach(PropertyInfo prop in props) {
          headerList.Add((string)prop.GetValue(null));
        }
        return headerList;
      }
    }

    public static Cols ReadCols(BinaryReader reader) {
      bool streamEnded = false,  matched = false;
      Cols cols = new Cols();

      while (!matched && !streamEnded) {
        bool foundStart = false, endMismatch = false;
        // find the starting byte
        try {
          byte nextByte = 1;
          while (nextByte > 0 && !foundStart && !streamEnded) {
            nextByte = reader.ReadByte();
            if (nextByte == (int)Cols.Headers.COLS[0]) {
              foundStart = true;
            }
            if (nextByte < 0) {
              streamEnded = true;
              cols = null;
            }
          }
        } catch (EndOfStreamException e) {
          cols = null;
          streamEnded = true;
        } catch (Exception e) {
          cols = null;
        }
        if (!streamEnded && foundStart) {
          // check for the subsequent bytes
          string remainingChars = Cols.Headers.COLS.Substring(1);
          for (int i = 0; i < remainingChars.Length && !streamEnded && !endMismatch; i++) {
            try {
              byte anotherByte = reader.ReadByte();
              if (anotherByte < 0) {
                streamEnded = true;
                endMismatch = true;
              } else if (anotherByte != (int)remainingChars[i]) {
                endMismatch = true;
              }
            } catch (EndOfStreamException e) {
              cols = null;
              streamEnded = true;
              endMismatch = true;
            } catch (Exception e) {
              cols = null; 
              endMismatch = true;
            }
          }
          if (foundStart && !streamEnded && !endMismatch) {
            byte[] colsSizeBytes = reader.ReadBytes(4);
            UInt32 colsSize = BitConverter.ToUInt32(colsSizeBytes, 0);
            // this size includes the 4 bytes for the coli data.
            cols.Size = colsSize;
            char[] identifier = reader.ReadChars(4);
            cols.Identifier = new string(identifier);
            matched = true;
            // subtract the 4 bytes for the size, read the remainder of the coli content
            cols.Content = reader.ReadBytes(Convert.ToInt32(colsSize) - 8);
          }
        }
      }
      return cols;
    }
  }
}
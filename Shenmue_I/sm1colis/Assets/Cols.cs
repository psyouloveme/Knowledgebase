using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

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

  public class Coli {
    uint Size { get; set; }
    byte[] Content { get; set; }
  }
  byte[] Content { get; set; }
  uint Size { get; set; }
  byte[] Coli { get; set; }
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
        headerList.Add(prop.GetValue(null, null));
      }
      return headerList;
    }
  }

  public static Cols ReadCols(BinaryReader reader) {
    bool found = false, streamEnded = false;
    byte nextByte = 1;
    Cols cols = new Cols();

    while (!found && !streamEnded) {
      // find the starting byte
      try {
        for (byte nextByte = 1; nextByte > 0 && !found; nextByte = reader.ReadByte()){
          if (nextByte == (int)Cols.Headers.COLS[0]) {
            found = true;
          }
        }
        if (nextByte < 0) {
          streamEnded = true;
          cols = null;
        }
      } catch (EndOfStreamException e) {
        cols = null;
        streamEnded = true;
      } catch (Exception e) {
        cols = null;
      }
      if (!streamEnded) {
        // check for the subsequent bytes
        string remainingChars = Cols.Headers.COLS.Substring(1);
        bool mismatch = false;
        for (int i = 0; i < remainingChars.Length && !streamEnded && !mismatch; i++) {
          try {
            byte nextByte = reader.ReadByte();
            if (nextByte < 0) {
              streamEnded = true;
              mismatch = true;
            }
            if (nextByte != (int)remainingChars[i]) {
              mismatch = true;
            }
          } catch (EndOfStreamException e) {
            cols = null;
            streamEnded = true;
            mismatch = true;
          } catch (Exception e) {
            cols = null; 
            mismatch = true;
          }
        }
      }
    }
    return cols;
  }
}
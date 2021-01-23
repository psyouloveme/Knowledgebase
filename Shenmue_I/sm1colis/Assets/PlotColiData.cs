using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotColiData : MonoBehaviour
{
  [SerializeField]
  TextAsset asset;
  [SerializeField]
  GameObject prefab;
  void FindColiStart(BinaryReader reader) {
      int currentByte = 0;
      while (currentByte != -1)
      {
          currentByte = (int)reader.Read();
          if (currentByte == -1)
          {
              throw new Exception("File ended without coli data");
          }
          else if (currentByte != 67)
          {
              continue;
          }
          // read the next 3 to see if we found it for sure
          currentByte = (int)reader.Read();
          if (currentByte == -1)
          {
              throw new Exception("File ended without coli data");
          }
          else if (currentByte != 79)
          {
              continue;
          }

          currentByte = (int)reader.Read();
          if (currentByte == -1)
          {
              throw new Exception("File ended without coli data");
          }
          else if (currentByte != 76)
          {
              continue;
          }

          currentByte = (int)reader.Read();
          if (currentByte == -1)
          {
              throw new Exception("File ended without coli data");
          }
          else if (currentByte != 73)
          {
              continue;
          }

          Debug.Log("Found coli ending at:" + reader.BaseStream.Position.ToString("X4"));

          break;
      }
  }
    void FindColsStart(BinaryReader reader)
    {
        int currentByte = 0;
        while (currentByte != -1)
        {
            currentByte = (int)reader.Read();
            if (currentByte == -1)
            {
                throw new Exception("File ended without COLS data");
            }
            else if (currentByte != 67)
            {
                continue;
            }
            // read the next 3 to see if we found it for sure
            currentByte = (int)reader.Read();
            if (currentByte == -1)
            {
                throw new Exception("File ended without COLS data");
            }
            else if (currentByte != 79)
            {
                continue;
            }

            currentByte = (int)reader.Read();
            if (currentByte == -1)
            {
                throw new Exception("File ended without COLS data");
            }
            else if (currentByte != 76)
            {
                continue;
            }

            currentByte = (int)reader.Read();
            if (currentByte == -1)
            {
                throw new Exception("File ended without COLS data");
            }
            else if (currentByte != 83)
            {
                continue;
            }

            Debug.Log("Found coli ending at:" + reader.BaseStream.Position.ToString("X4"));

            break;
        }
    }


    void FindWord() {

    }

  void Start()
  {
    if (asset == null) {
        throw new Exception("No asset provided.");
    }
    if (prefab == null) {
        throw new Exception("No game object prefab provided.");
    }
    Stream s = new MemoryStream(asset.bytes);
    using (BinaryReader reader = new BinaryReader(s, Encoding.ASCII)) {
      this.FindColsStart(reader);
      byte[] colsSizeBytes = reader.ReadBytes(4);
      UInt32 colsSize = BitConverter.ToUInt32(colsSizeBytes, 0);
      Debug.Log("Found a COLS section with size " + colsSize);

      this.FindColiStart(reader);
      byte[] coliSizeBytes = reader.ReadBytes(4);
      UInt32 coliSize = BitConverter.ToUInt32(coliSizeBytes, 0);
      Debug.Log("Found a COLI section with size " + coliSize);
      Debug.Log("Reading COLI starting at:" + reader.BaseStream.Position.ToString("X4"));

      // byte[] coliData = reader.ReadBytes((int)coliSize);
      // Debug.Log("Read COLI ending at:" + reader.BaseStream.Position.ToString("X4"));
      // char[] coliEnd = reader.ReadChars(4);
      // string coliChar = new string(coliEnd);
      // Debug.Log("Got coli term " + coliChar);


 
      // then process the coli data as in coli2floats.rb
      bool coliFound = false;
      Quaternion rotation = new Quaternion(1, 1, 1, 1);
      int i = 0;
      while (reader.PeekChar() != -1 && !coliFound ) {
        int b1, b2, b3, b4;
        b1 = reader.Read();
        b2 = reader.Read();
        b3 = reader.Read();
        b4 = reader.Read();

        // done with a coli section
        if (b1 == 99 && b2 == 111 && b3 == 108 && b4 == 105) {
          coliFound = true;
        }

        // Exclude words from [00 00 00 01] to [00 00 00 ff],
        // and words equal to [ff ff ff ff].
        if (((b4 + b3 + b2) == 0) || ((b4 + b3 + b2 + b1) == 1020)) {
          continue;
        }

        reader.BaseStream.Seek(-4, SeekOrigin.Current);
        float f1, f2;
        f1 = reader.ReadSingle();
        f2 = reader.ReadSingle();

        // then, continune with the other stuff
        Vector3 position = new Vector3(f1, 0, f2);
        GameObject obj = Instantiate(prefab, position, rotation) as GameObject;
        obj.name = $"Coli {i}";
        i++;
      }
      // 99 111 108 105 coli
    } 
  }
}
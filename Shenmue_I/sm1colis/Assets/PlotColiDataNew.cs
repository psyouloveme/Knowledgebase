using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using mapinforeader;
using coliplot;

public class PlotColiDataNew : MonoBehaviour {

  [SerializeField]
  TextAsset asset;

  void Start()
  {
    if (asset == null) {
        throw new Exception("No asset provided.");
    }
    Quaternion rotation = new Quaternion(1, 1, 1, 1);
    float q = 0.0f;
    Color color = new Color(q, q, 1.0f);
    Stream s = new MemoryStream(asset.bytes);
    List<Cols.ColiInfo> colis = new List<Cols.ColiInfo>();
    using (BinaryReader reader = new BinaryReader(s, Encoding.ASCII)) {
      colis = Cols.LocateColiOffsets(reader);
    }
    foreach (var coli in colis)
    {
      using (MemoryStream fs = new MemoryStream(asset.bytes))
      {
        // Console.WriteLine("reading stream");
        fs.Seek(coli.ContentOffset, SeekOrigin.Begin);
        coli.ReadColiObjs(fs);
      }
    }
    colis.ForEach(j => {
      j.ColiObjs.ForEach(k => {
        ColiPlotter.PlotColiObj(k, null);
      });
    });
    //   // Debug.Log("staring procesing.");
    //   Cols cols = Cols.ReadCols(reader);
    //   // Debug.Log("read cols.");
    //   cols.Colis.ForEach(h => {
    //     // Debug.Log("Creating with " + h);
    //     // Debug.Log("H.size is" + h.Size);
    //     // Debug.Log("H.coliobjs.length is" + h.ColiObjs.Count);
    //     int coliObjId = 0;
    //     h.ColiObjs.ForEach(j => {
    //       // Debug.Log("Processing obj" + j.ColiType.ToString("X2"));
    //       ColiPlotter.PlotColiObj(j, null);
    //     });
    //   });
    //   Validator.FindFarObjects();
    // } 
  }
}  
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using mapinforeader.Models.MapinfoSections;
using mapinforeader.Util;
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
    Cols cols = null;
    Stream s = new MemoryStream(asset.bytes);
    using (ColsReader r = new ColsReader(s)) {
      try {
        cols = r.ReadCols();
      } catch (Exception e) {
        Debug.Log("Fuck");
        Debug.LogException(e);
      }
    }
    if (cols == null || cols.Colis == null || cols.Colis.Count == 0) {
      Debug.Log("No collision found!");
      return;
    }

    // Plot the colis
    for (int coliIdx = 0; coliIdx < cols.Colis.Count; coliIdx++) {
      var coli = cols.Colis[coliIdx];
      var coliGameObj = new GameObject();
      coliGameObj.transform.SetParent(gameObject.transform);
      coliGameObj.name = $"COLI {coliIdx}";
      for (int coliObjIdx = 0; coliObjIdx < coli.ColiDatas.Count; coliObjIdx++) {
        ColiPlotter.PlotColiObj(coli.ColiDatas[coliObjIdx], coliGameObj, $"COLI {coliIdx}");
      }
    }

    // Plot the hghts
    for (int hghtIdx = 0; hghtIdx < cols.Hghts.Count; hghtIdx++) {
      var hght = cols.Hghts[hghtIdx];
      var hghtGameObj = new GameObject();
      hghtGameObj.name = $"HGHT {hghtIdx}";
      hghtGameObj.transform.SetParent(gameObject.transform);
      for (int hghtObjIdx = 0; hghtObjIdx < hght.HghtDatas.Count; hghtObjIdx++) {
        ColiPlotter.PlotHghtObj(hght.HghtDatas[hghtObjIdx], hghtGameObj, $"HGHT {hghtIdx}");
      }
    }
  }
}  
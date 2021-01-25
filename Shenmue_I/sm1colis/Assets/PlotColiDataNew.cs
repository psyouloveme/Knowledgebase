using System;
using System.IO;
using System.Text;
using UnityEngine;
using mapinforeader;

public class PlotColiDataNew : MonoBehaviour {
  [SerializeField]
  TextAsset asset;
  [SerializeField]
  GameObject prefab;
  void Start()
  {
    if (asset == null) {
        throw new Exception("No asset provided.");
    }
    if (prefab == null) {
        throw new Exception("No game object prefab provided.");
    }
    Quaternion rotation = new Quaternion(1, 1, 1, 1);
    float q = 0.0f;
    Color color = new Color(q, q, 1.0f);
    Stream s = new MemoryStream(asset.bytes);
    using (BinaryReader reader = new BinaryReader(s, Encoding.ASCII)) {
      Cols cols = Cols.ReadCols(reader);
      cols.Colis.ForEach(h => {
        Debug.Log("Creating with " + h);
        Debug.Log("H.size is" + h.Size);
        Debug.Log("H.coliobjs.length is" + h.ColiObjs.Count);
        h.ColiObjs.ForEach(j => {
          Debug.Log("Processing obj" + j.ColiType.ToString("X2"));
          if (j.ColiType == 0 && j.ColiSubType == 2) {
            ColiTypeZeroTwo c = (ColiTypeZeroTwo)j;
            Debug.Log("Creating with " + c);
            Vector3 prev = Vector3.negativeInfinity;
            foreach(var v in c.Points) {
              
              Vector3 pos = new Vector3(v.X, v.Y, v.Z);
              GameObject obj = Instantiate(prefab, pos, rotation) as GameObject;
              if (prev != Vector3.negativeInfinity){
                Debug.DrawLine(prev, pos, Color.red, 60);
              }
              prev = pos; 
            }
            // for (int x = 0; x < j.ObjData.Count; x++) {
            //   Vector3 pos = new Vector3(j.ObjData[x], 0, j.ObjData[++x]);
            //   GameObject obj = Instantiate(prefab, pos, rotation) as GameObject; 
            // }
          }
        });
      });
    } 
  }
}  
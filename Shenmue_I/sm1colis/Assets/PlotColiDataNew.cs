using System;
using System.IO;
using System.Text;
using UnityEngine;
using mapinforeader;
using coliplot;

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
      // Debug.Log("read cols.");
    Stream s = new MemoryStream(asset.bytes);
    using (BinaryReader reader = new BinaryReader(s, Encoding.ASCII)) {
      // Debug.Log("staring procesing.");
      Cols cols = Cols.ReadCols(reader);
      // Debug.Log("read cols.");
      cols.Colis.ForEach(h => {
        // Debug.Log("Creating with " + h);
        // Debug.Log("H.size is" + h.Size);
        // Debug.Log("H.coliobjs.length is" + h.ColiObjs.Count);
        int coliObjId = 0;
        h.ColiObjs.ForEach(j => {
          // Debug.Log("Processing obj" + j.ColiType.ToString("X2"));
          if (j.ColiType == 0) {
            switch (j.ColiSubType) {
              case 0x01:{
                ColiTypeZeroOne c = (ColiTypeZeroOne)j;
                // Debug.Log("Creating with " + c);
                Vector3 prev = Vector3.negativeInfinity;
                foreach (var v in c.Points)
                {
                    Vector3 pos = new Vector3(v.X, v.Y, v.Z);
                    GameObject obj = Instantiate(prefab, pos, rotation, gameObject.transform) as GameObject;
                    obj.name = "Point " + coliObjId;
                    obj.GetComponent<Renderer>().material.color = Color.blue;

                    if (Validator.ValidateVector3(prev))
                    {
                        var g = QuadFactory.MakeMesh(prev, pos, obj, "Quad" + coliObjId, Color.blue);
                        Debug.DrawLine(prev, pos, Color.red, 60);
                    }
                    prev = pos;
                    coliObjId++;
                }
                break;
              }
              case 0x02: {
                ColiTypeZeroTwo c = (ColiTypeZeroTwo)j;
                // Debug.Log("Creating with " + c);
                Vector3 prev = Vector3.negativeInfinity;
                foreach (var v in c.Points)
                {
                    Vector3 pos = new Vector3(v.X, v.Y, v.Z);
                    GameObject obj = Instantiate(prefab, pos, rotation, gameObject.transform) as GameObject;
                    obj.name = "Point " + coliObjId;
                    obj.GetComponent<Renderer>().material.color = Color.red;
                    if (Validator.ValidateVector3(prev))
                    {
                        var g = QuadFactory.MakeMesh(prev, pos, obj, "Quad" + coliObjId, Color.red);
                        Debug.DrawLine(prev, pos, Color.red, 60);
                    }
                    prev = pos;
                    coliObjId++;
                }
                break;
              }
              case 0x03: {
                  ColiTypeZeroThree c = (ColiTypeZeroThree)j;
                  foreach (var v in c.Points)
                  {
                      Vector3 pos = new Vector3(v.X, v.Y, v.Z);    
                      GameObject obj = Instantiate(prefab, pos, rotation, gameObject.transform) as GameObject;
                      obj.name = "Point " + coliObjId;
                      obj.GetComponent<Renderer>().material.color = Color.green;
                  }
                  break;
                }
              default:
                break;
            }
          }
        });
      });
      Validator.FindFarObjects();
    } 
  }
}  
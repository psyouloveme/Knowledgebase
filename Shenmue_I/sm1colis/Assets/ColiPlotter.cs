using System.Collections.Generic;
using UnityEngine;
using mapinforeader;

namespace coliplot {
  public static class ColiPlotter {
    public static void PlotColiObj(ColiObj c, GameObject parent) {
      switch(c.ColiType) {
        case 0x00:
          switch (c.ColiSubType) {
            case 0x01: {
              var coli = (ColiTypeZeroOne)c;
              PlotAsQuads(coli.Points, Color.blue, "00 01", parent);
              break;
            }
            case 0x02: {
              var coli = (ColiTypeZeroTwo)c;
              PlotAsQuads(coli.Points, Color.red, "00 02", parent);
              break;
            }
            case 0x05: {
              var coli = (ColiType0005)c;
              PlotAsPoints(coli.Points, Color.yellow, "00 05", parent);
              break;
            }
            case 0x03: {
              var coli = (ColiTypeZeroThree)c;
              PlotAsPoints(coli.Points, Color.green, "00 03", parent);
              break;
            }
            default:
              break;
          }
          break;
        case 0x07:
          switch (c.ColiSubType) {
            case 0x01: {
              var coli = (ColiType0701)c;
              PlotAsQuads(coli.Points, Color.magenta, "07 01", parent);
              break;
            }
            case 0x02:
              {
                var coli = (ColiType0702)c;
                PlotAsQuads(coli.Points, Color.magenta, "07 02", parent);
                break;
              }
            default:
              break;
          }
          break;
        case 0x08:
          switch (c.ColiSubType) {
            case 0x01: {
              var coli = (ColiType0801)c;
              PlotAsQuads(coli.Points, Color.green, "08 01", parent);
              break;
            }
            default:
              break;
          }
          break;
        case 0x09:
          switch (c.ColiSubType)
          {
            case 0x01:
              {
                var coli = (ColiType0901)c;
                PlotAsQuads(coli.Points, Color.cyan, "09 01", parent);
                break;
              }
            case 0x02:
              {
                var coli = (ColiType0902)c;
                PlotAsQuads(coli.Points, Color.black, "09 02", parent);
                break;
              }
            case 0x03:
              {
                var coli = (ColiType0903)c;
                PlotAsPoints(coli.Points, Color.red, "09 03", parent);
                break;
              }
            case 0x05:
              {
                var coli = (ColiType0905)c;
                PlotAsPoints(coli.Points, Color.red, "09 05", parent);
                break;
              }
            default:
              break;
          }
          break;
        case 0x0A:
          switch (c.ColiSubType)
          {
            case 0x01:
              {
                var coli = (ColiType0A01)c;
                PlotAsQuads(coli.Points, Color.white, "09 01", parent);
                break;
              }
            default:
              break;
          }
          break;
        case 0x64:
          switch (c.ColiSubType) {
            case 0x01: {
              var coli = (ColiTypeSixFourZeroOne)c;
              PlotAsQuads(coli.Points, Color.yellow, "64 01", parent);
              break;
            }
            default:
              break;
          }
          break;
        case 0x6E:
          switch (c.ColiSubType)
          {
            case 0x01:
              {
                var coli = (ColiType6E01)c;
                PlotAsQuads(coli.Points, Color.yellow, "6E 01", parent);
                break;
              }
            default:
              break;
          }
          break;
        default:
          break;
      }
    }

    public static void PlotAsPoints(List<System.Numerics.Vector3> vectors, Color color, string name, GameObject parent) {
      foreach(var v in vectors) {
        Vector3 vec = new Vector3(v.X, v.Y, v.Z);
        PlotColiQuad(vec, color, name, parent);
      }
    }

    public static void PlotColiQuad(Vector3 pos, Color color, string name, GameObject parent) {
      GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      sphere.name = name;
      sphere.transform.position = pos;
      sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      sphere.GetComponent<Renderer>().material.color = color;
    }


    public static void PlotAsQuads(List<System.Numerics.Vector3> vectors, Color color, string baseName, GameObject parent) {
      Vector3 prev = Vector3.negativeInfinity;
      foreach (var v in vectors) {
        Vector3 vec = new Vector3(v.X, v.Y, v.Z);
        if (Validator.ValidateVector3(vec)){
          PlotColiQuad(prev, vec, color, baseName, parent);
        }
        prev = vec;
      }
    }

    public static void PlotColiQuad(Vector3 v1, Vector3 v2, Color color, string name, GameObject parent) {
      if (!Validator.ValidateVector3(v1) || !Validator.ValidateVector3(v2)) {
        return;
      }
      QuadFactory.MakeMesh(v1, v2, parent, name, color);
    }
  }
}
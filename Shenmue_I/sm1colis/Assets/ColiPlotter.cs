using System;
using System.Collections.Generic;
using UnityEngine;
using mapinforeader.Models.ColiObjects;
using mapinforeader.Models.ColsSections;

namespace coliplot {
  public static class ColiPlotter {

    public static void PlotHghtObj(HghtObject h, GameObject parent, string labelStart = ""){
      switch(h.ShapeId) {
        case 0x06: {
          PlotAsQuads(h.Coordinates, Color.green, $"{labelStart} {h.ShapeId.ToString("X2")}", parent);
          break;
        }
        case 0x05: {
          PlotAsPoints(h.Coordinates, Color.green, $"{labelStart} {h.ShapeId.ToString("X2")}", parent);
          break;
        }
        default:
          break;
      }
    }

    public static void PlotColiObj(ColiObject c, GameObject parent, string labelStart = "") {
      switch(c.ShapeId) {
        case 1: {
          var coli = (ColiType1)c;
          PlotAsQuads(coli.Coordinates, Color.blue, $"{labelStart} 01", parent);
          break;
        }
        case 2: {
          var coli = (ColiType2)c;
          PlotAsQuads(coli.Coordinates, Color.red, $"{labelStart} 02", parent);
          break;
        }
        case 3: {
          var coli = (ColiType3)c;
          var name = BitConverter.ToString(BitConverter.GetBytes(coli.Data[0]));
          PlotAsPoint(coli.Coordinate, Color.cyan, $"{labelStart} 03 {name}", parent);
          break;
        }
        case 5: {
          var coli = (ColiType5)c;
          var name = BitConverter.ToString(BitConverter.GetBytes(coli.Data[0]));
          PlotAsPoint(coli.Coordinate, Color.yellow, $"{labelStart} 05 {name}", parent);
          break;
        }
      }
    }

    public static void PlotAsPoints(List<System.Numerics.Vector3> vectors, Color color, string name, GameObject parent) {
      foreach(var v in vectors) {
        Vector3 vec = new Vector3(v.X, v.Y, v.Z);
        PlotColiQuad(vec, color, name, parent);
      }
    }

    public static void PlotAsPoint(System.Numerics.Vector3 vector, Color color, string name, GameObject parent) {
      Vector3 vec = new Vector3(vector.X, vector.Y, vector.Z);
      PlotColiQuad(vec, color, name, parent);
    }

    public static void PlotColiQuad(Vector3 pos, Color color, string name, GameObject parent) {
      GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      sphere.name = name;
      sphere.transform.position = pos;
      sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
      sphere.GetComponent<Renderer>().material.color = color;
      if (parent != null)
      {
        sphere.transform.SetParent(parent.transform);
      }

    }


    public static void PlotAsQuads(List<System.Numerics.Vector3> vectors, Color color, string baseName, GameObject parent) {
      Vector3 prev = Vector3.negativeInfinity;
      foreach (var v in vectors) {
        Vector3 vec = new Vector3(v.X, v.Y, v.Z);
        if (Validator.ValidateVector3(prev) && Validator.ValidateVector3(vec)){
          PlotColiQuad(prev, vec, color, baseName, parent);
        }
        prev = vec;
      }
    }

    public static void PlotColiQuad(Vector3 v1, Vector3 v2, Color color, string name, GameObject parent) {
      if (!Validator.ValidateVector3(v1) || !Validator.ValidateVector3(v2)) {
        Debug.LogWarningFormat("Not plotting quad between invalid coordinates:\n{0}\n{1}", v1, v2);
        return;
      }
      QuadFactory.MakeMesh(v1, v2, parent, name, color);
    }
  }
}
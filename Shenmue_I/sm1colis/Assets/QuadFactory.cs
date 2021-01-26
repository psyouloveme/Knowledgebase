using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace coliplot {
    public static class QuadFactory {
        public static GameObject MakeMesh(Vector3 p1, Vector3 p2, GameObject parent, string basename, Color color)
        {
            Vector3[] vertices = new Vector3[4]
            {
                new Vector3(p1.x, p1.y, p1.z),
                new Vector3(p2.x, p1.y, p2.z),
                new Vector3(p1.x, 5, p1.z),
                new Vector3(p2.x, 5, p2.z)

            };
            int[] tris = new int[6]
            {
                0, 2, 1,
                3, 1, 2
            };
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            Vector3[] normals = new Vector3[4]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward
            };

            GameObject g = new GameObject();
            MeshRenderer meshRenderer = g.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
            MeshFilter meshFilter = g.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.triangles = tris;
            mesh.normals = normals;
            mesh.uv = uv;
            meshFilter.mesh = mesh;
            g.name = basename;
            if (color != null) {
                g.GetComponent<Renderer>().material.color = color;
            }
            
            if (parent != null)
            {
                g.transform.SetParent(parent.transform);
            }

            Array.Reverse(tris);

            GameObject r = new GameObject();
            MeshRenderer meshRendererr = r.AddComponent<MeshRenderer>();
            meshRendererr.sharedMaterial = new Material(Shader.Find("Standard"));
            MeshFilter meshFilterr = r.AddComponent<MeshFilter>();
            Mesh meshr = new Mesh();
            meshr.vertices = vertices;
            meshr.triangles = tris;
            meshr.normals = normals;
            meshr.uv = uv;
            meshFilterr.mesh = meshr;
            r.name = basename + " Back";
            if (color != null) {
                r.GetComponent<Renderer>().material.color = color;
            }
            if (g != null)
            {
                r.transform.SetParent(g.transform);
            }
            return g;
        }
    }
}
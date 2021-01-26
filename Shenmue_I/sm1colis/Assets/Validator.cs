using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

namespace coliplot {
    public static class Validator {
        public static bool ValidateVector3(Vector3 v) {
            return ValidateFloat(v.x) && ValidateFloat(v.y) && ValidateFloat(v.z);
        }
        public static bool ValidateFloat(float f) {
            return CheckNaN(f) && CheckInfinity(f);
        }
        public static bool CheckNaN(float f) {
            if (Single.IsNaN(f)){
                return false;
            }
            return true;
        }
        public static bool CheckInfinity(float f)
        {
            if (Single.IsInfinity(f))
            {
                return false;
            }
            return true;
        }

        //https://forum.unity.com/threads/assertion-failed-invalid-worldaabb-object-is-too-large-or-too-far-away-from-the-origin.486290/#post-6015386
        public static void FindFarObjects()
        {
            List<GameObject> farObjs = new List<GameObject>();
            var allObjs = GameObject.FindObjectsOfType<GameObject>();
            for (var i = 0; i < allObjs.Length; i++)
            {
                if ((Mathf.Abs(allObjs[i].transform.position.x) > 1000) ||
                    (Mathf.Abs(allObjs[i].transform.position.y) > 500) ||
                    (Mathf.Abs(allObjs[i].transform.position.z) > 1000)
                )
                {
                    farObjs.Add(allObjs[i]);
                }
            }

            if (farObjs.Count > 0)
            {
                for (var i = 0; i < farObjs.Count; i++)
                {
                    Debug.LogError($"Found object {farObjs[i].name} at location {farObjs[i].transform.position}");
                }
            }
            else
            {
                Debug.Log("No Far objects");
            }
        }
    }
}
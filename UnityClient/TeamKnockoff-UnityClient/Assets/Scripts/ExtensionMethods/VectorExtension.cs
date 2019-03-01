using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ExtensionMethods {
    public static class VectorExtension {
        public static Vector2 ToVector2(this Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector2Int ToVector2Int(this Vector3 vector) {
            var newCol = Convert.ToInt32(Math.Ceiling(vector.x));
            var newRow = Convert.ToInt32(Math.Ceiling(vector.y));
            return new Vector2Int(newCol, newRow);
        }

        public static Vector3 ToVector3(this Vector2 vector) {
            return new Vector3(vector.x, vector.y, 0f);
        }

        public static Vector2Int ToVector2Int(this Vector2 vector) {
            var newCol = Convert.ToInt32(Math.Ceiling(vector.x));
            var newRow = Convert.ToInt32(Math.Ceiling(vector.y));
            return new Vector2Int(newCol, newRow);
        }

    }
}

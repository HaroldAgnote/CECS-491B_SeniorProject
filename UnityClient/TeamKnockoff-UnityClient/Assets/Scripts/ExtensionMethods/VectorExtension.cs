using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public static Vector3 ToVector3(this Vector2Int vector) {
            return new Vector3(vector.x, vector.y, 0f);
        }

        public static Vector2Int ToVector2Int(this Vector2 vector) {
            var newCol = Convert.ToInt32(Math.Ceiling(vector.x));
            var newRow = Convert.ToInt32(Math.Ceiling(vector.y));
            return new Vector2Int(newCol, newRow);
        }

        public static IEnumerable<Vector2Int> GetNeighbors(this Vector2Int vector) {
            var neighbors = new List<Vector2Int> {
                vector + Vector2Int.up,
                vector + Vector2Int.down,
                vector + Vector2Int.left,
                vector + Vector2Int.right
            };

            return neighbors;
        }

        public static int CompareTo(this Vector2Int first, Vector2Int second) {
            if (first.x.CompareTo(second.x) == 0 ){
                return first.y.CompareTo(second.y);
            }
            return first.x.CompareTo(second.x);
        }

        public static IEnumerable<Vector2Int> GetRectangularPositions(int rows, int cols) {
            return Enumerable.Range(0, rows).SelectMany(
                row => Enumerable.Range(0, cols),
                (col, row) => new Vector2Int(col, row));

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Utilities.ExtensionMethods {
    public static class VectorExtension {

        public static Vector2Int NORTH = Vector2Int.up;
        public static Vector2Int NORTH_EAST = Vector2Int.up + Vector2Int.right;
        public static Vector2Int NORTH_WEST = Vector2Int.up + Vector2Int.left;
        public static Vector2Int SOUTH = Vector2Int.down;
        public static Vector2Int SOUTH_EAST = Vector2Int.down + Vector2Int.right;
        public static Vector2Int SOUTH_WEST = Vector2Int.down + Vector2Int.left;
        public static Vector2Int EAST = Vector2Int.right;
        public static Vector2Int WEST = Vector2Int.left;

        public enum VectorDirection {
            North,
            NorthEast,
            NorthWest,
            South,
            SouthEast,
            SouthWest,
            East,
            West,
            None,
        }

        public static Vector2 ToVector2(this Vector3 vector) {
            return new Vector2(vector.x, vector.y);
        }

        public static Vector2Int ToVector2Int(this Vector3 vector) {
            var newCol = Convert.ToInt32(Math.Ceiling(vector.x));
            var newRow = Convert.ToInt32(Math.Ceiling(vector.y));
            return new Vector2Int(newCol, newRow);
        }

        public static VectorDirection GetVectorDirection(this Vector2Int vector, Vector2Int other) {
            var difference = other - vector;
            if (difference.x > 0) {
                if (difference.y > 0) {
                    return VectorDirection.NorthEast;
                } else if (difference.y < 0) {
                    return VectorDirection.SouthEast;
                } else {
                    return VectorDirection.East;
                }
            } else if (difference.x < 0) {
                if (difference.y > 0) {
                    return VectorDirection.NorthWest;
                } else if (difference.y < 0) {
                    return VectorDirection.SouthWest;
                } else {
                    return VectorDirection.West;
                }
            } else {
                if (difference.y > 0) {
                    return VectorDirection.North;
                } else if (difference.y < 0) {
                    return VectorDirection.South;
                } else {
                    return VectorDirection.None;
                }
            }
        }

        public static Vector2Int GetVectorTowardsDirection(VectorDirection direction) {
            switch (direction) {
                case VectorDirection.North:
                    return NORTH;
                case VectorDirection.NorthEast:
                    return NORTH_EAST;
                case VectorDirection.NorthWest:
                    return NORTH_WEST;
                case VectorDirection.South:
                    return SOUTH;
                case VectorDirection.SouthEast:
                    return SOUTH_EAST;
                case VectorDirection.SouthWest:
                    return SOUTH_WEST;
                case VectorDirection.East:
                    return EAST;
                case VectorDirection.West:
                    return WEST;
                case VectorDirection.None:
                    return new Vector2Int(0, 0);
                default:
                    throw new Exception("Bad direction");
            }
        }

        public static Vector2Int Average(this IEnumerable <Vector2Int> vectors) {
            var averageX = 0;
            var averageY = 0;
            foreach (var pos in vectors)
            {
                averageX += pos.x;
                averageY += pos.y;
            }

            return new Vector2Int(averageX / vectors.Count(), averageY / vectors.Count());
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

        public static IEnumerable<Vector2Int> GetRectangularPositions(int cols, int rows) {
            return Enumerable.Range(0, cols)
                .SelectMany( col => Enumerable.Range(0, rows), (col, row) => new Vector2Int(col, row));

        }
    }
}

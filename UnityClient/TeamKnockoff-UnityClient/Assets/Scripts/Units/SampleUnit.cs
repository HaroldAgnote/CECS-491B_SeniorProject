using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Units {
    public class SampleUnit : Unit {
        public SampleUnit() {
            Name = "Sample Unit";
            HealthPoints = 100;
            MoveRange = 5;
            AttackRange = 1;
        }

        public override List<Vector2Int> GetMoveLocations(Vector2Int gridPoint) {
            var moveLocations = new List<Vector2Int>();
            int rows = GameManager.instance.boardScript.rows;
            int columns = GameManager.instance.boardScript.columns;
            int[,] distance = new int[rows, columns];
            for (int col = 0; col < columns; col++) {
                for (int row = 0; row < rows; row++) {
                    distance[col, row] = Int32.MaxValue;
                }
            }

            distance[gridPoint.x, gridPoint.y] = 0;
            var tileQueue = new Queue<Tile>();
            var rootTile = GameManager.instance.tiles[gridPoint.x, gridPoint.y];
            tileQueue.Enqueue(rootTile);

            while (tileQueue.Count > 0) {
                Tile current = tileQueue.Dequeue();
                foreach (var neighbor in current.Neighbors) {
                    if (distance[neighbor.XPosition, neighbor.YPosition] > MoveRange) {
                        int movementCost = neighbor.MoveCost;
                        distance[neighbor.XPosition, neighbor.YPosition] = movementCost + distance[current.XPosition, current.YPosition];

                        if (distance[neighbor.XPosition, neighbor.YPosition] <= MoveRange) {
                            tileQueue.Enqueue(neighbor);
                        }
                    }
                }
                if (distance[current.XPosition, current.YPosition] > 0 && distance[current.XPosition, current.YPosition] <= MoveRange) {
                    moveLocations.Add(new Vector2Int(current.XPosition, current.YPosition));
                }
            }

            return moveLocations;
        }

        public override List<Vector2Int> GetAttackLocations(Vector2Int gridPoint) {
            throw new NotImplementedException();
        }

    }
}

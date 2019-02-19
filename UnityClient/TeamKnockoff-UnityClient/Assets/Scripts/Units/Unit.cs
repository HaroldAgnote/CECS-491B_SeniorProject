using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public abstract class Unit : MonoBehaviour
    {
        public double HealthPoints { get; set; }

        public int Strength { get; set; }
        public int Magic { get; set; }
        public int Defense { get; set; }
        public int Resistance { get; set; }
        public int Speed { get; set; }
        public int Skill { get; set; }
        public int Luck { get; set; }

        public int MoveRange { get; set; }
        public IMover Mover { get; set; }

        public Weapon MainWeapon { get; set; }

        public string Name { get; set; }

        public List<Vector2Int> MoveLocations { get; set; }

        public List<Vector2Int> GetMoveLocations(Vector2Int gridPoint) {
                var moveLocations = new List<Vector2Int>();
                int rows = GameManager.instance.boardScript.rows;
                int columns = GameManager.instance.boardScript.columns;
                int[,] distance = new int[columns, rows];
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
                
                    // If unit can't move to this tile, check the next tile in the queue.
                    if (!Mover.CanMove(current)) {
                        continue;
                    }

                    // Otherwise, move to this tile and check the neighboring tiles.
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

        public List<Vector2Int> GetAttackLocations(Vector2Int gridPoint) {
                var moveLocations = new List<Vector2Int>();
                int rows = GameManager.instance.boardScript.rows;
                int columns = GameManager.instance.boardScript.columns;
                int[,] distance = new int[columns, rows];
                for (int col = 0; col < columns; col++) {
                    for (int row = 0; row < rows; row++) {
                        distance[col, row] = Int32.MaxValue;
                    }
                }

                distance[gridPoint.x, gridPoint.y] = 0;
                var tileQueue = new Queue<Tile>();
                var rootTile = GameManager.instance.tiles[gridPoint.x, gridPoint.y];
                tileQueue.Enqueue(rootTile);

                // TODO: Change maximum attack range according to whatever skills the unit has
                while (tileQueue.Count > 0) {
                    Tile current = tileQueue.Dequeue();
                    // If unit can't move to this tile, check the next tile in the queue.
                    if (!Mover.CanMove(current)) {
                        continue;
                    }

                    // Otherwise, move to this tile and check the neighboring tiles.
                    foreach (var neighbor in current.Neighbors) {
                        if (distance[neighbor.XPosition, neighbor.YPosition] > (MoveRange + MainWeapon.Range)) {
                            int movementCost = neighbor.MoveCost;
                            distance[neighbor.XPosition, neighbor.YPosition] = movementCost + distance[current.XPosition, current.YPosition];

                            if (distance[neighbor.XPosition, neighbor.YPosition] <= (MoveRange + MainWeapon.Range)) {
                                tileQueue.Enqueue(neighbor);
                            }
                        }
                    }
                    if (distance[current.XPosition, current.YPosition] > 0 && distance[current.XPosition, current.YPosition] <= (MoveRange + MainWeapon.Range)) {
                        moveLocations.Add(new Vector2Int(current.XPosition, current.YPosition));
                    }
                }

                return moveLocations;
        }
    }
}

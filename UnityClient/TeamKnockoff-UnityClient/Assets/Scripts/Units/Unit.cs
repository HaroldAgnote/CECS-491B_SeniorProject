using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Units {
    public abstract class Unit : MonoBehaviour, IMover {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Class { get; set; }

        // Health Points are the life points of the Unit
        // If Health Points is zero, the unit is dead
        // Health Points can never exceed max health points
        public double HealthPoints { get; set; }
        public double MaxHealthPoints { get; set; }

        public int Level { get; protected set; }
        public int ExperiencePoints { get; protected set; }

        public int Strength { get; set; }
        public int Magic { get; set; }

        public int Defense { get; set; }
        public int Resistance { get; set; }

        public int Speed { get; set; }
        public int Skill { get; set; }

        public int Luck { get; set; }

        public int MoveRange { get; set; }

        public Weapon MainWeapon { get; set; }

        // TODO: Add Item Properties

        // Abstract methods that must be overridden by Unit sub classes
        public abstract bool CanMove(Tile tile);
        public abstract int MoveCost(Tile tile);

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

            moveLocations.Add(new Vector2Int(gridPoint.x, gridPoint.y));
            while (tileQueue.Count > 0) {
                Tile current = tileQueue.Dequeue();

                // If unit can't move to this tile, check the next tile in the queue.
                if (!CanMove(current)) {
                    continue;
                }

                // TODO: Need to find a way to prevent unit moving through tiles where 
                // there's an enemy.
                var unitAtPoint = GameManager.instance.UnitAtGrid(new Vector3(current.XPosition, current.YPosition, 0f));
                if (unitAtPoint != null && !GameManager.instance.DoesUnitBelongToCurrentPlayer(unitAtPoint)&& GameManager.instance.EnemyUnitIsAlive(unitAtPoint) ) {
                    continue;
                }

                // Otherwise, move to this tile and check the neighboring tiles.
                foreach (var neighbor in current.Neighbors) {
                    if (distance[neighbor.XPosition, neighbor.YPosition] > MoveRange) {
                        int movementCost = MoveCost(neighbor);
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
            moveLocations = moveLocations.Where(loc => loc == gridPoint ||
                                                    GameManager.instance.UnitAtGrid(new Vector3(loc.x, loc.y, 0f)) == null).ToList();
            return moveLocations;
        }

        public List<Vector2Int> GetAttackLocations(Vector2Int gridPoint) {
            var attackLocations = GetMoveLocations(gridPoint);

            // Temporary
            MainWeapon = new Weapon(50, 2, 100, 1, DamageCalculator.DamageType.Physical);

            for (int i = 0; i < MainWeapon.Range; i++) {
                var tempAttackLocs = new List<Vector2Int>();
                foreach (var moveLoc in attackLocations) {
                    var moveLocNeighbors = GameManager.instance.tiles[moveLoc.x, moveLoc.y]
                        .Neighbors
                        .Select(tile => new Vector2Int(tile.XPosition, tile.YPosition))
                        .Where(pos => !attackLocations.Contains(pos));

                    tempAttackLocs.AddRange(moveLocNeighbors); 
                }
                attackLocations.AddRange(tempAttackLocs);
            }

            attackLocations = attackLocations.Where(x => 
                                !GameManager.instance.DoesUnitBelongToCurrentPlayer(
                                GameManager.instance.UnitAtGrid(new Vector3(x.x, x.y, 0f))))
                                .ToList();


            return attackLocations;
        }
    }
}

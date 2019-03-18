using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Application;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Utilities.WeightedGraph;

namespace Assets.Scripts.Model {
    public class GameModel : MonoBehaviour {

        #region Member fields

        private Unit[,] mUnits;
        private Tile[,] mTiles;
        private List<Player> mPlayers;

        #endregion

        #region Properties

        public int Turn { get; private set; }

        public Player CurrentPlayer { get; private set; }

        #endregion

        public void ConstructModel() {
            int cols = GameManager.instance.Columns;
            int rows = GameManager.instance.Rows;
            mUnits = new Unit[cols, rows];
            mTiles = new Tile[cols, rows];
            mPlayers = new List<Player>();

            // Start turns
            Turn = 1;
        }

        public void StartGame() {
            CurrentPlayer = mPlayers.First();
            CurrentPlayer.StartTurn();
        }

        public void AddPlayer(string playerName) {
            mPlayers.Add(new Player(playerName));
        }

        public void AddPlayer(Player player) {
            mPlayers.Add(player);
        }

        public void AddPlayers(IEnumerable<Player> players) {
            mPlayers.AddRange(players);
        }

        public void AddUnit(Unit unit, int playerNum, int col, int row) {
            mPlayers[playerNum - 1].AddUnit(unit);
            mUnits[col, row] = unit;
        }

        public void AddUnit(Unit unit, Player player, int col, int row) {
            player.AddUnit(unit);
            mUnits[col, row] = unit;
        }

        public void AddTile(Tile tile) {
            mTiles[tile.XPosition, tile.YPosition] = tile;
        }

        public Unit GetUnitAtPosition(Vector2Int vector) {
            return GetUnitAtPosition(vector.x, vector.y);
        }

        public Unit GetUnitAtPosition(int col, int row) {
            try {
                return mUnits[col, row];
            } catch {
                return null;
            }
        }

        public Tile GetTileAtPosition(Vector2Int vector) {
            return GetTileAtPosition(vector.x, vector.y);
        }

        public Tile GetTileAtPosition(int col, int row) {
            try {
                return mTiles[col, row];
            } catch {
                return null;
            }
        }

        public Vector2Int GridForUnit(Unit unit) {
            for (int col = 0; col < mUnits.GetLength(0); col++) {
                for (int row = 0; row < mUnits.GetLength(1); row++) {
                    if (mUnits[col, row] == unit) {
                        return new Vector2Int(col, row);
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }

        public bool DoesUnitBelongToCurrentPlayer(Unit unit) {
            return CurrentPlayer.Units.Contains(unit);
        }

        public bool UnitIsAlive(Unit unit) {
            return unit.HealthPoints > 0;
        }

        public void AddNeighbors() {
            int columns = GameManager.instance.Columns;
            int rows = GameManager.instance.Rows;

            for (int column = 0; column < columns; column++) {
                for (int row = 0; row < rows; row++) {
                    var tile = mTiles[column, row];
                    try {
                        mTiles[column - 1, row].Neighbors.Add(tile);
                    } catch { }
                    try {
                        mTiles[column + 1, row].Neighbors.Add(tile);
                    } catch { }

                    try {
                        mTiles[column, row - 1].Neighbors.Add(tile);
                    } catch { }
                    try {
                        mTiles[column, row + 1].Neighbors.Add(tile);
                    } catch { }
                }
            }
        }

        public bool CurrentPlayerHasNoMoves {
            get { return !CurrentPlayer.UnitHasMoved.Contains(false); }
        }


        public bool GameHasEnded {
            get {
                // Need a better way to check if game has ended 
                // With multiple players
                if (mPlayers.Count == 2) {
                    var playerOne = mPlayers[0];
                    var playerTwo = mPlayers[1];

                    if (!playerOne.HasAliveUnit()) {
                        return true;
                    } else if (!playerTwo.HasAliveUnit()) {
                        return true;
                    }
                    return false;
                } else {
                    return false;
                }
            }
        }

        public bool UnitHasMoved(Unit unit) {
            return CurrentPlayer.CheckUnitHasMoved(unit);
        }

        public void ApplyMove(GameMove move) {
            switch (move.MoveType) {
                case GameMove.GameMoveType.Move:
                    MoveUnit(move);
                    break;
                case GameMove.GameMoveType.Attack:
                    AttackUnit(move);
                    break;
                case GameMove.GameMoveType.Skill:
                    SkillUnit(move);
                    break;
                case GameMove.GameMoveType.Wait:
                    WaitUnit(move);
                    break;
            }
            // TODO: Check Game State


            // Check if Current Player has no moves and switch if so
            if (!GameHasEnded && CurrentPlayerHasNoMoves) {
                SwitchPlayer();
            }
        }

        public void SwitchPlayer() {
            Debug.Log("Switching Player!");
            var index = mPlayers.FindIndex(player => player == CurrentPlayer);
            CurrentPlayer = mPlayers[(index + 1) % mPlayers.Count];
            CurrentPlayer.StartTurn();
        }

        private void MoveUnit(GameMove move) {
            var unit = GetUnitAtPosition(move.StartPosition);
            mUnits[move.StartPosition.x, move.StartPosition.y] = null;
            mUnits[move.EndPosition.x, move.EndPosition.y] = unit;
        }

        private void AttackUnit(GameMove move) {
            var attackingUnit = GetUnitAtPosition(move.StartPosition);
            var defendingUnit = GetUnitAtPosition(move.EndPosition);

            // Attack Logic Here
            Debug.Log($"{attackingUnit.Name} attacks {defendingUnit.Name}");
            defendingUnit.HealthPoints = defendingUnit.HealthPoints - DamageCalculator.GetDamage(attackingUnit, defendingUnit);
            if (defendingUnit.HealthPoints > 0) //check if unit is alive 
                //TODO CHECK RANGE OF UNIT COUNTER
            {
                Debug.Log($"{defendingUnit.Name} counter-attacks {attackingUnit.Name}");
                attackingUnit.HealthPoints = attackingUnit.HealthPoints - DamageCalculator.GetDamage(defendingUnit, attackingUnit);

                if (attackingUnit.HealthPoints <= 0) {
                    CurrentPlayer.MarkUnitAsInactive(attackingUnit);
                }
            } else {
                foreach (var mPlayer in mPlayers) {
                    if (mPlayer.Units.Contains(defendingUnit)) {
                        mPlayer.MarkUnitAsInactive(defendingUnit);
                    } 
                }
            }
            Debug.Log(attackingUnit.UnitInformation + "\n\n");
            Debug.Log(defendingUnit.UnitInformation);

            CurrentPlayer.MarkUnitAsMoved(attackingUnit);
        }

        private void SkillUnit(GameMove move)
        {
            var attackingUnit = GetUnitAtPosition(move.StartPosition);
            var defendingUnit = GetUnitAtPosition(move.EndPosition);

            // Attack Logic Here
            Debug.Log($"{attackingUnit.Name} attacks with skill {defendingUnit.Name}");
            defendingUnit.HealthPoints = defendingUnit.HealthPoints - DamageCalculator.GetDamage(attackingUnit, defendingUnit);
            if (defendingUnit.HealthPoints > 0) //check if unit is alive 
                                                //TODO CHECK RANGE OF UNIT COUNTER
            {
                Debug.Log($"{defendingUnit.Name} counter-attacks {attackingUnit.Name}");
                //instead of Skill[0] of we probably need selected skill or something
                attackingUnit.HealthPoints = attackingUnit.HealthPoints - DamageCalculator.GetSkillDamage(defendingUnit, attackingUnit, attackingUnit.Skills[0]);
            }
            Debug.Log(attackingUnit.UnitInformation + "\n\n");
            Debug.Log(defendingUnit.UnitInformation);

            CurrentPlayer.MarkUnitAsMoved(attackingUnit);
        }

        private void WaitUnit(GameMove move) {
            var unit = GetUnitAtPosition(move.StartPosition);
            CurrentPlayer.MarkUnitAsMoved(unit);
        }

        public List<Vector2Int> GetUnitMoveLocations(Unit unit) {
            var gridPoint = GridForUnit(unit);

            var moveLocations = new List<Vector2Int>();
            int columns = mTiles.GetLength(0);
            int rows = mTiles.GetLength(1);
            int[,] distance = new int[columns, rows];
            for (int col = 0; col < columns; col++) {
                for (int row = 0; row < rows; row++) {
                    distance[col, row] = Int32.MaxValue;
                }
            }

            distance[gridPoint.x, gridPoint.y] = 0;
            var tileQueue = new Queue<Tile>();
            var rootTile = mTiles[gridPoint.x, gridPoint.y];
            tileQueue.Enqueue(rootTile);

            moveLocations.Add(new Vector2Int(gridPoint.x, gridPoint.y));
            while (tileQueue.Count > 0) {
                Tile current = tileQueue.Dequeue();

                // If unit can't move to this tile, check the next tile in the queue.
                if (!unit.CanMove(current)) {
                    continue;
                }

                // TODO: Need to find a way to prevent unit moving through tiles where 
                // there's an enemy.
                var unitAtPoint = GetUnitAtPosition(new Vector2Int(current.XPosition, current.YPosition));
                if (unitAtPoint != null && !DoesUnitBelongToCurrentPlayer(unitAtPoint) && UnitIsAlive(unitAtPoint) ) {
                    continue;
                }

                // Otherwise, move to this tile and check the neighboring tiles.
                foreach (var neighbor in current.Neighbors) {
                    if (distance[neighbor.XPosition, neighbor.YPosition] > unit.MoveRange) {
                        int movementCost = unit.MoveCost(neighbor);
                        distance[neighbor.XPosition, neighbor.YPosition] = movementCost + distance[current.XPosition, current.YPosition];

                        if (distance[neighbor.XPosition, neighbor.YPosition] <= unit.MoveRange) {
                            tileQueue.Enqueue(neighbor);
                        }
                    }
                }
                if (distance[current.XPosition, current.YPosition] > 0 && distance[current.XPosition, current.YPosition] <= unit.MoveRange) {
                    moveLocations.Add(new Vector2Int(current.XPosition, current.YPosition));
                }
            }
            return moveLocations;
        }

        public List<Vector2Int> GetPossibleUnitMoveLocations(Unit unit) {
            var gridPoint = GridForUnit(unit);
            var moveLocations = GetUnitMoveLocations(unit)
                                .Where(loc => 
                                    loc == gridPoint 
                                    || !TileIsOccupied(loc)
                                    || (TileIsOccupied(loc) && GetUnitAtPosition(loc).HealthPoints <= 0)).ToList();;
            return moveLocations;
        }


        public Dictionary<Vector2Int, int> GetUnitMoveCosts(Unit unit) {
            var moveCosts = new Dictionary<Vector2Int, int>();

            var moveLocations = GetUnitMoveLocations(unit);

            foreach (var loc in moveLocations) {
                var tile = mTiles[loc.x, loc.y];

                moveCosts.Add(loc, unit.MoveCost(tile));

            }

            return moveCosts;
        }

        public List<Vector2Int> GetUnitAttackLocations(Unit unit) {
            var gridPoint = GridForUnit(unit);
            var attackLocations = GetUnitMoveLocations(unit);

            // Temporary

            for (int i = 0; i < unit.MainWeapon.Range; i++) {
                var tempAttackLocs = new List<Vector2Int>();
                foreach (var moveLoc in attackLocations) {
                    var moveLocNeighbors = mTiles[moveLoc.x, moveLoc.y]
                        .Neighbors
                        .Select(tile => new Vector2Int(tile.XPosition, tile.YPosition))
                        .Where(pos => !attackLocations.Contains(pos));

                    tempAttackLocs.AddRange(moveLocNeighbors); 
                }
                attackLocations.AddRange(tempAttackLocs);
            }

            attackLocations = attackLocations
                                .Where(pos => 
                                    !TileIsOccupied(pos)
                                    || EnemyAtLocation(pos))
                                .ToList();

            return attackLocations;
        }

        public List<Vector2Int> GetPossibleUnitAttackLocations(Unit unit) {
            var moveLocations = GetPossibleUnitMoveLocations(unit);
            var attackLocations = GetUnitAttackLocations(unit);

            var filteredAttackLocations = new List<Vector2Int>();
            foreach (var loc in attackLocations) {
                var surroundingLocations = GetSurroundingAttackLocationsAtPoint(loc, unit.MainWeapon.Range);
                var attackPoints = surroundingLocations.Where(sLoc => moveLocations.Contains(sLoc));
                if (attackPoints.Count() > 0) {
                    filteredAttackLocations.Add(loc);
                }
            }

            return filteredAttackLocations;
        }

        public bool TileIsOccupied(Vector2Int position) {
            return GetUnitAtPosition(position) != null && GetUnitAtPosition(position).HealthPoints > 0;
        }

        public bool EnemyWithinRange(Vector2Int position, int range) {
            var surroundingLocations = GetSurroundingAttackLocationsAtPoint(position, range);

            return surroundingLocations.Any(pos => EnemyAtLocation(pos));
        }

        public List<Vector2Int> GetSurroundingAttackLocationsAtPoint(Vector2Int attackPoint, int range) {

            var possibleAttackLocations = new List<Vector2Int> {
                attackPoint
            };

            for (int i = 0; i < range; i++) {
                var tempAttackLocs = new List<Vector2Int>();
                foreach (var attackLoc in possibleAttackLocations) {
                    var attackLocNeighbors = mTiles[attackLoc.x, attackLoc.y]
                        .Neighbors
                        .Select(tile => new Vector2Int(tile.XPosition, tile.YPosition))
                        .Where(pos => !possibleAttackLocations.Contains(pos));

                    tempAttackLocs.AddRange(attackLocNeighbors);
                }
                possibleAttackLocations.AddRange(tempAttackLocs);
            }

            return possibleAttackLocations;

        }

        public bool EnemyAtLocation(Vector2Int location) {
            var unit = GetUnitAtPosition(location);
            return unit != null && unit.HealthPoints > 0 && !CurrentPlayer.Units.Contains(unit);
        }
        
        public List<Vector2Int> GetShortestPath(Unit unit, Vector2Int startPoint, Vector2Int endPoint) {
            var unitCosts = GetUnitMoveCosts(unit);
            var moveGraph = new WeightedGraph(unitCosts);

            var distances = moveGraph.GetShortestDistancesFrom(startPoint);

            var shortestDistanceToEnd = distances.SingleOrDefault(d => d.Vertex == endPoint);

            return shortestDistanceToEnd.Path;
        }
        
        public List<Vector2Int> GetShortestPathToAttack(Unit unit, Vector2Int startPoint, Vector2Int targetPoint) {
            var unitCosts = GetUnitMoveCosts(unit);
            var moveGraph = new WeightedGraph(unitCosts);

            var availableAttackLocations = GetUnitMoveLocations(unit);

            var possibleAttackLocations = GetSurroundingAttackLocationsAtPoint(targetPoint, unit.MainWeapon.Range);

            possibleAttackLocations = possibleAttackLocations.Where(pos =>
                                        (!TileIsOccupied(pos) || GetUnitAtPosition(pos) == unit))
                                        .Where(pos => availableAttackLocations.Contains(pos))
                                        .ToList();

            var distances = moveGraph.GetShortestDistancesFrom(startPoint);

            var attackDistances = distances.Where(pos => possibleAttackLocations.Contains(pos.Vertex));

            var shortestDistanceToAttack = attackDistances.Min();

            return shortestDistanceToAttack.Path;
        }

        //return copies of game data
        public Unit[,] GetAllUnitsAndPositions()
        {
            Unit[,] copy = mUnits.Clone() as Unit[,];
            return copy;
        }

        public Tile[,] GetAllTilesAndPositions()
        {
            Tile[,] copy = mTiles.Clone() as Tile[,];
            return copy;
        }
    } 
}

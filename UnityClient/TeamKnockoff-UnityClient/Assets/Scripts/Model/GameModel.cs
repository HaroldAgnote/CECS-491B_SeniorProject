using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Application;
using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Tiles;
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

        // Returns the Unit at a position
        public Unit GetUnitAtPosition(Vector2Int vector) {
            return GetUnitAtPosition(vector.x, vector.y);
        }

        // Returns the Unit at a position
        public Unit GetUnitAtPosition(int col, int row) {
            try {
                return mUnits[col, row];
            } catch {
                return null;
            }
        }

        // Returns the Tile at a position
        public Tile GetTileAtPosition(Vector2Int vector) {
            return GetTileAtPosition(vector.x, vector.y);
        }

        // Returns the Tile at a position
        public Tile GetTileAtPosition(int col, int row) {
            try {
                return mTiles[col, row];
            } catch {
                return null;
            }
        }

        // Returns the position of a Unit
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
            } else if (GameHasEnded) {
                Debug.Log("Game Over");
            }
        }

        public void SwitchPlayer() {
            Debug.Log("Switching Player!");
            int index = mPlayers.FindIndex(player => player == CurrentPlayer);
            index = (index + 1) % mPlayers.Count;
            CurrentPlayer = mPlayers[index];
            if (index == 0) {
                Turn++;
            }
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

            int hitChance = DamageCalculator.GetHitChance(attackingUnit, defendingUnit);
            if(DamageCalculator.DiceRoll(hitChance))
            {
                Debug.Log($"{attackingUnit.Name} attacks {defendingUnit.Name}");
                defendingUnit.HealthPoints = defendingUnit.HealthPoints - DamageCalculator.GetDamage(attackingUnit, defendingUnit);
            }
            else
            {
                Debug.Log($"{attackingUnit.Name} missed.");
            }
            // Attack Logic Here
           
            if (defendingUnit.IsAlive) //check if unit is alive 
                //TODO CHECK RANGE OF UNIT COUNTER
            {
                hitChance = DamageCalculator.GetHitChance(defendingUnit, attackingUnit);
                if(DamageCalculator.DiceRoll(hitChance))
                {
                    Debug.Log($"{defendingUnit.Name} counter-attacks {attackingUnit.Name}");
                    attackingUnit.HealthPoints = attackingUnit.HealthPoints - DamageCalculator.GetDamage(defendingUnit, attackingUnit);
                }
                else
                {
                    Debug.Log($"{defendingUnit.Name} missed.");
                }


                if (!attackingUnit.IsAlive) {
                    CurrentPlayer.MarkUnitAsInactive(attackingUnit);
                    Debug.Log($"{attackingUnit.Name} has been defeated");
                }
            } else {
                Debug.Log($"{defendingUnit.Name} has been defeated");
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
            var skill = move.UsedSkill;

            if (skill is SingleDamageSkill) {
                ApplySingleDamageSkill(move);
            } else if (skill is SingleSupportSkill) {
                ApplySingleSupportSkill(move);
            }
        }

        private void ApplySingleDamageSkill(GameMove move) {
            var attackingUnit = GetUnitAtPosition(move.StartPosition);
            var defendingUnit = GetUnitAtPosition(move.EndPosition);

            var usedSkill = move.UsedSkill;

            // Attack Logic Here
            Debug.Log($"{attackingUnit.Name} attacks with {usedSkill.SkillName} on {defendingUnit.Name}");
            defendingUnit.HealthPoints = defendingUnit.HealthPoints - DamageCalculator.GetSkillDamage(attackingUnit, defendingUnit, usedSkill);
            if (defendingUnit.IsAlive) //check if unit is alive 
                                                //TODO CHECK RANGE OF UNIT COUNTER
            {
                Debug.Log($"{defendingUnit.Name} counter-attacks {attackingUnit.Name}");
                //instead of Skill[0] of we probably need selected skill or something
                attackingUnit.HealthPoints = attackingUnit.HealthPoints - DamageCalculator.GetDamage(defendingUnit, attackingUnit);
                if (!attackingUnit.IsAlive)
                {
                    CurrentPlayer.MarkUnitAsInactive(attackingUnit);
                    Debug.Log($"{attackingUnit.Name} has been defeated");
                }
            } else
            {
                Debug.Log($"{defendingUnit.Name} has been defeated");
                foreach (var mPlayer in mPlayers)
                {
                    if (mPlayer.Units.Contains(defendingUnit))
                    {
                        mPlayer.MarkUnitAsInactive(defendingUnit);
                    }
                }
            }
            Debug.Log(attackingUnit.UnitInformation + "\n\n");
            Debug.Log(defendingUnit.UnitInformation);

            CurrentPlayer.MarkUnitAsMoved(attackingUnit);
        }

        private void ApplySingleSupportSkill(GameMove move) {
            var supportingUnit = GetUnitAtPosition(move.StartPosition);
            var supportedUnit = GetUnitAtPosition(move.EndPosition);

            var skill = move.UsedSkill as SingleSupportSkill;
            skill.SupportUnit(supportingUnit, supportedUnit);

            // Attack Logic Here
            Debug.Log($"{supportingUnit.Name} uses {move.UsedSkill.SkillName} on {supportedUnit.Name}");

            Debug.Log(supportingUnit.UnitInformation + "\n\n");
            Debug.Log(supportedUnit.UnitInformation);

            CurrentPlayer.MarkUnitAsMoved(supportingUnit);
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
                if (unitAtPoint != null && !DoesUnitBelongToCurrentPlayer(unitAtPoint) && unitAtPoint.IsAlive ) {
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
                                    || !TileIsOccupied(loc))
                                .ToList();
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
            var moveLocations = GetUnitMoveLocations(unit);

            var attackLocations = new List<Vector2Int>();

            // Temporary
            foreach (var moveLoc in moveLocations) {
                var surroundingLocations = GetSurroundingAttackLocationsAtPoint(moveLoc, unit.MainWeapon.Range);
                attackLocations.AddRange(surroundingLocations.Where(loc => !attackLocations.Contains(loc)));
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

        public List<Vector2Int> GetUnitSkillLocations(Unit unit) {
            var singleTargetSkills = unit.Skills
                                        .Select(sk => sk as SingleTargetSkill)
                                        .Where(sk => sk != null);

            var skillLocations = new List<Vector2Int>();
            foreach (var skill in singleTargetSkills) {
                skillLocations.AddRange(GetUnitSkillLocations(unit, skill));
            }

            return skillLocations;
        }

        public List<Vector2Int> GetPossibleUnitSkillLocations(Unit unit) {
            var singleTargetSkills = unit.Skills
                                        .Select(sk => sk as SingleTargetSkill)
                                        .Where(sk => sk != null);

            var skillLocations = new List<Vector2Int>();
            foreach (var skill in singleTargetSkills) {
                skillLocations.AddRange(GetPossibleUnitSkillLocations(unit, skill));
            }

            return skillLocations;
        }

        public List<Vector2Int> GetUnitSkillLocations(Unit unit, SingleTargetSkill skill) {
            var gridPoint = GridForUnit(unit);
            var moveLocations = GetUnitMoveLocations(unit);

            var skillLocations = new List<Vector2Int>();

            // Temporary
            foreach (var moveLoc in moveLocations) {
                var surroundingLocations = GetSurroundingAttackLocationsAtPoint(moveLoc, skill.Range);
                skillLocations.AddRange(surroundingLocations.Where(loc => !skillLocations.Contains(loc)));
            }

            skillLocations = skillLocations
                                .Where(pos => 
                                    !TileIsOccupied(pos)
                                    || (skill is SingleDamageSkill && EnemyAtLocation(pos))
                                    || (skill is SingleSupportSkill && AllyAtLocation(pos)))
                                .ToList();

            return skillLocations;
        }

        public List<Vector2Int> GetPossibleUnitSkillLocations(Unit unit, SingleTargetSkill skill) {
            var moveLocations = GetPossibleUnitMoveLocations(unit);
            var skillLocations = GetUnitSkillLocations(unit, skill);

            var filteredSkillLocations = new List<Vector2Int>();
            foreach (var loc in skillLocations) {
                var surroundingLocations = GetSurroundingAttackLocationsAtPoint(loc, skill.Range);
                var attackPoints = surroundingLocations.Where(sLoc => moveLocations.Contains(sLoc));
                if (attackPoints.Count() > 0) {
                    filteredSkillLocations.Add(loc);
                }
            }

            return filteredSkillLocations;
        }

        public List<Vector2Int> GetUnitDamageSkillLocations(Unit unit) {
            var singleTargetSkills = unit.Skills
                                        .Select(sk => sk as SingleDamageSkill)
                                        .Where(sk => sk != null);

            var skillLocations = new List<Vector2Int>();
            foreach (var skill in singleTargetSkills) {
                skillLocations.AddRange(GetUnitSkillLocations(unit, skill));
            }

            return skillLocations;
        }

        public List<Vector2Int> GetPossibleUnitDamageSkillLocations(Unit unit) {
            var singleTargetSkills = unit.Skills
                                        .Select(sk => sk as SingleDamageSkill)
                                        .Where(sk => sk != null);

            var skillLocations = new List<Vector2Int>();
            foreach (var skill in singleTargetSkills) {
                skillLocations.AddRange(GetPossibleUnitSkillLocations(unit, skill));
            }

            return skillLocations;
        }

        public List<Vector2Int> GetUnitSupportSkillLocations(Unit unit) {
            var singleTargetSkills = unit.Skills
                                        .Select(sk => sk as SingleSupportSkill)
                                        .Where(sk => sk != null);

            var skillLocations = new List<Vector2Int>();
            foreach (var skill in singleTargetSkills) {
                skillLocations.AddRange(GetUnitSkillLocations(unit, skill));
            }

            return skillLocations;
        }

        public List<Vector2Int> GetPossibleUnitSupportSkillLocations(Unit unit) {
            var singleTargetSkills = unit.Skills
                                        .Select(sk => sk as SingleSupportSkill)
                                        .Where(sk => sk != null);

            var skillLocations = new List<Vector2Int>();
            foreach (var skill in singleTargetSkills) {
                skillLocations.AddRange(GetPossibleUnitSkillLocations(unit, skill));
            }

            return skillLocations;
        }

        public bool TileIsOccupied(Vector2Int position) {
            return GetUnitAtPosition(position) != null && GetUnitAtPosition(position).IsAlive;
        }

        public bool EnemyWithinRange(Vector2Int position, int range) {
            var surroundingLocations = GetSurroundingAttackLocationsAtPoint(position, range);

            return surroundingLocations.Any(pos => EnemyAtLocation(pos));
        }

        public bool AllyWithinRange(Vector2Int position, int range) {
            var surroundingLocations = GetSurroundingAttackLocationsAtPoint(position, range);

            return surroundingLocations.Any(pos => AllyAtLocation(pos));
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
            return unit != null && unit.IsAlive && !CurrentPlayer.Units.Contains(unit);
        }

        public bool AllyAtLocation(Vector2Int location) {
            var unit = GetUnitAtPosition(location);
            return unit != null && unit.IsAlive && CurrentPlayer.Units.Contains(unit);
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

            var availableAttackLocations = GetPossibleUnitMoveLocations(unit);

            var possibleAttackLocations = GetSurroundingAttackLocationsAtPoint(targetPoint, unit.MainWeapon.Range);

            possibleAttackLocations = possibleAttackLocations.Where(pos => availableAttackLocations.Contains(pos)).ToList();

            var distances = moveGraph.GetShortestDistancesFrom(startPoint);

            var attackDistances = distances.Where(pos => possibleAttackLocations.Contains(pos.Vertex));

            var shortestDistanceToAttack = attackDistances.Min();

            return shortestDistanceToAttack.Path;
        }

        public List<Vector2Int> GetShortestPathToSkill(Unit unit, Vector2Int startPoint, Vector2Int targetPoint) {
            var unitCosts = GetUnitMoveCosts(unit);
            var moveGraph = new WeightedGraph(unitCosts);

            var availableAttackLocations = GetUnitMoveLocations(unit);

            var bestSkillRange = unit.Skills.Where(sk => sk is SingleTargetSkill).Select(sk => (sk as SingleTargetSkill).Range).Max();

            var possibleAttackLocations = GetSurroundingAttackLocationsAtPoint(targetPoint, bestSkillRange);

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

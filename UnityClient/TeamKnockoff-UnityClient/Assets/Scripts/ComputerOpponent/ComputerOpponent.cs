using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model;
using Assets.Scripts.Utilities.WeightedGraph;
using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.ComputerOpponent
{
    public class ComputerOpponent : MonoBehaviour { 

        public static Vector2Int NULL_VECTOR = new Vector2Int(-1, -1);

        public GameModel model;
        private Random RNG;

        private Vector2Int attackReadyLocation;

        private Player CurrentPlayer;
        private List<Unit> PlayerUnits;
        private Unit CurrentControllingUnit;
        private int currentUnitIndex;
        private bool hasDecidedMove;
        private bool hasMoved;
        private bool attacking;

        private void Start() {
            currentUnitIndex = 0;
            attackReadyLocation = NULL_VECTOR;
            RNG = new Random();
        }

        //Cycle through units, attack nearest enemy
        public GameMove FindBestMove() {

            // CPU has not decided a move to perform yet
            if (!hasDecidedMove) {
                // Set the CPU to the Current Model's Player and Units
                CurrentPlayer = model.CurrentPlayer;
                PlayerUnits = CurrentPlayer.Units;

                // If Unit index exceeds Unit Count, reset
                if (currentUnitIndex == PlayerUnits.Count) {
                    currentUnitIndex = 0;
                }

                // Cycle until CPU finds a unit they can control
                while (!PlayerUnits[currentUnitIndex].IsAlive || PlayerUnits[currentUnitIndex].HasMoved) {
                    currentUnitIndex++;
                    if (currentUnitIndex == PlayerUnits.Count) {
                        currentUnitIndex = 0;
                    }
                }

                // Set the Controlling Unit
                CurrentControllingUnit = PlayerUnits[currentUnitIndex];

                // Determine if there is a location for the Controlling Unit to attack
                var attackLocations = model.GetPossibleUnitAttackLocations(CurrentControllingUnit);
                attacking = attackLocations.Any(loc => model.EnemyAtLocation(loc));

                // Initialize attack location and hasMoved
                attackReadyLocation = NULL_VECTOR;
                hasMoved = false;

                // CPU now has a move they can make, so set this to true until the move is done
                hasDecidedMove = true;
            }

            if (attacking) {
                // CPU has not found an attack location
                if (attackReadyLocation == NULL_VECTOR) {
                    var possibleAttackLocations = model.GetPossibleUnitAttackLocations(CurrentControllingUnit)
                                                    .Where(loc => model.EnemyAtLocation(loc));

                    //Go through possible attack locations and return the closest one
                    var currentDijkstraDistance = new WeightedGraph.DijkstraDistance(new Vector2Int(), Double.MaxValue);
                    foreach (var pos in possibleAttackLocations)
                    {
                        var tempDijkstraDistance = model.GetShortestPathToAttack(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), pos);
                        if (tempDijkstraDistance.CurrentDistance < currentDijkstraDistance.CurrentDistance)
                        {
                            currentDijkstraDistance = tempDijkstraDistance;
                            attackReadyLocation = pos;
                        }
                    }
                }

                // CPU has not moved towards closest attack position yet
                if (!hasMoved) {
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    var movePoint = model.GetShortestPathToAttack(CurrentControllingUnit, startPosition, attackReadyLocation).Path.Last();
                    hasMoved = true;
                    return new GameMove(startPosition, movePoint, GameMove.GameMoveType.Move);
                } else {
                    // CPU has moved and is now ready to attack Unit at target position
                    hasDecidedMove = false;
                    currentUnitIndex++;
                    return new GameMove(model.GridForUnit(CurrentControllingUnit), attackReadyLocation, GameMove.GameMoveType.Attack);
                }

            } else {
                // Unit was not able to find an attack location and will now move towards the closest enemy unit

                // Unit has not moved to the closest enemy unit yet
                if (!hasMoved) {
                    //Iterate through closest enemy unit path 
                    //Find farthest point unit can move to 
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    var enemyUnitPath = GetClosestEnemyUnit().Path;
                    var counter = enemyUnitPath.Count - 1;
                    var movePosition = enemyUnitPath[counter];
                    var movePositions = model.GetPossibleUnitMoveLocations(CurrentControllingUnit).ToList();
                    while (!movePositions.Contains(movePosition))
                    {
                        counter--;
                        movePosition = enemyUnitPath[counter];
                    }

                    // Set hasMoved flag to true and return move 
                    hasMoved = true;
                    return new GameMove(startPosition, movePosition, GameMove.GameMoveType.Move);
                } else {
                    // Unit is done moving, so end their turn

                    // Reset flag to decide next move for next Unit
                    hasDecidedMove = false;
                    currentUnitIndex++;

                    // Return Wait Move to end Unit's turn
                    var waitPosition = model.GridForUnit(CurrentControllingUnit);
                    return new GameMove(waitPosition, waitPosition, GameMove.GameMoveType.Wait);
                }
            }
        }

        /// <summary>
        /// Gets the closest ally unit to a unit 
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns closest ally unit
        /// </returns>

        public WeightedGraph.DijkstraDistance GetClosestAllyUnit(Unit unit)
        {
            var unitLocation = model.GridForUnit(unit);
            var allAlliesPositions = CurrentPlayer.Units
                .Where(un => un != unit)
                .Select(un => model.GridForUnit(un));

            var currentDijkstra = new WeightedGraph.DijkstraDistance(new Vector2Int(), Double.MaxValue);
            foreach (var pos in allAlliesPositions)
            {
                var tempDijkstra = model.GetShortestPathAll(unit, model.GridForUnit(unit), pos);
                if (tempDijkstra.CurrentDistance < currentDijkstra.CurrentDistance)
                {
                    currentDijkstra = tempDijkstra;
                }
            }
            return currentDijkstra;
        }

        /// <summary>
        /// Gets the closest enemy unit to a unit 
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns closest enemy unit
        /// </returns>

        public WeightedGraph.DijkstraDistance GetClosestEnemyUnit()
        {
            var allEnemyPositions = model.Players.Where(player => player != CurrentPlayer)
                .Select(player => player.Units)
                .SelectMany(x => x)
                .Where(un => un.IsAlive)
                .Select(un => model.GridForUnit(un));

            var averagePoint = allEnemyPositions.Average();
            var distance = model.GetShortestPathAll(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), averagePoint);
            return distance;
        }
    }
}

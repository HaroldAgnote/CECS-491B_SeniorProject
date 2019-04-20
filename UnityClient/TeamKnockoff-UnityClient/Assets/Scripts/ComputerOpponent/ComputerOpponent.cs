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
        //attacking utility weights
        public static int ATTACK_DISTANCE_WEIGHT = 10;

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
        private bool supporting;

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

                /*
                //Determine whether supporting or attacking
                //Prioritize supporting (healing or buff)
                foreach (var skill in CurrentControllingUnit.Skills)
                {
                    if(skill.SkillName == "Heal")
                    {
                        //check and select whether a unit can be healed

                    }

                }*/

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

                    //Go through possible attack locations, return the optimal choice
                    var minDijkstraDistance = double.MaxValue;
                    var maxDijkstraDistance = double.MinValue;
                    var maxAttackUtility = 0;
                    Dictionary<Vector2Int, double> unitsUtility = new Dictionary<Vector2Int, double>();
                    Dictionary<Vector2Int, double> tempUnitsUtility = new Dictionary<Vector2Int, double>();

                    foreach (var pos in possibleAttackLocations)
                    {
                        var tempDijkstraDistance = model.GetShortestPathToAttack(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), pos);
                        if (tempDijkstraDistance.CurrentDistance < minDijkstraDistance)
                        {
                            minDijkstraDistance = tempDijkstraDistance.CurrentDistance;
                        }
                        if (tempDijkstraDistance.CurrentDistance > maxDijkstraDistance)
                        {
                            maxDijkstraDistance = tempDijkstraDistance.CurrentDistance;
                        }
                        unitsUtility.Add(pos, tempDijkstraDistance.CurrentDistance);
                    }

                    //Normalize distance utility score, add unit utility value, select position with highest utility score 
                    foreach (KeyValuePair<Vector2Int, double> pos in unitsUtility)
                    {
                        //TODO: add unit utility score
                        tempUnitsUtility[pos.Key] = ATTACK_DISTANCE_WEIGHT * (1 - Normalize(minDijkstraDistance, maxDijkstraDistance, pos.Value));

                        //Highest utility
                        if (unitsUtility[pos.Key] > maxAttackUtility)
                        {
                            attackReadyLocation = pos.Key;
                        }
                    }

                    //Normalize distance utility score, add unit utility value, select position with highest utility score 
                    foreach (KeyValuePair<Vector2Int, double> pos in tempUnitsUtility)
                    {
                        //TODO: add unit utility score
                        unitsUtility[pos.Key] = ATTACK_DISTANCE_WEIGHT * (1 - Normalize(minDijkstraDistance, maxDijkstraDistance, pos.Value));

                        //Highest utility
                        if (unitsUtility[pos.Key] > maxAttackUtility)
                        {
                            attackReadyLocation = pos.Key;
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
                // Unit was not able to find an attack location or support location
                // Unit moves towards enemy or ally unit depending on type

                // Unit has not moved to the closest enemy unit 
                if (!hasMoved && !(CurrentControllingUnit.Class == "Cleric"))
                {
                    //Iterate through average enemy unit path 
                    //Find farthest point unit can move to 
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    var enemyUnitPath = GetAverageEnemyPosition().Path;
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
                }
                else if(!hasMoved && (CurrentControllingUnit.Class == "Cleric"))
                {
                    //Iterate through average ally unit path 
                    //Find farthest point unit can move to 
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    var enemyUnitPath = GetAverageAllyPosition().Path;
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

                }
                // Unit is done moving, so end their turn

                // Reset flag to decide next move for next Unit
                hasDecidedMove = false;
                currentUnitIndex++;

                // Return Wait Move to end Unit's turn
                var waitPosition = model.GridForUnit(CurrentControllingUnit);
                return new GameMove(waitPosition, waitPosition, GameMove.GameMoveType.Wait);
            }
        }

        /// <summary>
        /// Finds the best unit to attack based on healt
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns location of the unit
        /// </returns>

        public WeightedGraph.DijkstraDistance OptimalAttack(Unit unit)
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
        /// Normalize a double
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns a normalized double
        /// </returns>

        public double Normalize(double min, double max, double var)
        {
            return (var - min) / (max - min);
        }

        /// <summary>
        /// Gets average position of ally units
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns path to average position of allies
        /// </returns>

        public WeightedGraph.DijkstraDistance GetAverageAllyPosition()
        {
            var allAllyPositions = CurrentPlayer.Units
                .Where(un => un.IsAlive)
                .Select(un => model.GridForUnit(un));

            var averagePoint = allAllyPositions.Average();
            var distance = model.GetShortestPathAll(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), averagePoint);
            return distance;
        }

        /// <summary>
        /// Gets average position of enemy units
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns closest enemy unit
        /// </returns>

        public WeightedGraph.DijkstraDistance GetAverageEnemyPosition()
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

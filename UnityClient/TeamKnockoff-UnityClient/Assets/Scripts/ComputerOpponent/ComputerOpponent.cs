using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Skills;
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
        private Vector2Int healReadyLocation;
        private Model.Skills.Skill skillToBeUsed;

        private Player CurrentPlayer;
        private List<Unit> PlayerUnits;
        private Unit CurrentControllingUnit;
        private int currentUnitIndex;
        private bool hasDecidedMove;
        private bool hasMoved;
        private bool attacking;
        private bool healing; 

        private void Start() {
            currentUnitIndex = 0;
            attackReadyLocation = NULL_VECTOR;
            healReadyLocation = NULL_VECTOR; 
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
                Debug.Log("Finding unit to control...");
                while (!(PlayerUnits[currentUnitIndex].IsAlive && !PlayerUnits[currentUnitIndex].HasMoved)) {
                    currentUnitIndex++;
                    if (currentUnitIndex == PlayerUnits.Count) {
                        currentUnitIndex = 0;
                    }
                }
                Debug.Log($"Unit found! Controlling: {PlayerUnits[currentUnitIndex].Name}");

                // Set the Controlling Unit
                CurrentControllingUnit = PlayerUnits[currentUnitIndex];


                Debug.Log("Thinking of move...");
                
                if (CurrentControllingUnit.Class == "Cleric") {
                    //Determine whether supporting or attacking
                    //Prioritize supporting (healing or buff)
                    foreach (var skill in CurrentControllingUnit.Skills) {
                        if(skill.SkillName == "Heal") {
                            var targetSkill = skill as SingleTargetSkill;

                            var healLocations = model.GetPossibleUnitSkillLocations(CurrentControllingUnit, targetSkill);

                            // Get all the allies that are close to the Cleric
                            var allAllyUnits = healLocations.Where(loc => model.AllyAtLocation(loc))
                                                            .Select(loc => model.GetUnitAtPosition(loc));

                            if (allAllyUnits.Count() > 0) {
                                healing = true;
                                attacking = false;
                                hasMoved = false;
                                skillToBeUsed = skill;
                                Debug.Log("I can heal a unit");
                            } else {
                                Debug.Log("I cannot heal a unit");
                                healReadyLocation = NULL_VECTOR;
                                hasMoved = false;
                                healing = false;
                            }
                        }
                    }
                } else {
                    Debug.Log("I cannot heal a unit");
                    healing = false;
                }

                if (!healing) {
                    // Determine if there is a location for the Controlling Unit to attack
                    var attackLocations = model.GetPossibleUnitAttackLocations(CurrentControllingUnit);
                    attacking = attackLocations.Any(loc => model.EnemyAtLocation(loc));
                    healing = false;
                    hasDecidedMove = true;

                    // Initialize attack location and hasMoved
                    attackReadyLocation = NULL_VECTOR;
                    hasMoved = false;
                }

                if (attacking) {
                    Debug.Log("I can attack a unit");
                    Debug.Log("Searching for unit to attack...");
                    // CPU has not found an attack location
                    if (attackReadyLocation == NULL_VECTOR) {
                        var possibleAttackLocations = model.GetPossibleUnitAttackLocations(CurrentControllingUnit)
                                                        .Where(loc => model.EnemyAtLocation(loc));

                        //Go through possible attack locations, return the optimal choice
                        var minDijkstraDistance = double.MaxValue;
                        var maxDijkstraDistance = double.MinValue;
                        double maxAttackUtility = -1;
                        Dictionary<Vector2Int, double> unitsUtility = new Dictionary<Vector2Int, double>();
                        Dictionary<Vector2Int, double> tempUnitsUtility = new Dictionary<Vector2Int, double>();

                        foreach (var pos in possibleAttackLocations) {
                            var tempDijkstraDistance = model.GetShortestPathToAttack(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), pos);
                            if (tempDijkstraDistance.CurrentDistance < minDijkstraDistance) {
                                minDijkstraDistance = tempDijkstraDistance.CurrentDistance;
                            }
                            if (tempDijkstraDistance.CurrentDistance > maxDijkstraDistance) {
                                maxDijkstraDistance = tempDijkstraDistance.CurrentDistance;
                            }
                            unitsUtility.Add(pos, tempDijkstraDistance.CurrentDistance);
                        }

                        //Normalize distance utility score, add unit utility value, select position with highest utility score 
                        foreach (KeyValuePair<Vector2Int, double> pos in unitsUtility) {
                            //TODO: add unit utility score
                            tempUnitsUtility[pos.Key] = ATTACK_DISTANCE_WEIGHT * (1 - Normalize(minDijkstraDistance, maxDijkstraDistance, pos.Value));
                            
                            //Highest utility
                            if (tempUnitsUtility[pos.Key] > maxAttackUtility) {
                                maxAttackUtility = tempUnitsUtility[pos.Key];
                                attackReadyLocation = pos.Key;
                            }
                        }

                        //Copy temporary dictionary 
                        foreach (KeyValuePair<Vector2Int, double> pos in tempUnitsUtility) {
                            unitsUtility[pos.Key] = tempUnitsUtility[pos.Key];
                            Debug.Log(unitsUtility[pos.Key]);
                        }
                    }

                    if (attackReadyLocation != NULL_VECTOR) {
                        Debug.Log("Attack location decided!");
                        Debug.Log($"Attacking Unit at ({attackReadyLocation.x}, {attackReadyLocation.y})");
                        hasMoved = false;
                    } else {
                        Debug.Log("Could not find optimal attack location...");
                        attacking = false;
                        hasMoved = false;
                    }
                } else if (healing) {
                    //Determine whether supporting or attacking
                    //Prioritize supporting (healing or buff)
                    Debug.Log("Searching for Heal Location");

                    foreach (var skill in CurrentControllingUnit.Skills) {
                        if(skill.SkillName == "Heal") {
                            var targetSkill = skill as SingleTargetSkill;

                            var healLocations = model.GetPossibleUnitSkillLocations(CurrentControllingUnit, targetSkill);

                            // Get all the allies that are close to the Cleric
                            var allAllyUnits = healLocations.Where(loc => model.AllyAtLocation(loc))
                                                            .Select(loc => model.GetUnitAtPosition(loc))
                                                            .Where(unit => unit != CurrentControllingUnit);

                            if (allAllyUnits.Count() > 0) {
                                int lowestHealth = Int32.MaxValue;
                                //select unit missing the most health
                                foreach (var unit in allAllyUnits) {
                                    if ((unit.MaxHealthPoints.Value > unit.HealthPoints) && (unit.HealthPoints < lowestHealth)) {
                                        lowestHealth = unit.HealthPoints;
                                        healReadyLocation = model.GridForUnit(unit);
                                    }
                                }
                            }
                        }
                    }

                    if (healReadyLocation != NULL_VECTOR) {
                        Debug.Log("Heal location decided!");
                        Debug.Log($"Healing Unit at ({healReadyLocation.x}, {healReadyLocation.y})");
                        hasMoved = false;
                    } else {
                        Debug.Log("Could not find optimal heal location...");
                        hasMoved = false;
                        healing = false;
                    }
                }

                // CPU now has a move they can make, so set this to true until the move is done
                hasDecidedMove = true;
            } else {
            }

            if (attacking) {
                if (!hasMoved) {
                    // CPU has not moved towards closest attack position yet
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    Debug.Log($"I am at Position: ({startPosition.x}, {startPosition.y})");
                    Debug.Log($"Moving towards Attack Position ({attackReadyLocation.x}, {attackReadyLocation.y})!");
                    var path = model.GetShortestPathToAttack(CurrentControllingUnit, startPosition, attackReadyLocation).Path;
                    var movePoint = path.Last();
                    Debug.Log($"Moving to Position ({movePoint.x}, {movePoint.y})!");
                    hasMoved = true;
                    return new GameMove(startPosition, movePoint, path);
                } else {
                    var startPoint = model.GridForUnit(CurrentControllingUnit);
                    Debug.Log($"I am at Position: ({startPoint.x}, {startPoint.y})");
                    Debug.Log("Executing Attack!");
                    // CPU has moved and is now ready to attack Unit at target position
                    hasDecidedMove = false;
                    currentUnitIndex++;
                    Debug.Log("Done Attacking!");
                    return new GameMove(startPoint, attackReadyLocation, GameMove.GameMoveType.Attack);
                }
            } else if (healing) {
                if (!hasMoved) {
                    // CPU has not moved towards closest attack position yet
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    Debug.Log($"I am at Position: ({startPosition.x}, {startPosition.y})");
                    Debug.Log($"Moving towards Heal Position ({healReadyLocation.x}, {healReadyLocation.y})!");
                    var path = model.GetShortestPathToSkill(CurrentControllingUnit, startPosition, healReadyLocation, skillToBeUsed as SingleTargetSkill);
                    var movePoint = path.Last();
                    Debug.Log($"Moving to Position ({movePoint.x}, {movePoint.y})!");
                    hasMoved = true;
                    return new GameMove(startPosition, movePoint, path);
                } else {
                    Debug.Log("Executing Heal!");
                    // CPU has moved and is now ready to attack Unit at target position
                    hasDecidedMove = false;
                    currentUnitIndex++;
                    Debug.Log("Done Healing!");
                    var startPoint = model.GridForUnit(CurrentControllingUnit);
                    return new GameMove(startPoint, healReadyLocation, skillToBeUsed as ActiveSkill);
                }
            } else {
                // Unit was not able to find an attack location or support location
                // Unit moves towards enemy or ally unit depending on type
                // Unit has not moved to the closest enemy unit 
                if (!hasMoved && CurrentControllingUnit.Class != "Cleric") {

                    Debug.Log("I will move closer to the enemy instead...");
                    //Iterate through average enemy unit path 
                    //Find farthest point unit can move to 
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    Debug.Log($"I am at Position: ({startPosition.x}, {startPosition.y})");
                    var averageEnemyPosition = GetAverageEnemyPosition();

                    var moveLocations = model.GetPossibleUnitMoveLocations(CurrentControllingUnit).ToList();
                    var movePosition = FindClosestPointToMoveTo(moveLocations, startPosition, averageEnemyPosition);
                    Debug.Log($"Moving to Position: ({movePosition.x}, {movePosition.y})");

                    var path = model.GetShortestPath(CurrentControllingUnit, startPosition, movePosition).Path;
                    hasMoved = true;
                    Debug.Log("Done moving!");
                    return new GameMove(startPosition, movePosition, path);

                    /*
                    var enemyUnitPath = GetAverageEnemyPosition().Path;
                    var counter = enemyUnitPath.Count - 1;
                    var movePosition = enemyUnitPath[counter];
                    Debug.Log($"Move position: {movePosition} ");
                    var movePositions = model.GetPossibleUnitMoveLocations(CurrentControllingUnit).ToList();
                    while (!movePositions.Contains(movePosition)) {
                        counter--;
                        movePosition = enemyUnitPath[counter];
                    }

                    // Set hasMoved flag to true and return move 
                    hasMoved = true;
                    return new GameMove(startPosition, movePosition, GameMove.GameMoveType.Move);
                    */

                } else if(!hasMoved && (CurrentControllingUnit.Class == "Cleric")) {

                    Debug.Log("I will move closer to my allies instead...");
                    //Iterate through average ally unit path 
                    //Find farthest point unit can move to 
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    Debug.Log($"I am at Position: ({startPosition.x}, {startPosition.y})");
                    var averageAllyPosition = GetAverageAllyPosition();

                    var moveLocations = model.GetPossibleUnitMoveLocations(CurrentControllingUnit).ToList();
                    var movePosition = FindClosestPointToMoveTo(moveLocations, startPosition, averageAllyPosition);

                    Debug.Log($"Moving to Position: ({movePosition.x}, {movePosition.y})");

                    var path = model.GetShortestPath(CurrentControllingUnit, startPosition, movePosition).Path;
                    hasMoved = true;
                    Debug.Log("Done moving!");
                    return new GameMove(startPosition, movePosition, path);

                    /*
                    var counter = enemyUnitPath.Count - 1;
                    var movePosition = enemyUnitPath[counter];                    var movePositions = model.GetPossibleUnitMoveLocations(CurrentControllingUnit).ToList();
                    while (!movePositions.Contains(movePosition))
                    {
                        counter--;
                        movePosition = enemyUnitPath[counter];
                    }

                    var path = model.GetShortestPath(CurrentControllingUnit, startPosition, movePosition).Path;

                    // Set hasMoved flag to true and return move 
                    hasMoved = true;
                    Debug.Log("Done moving!");
                    return new GameMove(startPosition, movePosition, path);
                    */
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

        public Vector2Int FindClosestPointToMoveTo(List<Vector2Int> moveLocations, Vector2Int startPosition, Vector2Int targetPoint) {
            var direction = startPosition.GetVectorDirection(targetPoint);
            var moveVector = VectorExtension.GetVectorTowardsDirection(direction);

            if (moveVector == new Vector2Int(0, 0)) {
                return moveLocations.First();
            }

            var movePosition = new Vector2Int(startPosition.x, startPosition.y);

            while (moveLocations.Contains(movePosition)) {
                movePosition += moveVector;
            }

            movePosition -= moveVector;

            return movePosition;
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
            if (max == min)
            {
                return 0;
            }
            return (var - min) / (max - min);
        }

        /// <summary>
        /// Gets average position of ally units
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns path to average position of allies
        /// </returns>

        // public WeightedGraph.DijkstraDistance GetAverageAllyPosition()
        public Vector2Int GetAverageAllyPosition()
        {
            var allAllyPositions = CurrentPlayer.Units
                .Where(un => un.IsAlive)
                .Select(un => model.GridForUnit(un));

            var averagePoint = allAllyPositions.Average();
            return averagePoint;
            /* Disabling this for now
            var distance = model.GetShortestPathAll(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), averagePoint);
            return distance;
            */
        }

        /// <summary>
        /// Gets average position of enemy units
        /// </summary>
        /// <param name="unit">The Unit who's being checked upon</param>
        /// <returns>
        /// Returns closest enemy unit
        /// </returns>

        public Vector2Int GetAverageEnemyPosition()
        {
            var allEnemyPositions = model.Players.Where(player => player != CurrentPlayer)
                .Select(player => player.Units)
                .SelectMany(x => x)
                .Where(un => un.IsAlive)
                .Select(un => model.GridForUnit(un));

            var averagePoint = allEnemyPositions.Average();
            return averagePoint;

            /* Disabling this for now
            var distance = model.GetShortestPathAll(CurrentControllingUnit, model.GridForUnit(CurrentControllingUnit), averagePoint);
            return distance;
            */
        }
    }
}

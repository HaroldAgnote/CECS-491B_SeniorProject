using System;
using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model;

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
                while (!CurrentPlayer.CheckUnitIsActive(PlayerUnits[currentUnitIndex])) {
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
                // CPU has not found a attack location
                if (attackReadyLocation == NULL_VECTOR) {
                    var possibleAttackLocations = model.GetPossibleUnitAttackLocations(CurrentControllingUnit)
                                                    .Where(loc => model.EnemyAtLocation(loc));
                    attackReadyLocation = possibleAttackLocations.FirstOrDefault();
                }

                // CPU has not moved towards closest attack position yet
                if (!hasMoved) {
                    var startPosition = model.GridForUnit(CurrentControllingUnit);
                    var movePoint = model.GetShortestPathToAttack(CurrentControllingUnit, startPosition, attackReadyLocation).Last();
                    hasMoved = true;
                    return new GameMove(startPosition, movePoint, GameMove.GameMoveType.Move);
                } else {
                    // CPU has moved and is now ready to attack Unit at target position
                    hasDecidedMove = false;
                    currentUnitIndex++;
                    return new GameMove(model.GridForUnit(CurrentControllingUnit), attackReadyLocation, GameMove.GameMoveType.Attack);
                }

            } else {
                // Unit was not able to find an attack location and will now move randomly

                // Unit has not moved to random position yet
                if (!hasMoved) {

                    var startPosition = model.GridForUnit(CurrentControllingUnit);

                    //generate random possible position
                    //move to random position
                    List<Vector2Int> movePositions = model.GetPossibleUnitMoveLocations(CurrentControllingUnit);
                    Vector2Int randomPosition = movePositions[RNG.Next(movePositions.Count)];

                    // Set hasMoved flag to true and return move 
                    hasMoved = true;
                    return new GameMove(model.GridForUnit(CurrentControllingUnit), randomPosition, GameMove.GameMoveType.Move);
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
    }
}

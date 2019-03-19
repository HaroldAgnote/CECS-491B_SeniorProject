using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model;

namespace Assets.Scripts.ComputerOpponent
{
    public class ComputerOpponent : MonoBehaviour
    {
        public GameModel model;
        public static int currentUnitIndex = 0;
        public Vector2Int vectorNull = new Vector2Int(-1, -1);
        public Vector2Int attackReadyLocation = new Vector2Int(-1, -1);
        public bool possibleAttack = true;
        public System.Random RNG = new System.Random();

        //Cycle through units, attack nearest enemy
        public GameMove FindBestMove()
        {
            var currentPlayer = model.CurrentPlayer;
            var currentPlayerUnits = currentPlayer.Units;
            var playerUnit = currentPlayerUnits[currentUnitIndex];
            
            if (!model.UnitHasMoved(playerUnit))
            {
                if (possibleAttack == true)
                {
                    //attack unit from attak locations
                    if (!(attackReadyLocation.Equals(vectorNull)))
                    {
                        return new GameMove(model.GridForUnit(playerUnit), attackReadyLocation, GameMove.GameMoveType.Attack);
                    }
                    //cycle through attack locations
                    //check if they contain units
                    var attackLocations = model.GetPossibleUnitAttackLocations(playerUnit);
                    foreach (Vector2Int location in attackLocations)
                    {
                        if (model.EnemyAtLocation(location))
                        {
                            List<Vector2Int> movePositions = model.GetShortestPathToAttack(playerUnit, model.GridForUnit(playerUnit), location);
                            Vector2Int movePoint = movePositions[movePositions.Count - 1];
                            attackReadyLocation = location;
                            return new GameMove(model.GridForUnit(playerUnit), movePoint, GameMove.GameMoveType.Move);
                        }
                    }
                    possibleAttack = false;
                }
                else
                {
                    //generate random possible position
                    //move to random position
                    List<Vector2Int> movePositions = model.GetPossibleUnitMoveLocations(playerUnit);
                    Vector2Int randomPosition = movePositions[RNG.Next(movePositions.Count)];
                    return new GameMove(model.GridForUnit(playerUnit), randomPosition, GameMove.GameMoveType.Move);
                }
            }
            return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model {
    public class GameMove {

        public enum GameMoveType {
            Move,
            Attack,
            Skill,
            Item,
            Wait
        }

        public Vector2Int StartPosition { get; }
        public Vector2Int EndPosition { get; }

        public GameMoveType MoveType { get; private set; }

        // TODO: Add properties for Skill?

        public GameMove(Vector2Int start, Vector2Int end) {
            StartPosition = start;
            EndPosition = end;

            if (StartPosition != EndPosition) {
                MoveType = GameMoveType.Move;
            } else {
                MoveType = GameMoveType.Wait;
            }
        }

        public GameMove(Vector2Int start, Vector2Int end, GameMoveType moveType) {
            StartPosition = start;
            EndPosition = end;
            MoveType = moveType;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Items;

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
        public List<Vector2Int> Path { get; }

        public GameMoveType MoveType { get; private set; }

        // TODO: Add properties for Skill?
        public ActiveSkill UsedSkill { get; }
        public ConsumableItem UsedItem { get; }

        public GameMove(GameMoveType gameMoveType) {
            
        }

        public GameMove(Vector2Int start, Vector2Int end, List<Vector2Int> path) {
            StartPosition = start;
            EndPosition = end;
            Path = path;
            
            MoveType = GameMoveType.Move;
        }

        public GameMove(Vector2Int start, Vector2Int end, GameMoveType moveType) {
            StartPosition = start;
            EndPosition = end;
            MoveType = moveType;
        }

        public GameMove(Vector2Int start, Vector2Int end, ActiveSkill skill) {
            StartPosition = start;
            EndPosition = end;
            MoveType = GameMoveType.Skill;
            UsedSkill = skill;
        }

        public GameMove(Vector2Int start, Vector2Int end, ConsumableItem item)
        {
            StartPosition = start;
            EndPosition = end;
            MoveType = GameMoveType.Item;
            UsedItem = item;
        }
    }
}

﻿using System.Collections;
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

        public GameMoveType MoveType { get; private set; }

        // TODO: Add properties for Skill?
        public ActiveSkill UsedSkill { get; }
        public ConsumableItem UsedItem { get; }

        public GameMove(GameMoveType gameMoveType) {
            
        }

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

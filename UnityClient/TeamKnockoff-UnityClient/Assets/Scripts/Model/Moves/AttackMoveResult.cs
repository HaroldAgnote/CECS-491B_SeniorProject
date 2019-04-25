using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model {
    public class AttackMoveResult : GameMoveResult {

        public class AttackResult {

            public enum AttackStatus {
                Regular,
                Critical,
                Lethal,
                Miss,
                None
            }

            private Vector2Int mAttackerPosition;
            private Vector2Int mDefenderPosition;

            private AttackStatus mAttackStatus;
            private int mDamageDealt;

            public Vector2Int AttackerPosition => mAttackerPosition;
            public Vector2Int DefenderPosition => mDefenderPosition;

            public AttackStatus Result => mAttackStatus;

            public int DamageDealt => mDamageDealt;

            public AttackResult(Vector2Int attackerPosition, Vector2Int defenderPosition, int damageDealt, AttackStatus attackStatus) {
                mAttackerPosition = attackerPosition;
                mDefenderPosition = defenderPosition;
                mAttackStatus = attackStatus;
                mDamageDealt = damageDealt;
            }
        }

        private List<AttackResult> mAttackResults;

        public List<AttackResult> AttackResults => mAttackResults;

        public AttackMoveResult(List<AttackResult> attackResults) {
            mAttackResults = attackResults;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Skills;

using AttackResult = Assets.Scripts.Model.AttackMoveResult.AttackResult;

namespace Assets.Scripts.Model{
    public class DamageSkillMoveResult : SkillMoveResult {
        public class DamageSkillResult {
            public enum DamageSkillStatus {
                Regular,
                Critical,
                Lethal,
                Miss,
                None
            }

            private Skill mSkillUsed;
            private Vector2Int mAttackerPosition;
            private Vector2Int mDefenderPosition;

            private DamageSkillStatus mSkillStatus;
            private int mDamageDealt;

            public Skill SkillUsed => mSkillUsed;
            public Vector2Int AttackerPosition => mAttackerPosition;
            public Vector2Int DefenderPosition => mDefenderPosition;

            public DamageSkillStatus Result => mSkillStatus;

            public int DamageDealt => mDamageDealt;

            public DamageSkillResult (Vector2Int attackerPosition, Vector2Int defenderPosition, Skill skillUsed, int damageDealt, DamageSkillStatus skillStatus) {
                mSkillUsed = skillUsed;
                mAttackerPosition = attackerPosition;
                mDefenderPosition = defenderPosition;
                mSkillStatus = skillStatus;
                mDamageDealt = damageDealt;
            }
        }

        private DamageSkillResult mAttackerResult;
        private List<AttackResult> mDefenderResults;

        public DamageSkillResult AttackerResult => mAttackerResult;
        public List<AttackResult> DefenderResults => mDefenderResults;

        public DamageSkillMoveResult(DamageSkillResult attackerResult, List<AttackResult> defenderResults) {
            SkillUsed = attackerResult.SkillUsed;
            mAttackerResult = attackerResult;
            mDefenderResults = defenderResults;
        }
    }
}

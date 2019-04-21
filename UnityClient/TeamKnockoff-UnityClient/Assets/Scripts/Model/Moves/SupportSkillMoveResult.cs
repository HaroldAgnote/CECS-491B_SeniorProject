using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model{
    public class SupportSkillMoveResult : SkillMoveResult {
        public class SupportSkillResult {
            public enum SupportSkillStatus {
                Heal,
                Buff,
                None
            }

            private Skill mSkillUsed;
            private Vector2Int mSupporterPosition;
            private Vector2Int mSupportedPosition;

            private SupportSkillStatus mSkillStatus;
            private int mDamageHealed;

            public Skill SkillUsed => mSkillUsed;
            public Vector2Int SupporterPosition => mSupporterPosition;
            public Vector2Int SupportedPosition => mSupportedPosition;

            public SupportSkillStatus Result => mSkillStatus;

            public int DamageHealed => mDamageHealed;

            public SupportSkillResult (Vector2Int supporterPosition, Vector2Int supportedPosition, Skill skillUsed, int damageHealed, SupportSkillStatus skillStatus) {
                mSkillUsed = skillUsed;
                mSupporterPosition = supporterPosition;
                mSupportedPosition = supportedPosition;
                mSkillStatus = skillStatus;
                mDamageHealed = damageHealed;
            }
        }

        private SupportSkillResult mSupporterResult;

        public SupportSkillResult SupporterResult => mSupporterResult;

        public SupportSkillMoveResult(SupportSkillResult supporterResult) {
            SkillUsed = supporterResult.SkillUsed;
            mSupporterResult = supporterResult;
        }
    }
}

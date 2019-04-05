using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class SingleDamageSkill : SingleTargetSkill {
        [SerializeField]
        private DamageType mDamageType;

        public DamageType DamageType { get { return mDamageType; } }

        public SingleDamageSkill(string skillName, int skillCost, int range, bool targetSelf, DamageType damageType) 
            : base(skillName, skillCost, range, targetSelf) {
            mDamageType = damageType;
        }
        public int CRIT_MULTIPLIER = 3;
        public abstract int GetDamage(Unit attacker, Unit defender);
        public abstract int GetCritDamage(Unit attacker, Unit defender);
        public abstract int GetHitChance(Unit attacker, Unit defender);
        public abstract int GetCritRate(Unit attacker, Unit defender);

        public abstract void ApplyDamageSkill(Unit attacker, Unit defender);
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

using DamageType = DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Skills
{
    public abstract class SingleDamageSkill : SingleTargetSkill {

        public DamageType DamageType { get; }

        public SingleDamageSkill(string skillName, int skillCost, int range, bool targetSelf, DamageType damageType) 
            : base(skillName, skillCost, range, targetSelf) {
            DamageType = damageType;
        }

        public abstract int GetDamage(Unit attacker, Unit defender);
        public abstract int GetHitChance(Unit attacker, Unit defender);
        public abstract int GetCritRate(Unit attacker, Unit defender);

        // TODO: Is there any reason a damage skill wouldn't be usable on another unit?
        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return true;
        }
    }
}



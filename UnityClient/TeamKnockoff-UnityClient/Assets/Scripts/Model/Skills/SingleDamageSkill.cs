using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills
{
    public abstract class SingleDamageSkill : Skill
    {
        //public enum DamageType { Physical, Magical };
        public DamageCalculator.DamageType DamageType { get; set; }
        public abstract int GetDamage(Unit attacker, Unit defender);
        public abstract int GetHitChance(Unit attacker, Unit defender);
        public abstract int GetCritRate(Unit attacker, Unit defender);
    }
}



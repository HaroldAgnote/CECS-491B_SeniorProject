using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model.Units;
using UnityEngine;

using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class PiercingShot : SingleDamageSkill {

        private const string SKILL_NAME = "Piercing Shot";
        private const int SKILL_COST = 10;
        private const int RANGE = 3;
        private const int STRENGTH_MODIFIER = 15;
        private const int SPEED_MODIFIER = 5;
        private const int SKILL_MODIFIER = 8;
        private const int HITRATE_MODIFIER = 90;
        private const int CRITRATE_MODIFIER = 50;
        private const bool TARGET_SELF = false;
        private const DamageType DAMAGE_TYPE = DamageType.Physical;

        public PiercingShot() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF, DAMAGE_TYPE) { }

        public override int GetDamage(Unit attacker, Unit defender)
        {
            int damageDone = attacker.Strength.Value + STRENGTH_MODIFIER - defender.Defense.Value;
            if (damageDone <= 0) {
                return 1;
            }
            return damageDone;
        }

        public override int GetCritDamage(Unit attacker, Unit defender) {
            return GetDamage(attacker, defender) * CRIT_MULTIPLIER;
        }

        public override int GetHitChance(Unit attacker, Unit defender)
        {
            double hitRate = attacker.MainWeapon.HitRate + HITRATE_MODIFIER;// + attacker.Skill * 0.01;
            double evasionRate = defender.Speed.Value + defender.Luck.Value;
            return (int)(hitRate - evasionRate);
        }

        public override int GetCritRate(Unit attacker, Unit defender)
        {
            double critRate = attacker.MainWeapon.CritRate + CRITRATE_MODIFIER; // + attacker.Skill * 0.01
            double evasionRate = defender.Luck.Value;
            return (int)(critRate - evasionRate);
        }

        public override void ApplyDamageSkill(Unit attacker, Unit defender) {
        }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return true;
        }
    }
}


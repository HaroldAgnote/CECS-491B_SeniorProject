﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.UnitEffects;
using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class Gravity : SingleDamageSkill {
        class GravityEffect : Debuff {
            public const string EFFECT_NAME = "Gravity";
            public const int TURN_DURATION = 3;

            public GravityEffect() : base(EFFECT_NAME, TURN_DURATION) { }

            private int mGravityValue;

            public override bool IsApplied {
                get;
                set;
            }

            public override void ApplyEffect(Unit unit) {
                IsApplied = true;
                mGravityValue = unit.Movement.Base / 2;

                unit.Movement.Modifier -= mGravityValue;
            }

            public override bool IsApplicable() {
                return !IsApplied;
            }

            public override void RemoveEffect(Unit unit) {
                unit.Movement.Modifier += mGravityValue;
                IsApplied = false;
            }
        }

        private const string SKILL_NAME = "Gravity";
        private const int SKILL_COST = 6;
        private const int RANGE = 2;
        private const bool TARGET_SELF = false;
        private const DamageType DAMAGE_TYPE = DamageType.Magical;

        private const int DAMAGE_MODIFIER = 3;
        private const int HIT_RATE = 5;
        private const int CRIT_RATE = 5;

        public Gravity() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF, DAMAGE_TYPE) { }

        public override void ApplyDamageSkill(Unit attacker, Unit defender) {
            var gravityEffect = new GravityEffect();
            gravityEffect.ApplyEffect(defender);
            defender.UnitEffects.Add(gravityEffect);
        }

        public override int GetDamage(Unit attacker, Unit defender)
        {
            int damageDone = (attacker.Magic.Value + DAMAGE_MODIFIER) - defender.Resistance.Value;
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
            double hitRate = attacker.MainWeapon.HitRate + HIT_RATE;
            double evasionRate = defender.Speed.Value + defender.Luck.Value;
            return (int)(hitRate - evasionRate);
        }

        public override int GetCritRate(Unit attacker, Unit defender)
        {
            double critRate = attacker.MainWeapon.CritRate + CRIT_RATE; // + attacker.Skill * 0.01
            double evasionRate = defender.Luck.Value;
            return (int)(critRate - evasionRate);
        }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return usingUnit.PlayerNumber != targetUnit.PlayerNumber && !targetUnit.UnitEffects.Any(effect => effect.EffectName == GravityEffect.EFFECT_NAME);
        }

        public override Skill Generate() {
            return new Gravity();
        }

        public override int GetOffensive(Unit attacker) {
            int damageDone = attacker.Magic.Value + DAMAGE_MODIFIER;
            return damageDone;
        }

        public override int GetDefensive(Unit attacker, Unit defender) {
            return defender.Resistance.Value;
        }
    }
}

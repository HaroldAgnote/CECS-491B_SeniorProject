using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model.Units;
using UnityEngine;

using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;

namespace Assets.Scripts.Model.Skills
{
    public class Bash : SingleDamageSkill
    {
        private const string SKILL_NAME = "Bash";
        private const int SKILL_COST = 10;
        private const int RANGE = 1;
        private const bool TARGET_SELF = false;
        private const DamageType DAMAGE_TYPE = DamageType.Physical;

        public Bash() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF, DAMAGE_TYPE) { }

        public double HealthPoints = -4;

        public int Strength = 1000;

        public int Speed = -1;
        public int Skill = -3;

        public int Hit = -10;
        //public int CritRate = 3;

        public new Assets.Scripts.Model.DamageCalculator.DamageType DamageType = Assets.Scripts.Model.DamageCalculator.DamageType.Physical;

        public override int GetDamage(Unit attacker, Unit defender)
        {
            int damageDone = attacker.Strength.Value + Strength - defender.Defense.Value;
            if (damageDone <= 0) {
                return 1;
            }
            return damageDone;
        }

        public override int GetHitChance(Unit attacker, Unit defender)
        {
            double hitRate = attacker.MainWeapon.Hit + Hit;// + attacker.Skill * 0.01;
            double evasionRate = defender.Speed.Value + defender.Luck.Value;
            return (int)(hitRate - evasionRate);
        }

        public override int GetCritRate(Unit attacker, Unit defender)
        {
            double critRate = attacker.MainWeapon.CritRate; // + attacker.Skill * 0.01
            double evasionRate = defender.Luck.Value;
            return (int)(critRate - evasionRate);
        }

        public override void ApplyDamageSkill(Unit attacker, Unit defender) {
            throw new System.NotImplementedException();
        }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return true;
        }
    }
}


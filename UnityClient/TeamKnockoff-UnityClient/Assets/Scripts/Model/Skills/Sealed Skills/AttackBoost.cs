using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model.Units;
using UnityEngine;

namespace Assets.Scripts.Model.Skills
{
    public class AttackBoost : FieldSkill
    {
        const string SKILL_NAME = "Strength +3";
        const int STAT_BOOST = 3;

        public AttackBoost() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit) {
            IsApplied = true;
            unit.Strength.Modifier += STAT_BOOST;
        }

        public override void RevertFieldSkill(Unit unit) {
            IsApplied = false;
            unit.Strength.Modifier -= STAT_BOOST;
        }

        public override Skill Generate()
        {
            return new AttackBoost();
        }
    }
}
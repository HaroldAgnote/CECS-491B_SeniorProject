using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills
{
    public class DefenseBoost : FieldSkill
    {
        const string SKILL_NAME = "Defense +3";
        const int STAT_BOOST = 3;

        public DefenseBoost() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit)
        {
            unit.Defense.Modifier += STAT_BOOST;
        }

        public override void RevertFieldSkill(Unit unit)
        {
            unit.Defense.Modifier -= STAT_BOOST;
        }

        public override Skill Generate()
        {
            return new AttackDrop();
        }
    }
}
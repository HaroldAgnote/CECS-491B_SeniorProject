using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model.Units;
using UnityEngine;

namespace Assets.Scripts.Model.Skills
{
    public class AttackDrop : FieldSkill
    {
        const string SKILL_NAME = "Attack -2";
        const int STAT_DROP = -2;

        public AttackDrop() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit)
        {
            unit.Strength.Modifier += STAT_DROP;
        }

        public override void RevertFieldSkill(Unit unit)
        {
            unit.Strength.Modifier -= STAT_DROP;
        }

        public override Skill Generate()
        {
            return new AttackDrop();
        }
    }
}

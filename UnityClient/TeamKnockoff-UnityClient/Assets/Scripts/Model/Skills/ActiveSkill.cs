using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills
{
    public class ActiveSkill : Skill {
        public int SkillCost { get; }

        public ActiveSkill(string skillName, int skillCost) 
            : base(skillName) {
            SkillCost = skillCost;
        }

        public bool CanUse(Unit usingUnit) {
            return usingUnit.HealthPoints > SkillCost;
        }
    }
}

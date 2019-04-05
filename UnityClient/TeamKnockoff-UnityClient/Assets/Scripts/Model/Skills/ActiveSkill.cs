using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class ActiveSkill : Skill {

        [SerializeField]
        private int mSkillCost;

        public int SkillCost { get { return mSkillCost; } }

        public ActiveSkill(string skillName, int skillCost) 
            : base(skillName) {
            mSkillCost = skillCost;
        }

        public bool CanUse(Unit usingUnit) {
            return usingUnit.HealthPoints > SkillCost;
        }
    }
}

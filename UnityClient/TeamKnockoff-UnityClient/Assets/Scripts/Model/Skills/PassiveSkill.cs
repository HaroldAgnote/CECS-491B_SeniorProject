using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class PassiveSkill : Skill {
        public PassiveSkill(string skillName) 
            : base(skillName) {
        }
    }
}

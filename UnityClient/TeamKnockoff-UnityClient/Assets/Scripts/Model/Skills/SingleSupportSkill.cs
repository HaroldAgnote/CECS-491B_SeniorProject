using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    public abstract class SingleSupportSkill : SingleTargetSkill {
        public SingleSupportSkill(string skillName, int skillCost, int range, bool targetSelf) 
            : base(skillName, skillCost, range, targetSelf) {
        }

        public abstract void SupportUnit(Unit usingUnit, Unit targetUnit);
    }
}

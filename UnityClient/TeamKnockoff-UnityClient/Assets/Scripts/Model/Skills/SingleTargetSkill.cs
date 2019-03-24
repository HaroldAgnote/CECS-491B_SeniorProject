using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills
{
    public abstract class SingleTargetSkill : ActiveSkill {
        
        public int Range { get; }
        public bool CanTargetSelf { get; }

        public SingleTargetSkill(string skillName, int skillCost, int range, bool targetSelf) 
            : base(skillName, skillCost) {

            Range = range;
            CanTargetSelf = targetSelf;
        }

        public abstract bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit);
    }
}



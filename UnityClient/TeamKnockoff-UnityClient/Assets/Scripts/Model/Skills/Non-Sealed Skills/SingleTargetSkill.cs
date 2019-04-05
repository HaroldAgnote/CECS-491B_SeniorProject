using System;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class SingleTargetSkill : ActiveSkill {
        [SerializeField]
        private int mRange;

        [SerializeField]
        private bool mCanTargetSelf;
        
        public int Range { get { return mRange; } }
        public bool CanTargetSelf { get { return mCanTargetSelf; } }

        public SingleTargetSkill(string skillName, int skillCost, int range, bool targetSelf) 
            : base(skillName, skillCost) {

            mRange = range;
            mCanTargetSelf = targetSelf;
        }

        public abstract bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit);
    }
}



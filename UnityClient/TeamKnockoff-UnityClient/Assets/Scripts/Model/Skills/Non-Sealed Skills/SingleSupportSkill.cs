using System;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class SingleSupportSkill : SingleTargetSkill {
        public SingleSupportSkill(string skillName, int skillCost, int range, bool targetSelf) 
            : base(skillName, skillCost, range, targetSelf) {
        }

        public abstract void ApplySupportSkill(Unit usingUnit, Unit targetUnit);

        public abstract int GetHealAmount(Unit usingUnit, Unit targetUnit);

        //public abstract Skill Generate();
    }
}

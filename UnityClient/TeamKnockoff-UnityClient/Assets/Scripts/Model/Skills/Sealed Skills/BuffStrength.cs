using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.UnitEffects;

namespace Assets.Scripts.Model.Skills {

    [Serializable]
    public class BuffStrength : SingleSupportSkill {
        private const string SKILL_NAME = "Buff Strength";
        private const int SKILL_COST = 10;
        private const int RANGE = 1;
        private const bool TARGET_SELF = false;

        public BuffStrength() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF) { }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return !targetUnit.UnitEffects.Contains(new StrengthBoost());
        }

        public override void ApplySupportSkill(Unit user, Unit target) {
            var strengthBoost = new StrengthBoost();
            strengthBoost.ApplyEffect(target);
            target.UnitEffects.Add(strengthBoost);
        }

        public override Skill Generate() {
            return new BuffStrength();
        }
    }
}

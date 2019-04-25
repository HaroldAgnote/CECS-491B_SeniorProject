using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class Physic : SingleSupportSkill {
        private const string SKILL_NAME = "Physic";
        private const int SKILL_COST = 3;
        private const int RANGE = 3;
        private const bool TARGET_SELF = false;
        private const int HEAL_AMOUNT = 15;

        public Physic() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF) { }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return usingUnit.PlayerNumber == targetUnit.PlayerNumber && targetUnit.HealthPoints != targetUnit.MaxHealthPoints.Value;
        }

        public override void ApplySupportSkill(Unit healer, Unit patient) {
            // Change this later
            patient.HealthPoints += HEAL_AMOUNT;
        }
        public override Skill Generate() {
            return new Physic();
        }
    }
}

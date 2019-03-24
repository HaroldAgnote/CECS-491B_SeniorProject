using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model.Units;
using UnityEngine;

namespace Assets.Scripts.Model.Skills {
    public class Heal : SingleSupportSkill {

        private const string SKILL_NAME = "Heal";
        private const int SKILL_COST = 10;
        private const int RANGE = 1;
        private const bool TARGET_SELF = false;

        public Heal() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF) { }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return targetUnit.HealthPoints != targetUnit.MaxHealthPoints;
        }

        public override void SupportUnit(Unit healer, Unit patient) {
            // Change this later
            patient.HealthPoints += 10;
        }
    }
}

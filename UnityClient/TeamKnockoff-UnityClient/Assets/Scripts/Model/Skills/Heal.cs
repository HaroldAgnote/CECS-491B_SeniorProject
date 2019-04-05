using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class Heal : SingleSupportSkill {
        private const string SKILL_NAME = "Heal";
        private const int SKILL_COST = 10;
        private const int RANGE = 1;
        private const bool TARGET_SELF = false;
        private const int HEAL_AMOUNT = 10;

        public Heal() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF) { }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return targetUnit.HealthPoints != targetUnit.MaxHealthPoints.Value;
        }

        public override void ApplySupportSkill(Unit healer, Unit patient) {
            // Change this later
            patient.HealthPoints += HEAL_AMOUNT;
        }
    }
}

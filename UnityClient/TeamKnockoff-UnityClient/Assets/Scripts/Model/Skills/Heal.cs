using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model.Units;
using UnityEngine;

namespace Assets.Scripts.Model.Skills {
    public class Heal : SingleSupportSkill {
        public Heal() {
            SkillName = "Heal";
            Range = 1;
        }

        public override void SupportUnit(Unit healer, Unit patient) {
            // Change this later
            patient.HealthPoints += 10;
        }
    }
}

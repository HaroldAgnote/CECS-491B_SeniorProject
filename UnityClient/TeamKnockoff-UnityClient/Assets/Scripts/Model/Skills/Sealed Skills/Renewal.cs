using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Scripts.Model.UnitEffects;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class Renewal : FieldSkill {
        public class RenewalEffect : PassiveEffect, IApplicableEffect {

            public const string EFFECT_NAME = "Renewal";

            public const int TURN_TRIGGER = 3;

            public const int HEAL_AMOUNT = 10;

            public RenewalEffect() : base(EFFECT_NAME) { }

            public bool IsApplied { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void ApplyEffect(Unit unit) {
                Debug.Log($"{unit.Name} heals for {HEAL_AMOUNT} points");
                unit.HealthPoints += HEAL_AMOUNT;
            }

            public bool IsApplicable() {
                return CurrentTurn % TURN_TRIGGER == 0;
            }
        }

        public const string SKILL_NAME = "Renewal";

        public Renewal() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit) {
            unit.UnitEffects.Add(new RenewalEffect());
        }
        public override Skill Generate() {
            return new Renewal();
        }
    }
}

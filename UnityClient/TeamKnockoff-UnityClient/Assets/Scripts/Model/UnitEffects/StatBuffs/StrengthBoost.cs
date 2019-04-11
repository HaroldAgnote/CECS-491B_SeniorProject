using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {

    public class StrengthBoost : Buff {
        public const string EFFECT_NAME = "Strength +5";
        public const int TURN_DURATION = 0;
        public const int STRENGTH_BOOST = 5;

        public StrengthBoost() : base(EFFECT_NAME, TURN_DURATION) { }

        public override bool IsApplied {
            get;
            set;
        }

        public override void ApplyEffect(Unit unit) {
            IsApplied = true;
            unit.Strength.Modifier += STRENGTH_BOOST;
        }

        public override bool IsApplicable() {
            return !IsApplied;
        }

        public override void RemoveEffect(Unit unit) {
            unit.Strength.Modifier -= STRENGTH_BOOST;
            IsApplied = false;
        }
    }
}

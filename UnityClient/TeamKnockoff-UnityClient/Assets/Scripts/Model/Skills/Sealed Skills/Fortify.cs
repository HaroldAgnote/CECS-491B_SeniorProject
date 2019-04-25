using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.UnitEffects;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class Fortify : SingleSupportSkill {
        class FortifyEffect : Buff {
            public const string EFFECT_NAME = "Fortify";
            public const int TURN_DURATION = 0;

            public FortifyEffect(int fortifyValue) : base(EFFECT_NAME, TURN_DURATION) {
                mFortifyValue = fortifyValue;
            }

            private int mFortifyValue;

            public override bool IsApplied {
                get;
                set;
            }

            public override void ApplyEffect(Unit unit) {
                IsApplied = true;

                unit.Defense.Modifier += mFortifyValue;
            }

            public override bool IsApplicable() {
                return !IsApplied;
            }

            public override void RemoveEffect(Unit unit) {
                unit.Defense.Modifier -= mFortifyValue;
                IsApplied = false;
            }
        }

        private const string SKILL_NAME = "Fortify";
        private const int SKILL_COST = 3;
        private const int RANGE = 1;
        private const bool TARGET_SELF = true;

        public Fortify() : base(SKILL_NAME, SKILL_COST, RANGE, TARGET_SELF) { }

        public override void ApplySupportSkill(Unit usingUnit, Unit targetUnit) {
            var fortifyValue = usingUnit.Defense.Base;
            var fortifyEffect = new FortifyEffect(fortifyValue);

            fortifyEffect.ApplyEffect(targetUnit);
            targetUnit.UnitEffects.Add(fortifyEffect);
        }

        public override bool IsUsableOnTarget(Unit usingUnit, Unit targetUnit) {
            return usingUnit.PlayerNumber == targetUnit.PlayerNumber && !targetUnit.UnitEffects.Any(effect => effect.EffectName == FortifyEffect.EFFECT_NAME);
        }

        public override int GetHealAmount(Unit usingUnit, Unit targetUnit) {
            return 0;
        }

        public override Skill Generate() {
            return new Fortify();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class UnitEffect {

        public string EffectName { get; }

        public bool IsApplied { get; private set; }

        public UnitEffect(string name) {
            EffectName = name;
        }

        public abstract bool IsApplicable();

        public abstract void ApplyEffect();
        public abstract void RemoveEffect();
    }
}

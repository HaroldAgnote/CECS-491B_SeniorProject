using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Units {
    public abstract class UnitEffect {
        public int TurnsRemaining { get; protected set; }

        public bool EffectApplied { get; }

        public UnitEffect(int turns) {
            EffectApplied = false;
            TurnsRemaining = turns;
        }
    }
}

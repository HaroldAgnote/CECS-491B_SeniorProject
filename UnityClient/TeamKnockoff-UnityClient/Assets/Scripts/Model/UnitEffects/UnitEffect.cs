using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class UnitEffect : IEquatable<UnitEffect> {

        public string EffectName { get; }

        public UnitEffect(string name) {
            EffectName = name;
        }

        public bool Equals(UnitEffect other) {
            return this.EffectName == other.EffectName;
        }
    }
}

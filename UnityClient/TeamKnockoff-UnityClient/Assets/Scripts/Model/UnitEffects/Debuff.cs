using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class Debuff : TemporaryEffect {

        public Debuff(string name, int startingTurns)
            : base(name, startingTurns) {

        }
    }
}

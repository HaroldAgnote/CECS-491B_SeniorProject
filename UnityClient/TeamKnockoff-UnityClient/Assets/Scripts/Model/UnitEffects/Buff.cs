using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class Buff : TemporaryEffect {

        public Buff(string name, int startingTurns)
            : base(name, startingTurns) {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Units {
    public class Buff : UnitEffect {
        public Buff(int turns) : base(turns) {

        }

        public override void ApplyEffect(Unit unit) {
            throw new System.NotImplementedException();
        }

        public override void RemoveEffect(Unit unit) {
            throw new System.NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class PassiveEffect : UnitEffect, ITurnEffect {

        public int StartingTurns { get; }
        public int CurrentTurn { get; set; }

        public PassiveEffect(string name) 
            : base(name) {

            StartingTurns = 1;
            CurrentTurn = StartingTurns;
        }

        public void UpdateTurns() {
            CurrentTurn++;
        }
    }
}

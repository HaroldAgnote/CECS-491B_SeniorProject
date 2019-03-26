using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class TemporaryEffect : UnitEffect {

        private readonly int mStartTurns;

        public int TurnsRemaining { get; private set; }

        public TemporaryEffect(string name, int startingTurns) 
            : base(name) {

            mStartTurns = startingTurns;
            TurnsRemaining = startingTurns;
        }

        public void ResetEffect() {
            TurnsRemaining = mStartTurns;
        }

        public void ReduceTurnCount() {
            TurnsRemaining--;
        }
    }
}

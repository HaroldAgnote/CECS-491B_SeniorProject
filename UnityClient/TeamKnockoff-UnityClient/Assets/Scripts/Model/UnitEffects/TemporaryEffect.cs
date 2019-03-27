using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class TemporaryEffect : UnitEffect, IApplicableEffect, IRemovableEffect, ITurnEffect, IResettableEffect {

        public int StartingTurns { get; }

        public int CurrentTurn { get; set; }

        public abstract bool IsApplied { get; set; }

        public TemporaryEffect(string name, int startingTurns) 
            : base(name) {

            StartingTurns = startingTurns;
            CurrentTurn = startingTurns;
        }

        public void UpdateTurns() {
            if (CurrentTurn > 0) {
                CurrentTurn--;
            }
        }

        public void ResetEffect() {
            CurrentTurn = StartingTurns;
        }

        public bool IsRemovable() {
            return CurrentTurn <= 0;
        }

        public abstract bool IsApplicable();
        public abstract void ApplyEffect(Unit unit);
        public abstract void RemoveEffect(Unit unit);
    }
}

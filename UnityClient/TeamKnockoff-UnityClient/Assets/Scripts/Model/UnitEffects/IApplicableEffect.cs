using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    interface IApplicableEffect {

        bool IsApplied { get; set; }

        bool IsApplicable();
        void ApplyEffect(Unit unit);
    }
}

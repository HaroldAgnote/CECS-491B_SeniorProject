using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.UnitEffects {
    interface IRemovableEffect {

        bool IsRemovable();
        void RemoveEffect(Unit unit);
    }
}

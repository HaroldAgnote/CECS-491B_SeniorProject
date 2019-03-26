using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.UnitEffects {
    public abstract class PassiveEffect : UnitEffect {

        public PassiveEffect(string name) 
            : base(name) {
        }
    }
}

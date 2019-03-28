using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    public abstract class MovementSkill : PassiveSkill {
        public MovementSkill(string skillName) : base(skillName) { }

        public abstract void ApplyMovementSkill(Unit unit);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class CombatSkill : PassiveSkill {
        public CombatSkill(string skillName) : base(skillName) { }

        public abstract void ApplyCombatSkill(Unit unit);
    }
}

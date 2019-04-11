using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class FieldSkill : PassiveSkill {
        public FieldSkill(string skillName) : base(skillName) { }

        public abstract void ApplyFieldSkill(Unit unit);

        public abstract void RevertFieldSkill(Unit unit);
    }
}

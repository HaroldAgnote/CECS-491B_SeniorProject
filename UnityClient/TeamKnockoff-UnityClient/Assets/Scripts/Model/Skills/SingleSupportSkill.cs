using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    public abstract class SingleSupportSkill : SingleTargetSkill {
        public bool SingleTurnOnly;
        public abstract void SupportUnit(Unit supporter, Unit supported);
    }
}

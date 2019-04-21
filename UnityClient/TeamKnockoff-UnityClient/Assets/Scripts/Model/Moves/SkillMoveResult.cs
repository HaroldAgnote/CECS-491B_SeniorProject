using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Skills;

namespace Assets.Scripts.Model {
    public abstract class SkillMoveResult : GameMoveResult {
        private Skill mSkillUsed;

        public Skill SkillUsed {
            get { return mSkillUsed; }
            protected set {
                mSkillUsed = value;
            }
        }
    }
}

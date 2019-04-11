using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public class SmallSpeedBoost : FieldSkill {
        const string SKILL_NAME = "Speed +3";
        const int STAT_BOOST = 3;

        public SmallSpeedBoost() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit) {
            unit.Speed.Modifier += STAT_BOOST;
        }

        public override void RevertFieldSkill(Unit unit)
        {
            unit.Speed.Modifier -= STAT_BOOST;
        }
        
        public override Skill Generate() {
            return new SmallSpeedBoost();
        }

        
    }

    [Serializable]
    public class MediumSpeedBoost : FieldSkill {
        const string SKILL_NAME = "Speed +5";
        const int STAT_BOOST = 5;

        public MediumSpeedBoost() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit) {
            unit.Speed.Modifier += STAT_BOOST;
        }

        public override void RevertFieldSkill(Unit unit)
        {
            unit.Speed.Modifier -= STAT_BOOST;
        }

        public override Skill Generate() {
            return new MediumSpeedBoost();
        }
    }

    [Serializable]
    public class LargeSpeedBoost : FieldSkill {
        const string SKILL_NAME = "Speed +10";
        const int STAT_BOOST = 10;

        public LargeSpeedBoost() : base(SKILL_NAME) { }

        public override void ApplyFieldSkill(Unit unit) {
            unit.Speed.Modifier += STAT_BOOST;
        }

        public override void RevertFieldSkill(Unit unit)
        {
            unit.Speed.Modifier -= STAT_BOOST;
        }

        public override Skill Generate() {
            return new LargeSpeedBoost();
        }
    }
}

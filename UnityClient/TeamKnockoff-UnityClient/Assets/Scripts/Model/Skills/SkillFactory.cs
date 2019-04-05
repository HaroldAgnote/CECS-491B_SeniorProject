using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Skills {
    class SkillFactory {
        public static HashSet<Skill> SkillBank;
        public static Dictionary<string, Action<Skill>> SkillGenerator;

        public SkillFactory() {
            //initalize SkillBank with all skills
        }
    }
}

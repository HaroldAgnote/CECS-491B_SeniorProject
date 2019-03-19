using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Skills
{
    public abstract class Skill {
        public string SkillName { get; protected set; }
        public int SkillCost { get; protected set; }
    }
}

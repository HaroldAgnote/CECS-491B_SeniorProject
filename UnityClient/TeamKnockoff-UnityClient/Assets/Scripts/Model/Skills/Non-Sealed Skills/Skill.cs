using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utilities.Generator;

namespace Assets.Scripts.Model.Skills {
    public abstract class Skill:IGenerator<Skill> {
        
        private readonly string mSkillName;

        public string SkillName { get { return mSkillName; } }

        public Skill(string skillName) {
            mSkillName = skillName;
            // TODO: Matthew
            // Process Skills.resx resource file and use the name of the skill
            // to instantiate the description of the skill
        }

        public abstract Skill Generate(); 
    }
}

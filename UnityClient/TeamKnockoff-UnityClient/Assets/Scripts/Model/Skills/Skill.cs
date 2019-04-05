using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Skills {
    [Serializable]
    public abstract class Skill {

        [SerializeField]
        private readonly string mSkillName;

        public string SkillName { get { return mSkillName; } }

        public Skill(string skillName) {
            mSkillName = skillName;
        }
    }
}

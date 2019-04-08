using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Model.Weapons {
    [Serializable]
    public class WeaponWrapper {
        [SerializeField]
        private string mWeaponName;

        [SerializeField]
        private List<string> mSkills;

        public string WeaponName {
            get { return mWeaponName; }
            private set {
                if (mWeaponName != value) {
                    mWeaponName = value;
                }
            }
        }

        public List<string> Skills {
            get { return mSkills; }
            private set {
                if (mSkills != value) {
                    mSkills = value;
                }
            }
        }

        public WeaponWrapper(Weapon weapon) {
            mWeaponName = weapon.Name;
            mSkills = weapon.Skills.Select(skill => skill.SkillName).ToList();
        }
    }
}

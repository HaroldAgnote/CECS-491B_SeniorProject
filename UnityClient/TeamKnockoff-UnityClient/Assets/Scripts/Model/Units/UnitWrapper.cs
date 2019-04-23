using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.Model.Items;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model.Units {
    [Serializable]
    public class UnitWrapper {
        public string unitName;
        public string unitType;
        public string unitClass;
        public string unitMoveType;

        public int unitLevel;
        public int unitExperiencePoints;

        public Stat unitMaxHealthPoints;
        public Stat unitStrength;
        public Stat unitMagic;
        public Stat unitDefense;
        public Stat unitResistance;
        public Stat unitSpeed;
        public Stat unitSkill;
        public Stat unitLuck;
        public Stat unitMovement;

        public string unitWeapon;
        public List<string> unitSkills;
        public List<string> unitItems;
        
        // TODO: Sebastian:
        // Add List<string> for unitItems

        public UnitWrapper(Unit unit) {
            unitName = unit.Name;
            unitType = unit.Type;
            unitClass = unit.Class;
            unitMoveType = unit.MoveType;

            unitLevel = unit.Level;
            unitExperiencePoints = unit.ExperiencePoints;

            unitMaxHealthPoints = unit.MaxHealthPoints;
            unitStrength = unit.Strength;
            unitMagic = unit.Magic;
            unitDefense = unit.Defense;
            unitResistance = unit.Resistance;
            unitSpeed = unit.Speed;
            unitSkill = unit.Skill;
            unitLuck = unit.Luck;
            unitMovement = unit.Movement;

            unitWeapon = unit.MainWeapon.Name;
            unitSkills = new List<string>();
            unitSkills.AddRange(unit.UnitSkills.Select(skill => skill.SkillName));

            unitItems = new List<string>();
            unitItems.AddRange(unit.Items.Select(item => item.ItemName));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model.Skills;
using DamageType = Assets.Scripts.Model.DamageCalculator.DamageType;
using WeaponType = Assets.Scripts.Model.Weapons.Weapon.WeaponType;

namespace Assets.Scripts.Model.Weapons {
    class WeaponFactory : MonoBehaviour {
        public static WeaponFactory instance;

        public TextAsset weaponTextAsset;

        public HashSet<Weapon> WeaponBank { get; private set; }
        private Dictionary<string, Func<Weapon>> weaponGenerator;

        void Awake() {
            //Check if instance already exists
            if (instance == null) {
                //if not, set instance to this
                instance = this;
            }

            //If instance already exists and it's not this:
            else if (instance != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void Start() {
            if (weaponTextAsset == null) {
                weaponTextAsset = new TextAsset();
            }

            WeaponBank = new HashSet<Weapon>();
            weaponGenerator = new Dictionary<string, Func<Weapon>>();
            WeaponBank.Add(Weapon.FISTS);

            //Read file to add weapons to WeaponGenerator
            var reader = new StringReader(weaponTextAsset.text);
            // Skip first line
            reader.ReadLine();
            while (reader.Peek() >= 0) {
                var weaponLine = reader.ReadLine();
                var weaponParts = weaponLine.Split(',');
                var newWeapon = CreateWeapon(weaponParts);
                WeaponBank.Add(newWeapon);
            }

            foreach (var weapon in WeaponBank) {
                weaponGenerator.Add(weapon.Name, weapon.Generate);
            }
        }

        private Weapon CreateWeapon(string[] weaponParts) {
            const int SKILL_INDEX_START = 11;

            var weaponName = weaponParts[0];

            Enum.TryParse(weaponParts[1], true, out WeaponType weaponType);
            Enum.TryParse(weaponParts[2], true, out DamageType damageType);

            var weaponRange = Int32.Parse(weaponParts[3]);
            var weaponWeight = Int32.Parse(weaponParts[4]);
            var weaponMight = Int32.Parse(weaponParts[5]);
            var weaponHitRate = Int32.Parse(weaponParts[6]);
            var weaponCritRate = Int32.Parse(weaponParts[7]);
            var weaponRarity = Int32.Parse(weaponParts[8]);
            var weaponBuyingPrice = Int32.Parse(weaponParts[9]);
            var weaponSellingPrice = Int32.Parse(weaponParts[10]);

            HashSet<Skill> weaponSkills = new HashSet<Skill>();

            for (int skillIndex = SKILL_INDEX_START; skillIndex < weaponParts.Length; skillIndex++) {
                var skillName = weaponParts[skillIndex];
                if (skillName == "") {
                    break;
                }

                var skill = SkillFactory.instance.GenerateSkill(skillName);
                weaponSkills.Add(skill);
            }

            return new Weapon(weaponName, weaponRange, weaponWeight, weaponMight, weaponHitRate, weaponCritRate,
                weaponRarity, weaponBuyingPrice, weaponSellingPrice, weaponType, damageType, weaponSkills);
        }

        public Weapon GenerateWeapon(string name) {
            //return weapon from WeaponGenerator using name as a key
            return weaponGenerator[name].Invoke();
        }

    }
}

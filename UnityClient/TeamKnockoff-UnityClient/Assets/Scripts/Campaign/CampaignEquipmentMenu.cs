using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Campaign {
    public class CampaignEquipmentMenu : MonoBehaviour {

        public GameObject equipmentButtonPrefab;
        public GameObject unitEquipmentMenuPrefab;
        public GameObject unitSkillPrefab;

        public Button backButton;
        public Button weaponsButton;
        public Button itemsButton;
        public Button equipButton;
        public Button unequipButton;

        public TextMeshProUGUI selectedUnitName;
        public TextMeshProUGUI selectedUnitTypeClass;
        public TextMeshProUGUI selectedUnitHealthPoints;
        public TextMeshProUGUI selectedUnitLevel;
        public TextMeshProUGUI selectedUnitExperiencePoints;
        public TextMeshProUGUI selectedUnitStrength;
        public TextMeshProUGUI selectedUnitMagic;
        public TextMeshProUGUI selectedUnitDefense;
        public TextMeshProUGUI selectedUnitResistance;
        public TextMeshProUGUI selectedUnitSpeed;
        public TextMeshProUGUI selectedUnitSkill;
        public TextMeshProUGUI selectedUnitLuck;
        public TextMeshProUGUI selectedUnitMovement;

        public TextMeshProUGUI selectedUnitWeaponName;
        public TextMeshProUGUI selectedUnitWeaponType;
        public TextMeshProUGUI selectedUnitWeaponRange;
        public TextMeshProUGUI selectedUnitWeaponWeight;
        public TextMeshProUGUI selectedUnitWeaponMight;
        public TextMeshProUGUI selectedUnitWeaponDamageType;
        public TextMeshProUGUI selectedUnitWeaponHitRate;
        public TextMeshProUGUI selectedUnitWeaponCritRate;

        public TextMeshProUGUI selectedWeaponName;
        public TextMeshProUGUI selectedWeaponType;
        public TextMeshProUGUI selectedWeaponRange;
        public TextMeshProUGUI selectedWeaponWeight;
        public TextMeshProUGUI selectedWeaponMight;
        public TextMeshProUGUI selectedWeaponDamageType;
        public TextMeshProUGUI selectedWeaponHitRate;
        public TextMeshProUGUI selectedWeaponCritRate;
        public TextMeshProUGUI selectedWeaponDescription;

        public GameObject availableUnitsContent;
        public GameObject availableWeaponsContent;
        public GameObject selectedUnitSkillsContent;

        public List<GameObject> availableWeaponObjects;
        public List<GameObject> unitSkillObjects;

        private Unit selectedUnit;
        private Weapon selectedWeapon;

        // Start is called before the first frame update
        void Start() {
            backButton.onClick.AddListener(SceneLoader.instance.GoToCampaignChapterMenu);

            var units = CampaignManager.instance.CampaignPlayerData.Units;
            foreach (var unit in units) {
                var button = CreateUnitButton(unit);
                button.onClick.AddListener(() => {
                    selectedUnit = unit;
                    RefreshInformation();
                    UpdateUnitInformation(unit);
                });
            }

            equipButton.onClick.AddListener(EquipWeapon);
            unequipButton.onClick.AddListener(UnequipWeapon);

            equipButton.interactable = false;
            unequipButton.interactable = false;
        }

        private Button CreateUnitButton(Unit unit) {
            var buttonObject = Instantiate(unitEquipmentMenuPrefab, availableUnitsContent.transform);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = unit.Name.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();

            var buttonSprites = buttonComponent.gameObject.GetComponentsInChildren<Image>();

            var unitSprite = UnitFactory.instance.GetUnitSprite(unit.Name);
            buttonSprites[1].sprite = unitSprite;

            return buttonComponent;
        }

        private void UpdateUnitInformation(Unit unit) {
            selectedUnitName.text = unit.Name;
            selectedUnitTypeClass.text = $"{unit.Type} - {unit.Class}";
            selectedUnitHealthPoints.text = $"{unit.MaxHealthPoints.Value}";
            selectedUnitLevel.text = $"{unit.Level}";
            selectedUnitExperiencePoints.text = $"{unit.ExperiencePoints}";
            selectedUnitStrength.text = $"{unit.Strength.Value}";
            selectedUnitMagic.text = $"{unit.Magic.Value}";
            selectedUnitDefense.text = $"{unit.Defense.Value}";
            selectedUnitResistance.text = $"{unit.Resistance.Value}";
            selectedUnitSpeed.text = $"{unit.Speed.Value}";
            selectedUnitSkill.text = $"{unit.Skill.Value}";
            selectedUnitLuck.text = $"{unit.Luck.Value}";
            selectedUnitMovement.text = $"{unit.Movement.Value}";

            var unitWeapon = unit.MainWeapon;
            selectedUnitWeaponName.text = unitWeapon.Name;
            selectedUnitWeaponType.text = unitWeapon.WeapType.ToString();
            selectedUnitWeaponRange.text = $"{unitWeapon.Range}";
            selectedUnitWeaponWeight.text = $"{unitWeapon.Weight}";
            selectedUnitWeaponMight.text = $"{unitWeapon.Might}";
            selectedUnitWeaponDamageType.text = unitWeapon.DamageType.ToString();
            selectedUnitWeaponHitRate.text = $"{unitWeapon.HitRate}";
            selectedUnitWeaponCritRate.text = $"{unitWeapon.CritRate}";

            var unitSkills = unit.Skills;
            unitSkillObjects = new List<GameObject>();
            foreach (var skill in unitSkills) {
                var skillObject = Instantiate(unitSkillPrefab, selectedUnitSkillsContent.transform);
                unitSkillObjects.Add(skillObject);
                var skillLabel = skillObject.GetComponentInChildren<TextMeshProUGUI>();
                skillLabel.text = skill.SkillName;
            }

            UpdateWeaponButtons();
        }

        private void EquipWeapon() {
            if (selectedUnit.MainWeapon != Weapon.FISTS) {
                var unequippedWeapon = selectedUnit.UnequipWeapon();
                CampaignManager.instance.CampaignPlayerData.Weapons.Add(unequippedWeapon);
            }
            selectedUnit.EquipWeapon(selectedWeapon);
            CampaignManager.instance.CampaignPlayerData.Weapons.Remove(selectedWeapon);
            UpdateWeaponButtons();
            UpdateUnitInformation(selectedUnit);
        }

        private void UnequipWeapon() {
            var unequippedWeapon = selectedUnit.UnequipWeapon();
            CampaignManager.instance.CampaignPlayerData.Weapons.Add(unequippedWeapon);
            UpdateWeaponButtons();
            UpdateUnitInformation(selectedUnit);
        }

        private void UpdateWeaponButtons() {
            foreach (var weaponObject in availableWeaponObjects) {
                Destroy(weaponObject);
            }

            availableWeaponObjects = new List<GameObject>();
            var equippedWeapon = selectedUnit.MainWeapon;
            var equippedWeaponButton = CreateWeaponButton($"{equippedWeapon.Name} - Equipped");

            equippedWeaponButton.onClick.AddListener(() => {
                unequipButton.interactable = true;
                equipButton.interactable = false;
            });


            var unitPossibleWeapons = CampaignManager.instance.CampaignPlayerData.Weapons.Where(weapon => selectedUnit.CanUse(weapon));
            foreach (var weapon in unitPossibleWeapons) {
                var weaponButton = CreateWeaponButton(weapon.Name);

                weaponButton.onClick.AddListener(() => {
                    selectedWeapon = weapon;
                    UpdateWeaponInformation(weapon);
                    unequipButton.interactable = false;
                    equipButton.interactable = true;
                });
            }
        }

        private void UpdateWeaponInformation(Weapon weapon) {
            selectedWeaponName.text = weapon.Name;
            selectedWeaponType.text = weapon.WeapType.ToString();
            selectedWeaponRange.text = $"{weapon.Range}";
            selectedWeaponWeight.text = weapon.Weight.ToString();
            selectedWeaponMight.text = weapon.Might.ToString();
            selectedWeaponDamageType.text = weapon.DamageType.ToString();
            selectedWeaponHitRate.text = weapon.HitRate.ToString();
            selectedWeaponCritRate.text = weapon.CritRate.ToString();
        }

        private Button CreateWeaponButton(string weaponName) {
            var buttonObject = Instantiate(equipmentButtonPrefab, availableWeaponsContent.transform);
            availableWeaponObjects.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = weaponName.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();
            return buttonComponent;
        }


        private void RefreshInformation() {
            foreach (var skillObject in unitSkillObjects) {
                Destroy(skillObject);
            }

        }
    }
}

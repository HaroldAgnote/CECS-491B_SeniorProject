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

        public GameObject selectedUnitGameObject;
        public GameObject selectedWeaponGameObject;

        private Unit selectedUnit;
        private Weapon selectedWeapon;

        private Color SELECTED_COLOR = Color.blue;
        private Color UNSELECTED_COLOR = Color.white;

        // Start is called before the first frame update
        void Start() {
            backButton.onClick.AddListener(GoBack);

            var units = CampaignManager.instance.CampaignPlayerData.CampaignUnits;
            foreach (var unit in units) {
                var button = CreateUnitButton(unit);
                button.onClick.AddListener(() => {
                    if (selectedUnitGameObject != null) {
                        var selectedUnitButton = selectedUnitGameObject.GetComponent<Image>();
                        selectedUnitButton.color = UNSELECTED_COLOR;
                    }
                    selectedUnit = unit;
                    UpdateUnitInformation(unit);

                    selectedUnitGameObject = button.gameObject;
                    var buttonImage = selectedUnitGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;

                    if (selectedWeapon != null) {
                        if (!unit.CanUse(selectedWeapon)) {
                            selectedWeapon = null;
                            selectedWeaponGameObject = null;
                            equipButton.interactable = false;
                            ClearWeaponInformation();
                        }
                    }
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
            selectedUnitHealthPoints.text = $"{unit.MaxHealthPoints}";
            selectedUnitLevel.text = $"{unit.Level}";
            selectedUnitExperiencePoints.text = $"{unit.ExperiencePoints}";
            selectedUnitStrength.text = $"{unit.Strength}";
            selectedUnitMagic.text = $"{unit.Magic}";
            selectedUnitDefense.text = $"{unit.Defense}";
            selectedUnitResistance.text = $"{unit.Resistance}";
            selectedUnitSpeed.text = $"{unit.Speed}";
            selectedUnitSkill.text = $"{unit.Skill}";
            selectedUnitLuck.text = $"{unit.Luck}";
            selectedUnitMovement.text = $"{unit.Movement}";

            if (selectedUnit.MainWeapon != null) {
                var unitWeapon = unit.MainWeapon;
                selectedUnitWeaponName.text = unitWeapon.Name;
                selectedUnitWeaponType.text = unitWeapon.WeapType.ToString();
                selectedUnitWeaponRange.text = $"{unitWeapon.Range}";
                selectedUnitWeaponWeight.text = $"{unitWeapon.Weight}";
                selectedUnitWeaponMight.text = $"{unitWeapon.Might}";
                selectedUnitWeaponDamageType.text = unitWeapon.DamageType.ToString();
                selectedUnitWeaponHitRate.text = $"{unitWeapon.HitRate}";
                selectedUnitWeaponCritRate.text = $"{unitWeapon.CritRate}";
            }

            foreach (var skillObject in unitSkillObjects) {
                Destroy(skillObject);
            }

            unitSkillObjects = new List<GameObject>();
            var unitSkills = unit.Skills;

            foreach (var skill in unitSkills) {
                var skillObject = Instantiate(unitSkillPrefab, selectedUnitSkillsContent.transform);
                unitSkillObjects.Add(skillObject);
                var skillLabel = skillObject.GetComponentInChildren<TextMeshProUGUI>();
                skillLabel.text = skill.SkillName;
            }

            UpdateWeaponButtons();
        }

        private void EquipWeapon() {
            if (!selectedUnit.MainWeapon.Equals(Weapon.FISTS)) {
                var unequippedWeapon = selectedUnit.UnequipWeapon();
                CampaignManager.instance.CampaignPlayerData.Weapons.Add(unequippedWeapon);
            }
            selectedUnit.EquipWeapon(selectedWeapon);
            CampaignManager.instance.CampaignPlayerData.Weapons.Remove(selectedWeapon);

            UpdateWeaponButtons();
            UpdateUnitInformation(selectedUnit);
            equipButton.interactable = false;
            unequipButton.interactable = false;
        }

        private void UnequipWeapon() {
            var unequippedWeapon = selectedUnit.UnequipWeapon();
            CampaignManager.instance.CampaignPlayerData.Weapons.Add(unequippedWeapon);

            UpdateWeaponButtons();
            UpdateUnitInformation(selectedUnit);
            equipButton.interactable = false;
            unequipButton.interactable = false;
        }

        private void UpdateWeaponButtons() {
            foreach (var weaponObject in availableWeaponObjects) {
                Destroy(weaponObject);
            }

            availableWeaponObjects = new List<GameObject>();

            if (!selectedUnit.MainWeapon.Equals(Weapon.FISTS)) {
                var equippedWeapon = selectedUnit.MainWeapon;
                var equippedWeaponButton = CreateWeaponButton($"{equippedWeapon.Name} - Equipped");

                equippedWeaponButton.onClick.AddListener(() => {
                    if (selectedWeaponGameObject != null) {
                        var selectedButton = selectedWeaponGameObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedWeaponGameObject = equippedWeaponButton.gameObject;
                    var buttonImage = selectedWeaponGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;
                    
                    selectedWeapon = null;
                    UpdateWeaponInformation(equippedWeapon);
                    unequipButton.interactable = true;
                    equipButton.interactable = false;
                });
            }

            var unitPossibleWeapons = CampaignManager.instance.CampaignPlayerData.Weapons.Where(weapon => selectedUnit.CanUse(weapon));
            foreach (var weapon in unitPossibleWeapons) {
                var weaponButton = CreateWeaponButton(weapon.Name);

                weaponButton.onClick.AddListener(() => {
                    if (selectedWeaponGameObject != null) {
                        var selectedButton = selectedWeaponGameObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedWeaponGameObject = weaponButton.gameObject;
                    var buttonImage = selectedWeaponGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;
                    
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

        private void ClearWeaponInformation() {
            selectedWeaponName.text = "";
            selectedWeaponType.text = "";
            selectedWeaponRange.text = "";
            selectedWeaponWeight.text = "";
            selectedWeaponMight.text = "";
            selectedWeaponDamageType.text = "";
            selectedWeaponHitRate.text = "";
            selectedWeaponCritRate.text = "";
        }

        private Button CreateWeaponButton(string weaponName) {
            var buttonObject = Instantiate(equipmentButtonPrefab, availableWeaponsContent.transform);
            availableWeaponObjects.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = weaponName.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();
            return buttonComponent;
        }

        private void GoBack() {
            CampaignManager.instance.CampaignPlayerUnitData = CampaignManager.instance.CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();
            CampaignManager.instance.LoadCampaignChapterMenu();
        }
    }
}

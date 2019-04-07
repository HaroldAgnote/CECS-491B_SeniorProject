using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;
using UnityEditor.Profiling.Memory.Experimental;

namespace Assets.Scripts.Campaign {
    public class CampaignShopMenu : MonoBehaviour {

        public GameObject equipmentButtonPrefab;
        public GameObject unitEquipmentMenuPrefab;
        public GameObject unitSkillPrefab;

        public Button backButton;
        public Button buyMenuButton;
        public Button sellMenuButton;
        public Button actionButton;

        public TextMeshProUGUI playerLabel;
        public TextMeshProUGUI moneyLabel;

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
        public TextMeshProUGUI selectedWeaponPrice;
        public TextMeshProUGUI selectedWeaponInventory;
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

        private List<GameObject> availableWeaponObjects;
        private List<GameObject> availableUnitObjects;
        private List<GameObject> unitSkillObjects;

        private Unit selectedUnit;
        private Weapon selectedWeapon;

        private bool isBuying;
        private bool isSelling;

        void Start() {
            availableWeaponObjects = new List<GameObject>();
            availableUnitObjects = new List<GameObject>();
            unitSkillObjects = new List<GameObject>();

            playerLabel.text = CampaignManager.instance.CampaignPlayerData.Name;

            UpdateMoney();
            SwitchToBuyingMenu();
            buyMenuButton.onClick.AddListener(SwitchToBuyingMenu);
            sellMenuButton.onClick.AddListener(SwitchToSellingMenu);
            actionButton.onClick.AddListener(ActionButton);
            backButton.onClick.AddListener(GoBack);
        }

        private Button CreateWeaponButton(string weaponName) {
            var buttonObject = Instantiate(equipmentButtonPrefab, availableWeaponsContent.transform);
            availableWeaponObjects.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = weaponName.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();
            return buttonComponent;
        }

        private Button CreateUnitButton(Unit unit, bool equipped) {
            var buttonObject = Instantiate(unitEquipmentMenuPrefab, availableUnitsContent.transform);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = unit.Name.ToUpper();

            availableUnitObjects.Add(buttonObject);

            var buttonComponent = buttonObject.GetComponent<Button>();
            if (equipped) {
                buttonLabel.text += " - EQUIPPED";
                actionButton.onClick.AddListener(() => {
                    buttonLabel.text = unit.Name.ToUpper();
                    UpdateUnitInformation(unit);
                });
            }


            var buttonSprites = buttonComponent.gameObject.GetComponentsInChildren<Image>();

            var unitSprite = UnitFactory.instance.GetUnitSprite(unit.Name);
            buttonSprites[1].sprite = unitSprite;

            return buttonComponent;
        }

        private void SwitchToBuyingMenu() {
            foreach (var availableWeaponObject in availableWeaponObjects) {
                Destroy(availableWeaponObject);
            }
            availableWeaponObjects = new List<GameObject>();

            isBuying = true;
            isSelling = false;
            var actionButtonLabel = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            actionButtonLabel.text = "Buy";

            var weaponBank = WeaponFactory.instance.WeaponBank.Where(weapon => weapon != Weapon.FISTS);
            foreach (var weapon in weaponBank) {
                var weaponNamePrice = $"{weapon.Name} - {weapon.BuyingPrice}";
                var button = CreateWeaponButton(weaponNamePrice);
                button.onClick.AddListener(() => {
                    selectedWeapon = weapon;
                    UpdateWeaponInformation(weapon);
                    UpdateUnitButtons();
                    if (PlayerCanBuy(weapon)) {
                        actionButton.interactable = true;
                    }
                });
            }
        }

        private bool PlayerCanBuy(Weapon weapon) {
            return CampaignManager.instance.CampaignPlayerData.Money > weapon.BuyingPrice;
        }

        private void SwitchToSellingMenu() {
            foreach (var availableWeaponObject in availableWeaponObjects) {
                Destroy(availableWeaponObject);
            }

            availableWeaponObjects = new List<GameObject>();
            isBuying = false;
            isSelling = true;
            var actionButtonLabel = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            actionButtonLabel.text = "Sell";

            var inventory = CampaignManager.instance.CampaignPlayerData.Weapons;

            inventory.AddRange(CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .Where(unit => unit.MainWeapon.IsSellable)
                .Select(unit => unit.MainWeapon).ToList());

            foreach (var weapon in inventory) {
                var weaponNamePrice = $"{weapon.Name} - {weapon.SellingPrice}";
                var button = CreateWeaponButton(weaponNamePrice);
                button.onClick.AddListener(() => {
                    selectedWeapon = weapon;
                    UpdateWeaponInformation(weapon);
                    UpdateUnitButtons();
                    actionButton.interactable = true;
                });
            }
        }

        private void ActionButton() {
            if (isBuying) {
                BuyItem();
            } else if (isSelling) {
                SellItem();
                SwitchToSellingMenu();
            }
            UpdateMoney();
            UpdateWeaponInformation(selectedWeapon);
            selectedWeapon = null;
            actionButton.interactable = false;
        }

        private void BuyItem() {
            var boughtWeapon = selectedWeapon;
            CampaignManager.instance.CampaignPlayerData.Weapons.Add(boughtWeapon);
            CampaignManager.instance.CampaignPlayerData.Money -= boughtWeapon.BuyingPrice;
        }

        private void SellItem() {
            var soldWeapon = selectedWeapon;
            var equippedUnit =
                CampaignManager.instance.CampaignPlayerData.CampaignUnits.SingleOrDefault(unit =>
                    unit.MainWeapon == soldWeapon);

            if (equippedUnit != null) {
                equippedUnit.UnequipWeapon();
            }

            CampaignManager.instance.CampaignPlayerData.Weapons.Remove(soldWeapon);
            CampaignManager.instance.CampaignPlayerData.Money += soldWeapon.SellingPrice;
        }

        private void UpdateMoney() {
            moneyLabel.text = $"{CampaignManager.instance.CampaignPlayerData.Money}";
        }

        private void UpdateUnitButtons() {
            foreach (var unitObject in availableUnitObjects) {
                Destroy(unitObject);
            }

            availableUnitObjects = new List<GameObject>();

            var possibleUnits =
                CampaignManager.instance.CampaignPlayerData.CampaignUnits.Where(unit => unit.CanUse(selectedWeapon));

            foreach (var possibleUnit in possibleUnits) {

                var equipped = possibleUnit.MainWeapon == selectedWeapon;

                var button = CreateUnitButton(possibleUnit, equipped);
                button.onClick.AddListener(() => {
                    selectedUnit = possibleUnit;
                    UpdateUnitInformation(possibleUnit);
                });
            }
        }

        private void UpdateWeaponInformation(Weapon weapon) {
            selectedWeaponName.text = weapon.Name;
            if (isBuying) {
                selectedWeaponPrice.text = $"{weapon.BuyingPrice}";
            } else if (isSelling) {
                selectedWeaponPrice.text = $"{weapon.SellingPrice}";
            }

            int count = CampaignManager.instance.CampaignPlayerData.Weapons.Count(weap => weap.Equals(selectedWeapon));
            count += CampaignManager.instance.CampaignPlayerData.CampaignUnits.Select(unit => unit.MainWeapon)
                .Count(weap => weap.Equals(selectedWeapon));

            selectedWeaponInventory.text = $"{count}";

            selectedWeaponType.text = weapon.WeapType.ToString();
            selectedWeaponRange.text = $"{weapon.Range}";
            selectedWeaponWeight.text = weapon.Weight.ToString();
            selectedWeaponMight.text = weapon.Might.ToString();
            selectedWeaponDamageType.text = weapon.DamageType.ToString();
            selectedWeaponHitRate.text = weapon.HitRate.ToString();
            selectedWeaponCritRate.text = weapon.CritRate.ToString();
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

            if (unit.MainWeapon != null) {
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
        }

        private void GoBack() {
            CampaignManager.instance.CampaignPlayerUnitData = CampaignManager.instance.CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();

            SceneLoader.instance.GoToCampaignChapterMenu();
        }
    }
}

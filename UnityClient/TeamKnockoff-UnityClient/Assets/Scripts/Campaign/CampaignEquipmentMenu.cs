using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;
using Assets.Scripts.Model.Items;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Campaign {
    public class CampaignEquipmentMenu : MonoBehaviour {

        public GameObject equipmentButtonPrefab;
        public GameObject unitEquipmentMenuPrefab;
        public GameObject unitSubItemPrefab;

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

        public GameObject selectedWeaponInformation;
        public TextMeshProUGUI selectedWeaponName;
        public TextMeshProUGUI selectedWeaponType;
        public TextMeshProUGUI selectedWeaponRange;
        public TextMeshProUGUI selectedWeaponWeight;
        public TextMeshProUGUI selectedWeaponMight;
        public TextMeshProUGUI selectedWeaponDamageType;
        public TextMeshProUGUI selectedWeaponHitRate;
        public TextMeshProUGUI selectedWeaponCritRate;
        public TextMeshProUGUI selectedWeaponDescription;

        public GameObject selectedItemInformation;
        public TextMeshProUGUI selectedItemName;
        public TextMeshProUGUI selectedItemInventory;
        public TextMeshProUGUI selectedItemDescription;

        public GameObject weaponMenu;
        public GameObject itemMenu;

        public GameObject availableUnitsContent;
        public GameObject availableWeaponsContent;
        public GameObject availableItemsContent;
        public GameObject selectedUnitSkillsContent;
        public GameObject selectedUnitItemsContent;

        private List<GameObject> availableWeaponObjects;
        private List<GameObject> availableItemsObjects;
        private List<GameObject> unitSkillObjects;
        private List<GameObject> unitItemObjects;

        private GameObject selectedUnitGameObject;
        private GameObject selectedEquipmentGameObject;

        private Unit selectedUnit;
        private Weapon selectedWeapon;
        private Item selectedItem;

        private Color SELECTED_COLOR = Color.blue;
        private Color UNSELECTED_COLOR = Color.white;

        // Start is called before the first frame update
        void Start() {
            unitSkillObjects = new List<GameObject>();
            unitItemObjects = new List<GameObject>();
            availableWeaponObjects = new List<GameObject>();
            availableItemsObjects = new List<GameObject>();

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
                            selectedEquipmentGameObject = null;
                            equipButton.interactable = false;
                            ClearWeaponInformation();
                        }
                    }
                });
            }

            SwitchToWeaponEquipment();

            weaponsButton.onClick.AddListener(SwitchToWeaponEquipment);
            itemsButton.onClick.AddListener(SwitchToItemEquipment);

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
                var skillObject = Instantiate(unitSubItemPrefab, selectedUnitSkillsContent.transform);
                unitSkillObjects.Add(skillObject);
                var skillLabel = skillObject.GetComponentInChildren<TextMeshProUGUI>();
                skillLabel.text = skill.SkillName;
            }

            foreach (var itemObject in unitItemObjects) {
                Destroy(itemObject);
            }

            unitItemObjects = new List<GameObject>();
            var unitItems = unit.Items;

            foreach (var item in unitItems) {
                var itemObject = Instantiate(unitSubItemPrefab, selectedUnitItemsContent.transform);
                unitItemObjects.Add(itemObject);
                var itemLabel = itemObject.GetComponentInChildren<TextMeshProUGUI>();
                itemLabel.text = item.ItemName;
            }

            UpdateWeaponButtons();
            UpdateItemButtons();
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

        private void EquipItem() {
            selectedUnit.EquipItem(selectedItem);
            CampaignManager.instance.CampaignPlayerData.Items.Remove(selectedItem);

            UpdateItemButtons();
            UpdateUnitInformation(selectedUnit);
            equipButton.interactable = false;
            unequipButton.interactable = false;
        }

        private void UnequipItem() {
            var unequippedItem = selectedUnit.UnequipItem(selectedItem);
            CampaignManager.instance.CampaignPlayerData.Items.Add(unequippedItem);

            UpdateItemButtons();
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
                    if (selectedEquipmentGameObject != null) {
                        var selectedButton = selectedEquipmentGameObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedEquipmentGameObject = equippedWeaponButton.gameObject;
                    var buttonImage = selectedEquipmentGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;

                    selectedItem = null;
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
                    if (selectedEquipmentGameObject != null) {
                        var selectedButton = selectedEquipmentGameObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedEquipmentGameObject = weaponButton.gameObject;
                    var buttonImage = selectedEquipmentGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;

                    selectedItem = null;
                    selectedWeapon = weapon;
                    UpdateWeaponInformation(weapon);
                    unequipButton.interactable = false;
                    equipButton.interactable = true;
                });
            }
        }

        private void UpdateItemButtons() {
            foreach (var itemObject in availableItemsObjects) {
                Destroy(itemObject);
            }

            availableItemsObjects = new List<GameObject>();

            var equippedItems = selectedUnit.Items;
            foreach (var equippedItem in equippedItems) {
                var equippedItemButton = CreateItemButton($"{equippedItem.ItemName} - Equipped");

                equippedItemButton.onClick.AddListener(() => {
                    if (selectedEquipmentGameObject != null) {
                        var selectedButton = selectedEquipmentGameObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedEquipmentGameObject = equippedItemButton.gameObject;
                    var buttonImage = selectedEquipmentGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;
                    
                    selectedWeapon = null;
                    selectedItem = equippedItem;
                    UpdateItemInformation(equippedItem);
                    unequipButton.interactable = true;
                    equipButton.interactable = false;
                });
            }

            var unitPossibleItems = CampaignManager.instance.CampaignPlayerData.Items.OrderBy(item => item.ItemRarity);
            foreach (var item in unitPossibleItems) {
                var itemButton = CreateItemButton(item.ItemName);

                itemButton.onClick.AddListener(() => {
                    if (selectedEquipmentGameObject != null) {
                        var selectedButton = selectedEquipmentGameObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedEquipmentGameObject = itemButton.gameObject;
                    var buttonImage = selectedEquipmentGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;
                    
                    selectedWeapon = null;
                    selectedItem = item;
                    UpdateItemInformation(item);
                    unequipButton.interactable = false;
                    if (selectedUnit.CanEquipItem(item)) {
                        equipButton.interactable = true;
                    }
                });
            }
        }

        private void UpdateWeaponInformation(Weapon weapon) {
            selectedItemInformation.SetActive(false);
            selectedWeaponInformation.SetActive(true);

            selectedWeaponName.text = weapon.Name;
            selectedWeaponType.text = weapon.WeapType.ToString();
            selectedWeaponRange.text = $"{weapon.Range}";
            selectedWeaponWeight.text = weapon.Weight.ToString();
            selectedWeaponMight.text = weapon.Might.ToString();
            selectedWeaponDamageType.text = weapon.DamageType.ToString();
            selectedWeaponHitRate.text = weapon.HitRate.ToString();
            selectedWeaponCritRate.text = weapon.CritRate.ToString();
        }

        private void UpdateItemInformation(Item item) {
            selectedWeaponInformation.SetActive(false);
            selectedItemInformation.SetActive(true);

            selectedItemName.text = item.ItemName;

            int count = CampaignManager.instance.CampaignPlayerData.Items.Count(it => it.Equals(item));
            count += CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .SelectMany(unit => unit.Items)
                .Count(it => it.Equals(item));

            selectedItemInventory.text = $"{count}";
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

        private void SwitchToWeaponEquipment() {
            itemMenu.SetActive(false);
            weaponMenu.SetActive(true);

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(EquipWeapon);

            unequipButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(UnequipWeapon);
        }

        private void SwitchToItemEquipment() {
            itemMenu.SetActive(true);
            weaponMenu.SetActive(false);

            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(EquipItem);

            unequipButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(UnequipItem);
        }

        private Button CreateWeaponButton(string weaponName) {
            var buttonObject = Instantiate(equipmentButtonPrefab, availableWeaponsContent.transform);
            availableWeaponObjects.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = weaponName.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();
            return buttonComponent;
        }

        private Button CreateItemButton(string itemName) {
            var buttonObject = Instantiate(equipmentButtonPrefab, availableItemsContent.transform);
            availableItemsObjects.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = itemName.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();
            return buttonComponent;
        }

        private void GoBack() {
            CampaignManager.instance.CampaignPlayerUnitData = CampaignManager.instance.CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();
            CampaignManager.instance.LoadCampaignChapterMenu();
        }
    }
}

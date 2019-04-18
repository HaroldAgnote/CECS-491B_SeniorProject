using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;
using Assets.Scripts.Model.Items;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Campaign {
    public class CampaignShopMenu : MonoBehaviour {

        public GameObject equipmentButtonPrefab;
        public GameObject unitEquipmentMenuPrefab;
        public GameObject unitSubItemPrefab;

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

        public GameObject selectedWeaponInformation;
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

        public GameObject selectedItemInformation;
        public TextMeshProUGUI selectedItemName;
        public TextMeshProUGUI selectedItemPrice;
        public TextMeshProUGUI selectedItemInventory;
        public TextMeshProUGUI selectedItemDescription;

        public GameObject availableUnitsContent;
        public GameObject availableMerchandiseContent;
        public GameObject selectedUnitSkillsContent;
        public GameObject selectedUnitItemsContent;

        private List<GameObject> availableMerchandiseObjects;
        private List<GameObject> availableUnitObjects;
        private List<GameObject> unitSkillObjects;
        private List<GameObject> unitItemObjects;

        private GameObject selectedUnitGameObject;
        private GameObject selectedMerchObject;
        private GameObject selectedItemGameObject;

        private Unit selectedUnit;
        private Weapon selectedWeapon;
        private Item selectedItem;

        private bool isBuying;
        private bool isSelling;

        private Color SELECTED_COLOR = Color.blue;
        private Color UNSELECTED_COLOR = Color.white;

        void Start() {
            availableMerchandiseObjects = new List<GameObject>();
            availableUnitObjects = new List<GameObject>();
            unitSkillObjects = new List<GameObject>();
            unitItemObjects = new List<GameObject>();

            playerLabel.text = CampaignManager.instance.CampaignPlayerData.Name;

            UpdateMoney();
            SwitchToBuyingMenu();
            buyMenuButton.onClick.AddListener(SwitchToBuyingMenu);
            sellMenuButton.onClick.AddListener(SwitchToSellingMenu);
            actionButton.onClick.AddListener(ActionButton);
            backButton.onClick.AddListener(GoBack);
        }

        private Button CreateMerchSlotButton(string weaponName) {
            var buttonObject = Instantiate(equipmentButtonPrefab, availableMerchandiseContent.transform);
            availableMerchandiseObjects.Add(buttonObject);
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
                    if (isSelling) {
                        buttonLabel.text = unit.Name.ToUpper();
                        UpdateUnitInformation(unit);
                    }
                });
            }


            var buttonSprites = buttonComponent.gameObject.GetComponentsInChildren<Image>();

            var unitSprite = UnitFactory.instance.GetUnitSprite(unit.Name);
            buttonSprites[1].sprite = unitSprite;

            return buttonComponent;
        }

        private void SwitchToBuyingMenu() {
            foreach (var availableWeaponObject in availableMerchandiseObjects) {
                Destroy(availableWeaponObject);
            }
            availableMerchandiseObjects = new List<GameObject>();

            isBuying = true;
            isSelling = false;
            var actionButtonLabel = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            actionButtonLabel.text = "Buy";

            for (int rarity = 1; rarity <= 3; rarity++) {
                var itemBank = ItemFactory.instance.ItemBank.Where(item => item.IsBuyable && item.ItemRarity == rarity);
                foreach (var item in itemBank) {
                    var itemNamePrice = $"{item.ItemName} - {item.BuyingPrice}";
                    var button = CreateMerchSlotButton(itemNamePrice);
                    button.onClick.AddListener(() => {
                        if (selectedMerchObject != null) {
                            var selectedButton = selectedMerchObject.GetComponent<Image>();
                            selectedButton.color = UNSELECTED_COLOR;
                        }

                        selectedMerchObject = button.gameObject;
                        var buttonImage = selectedMerchObject.GetComponent<Image>();
                        buttonImage.color = SELECTED_COLOR;

                        selectedWeapon = null;
                        selectedItem = item;
                        UpdateItemInformation(item);
                        UpdateUnitButtons();
                        if (PlayerCanBuyItem(item)) {
                            actionButton.interactable = true;
                        }
                    });
                }


                var weaponBank = WeaponFactory.instance.WeaponBank.Where(weapon => weapon != Weapon.FISTS
                                                                                   && weapon.IsBuyable
                                                                                   && weapon.Rarity == rarity);

                foreach (var weapon in weaponBank) {
                    var weaponNamePrice = $"{weapon.Name} - {weapon.BuyingPrice}";
                    var button = CreateMerchSlotButton(weaponNamePrice);
                    button.onClick.AddListener(() => {
                        if (selectedMerchObject != null) {
                            var selectedButton = selectedMerchObject.GetComponent<Image>();
                            selectedButton.color = UNSELECTED_COLOR;
                        }

                        selectedMerchObject = button.gameObject;
                        var buttonImage = selectedMerchObject.GetComponent<Image>();
                        buttonImage.color = SELECTED_COLOR;

                        selectedItem = null;
                        selectedWeapon = weapon;
                        UpdateWeaponInformation(weapon);
                        UpdateUnitButtons();
                        if (PlayerCanBuyWeapon(weapon)) {
                            actionButton.interactable = true;
                        }
                    });
                }
            }

        }

        private bool PlayerCanBuyItem(Item item) {
            return CampaignManager.instance.CampaignPlayerData.Money >= item.BuyingPrice;
        }

        private bool PlayerCanBuyWeapon(Weapon weapon) {
            return CampaignManager.instance.CampaignPlayerData.Money >= weapon.BuyingPrice;
        }

        private void SwitchToSellingMenu() {
            foreach (var availableWeaponObject in availableMerchandiseObjects) {
                Destroy(availableWeaponObject);
            }

            availableMerchandiseObjects = new List<GameObject>();
            isBuying = false;
            isSelling = true;
            var actionButtonLabel = actionButton.GetComponentInChildren<TextMeshProUGUI>();
            actionButtonLabel.text = "Sell";

            var itemInventory = new List<Item>();
            itemInventory.AddRange(CampaignManager.instance.CampaignPlayerData.Items);
            itemInventory.AddRange(CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .SelectMany(unit => unit.Items)
                .Where(item => item.IsSellable).ToList());

            foreach (var item in itemInventory) {
                var itemNamePrice = $"{item.ItemName} - {item.SellingPrice}";
                var button = CreateMerchSlotButton(itemNamePrice);
                button.onClick.AddListener(() => {
                    if (selectedMerchObject != null) {
                        var selectedButton = selectedMerchObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedMerchObject = button.gameObject;
                    var buttonImage = selectedMerchObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;

                    selectedWeapon = null;
                    selectedItem = item;
                    UpdateItemInformation(item);
                    UpdateUnitButtons();
                    actionButton.interactable = true;
                });
            }

            var weaponInventory = new List<Weapon>();
            weaponInventory.AddRange(CampaignManager.instance.CampaignPlayerData.Weapons);
            weaponInventory.AddRange(CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .Where(unit => unit.MainWeapon.IsSellable)
                .Select(unit => unit.MainWeapon).ToList());

            foreach (var weapon in weaponInventory) {
                var weaponNamePrice = $"{weapon.Name} - {weapon.SellingPrice}";
                var button = CreateMerchSlotButton(weaponNamePrice);
                button.onClick.AddListener(() => {
                    if (selectedMerchObject != null) {
                        var selectedButton = selectedMerchObject.GetComponent<Image>();
                        selectedButton.color = UNSELECTED_COLOR;
                    }

                    selectedMerchObject = button.gameObject;
                    var buttonImage = selectedMerchObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;

                    selectedItem = null;
                    selectedWeapon = weapon;
                    UpdateWeaponInformation(weapon);
                    UpdateUnitButtons();
                    actionButton.interactable = true;
                });
            }
        }

        private void ActionButton() {
            if (isBuying) {
                if (selectedItem != null) {
                    BuyItem();
                    UpdateItemInformation(selectedItem);
                    if (!PlayerCanBuyItem(selectedItem)) {
                        selectedItem = null;
                        actionButton.interactable = false;
                    }
                } else if (selectedWeapon != null) {
                    BuyWeapon();
                    UpdateWeaponInformation(selectedWeapon);
                    if (!PlayerCanBuyWeapon(selectedWeapon)) {
                        selectedWeapon = null;
                        actionButton.interactable = false;
                    }
                }
            } else if (isSelling) {
                if (selectedItem != null) {
                    SellItem();
                    UpdateItemInformation(selectedItem);
                } else if (selectedWeapon != null) {
                    SellWeapon();
                    UpdateWeaponInformation(selectedWeapon);
                }

                selectedWeapon = null;
                selectedItem = null;
                actionButton.interactable = false;

                SwitchToSellingMenu();
            }
            UpdateMoney();
        }

        private void BuyItem() {
            var boughtItem = selectedItem;
            CampaignManager.instance.CampaignPlayerData.Items.Add(boughtItem);
            CampaignManager.instance.CampaignPlayerData.Money -= boughtItem.BuyingPrice;
        }

        private void SellItem() {
            var soldItem = selectedItem;
            var equippedUnit =
                CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .SingleOrDefault(
                    unit => unit.Items.Any(
                        item => System.Object.ReferenceEquals(item, soldItem)));

            if (equippedUnit != null) {
                equippedUnit.Items.Remove(soldItem);
            }

            CampaignManager.instance.CampaignPlayerData.Items.Remove(soldItem);
            CampaignManager.instance.CampaignPlayerData.Money += soldItem.SellingPrice;
        }

        private void BuyWeapon() {
            var boughtWeapon = selectedWeapon;
            CampaignManager.instance.CampaignPlayerData.Weapons.Add(boughtWeapon);
            CampaignManager.instance.CampaignPlayerData.Money -= boughtWeapon.BuyingPrice;
        }

        private void SellWeapon() {
            var soldWeapon = selectedWeapon;
            var equippedUnit =
                CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .SingleOrDefault(
                    unit => System.Object.ReferenceEquals(unit.MainWeapon, soldWeapon));

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

            selectedUnit = null;

            var possibleUnits = CampaignManager.instance.CampaignPlayerData.CampaignUnits;

            if (selectedItem == null && selectedWeapon != null) {
                possibleUnits = possibleUnits.Where(unit => unit.CanUse(selectedWeapon)).ToList();
            }

            foreach (var possibleUnit in possibleUnits) {
                bool equipped = false;
                if (selectedWeapon != null) {
                    equipped = System.Object.Equals(possibleUnit.MainWeapon, selectedWeapon);
                } else if (selectedItem != null) {
                    equipped = possibleUnit.Items.Any(item => System.Object.ReferenceEquals(item, selectedItem));
                }

                var button = CreateUnitButton(possibleUnit, equipped);
                button.onClick.AddListener(() => {
                    if (selectedUnitGameObject != null) {
                        var selectedUnitButton = selectedUnitGameObject.GetComponent<Image>();
                        selectedUnitButton.color = UNSELECTED_COLOR;
                    }
                    selectedUnit = possibleUnit;
                    UpdateUnitInformation(possibleUnit);

                    selectedUnitGameObject = button.gameObject;
                    var buttonImage = selectedUnitGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;
                });
            }
        }

        private void UpdateItemInformation(Item item) {
            selectedWeaponInformation.SetActive(false);
            selectedItemInformation.SetActive(true);

            selectedItemName.text = item.ItemName;
            if (isBuying) {
                selectedItemPrice.text = $"{item.BuyingPrice}";
            } else if (isSelling) {
                selectedItemPrice.text = $"{item.SellingPrice}";
            }

            int count = CampaignManager.instance.CampaignPlayerData.Items.Count(it => it.Equals(item));
            count += CampaignManager.instance.CampaignPlayerData.CampaignUnits
                .SelectMany(unit => unit.Items)
                .Count(it => it.Equals(item));

            selectedItemInventory.text = $"{count}";

        }

        private void UpdateWeaponInformation(Weapon weapon) {
            selectedItemInformation.SetActive(false);
            selectedWeaponInformation.SetActive(true);
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
        }

        private void GoBack() {
            CampaignManager.instance.CampaignPlayerUnitData = CampaignManager.instance.CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();
            CampaignManager.instance.LoadCampaignChapterMenu();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class UnitInformation : MonoBehaviour {
        public GameView gameView;

        public GameObject unitSubItemPrefab;

        public Image unitInformationBackground;

        public TextMeshProUGUI unitNameLabel;
        public TextMeshProUGUI unitClassLabel;
        public TextMeshProUGUI currentHpLabel;
        public TextMeshProUGUI maxHpLabel;
        public TextMeshProUGUI strengthLabel;
        public TextMeshProUGUI magicLabel;
        public TextMeshProUGUI defenseLabel;
        public TextMeshProUGUI resistanceLabel;
        public TextMeshProUGUI speedLabel;
        public TextMeshProUGUI skillLabel;
        public TextMeshProUGUI luckLabel;
        public TextMeshProUGUI moveLabel;
        public TextMeshProUGUI expHeader;
        public TextMeshProUGUI expLabel;
        public TextMeshProUGUI lvLabel;

        private GameViewModel gameViewModel;

        public GameObject selectedUnitSkillsContent;
        public GameObject selectedUnitItemsContent;
        private List<GameObject> unitSkillObjects;
        private List<GameObject> unitItemObjects;

        private static Color ALLY_COLOR = Color.green;
        private static Color ENEMY_COLOR = Color.red;

        public void ConstructUnitInformation() {
            gameViewModel = gameView.gameViewModel;
            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
            unitSkillObjects = new List<GameObject>();
            unitItemObjects = new List<GameObject>();
        }

        public void UpdateUnitInformation(Unit unit) {
            if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber) {
                unitInformationBackground.color = ALLY_COLOR;
                expHeader.gameObject.SetActive(true);
                expLabel.gameObject.SetActive(true);
            } else {
                unitInformationBackground.color = ENEMY_COLOR;
                expHeader.gameObject.SetActive(false);
                expLabel.gameObject.SetActive(false);
            }
            unitNameLabel.text = unit.Name;
            unitClassLabel.text = unit.Class;
            currentHpLabel.text = $"{unit.HealthPoints}";
            maxHpLabel.text = $"{unit.MaxHealthPoints}";
            strengthLabel.text = $"{unit.Strength}";
            magicLabel.text = $"{unit.Magic}";
            defenseLabel.text = $"{unit.Defense}";
            resistanceLabel.text = $"{unit.Resistance}";
            speedLabel.text = $"{unit.Speed}";
            skillLabel.text = $"{unit.Skill}";
            luckLabel.text = $"{unit.Luck}";
            moveLabel.text = $"{unit.Movement}";
            expLabel.text = $"{unit.ExperiencePoints}";
            lvLabel.text = $"{unit.Level}";

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

        private void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "HoveredSquare") {
                if (gameViewModel.HoveredSquare.Unit != null && !gameViewModel.CombatMode) {
                    var unit = gameViewModel.HoveredSquare.Unit;

                    if (unit != null) {
                        UpdateUnitInformation(unit);
                    }
                }
            }
            if (e.PropertyName == "CombatMode")
            {
                if (gameViewModel.CombatMode == true) {
                    gameObject.SetActive(false);
                } else {
                    gameObject.SetActive(true);
                    var unit = gameViewModel.SelectedUnit;
                    UpdateUnitInformation(unit);
                }
            }
        }
    }
}

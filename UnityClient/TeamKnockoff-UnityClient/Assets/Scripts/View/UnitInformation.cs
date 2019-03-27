using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class UnitInformation : MonoBehaviour {
        public GameView gameView;

        public TextMeshProUGUI unitNameLabel;
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

        private GameViewModel gameViewModel;

        public void ConstructUnitInformation() {
            gameViewModel = gameView.gameViewModel;
            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
        }

        private void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "HoveredSquare") {
                if (gameViewModel.HoveredSquare.Unit != null && !gameViewModel.CombatMode) {
                    var unit = gameViewModel.HoveredSquare.Unit;

                    if (unit != null) {
                        unitNameLabel.text = unit.Name;
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
                    }
                }
            }
            if (e.PropertyName == "CombatMode")
            {
                if (gameViewModel.CombatMode == true)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }
    }
}

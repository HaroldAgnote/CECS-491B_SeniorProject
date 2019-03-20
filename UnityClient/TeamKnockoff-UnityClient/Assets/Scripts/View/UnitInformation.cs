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
                        currentHpLabel.text = $"CUR {unit.HealthPoints}";
                        maxHpLabel.text = $"MAX {unit.MaxHealthPoints}";
                        strengthLabel.text = $"STR {unit.Strength}";
                        magicLabel.text = $"MAG {unit.Magic}";
                        defenseLabel.text = $"DEF {unit.Defense}";
                        resistanceLabel.text = $"RES {unit.Resistance}";
                        speedLabel.text = $"SPD {unit.Speed}";
                        skillLabel.text = $"SKL {unit.Skill}";
                        luckLabel.text = $"LCK {unit.Luck}";
                        moveLabel.text = $"MOV {unit.MoveRange}";
                    }
                }
            }
            if (e.PropertyName == "CombatMode")
            {
                if (gameViewModel.CombatMode == true)
                {
                    this.enabled = false;
                }
                else
                {
                    this.enabled = true;
                }
            }
        }
    }
}

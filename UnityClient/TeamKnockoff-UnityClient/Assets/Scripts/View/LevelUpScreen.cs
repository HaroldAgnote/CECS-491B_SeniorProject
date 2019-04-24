using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class LevelUpScreen : MonoBehaviour {
        public GameView gameView;

        public TextMeshProUGUI unitNameLabel;
        public TextMeshProUGUI maxHpLabel;
        public TextMeshProUGUI strengthLabel;
        public TextMeshProUGUI magicLabel;
        public TextMeshProUGUI defenseLabel;
        public TextMeshProUGUI resistanceLabel;
        public TextMeshProUGUI speedLabel;
        public TextMeshProUGUI skillLabel;
        public TextMeshProUGUI luckLabel;
        public TextMeshProUGUI expLabel;
        public TextMeshProUGUI lvLabel;
        public TextMeshProUGUI levelUpMessage;

        private GameViewModel gameViewModel;

        public Button okButton;

        public void ConstructLevelUpScreen() {
            gameViewModel = gameView.gameViewModel;

            var unitViewModels = gameViewModel.UnitViewModels;

            foreach (var unitViewModel in unitViewModels) {
                if (unitViewModel.Unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber) {
                    unitViewModel.PropertyChanged += UnitViewModel_PropertyChanged;
                }
            }

            this.gameObject.SetActive(false);
            okButton.onClick.AddListener(() => {
                this.gameObject.SetActive(false);
                gameView.mPauseButton.interactable = true;
                gameView.tileSelector.gameObject.SetActive(true);
                gameView.moveSelector.gameObject.SetActive(true);
                gameView.UnlockCamera();
                gameView.HasScreenOverlay = false;
                gameViewModel.UnpauseGame();
            });
        }

        private async void UnitViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "Level") {
                var unitViewModel = sender as UnitViewModel;

                var unit = unitViewModel.Unit;
                if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber) {
                    gameView.HasScreenOverlay = true;

                    await Task.Run(() => {
                        while (gameView.HasMoveText || gameView.IsUpdating) { }
                    });

                    this.gameObject.SetActive(true);

                    gameView.mPauseButton.interactable = false;
                    gameView.tileSelector.gameObject.SetActive(false);
                    gameView.moveSelector.gameObject.SetActive(false);
                    gameView.LockCamera();
                    gameViewModel.PauseGame();


                    unitNameLabel.text = unitViewModel.Unit.Name;

                    lvLabel.text = unitViewModel.Level.ToString();
                    expLabel.text = unitViewModel.ExperiencePoints.ToString();

                    maxHpLabel.text = unitViewModel.Health.ToString();
                    strengthLabel.text = unitViewModel.Strength.ToString();
                    magicLabel.text = unitViewModel.Magic.ToString();
                    defenseLabel.text = unitViewModel.Defense.ToString();
                    resistanceLabel.text = unitViewModel.Resistance.ToString();
                    speedLabel.text = unitViewModel.Speed.ToString();
                    skillLabel.text = unitViewModel.Skill.ToString();
                    luckLabel.text = unitViewModel.Luck.ToString();

                    unitViewModel.ResetStats();
                }
            }
        }
    }
}

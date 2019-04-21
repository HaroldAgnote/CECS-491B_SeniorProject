using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Application;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Model;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class GameOverScreen : MonoBehaviour {
        public GameView gameView;

        public TextMeshProUGUI gameOverLabel;

        public Button actionButton;
        public Button restartButton;
        public Button quitButton;

        private GameViewModel gameViewModel;

        public void ConstructGameOverScreen() {
            gameViewModel = gameView.gameViewModel;
            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;

            this.gameObject.SetActive(false);

            if (GameManager.instance.singleplayerGameType == GameManager.SingleplayerGameType.Practice) {
                actionButton.gameObject.SetActive(false);
            }

            restartButton.onClick.AddListener(ReloadMap);
            quitButton.onClick.AddListener(QuitGame);
        }

        private async void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "IsGameOver") {
                if (gameViewModel.IsGameOver) {

                    await Task.Run(() => {
                        while (gameView.HasMoveText || gameView.IsUpdating || gameView.HasScreenOverlay) { }
                    });

                    this.gameObject.SetActive(true);
                    if (gameViewModel.ControllingPlayer.HasAliveUnit()) {
                        gameOverLabel.text = "Map Cleared!";
                        actionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
                        actionButton.onClick.AddListener(NextAction);

                    } else {
                        gameOverLabel.text = "Game Over";
                        actionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Back";
                        actionButton.onClick.AddListener(GoBack);
                    }
                }
            }
        }

        private void NextAction() {
            gameViewModel.FinishGame();
        }

        private void ReloadMap() {
            GameManager.instance.RestartGame();
        }

        private void GoBack() {
            SceneLoader.instance.GoToLastMenu();
        }

        private void QuitGame() {
            SceneLoader.instance.GoToMainMenu();
        }
    }
}

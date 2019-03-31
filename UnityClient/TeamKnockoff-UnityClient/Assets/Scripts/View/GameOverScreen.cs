using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Model;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class GameOverScreen : MonoBehaviour {
        public GameView gameView;

        public TextMeshProUGUI gameOverLabel;

        public Button nextButton;

        private GameViewModel gameViewModel;

        public void ConstructGameOverScreen() {
            gameViewModel = gameView.gameViewModel;
            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
            nextButton.onClick.AddListener(NextAction);
        }

        private void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "IsGameOver") {
                if (gameViewModel.IsGameOver) {
                    this.gameObject.SetActive(true);
                    if (gameViewModel.ControllingPlayer.HasAliveUnit()) {
                        gameOverLabel.text = "Map Cleared!";
                    } else {
                        gameOverLabel.text = "Game Over";
                    }
                }
            }
        }

        private void NextAction() {
            gameViewModel.FinishGame();
        }
    }
}

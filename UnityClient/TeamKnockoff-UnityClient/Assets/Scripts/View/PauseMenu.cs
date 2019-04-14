using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class PauseMenu : MonoBehaviour {

        public GameView gameView;

        public Button mResumeButton;
        public Button mRestartButton;
        public Button mQuitButton;

        public void ConstructPauseMenu() {
            mResumeButton.onClick.AddListener(ResumeGame);
            mRestartButton.onClick.AddListener(ReloadMap);
            mQuitButton.onClick.AddListener(QuitGame);
        }

        private void ResumeGame() {
            gameView.UnpauseGame();
        }

        private void ReloadMap() {
            GameManager.instance.RestartGame();
        }

        private void QuitGame() {
            SceneLoader.instance.GoToMainMenu();
        }

    }
}

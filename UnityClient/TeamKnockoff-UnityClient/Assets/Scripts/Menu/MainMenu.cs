using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Menu {
    public class MainMenu : MonoBehaviour {
        public Button singlePlayerButton;
        public Button multiPlayerButton;
        public Button settingsButton;
        public Button quitButton;

        void Start() {
            var sceneLoader = SceneLoader.instance;
            singlePlayerButton.onClick.AddListener(sceneLoader.GoToSingleplayerMenu);
            multiPlayerButton.onClick.AddListener(sceneLoader.GoToMultiplayerMenu);
            settingsButton.onClick.AddListener(sceneLoader.GoToSettingsMenu);
            quitButton.onClick.AddListener(sceneLoader.QuitGame);
        }
    }
}

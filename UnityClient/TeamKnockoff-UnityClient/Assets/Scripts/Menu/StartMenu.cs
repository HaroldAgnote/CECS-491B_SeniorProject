using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Application;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu {
    public class StartMenu : MonoBehaviour {

        public Button startButton;
        public Button creditsButton;
        public Button quitButton;

        // Start is called before the first frame update
        void Start() {
            // Disabling this since only singleplayer is functioning
            // startButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);
            startButton.onClick.AddListener(SceneLoader.instance.GoToSingleplayerMenu);
            creditsButton.onClick.AddListener(SceneLoader.instance.GoToCreditsScreen);
            quitButton.onClick.AddListener(SceneLoader.instance.QuitGame);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

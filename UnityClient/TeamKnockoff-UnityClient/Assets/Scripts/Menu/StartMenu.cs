using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Application;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu {
    public class StartMenu : MonoBehaviour {

        public Button startButton;
        public Button quitButton;

        // Start is called before the first frame update
        void Start() {
            startButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);
            quitButton.onClick.AddListener(SceneLoader.instance.QuitGame);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

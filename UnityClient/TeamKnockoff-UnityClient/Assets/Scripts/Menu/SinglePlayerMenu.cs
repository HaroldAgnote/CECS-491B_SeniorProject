using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Menu {
    public class SinglePlayerMenu : MonoBehaviour {
        public Button[] mainMenuButtons;
        public Button campaignButton;
        public Button practiceButton;

        void Start() {
            var sceneLoader = SceneLoader.instance;
            foreach (var mainMenuButton in mainMenuButtons) {
                mainMenuButton.onClick.AddListener(sceneLoader.GoToMainMenu);
            }
            campaignButton.onClick.AddListener(sceneLoader.GoToCampaignMenu);
            practiceButton.onClick.AddListener(sceneLoader.GoToPracticeMenu);
        }
    }
}

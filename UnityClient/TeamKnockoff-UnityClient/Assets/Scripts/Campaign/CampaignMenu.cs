using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Campaign {
    public class CampaignMenu : MonoBehaviour {
        public Button[] singlePlayerButton;
        public Button newCampaignButton;
        public Button continueCampaignButton;
        public Button mainMenuButton;

        void Start() {
            var sceneLoader = SceneLoader.instance;
            foreach (var button in singlePlayerButton) {
                button.onClick.AddListener(sceneLoader.GoToSingleplayerMenu);
            }
            mainMenuButton.onClick.AddListener(sceneLoader.GoToMainMenu);

            continueCampaignButton.onClick.AddListener(() => {
                SceneLoader.instance.GoToCampaignLoadMenu();
            });

            newCampaignButton.onClick.AddListener(() => {
                SceneLoader.instance.GoToCampaignNewStoryMenu();
            });
        }
    }
}

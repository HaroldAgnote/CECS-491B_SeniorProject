using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;
using Assets.Scripts.Campaign;

namespace Assets.Scripts.Menu {
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

            newCampaignButton.onClick.AddListener(() => {
                CampaignManager.instance.RestartCampaign();
                CampaignManager.instance.LoadNextMap();
            });

            continueCampaignButton.onClick.AddListener(() => {
                CampaignManager.instance.LoadNextMap();
            });
        }
    }
}

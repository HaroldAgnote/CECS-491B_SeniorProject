using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Campaign {
    public class CampaignNewStoryMenu : MonoBehaviour {
        public Dropdown campaignMenuDropdown;
        public Button[] singlePlayerButtons;
        public Button mainMenuButton;
        public Button goButton;

        public CampaignSequence selectedCampaignSequence;

        void Start() {
            foreach (var singlePlayerButton in singlePlayerButtons) {
                singlePlayerButton.onClick.AddListener(SceneLoader.instance.GoToSingleplayerMenu);
            }

            mainMenuButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);
            campaignMenuDropdown.ClearOptions();

            var campaigns = CampaignManager.instance.availableCampaigns.Select(campaign => campaign.campaignName) .ToList();
            selectedCampaignSequence = CampaignManager.instance.availableCampaigns.First();

            campaignMenuDropdown.AddOptions(campaigns);
            campaignMenuDropdown.onValueChanged.AddListener(delegate {
                UpdateSelectedCampaign();
            });

            goButton.onClick.AddListener(StartCampaign);
        }

        private void UpdateSelectedCampaign() {
            var selectedIndex = campaignMenuDropdown.value;
            var selectedCampaign = campaignMenuDropdown.options[selectedIndex].text;
            selectedCampaignSequence = CampaignManager.instance.NamesToCampaignSequences[selectedCampaign];
        }

        private void StartCampaign() {
            CampaignManager.instance.StartNewCampaign(selectedCampaignSequence);
        }
    }
}

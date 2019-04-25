using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

using CampaignDifficulty = Assets.Scripts.Campaign.CampaignManager.CampaignDifficulty;

namespace Assets.Scripts.Campaign {
    public class CampaignNewStoryMenu : MonoBehaviour {
        public Dropdown campaignMenuDropdown;
        public Dropdown campaignDifficultyDropdown;

        public Button[] singlePlayerButtons;
        public Button mainMenuButton;
        public Button goButton;

        public CampaignSequence selectedCampaignSequence;
        public CampaignDifficulty selectedCampaignDifficulty;

        void Start() {
            foreach (var singlePlayerButton in singlePlayerButtons) {
                singlePlayerButton.onClick.AddListener(SceneLoader.instance.GoToSingleplayerMenu);
            }

            mainMenuButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);
            campaignMenuDropdown.ClearOptions();

            var campaigns = CampaignManager.instance.availableCampaigns.Select(campaign => campaign.campaignName).ToList();

            selectedCampaignSequence = CampaignManager.instance.availableCampaigns.First();
            selectedCampaignDifficulty = CampaignDifficulty.Normal;

            campaignMenuDropdown.AddOptions(campaigns);
            campaignMenuDropdown.onValueChanged.AddListener(delegate {
                UpdateSelectedCampaign();
            });

            campaignDifficultyDropdown.ClearOptions();
            campaignDifficultyDropdown.AddOptions(Enum.GetNames(typeof(CampaignDifficulty)).ToList());

            campaignDifficultyDropdown.onValueChanged.AddListener(delegate {
                UpdateSelectedDifficulty();
            });

            goButton.onClick.AddListener(StartCampaign);
        }

        private void UpdateSelectedCampaign() {
            var selectedIndex = campaignMenuDropdown.value;
            var selectedCampaign = campaignMenuDropdown.options[selectedIndex].text;
            selectedCampaignSequence = CampaignManager.instance.NamesToCampaignSequences[selectedCampaign];
        }

        private void UpdateSelectedDifficulty() {
            var selectedIndex = campaignDifficultyDropdown.value;
            switch (selectedIndex) {
                case 0:
                    selectedCampaignDifficulty = CampaignDifficulty.Easy;
                    break;
                case 1:
                    selectedCampaignDifficulty = CampaignDifficulty.Normal;
                    break;
                case 2:
                    selectedCampaignDifficulty = CampaignDifficulty.Hard;
                    break;
                case 3:
                    selectedCampaignDifficulty = CampaignDifficulty.Lunatic;
                    break;
            }
        }

        private void StartCampaign() {
            CampaignManager.instance.StartNewCampaign(selectedCampaignSequence, selectedCampaignDifficulty);
        }
    }
}

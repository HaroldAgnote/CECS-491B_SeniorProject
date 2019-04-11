using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Campaign {
    public class CampaignChapterMenu : MonoBehaviour {
        public Dropdown campaignChapterMenuDropdown;
        public Button[] singlePlayerButtons;
        public Button mainMenuButton;
        public Button goButton;
        public Button saveMenuButton;
        public Button equipmentMenuButton;
        public Button shopMenuButton;

        private int selectedIndex;

        void Start() {
            foreach (var singlePlayerButton in singlePlayerButtons) {
                singlePlayerButton.onClick.AddListener(SceneLoader.instance.GoToSingleplayerMenu);
            }
            mainMenuButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);

            campaignChapterMenuDropdown.ClearOptions();
            var campaignChapters = CampaignManager.instance.CurrentCampaignSequence.chapterNames
                .Take(CampaignManager.instance.FarthestCampaignIndex + 1).ToList();
            campaignChapterMenuDropdown.AddOptions(campaignChapters);

            selectedIndex = CampaignManager.instance.CurrentCampaignIndex;

            campaignChapterMenuDropdown.value = CampaignManager.instance.CurrentCampaignIndex;

            campaignChapterMenuDropdown.onValueChanged.AddListener(delegate {
                UpdateSelectedCampaignChapter();
            });

            goButton.onClick.AddListener(LoadCampaignChapter);

            saveMenuButton.onClick.AddListener(CampaignManager.instance.LoadCampaignSaveMenu);
            equipmentMenuButton.onClick.AddListener(CampaignManager.instance.LoadCampaignEquipmentMenu);
            shopMenuButton.onClick.AddListener(CampaignManager.instance.LoadCampaignShopMenu);
        }

        private void UpdateSelectedCampaignChapter() {
            selectedIndex = campaignChapterMenuDropdown.value;

            // TODO: Add/Update Chapter description here?
        }

        private void LoadCampaignChapter() {
            CampaignManager.instance.CurrentCampaignIndex = selectedIndex;
            CampaignManager.instance.LoadNextCampaignEvent();
        }
    }
}

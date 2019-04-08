using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Campaign {
    public class CampaignLoadMenu : MonoBehaviour {
        public GameObject campaignLoadSlotPrefab;
        public GameObject campaignLoadSlotContent;
        public Button singlePlayerButton;
        public Button mainMenuButton;
        public Button campaignCancelButton;
        public Button campaignActionButton;

        private CampaignData selectedSaveSlot;
        private GameObject selectedGameObject;

        private List<GameObject> saveSlots;

        private Color SELECTED_COLOR = Color.blue;
        private Color UNSELECTED_COLOR = Color.white;

        void Start() {

            singlePlayerButton.onClick.AddListener(SceneLoader.instance.GoToSingleplayerMenu);
            mainMenuButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);

            saveSlots = new List<GameObject>();

            campaignActionButton.interactable = false;

            var allSlots = CampaignManager.instance.CampaignDataSlots
                                .OrderByDescending(slot => slot.TimeStamp);

            if (allSlots.Count() == 0) {
                var button = CreateButton("Start New Story...");
                button.onClick.AddListener(() => {
                    selectedGameObject = button.gameObject;
                    var buttonImage = selectedGameObject.GetComponent<Image>();
                    buttonImage.color = SELECTED_COLOR;
                    campaignActionButton.interactable = true;
                });
                var actionLabel = campaignActionButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                actionLabel.text = "Start";
                campaignActionButton.onClick.AddListener(() => {
                    SceneLoader.instance.GoToCampaignNewStoryMenu();
                });
            } else {
                foreach (var slot in allSlots) {
                    var button = CreateButton(CampaignManager.DataToString(slot));
                    button.onClick.AddListener(() => {
                        if (selectedGameObject != null) {
                            var selectedButton = selectedGameObject.GetComponent<Image>();
                            selectedButton.color = UNSELECTED_COLOR;
                        }
                        selectedSaveSlot = slot;
                        selectedGameObject = button.gameObject;
                        var buttonImage = selectedGameObject.GetComponent<Image>();
                        buttonImage.color = SELECTED_COLOR;
                        campaignActionButton.interactable = true;
                    });
                }

                campaignActionButton.onClick.AddListener(LoadButton);
            }
            campaignCancelButton.onClick.AddListener(() => {
                SceneLoader.instance.GoToLastMenu();
            });
        }

        private Button CreateButton(string label) {
            var buttonObject = Instantiate(campaignLoadSlotPrefab, campaignLoadSlotContent.transform);
            saveSlots.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            buttonLabel.text = label;

            var button = buttonObject.GetComponent<Button>();
            return button;
        }

        private void LoadButton() {
            CampaignManager.instance.LoadCampaign(selectedSaveSlot);
        }
    }
}

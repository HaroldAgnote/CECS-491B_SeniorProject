using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Campaign {
    public class CampaignSaveMenu : MonoBehaviour {
        public GameObject campaignSaveSlotPrefab;
        public GameObject campaignSaveSlotContent;
        public Button campaignCreateNewDataButton;
        public Button campaignSaveButton;
        public Button campaignContinueButton;

        private CampaignData selectedSaveSlot;
        private GameObject selectedGameObject;

        private List<GameObject> saveSlots;

        private Color SELECTED_COLOR = Color.blue;
        private Color UNSELECTED_COLOR = Color.white;

        private bool dataSaved;

        void Start() {
            saveSlots = new List<GameObject>();

            saveSlots.Add(campaignCreateNewDataButton.gameObject);

            campaignSaveButton.interactable = false;

            campaignCreateNewDataButton.onClick.AddListener(() => {
                if (selectedGameObject != null) {
                    var selectedButton = selectedGameObject.GetComponent<Image>();
                    selectedButton.color = UNSELECTED_COLOR;
                }
                selectedSaveSlot = null;
                selectedGameObject = campaignCreateNewDataButton.gameObject;
                var buttonImage = selectedGameObject.GetComponent<Image>();
                buttonImage.color = SELECTED_COLOR;
                campaignSaveButton.interactable = true;
            });
    
            var allSlots = CampaignManager.instance.CampaignDataSlots
                                .OrderByDescending(slot => slot.TimeStamp);

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
                    campaignSaveButton.interactable = true;
                });
            }

            campaignSaveButton.onClick.AddListener(SaveButton);
            campaignContinueButton.onClick.AddListener(ContinueButton);
        }

        private Button CreateButton(string label) {
            var buttonObject = Instantiate(campaignSaveSlotPrefab, campaignSaveSlotContent.transform);
            saveSlots.Add(buttonObject);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            buttonLabel.text = label;

            var button = buttonObject.GetComponent<Button>();
            return button;
        }

        private void SaveButton() {
            CampaignData newData = null;
            if (selectedSaveSlot == null) {
                newData = CampaignManager.instance.SaveNewCampaign();
            } else {
                newData = CampaignManager.instance.SaveCampaign(selectedSaveSlot);
            }

            if (newData != null) {
                var newLabel = selectedGameObject.GetComponentInChildren<TextMeshProUGUI>();
                newLabel.text = CampaignManager.DataToString(newData);
                dataSaved = true;
            }
        }

        private void ContinueButton() {
            if (dataSaved) {
                CampaignManager.instance.LoadNextCampaignEvent();
            }
            else {
                Debug.Log("Data not saved!");
                // TODO: Create confirm window and remove this line
                CampaignManager.instance.LoadNextCampaignEvent();
            }
        }
    }
}

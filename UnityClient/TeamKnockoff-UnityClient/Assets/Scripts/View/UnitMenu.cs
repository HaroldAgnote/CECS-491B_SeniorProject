using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View {
    public class UnitMenu : MonoBehaviour {
        public GameView gameView;

        public GameObject unitMenuButtonPrefab;
        public GameObject unitMainMenuContent;
        public GameObject unitSubMenuContent;

        public Button mBackButton;

        private List<GameObject> mainMenu;
        private List<GameObject> subMenu;

        private bool isSubMenu;

        public static SortedDictionary<int, int> CHARACTER_COUNT_TO_FONT_SIZE;

        void Start() {
            mainMenu = new List<GameObject>();
            subMenu = new List<GameObject>();

            isSubMenu = false;

            CHARACTER_COUNT_TO_FONT_SIZE = new SortedDictionary<int, int>() {
                {10, 32 },
                {15, 24 },
                {20, 20 }
            };
        }

        public void SetActive(bool value) {
            this.gameObject.SetActive(value);
        }

        public void CreateMainMenu() {
            foreach (var obj in mainMenu) {
                Destroy(obj);
            }

            mainMenu = new List<GameObject>();
        }

        public void CreateSubMenu() {
            foreach (var obj in subMenu) {
                Destroy(obj);
            }

            subMenu = new List<GameObject>();
        }

        public void SwitchtoMainMenu() {

            isSubMenu = false;
            unitMainMenuContent.SetActive(true);
            unitSubMenuContent.SetActive(false);
        }

        public void SwitchtoSubMenu() {
            isSubMenu = true;

            mBackButton = CreateButton("BACK");
            mBackButton.onClick.AddListener(SwitchtoMainMenu);
            CreateButton("").interactable = false;

            unitMainMenuContent.SetActive(false);
            unitSubMenuContent.SetActive(true);
        }

        public Button CreateButton(string newButtonLabel) {
            var parent = (isSubMenu) ? unitSubMenuContent.transform : unitMainMenuContent.transform;
            var buttonObject = Instantiate(unitMenuButtonPrefab, parent);

            if (isSubMenu) {
                subMenu.Add(buttonObject);
            } else {
                mainMenu.Add(buttonObject);
            }

            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();

            foreach (var characterCount in CHARACTER_COUNT_TO_FONT_SIZE) {
                if (newButtonLabel.Length > characterCount.Key) {
                    continue;
                }

                buttonLabel.fontSize = characterCount.Value;
                break;

            }

            buttonLabel.text = newButtonLabel.ToUpper();

            var button = buttonObject.GetComponent<Button>();
            return button;
        }

    }
}

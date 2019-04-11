using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Application {
    public class PracticeMenu : MonoBehaviour {
        public Dropdown mapMenuDropdown;

        public Button[] singlePlayerButtons;
        public Button goButton;
        public Button mainMenuButton;

        private void Start() {
            foreach (var singlePlayerButton in singlePlayerButtons) {
                singlePlayerButton.onClick.AddListener(SceneLoader.instance.GoToSingleplayerMenu);
            }

            goButton.onClick.AddListener(GoToGame);
            mainMenuButton.onClick.AddListener(SceneLoader.instance.GoToMainMenu);

            mapMenuDropdown.ClearOptions();

            var maps = MapLoader.instance.availableMaps.Select(map => map.name).ToList();
            SceneLoader.SetParam(SceneLoader.LOAD_MAP_PARAM, maps.First());

            mapMenuDropdown.AddOptions(maps);
            mapMenuDropdown.onValueChanged.AddListener(delegate {
                UpdateSelectedMap();
            });
        }

        private void UpdateSelectedMap() {
            var selectedIndex = mapMenuDropdown.value;
            var selectedMap = mapMenuDropdown.options[selectedIndex].text;
            SceneLoader.SetParam(SceneLoader.LOAD_MAP_PARAM, selectedMap);
        }

        public void GoToGame() {
            SceneLoader.SetParam(SceneLoader.GAME_TYPE_PARAM, GameManager.SINGLEPLAYER_GAME_TYPE);
            SceneLoader.SetParam(SceneLoader.SINGLEPLAYER_GAME_TYPE_PARAM, GameManager.PRACTICE_GAME_TYPE);
            SceneLoader.instance.GoToMap();
        }
    }
}

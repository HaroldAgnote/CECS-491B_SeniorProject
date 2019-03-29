using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Application {
    public class PracticeMenu : MonoBehaviour {
        Dropdown mDropdown;

        private void Start() {
            mDropdown = GetComponent<Dropdown>();
            mDropdown.ClearOptions();

            var maps = MapLoader.instance.availableMaps.Select(map => map.name).ToList();

            mDropdown.AddOptions(maps);
            mDropdown.onValueChanged.AddListener(delegate {
                UpdateSelectedMap();
            });
        }

        private void UpdateSelectedMap() {
            var selectedIndex = mDropdown.value;
            var selectedMap = mDropdown.options[selectedIndex].text;
            SceneLoader.SetParam(SceneLoader.LOAD_MAP_PARAM, selectedMap);
        }

        public void GoToGame() {
            SceneLoader.instance.GoToMap();
        }
    }
}

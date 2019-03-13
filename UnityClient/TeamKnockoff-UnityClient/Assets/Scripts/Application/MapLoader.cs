using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Scripts.Application {
    public class MapLoader : MonoBehaviour
    {
        const int MAP_OFFSET = 7;
        int map_index = 0; 
        // Debug.Log("QUIT!");
        Dropdown m_Dropdown;

        private void Start()
        {
            m_Dropdown = GetComponent<Dropdown>();
            m_Dropdown.onValueChanged.AddListener(delegate {
                UpdateMapIndex(m_Dropdown);
            });
        }

        void UpdateMapIndex(Dropdown change)
        {
            map_index = change.value + MAP_OFFSET;
        }

        public void GoToMap()
        {
            SceneManager.LoadScene(map_index);
        }
    }
}

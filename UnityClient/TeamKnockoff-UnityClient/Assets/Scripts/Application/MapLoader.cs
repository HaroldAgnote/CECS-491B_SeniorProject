using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Scripts.Application {
    public class MapLoader : MonoBehaviour {

        public static MapLoader instance;

        public List<TextAsset> availableMaps;

        private Dictionary<string, TextAsset> stringsToMapAsset;

        private const int GAME_SCENE_INDEX = 6;


        private void Awake() {
            //Check if instance already exists
            if (instance == null) {
                //if not, set instance to this
                instance = this;
            }

            //If instance already exists and it's not this:
            else if (instance != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }


            DontDestroyOnLoad(this.gameObject);
        }

        private void Start() {
            
            stringsToMapAsset = new Dictionary<string, TextAsset>();

            foreach (var map in availableMaps) {
                stringsToMapAsset.Add(map.name, map);
            }
        }

        public TextAsset GetMapAsset(string mapName) {
            return stringsToMapAsset[mapName];
        }
    }
}

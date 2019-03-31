using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Application {
    public class SceneLoader : MonoBehaviour {

        public static SceneLoader instance;

        public const string LOAD_MAP_PARAM = "LoadMap";

        const int MAIN_MENU_INDEX = 0;
        const int SINGLEPLAYER_MENU_INDEX = 1;
        const int CAMPAIGN_MENU_INDEX = 2;
        const int MULTIPLAYER_MENU_INDEX = 3;
        const int PRACTICE_MENU_INDEX = 4;
        const int SETTINGS_MENU_INDEX = 5;
        const int GAME_INDEX = 6;

        private Dictionary<string, string> parameters;

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

        public static void Load(string sceneName, Dictionary<string, string> parameters = null) {
            instance.parameters = parameters;
            SceneManager.LoadScene(sceneName);
        }

        public static void Load(string sceneName, string paramKey, string paramValue) {
            instance.parameters = new Dictionary<string, string>();
            instance.parameters.Add(paramKey, paramValue);
            SceneManager.LoadScene(sceneName);
        }

        public static void Load(int sceneIndex, Dictionary<string, string> parameters = null) {
            instance.parameters = parameters;
            SceneManager.LoadScene(sceneIndex);
        }

        public static void Load(int sceneIndex, string paramKey, string paramValue) {
            instance.parameters = new Dictionary<string, string>();
            instance.parameters.Add(paramKey, paramValue);
            SceneManager.LoadScene(sceneIndex);
        }

        public static Dictionary<string, string> getSceneParameters() {
            return instance.parameters;
        }

        public static string GetParam(string paramKey) {
            if (instance == null || instance.parameters == null) {
                return "";
            }

            return instance.parameters[paramKey];
        }

        public static void SetParam(string paramKey, string paramValue) {
            if (instance.parameters == null) {
                instance.parameters = new Dictionary<string, string>();
            }
            if (instance.parameters.ContainsKey(paramKey)) {
                instance.parameters[paramKey] = paramValue;
            } else {
                instance.parameters.Add(paramKey, paramValue);
            }
        }

        public void GoToSettingsMenu()
        {
            Load(SETTINGS_MENU_INDEX);
        }

        public void GoToPracticeMenu()
        {
            Load(PRACTICE_MENU_INDEX);
        }

        public void GoToMainMenu() {
            Load(MAIN_MENU_INDEX);
        }

        public void GoToSingleplayerMenu() {
            Load(SINGLEPLAYER_MENU_INDEX);
        }

        public void GoToCampaignMenu() {
            Load(CAMPAIGN_MENU_INDEX);
        }

        public void GoToMultiplayerMenu() {
            Load(MULTIPLAYER_MENU_INDEX);
        }

        public void GoToMap() {
            Load(GAME_INDEX, instance.parameters);
        }

        public void QuitGame () {
            UnityEngine.Application.Quit();
        }
    }
}

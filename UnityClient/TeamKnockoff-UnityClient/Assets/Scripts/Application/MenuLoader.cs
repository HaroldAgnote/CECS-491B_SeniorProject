using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Application {
    public class MenuLoader : MonoBehaviour {
        const int MAIN_MENU_INDEX = 0;
        const int SINGLEPLAYER_MENU_INDEX = 1;
        const int CAMPAIGN_MENU_INDEX = 2;
        const int MULTIPLAYER_MENU_INDEX = 3;
        const int PRACTICE_MENU_INDEX = 4;
        const int SETTINGS_MENU_INDEX = 5;

        public void GoToMenu(int menuIndex) {
            SceneManager.LoadScene(menuIndex);
        }

        public void GoToSettingsMenu()
        {
            GoToMenu(SETTINGS_MENU_INDEX);
        }

        public void GoToPracticeMenu()
        {
            GoToMenu(PRACTICE_MENU_INDEX);
        }

        public void GoToMainMenu() {
            GoToMenu(MAIN_MENU_INDEX);
        }

        public void GoToSingleplayerMenu() {
            GoToMenu(SINGLEPLAYER_MENU_INDEX);
        }

        public void GoToCampaignMenu() {
            GoToMenu(CAMPAIGN_MENU_INDEX);
        }

        public void GoToMultiplayerMenu() {
            GoToMenu(MULTIPLAYER_MENU_INDEX);
        }

        public void QuitGame ()
        {
            Debug.Log("QUIT!");
            UnityEngine.Application.Quit();
        }
    }
}

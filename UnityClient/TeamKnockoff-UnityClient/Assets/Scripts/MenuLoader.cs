using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    const int MAIN_MENU_INDEX = 0;
    const int SINGLEPLAYER_MENU_INDEX = 1;
    const int CAMPAIGN_MENU_INDEX = 2;
    const int MULTIPLAYER_MENU_INDEX = 3;
    const int GAME_INDEX = 4;

    public void GoToMenu(int menuIndex) {
        SceneManager.LoadScene(menuIndex);
    }

    public void GoToGame() {
        GoToMenu(GAME_INDEX);
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
        Application.Quit();
    }
}

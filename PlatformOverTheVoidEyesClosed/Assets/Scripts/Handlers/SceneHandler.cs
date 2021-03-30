using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private UISFXHandler uiSFX;

    private void OnEnable()
    {
        uiSFX = GameObject.Find("UISFX").GetComponent<UISFXHandler>();
    }

    public enum Scene { 
        MainMenu, LevelSelect, Loading, Level, JessScene
    }
    public void ExitGame() {
        Debug.Log("Game Exited.");
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
        Application.Quit();
    }

    public void GoToLevelSelect() {
        SceneManager.LoadScene(Scene.LevelSelect.ToString());
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
    }

    public void GoToLevel(int levelIndex) {
        SceneManager.LoadScene(Scene.Level.ToString() + levelIndex.ToString());
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
    }

    public void GoToLevel(Scene s) {
        SceneManager.LoadScene(s.ToString());
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
    }
}

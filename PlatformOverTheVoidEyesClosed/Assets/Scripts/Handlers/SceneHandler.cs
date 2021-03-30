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
        Time.timeScale = 1f;
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
        SceneManager.LoadScene(Scene.LevelSelect.ToString());
    }

    public void GoToMainMenu() {
        Time.timeScale = 1f;
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

    public void GoToLevel(int levelIndex) {
        Time.timeScale = 1f;
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
        SceneManager.LoadScene(Scene.Level.ToString() + levelIndex.ToString());
    }

    public void GoToLevel(Scene s) {
        Time.timeScale = 1f;
        if (uiSFX != null)
            uiSFX.PlayButtonClick();
        SceneManager.LoadScene(s.ToString());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public enum Scene { 
        MainMenu, LevelSelect, Loading, Level, JessScene
    }
    public void ExitGame() {
        Debug.Log("Game Exited.");
        Application.Quit();
    }

    public void GoToLevelSelect() {
        SceneManager.LoadScene(Scene.LevelSelect.ToString());
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene(Scene.MainMenu.ToString());
    }

    public void GoToLevel(int levelIndex) {
        SceneManager.LoadScene(Scene.Level.ToString() + levelIndex.ToString());
    }

    public void GoToLevel(Scene s) {
        SceneManager.LoadScene(s.ToString());
    }
}

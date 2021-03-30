using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/* THINGS TO ASSIGN IN A NEW SCENE:
 * eyesOpenCloseText: PauseMenu->EyesOpen->Button->Text(TMP)
 * Settings Open on Pause: PauseMenu->Settings->OnClick()->Handlers+OpenCloseSettingsMenu, SaveSystem+SaveVolumes
 * Save & Exit Settings: SettingsMenu->Save->OnClick()->Handlers+OpenCloseSettingsMenu, SaveSystem+SaveVolumes
 * BackToLevelSelect: PauseMenu->BackToLevelSelect->OnClick()->Handlers+GoToLevelSelect
 * BackToMain: PauseMenu->BackToMainMenu->OnClick()->Handlers+GoToMain
 */
public class MenuHandler : MonoBehaviour
{
    public const int MAXLEVELINDEX = 4;
    public bool inLevel = true;
    #region Menus
    [SerializeField] private GameObject Settings;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject Blindfold;
    [SerializeField] private GameObject GoalPanel;
    #endregion
    #region Affected objects
    [SerializeField] private PlayerMovement player;
    [SerializeField] private TextMeshProUGUI eyesOpenCloseText;
    [SerializeField] private GameObject nextLevelButtonGoal;
    [SerializeField] private TextMeshProUGUI timerTextGoal;
    #endregion
    
    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += CheckForPauseOnUpdate;
        
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= CheckForPauseOnUpdate;
    }

    public void CheckForPauseOnUpdate() {
        if (Input.GetButtonUp("Cancel") && inLevel)
            OpenClosePauseMenu();
    }

    public void EnableDisableBlindfoldInPause() {
        if (Blindfold.activeSelf) {
            Blindfold.SetActive(false);
            if (eyesOpenCloseText != null)
                eyesOpenCloseText.text = "Open";
            else
                Debug.Log("Hey you! Yeah, you! Just and/or Jess!! Assign the EyesOpen's button's Text(TMP) object to the 'Eyes Open Close Text' field!");
        }
        else {
            Blindfold.SetActive(true);
            if (eyesOpenCloseText != null)
                eyesOpenCloseText.text = "Closed";
            else
                Debug.Log("Hey you! Yeah, you! Just and/or Jess!! Assign the EyesOpen's button's Text(TMP) object to the 'Eyes Open Close Text' field!");
        }
    }

    public void OpenClosePauseMenu() {
        if (PauseMenu != null) {
            if (PauseMenu.activeSelf)
            {
                if (!GoalPanel.activeSelf)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    player.MouseLookEnabled = true;
                    Time.timeScale = 1.0f;
                }
                if (Settings.activeSelf) {
                    Settings.SetActive(false);
                }
                PauseMenu.SetActive(false);
            }
            else {
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
                player.MouseLookEnabled = false;
                PauseMenu.SetActive(true);
            }
        }
    }

    public void OpenCloseSettingsMenu() {
        if (Settings.activeSelf) {
            Settings.SetActive(false);
        }
        else {
            Settings.SetActive(true);
        }
    }

    public void OpenGoalPanel() { // Should be called ONLY on level completion
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        LevelInformation info = GetComponent<LevelInformation>();
        timerTextGoal.text = info.SetTimerValueOnComplete();
        LevelHandler.RequestUpdate(info.LevelIndex, info.Timer);
        if (info.LevelIndex >= MAXLEVELINDEX && nextLevelButtonGoal.activeSelf)
            nextLevelButtonGoal.SetActive(false);
        GoalPanel.SetActive(true);
    }
}

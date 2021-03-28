using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    #region Menus
    [SerializeField] private GameObject Settings;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject Blindfold;
    #endregion
    #region Effected objects
    [SerializeField] private PlayerMovement Player;
    #endregion

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += OpenClosePauseMenu;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= OpenClosePauseMenu;
    }
    public void EnableDisableBlindfold() {
        if (Blindfold.activeSelf)
            Blindfold.SetActive(false);
        else
            Blindfold.SetActive(true);
    }

    public void OpenClosePauseMenu() {
        if (Input.GetButtonUp("Cancel")) {
            if (PauseMenu.activeSelf)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Settings.SetActive(false);
                PauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Player.MouseLookEnabled = true;
                Time.timeScale = 1.0f; 
            }
            else {
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
                Player.MouseLookEnabled = false;
                PauseMenu.SetActive(true);
            }
        
        }
        
    }

}

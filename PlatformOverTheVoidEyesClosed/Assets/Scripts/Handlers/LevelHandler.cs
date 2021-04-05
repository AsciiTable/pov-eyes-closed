using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelHandler : MonoBehaviour
{
    public List<GameObject> AllLevels;
    private List<Level> levels;
    private static bool levelUpdateRequested;
    private static int levelUpdateIndex;
    private static double levelUpdateTime;

    private void Start()
    {
        levels = new List<Level>();
        levels = SaveSystem.LoadLevels();               // Load the data
        if (levels == null || levels.Count == 0) {      // If the data doesn't exist (i.e. playing the game for the first time)
            levels = new List<Level>();                 // Init a new list of levels & data
            for (int i = 0; i < AllLevels.Count; i++)
            {
                levels.Add(new Level(i + 1, true));
            }
            UnlockLevel(1);                             // Automatically unlock the first level
            SaveSystem.SaveLevels(levels);
        }
        else {
            for (int i = 0; i < AllLevels.Count; i++) {
                if (!levels[i].locked) {
                    AllLevels[i].transform.Find("Locked").gameObject.SetActive(false);
                    AllLevels[i].GetComponent<Button>().enabled = true;
                }
            }
        }
    }

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += CheckAndUpdate;
    }

    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= CheckAndUpdate;
    }

    public void UnlockLevel(int levelIndex) {
        levels[levelIndex - 1].UpdateData(false, false, 0);
        AllLevels[levelIndex - 1].transform.Find("Locked").gameObject.SetActive(false);
        AllLevels[levelIndex - 1].GetComponent<Button>().enabled = true;
        SaveSystem.SaveLevels(levels);
    }

    public static void RequestUpdate(int ind, double time)
    {
        levelUpdateRequested = true;
        levelUpdateIndex = ind;
        levelUpdateTime = time;
    }
    public void CheckAndUpdate() {
        if (levelUpdateRequested) {
            levelUpdateRequested = false;
            if (levelUpdateIndex > 0 && levelUpdateIndex <= AllLevels.Count) {
                if (levelUpdateIndex != AllLevels.Count)
                    UnlockLevel(levelUpdateIndex + 1);
                levels[levelUpdateIndex - 1].UpdateData(true, true, levelUpdateTime);
            }
        }
    }
}

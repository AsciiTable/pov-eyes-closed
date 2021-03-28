using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHandler : MonoBehaviour
{
    public List<GameObject> AllLevels;
    private List<Level> levels;

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

    public void UnlockLevel(int levelIndex) {
        levels[levelIndex - 1].UpdateData(false, false, 0);
        AllLevels[levelIndex - 1].transform.Find("Locked").gameObject.SetActive(false);
        AllLevels[levelIndex - 1].GetComponent<Button>().enabled = true;
        SaveSystem.SaveLevels(levels);
    }
}

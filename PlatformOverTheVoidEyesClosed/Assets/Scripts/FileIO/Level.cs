using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Level
{
    public int levelIndex;
    public bool locked;
    public bool completed;
    public int highestScore;

    // Default constructor
    public Level() {
        levelIndex = 0;
        locked = true;
        completed = false;
        highestScore = 0;
    }

    // Full constructor
    public Level(int indexOfLevel, bool lockedLevel, bool completedLevel, int highestScoreAchieved) {
        levelIndex = indexOfLevel;
        locked = lockedLevel;
        completed = completedLevel;
        highestScore = highestScoreAchieved;
    }

    // New level constructor
    public Level(int indexOfLevel, bool lockedLevel) {
        levelIndex = indexOfLevel;
        locked = lockedLevel;
        completed = false;
        highestScore = 0;
    }

    public void UpdateData(bool lockedLevel, bool completedLevel, int highestScoreAchieved) {
        locked = lockedLevel;
        completed = completedLevel;
        if(highestScoreAchieved > highestScore)
            highestScore = highestScoreAchieved;
    }
}

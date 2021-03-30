using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Level
{
    public int levelIndex;
    public bool locked;
    public bool completed;
    public double lowestTime;

    // Default constructor
    public Level() {
        levelIndex = 0;
        locked = true;
        completed = false;
        lowestTime = double.MaxValue;
    }

    // Full constructor
    public Level(int indexOfLevel, bool lockedLevel, bool completedLevel, int highestScoreAchieved) {
        levelIndex = indexOfLevel;
        locked = lockedLevel;
        completed = completedLevel;
        lowestTime = highestScoreAchieved;
    }

    // New level constructor
    public Level(int indexOfLevel, bool lockedLevel) {
        levelIndex = indexOfLevel;
        locked = lockedLevel;
        completed = false;
        lowestTime = double.MaxValue;
    }

    public void UpdateData(bool lockedLevel, bool completedLevel, double time) {
        locked = lockedLevel;
        completed = completedLevel;
        if(time < lowestTime)
            lowestTime = time;
    }
}

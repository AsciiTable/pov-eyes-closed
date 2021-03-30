using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInformation : MonoBehaviour
{
    [SerializeField] private int levelIndex = 0;
    private double timer;
    public int LevelIndex { get { return levelIndex; } }
    public double Timer
    {
        get
        {
            return timer;
        }
    }

    public string SetTimerValueOnComplete() {
        timer = Time.timeSinceLevelLoadAsDouble;
        return GetTimeString();
    }

    public string GetTimeString() {
        timer = Time.timeSinceLevelLoad;
        int hours = 0;
        int minutes = 0;
        float seconds = 0;
        if (timer >= 3600)
        { // if it's been an hour +
            hours = (int)(timer % 3600);
            timer = timer - (hours * 3600);
        }
        if (timer >= 60)
        { // if it's been over a minute
            minutes = (int)(timer % 60);
            timer = timer - (minutes * 60);
        }
        seconds = (float)timer;

        if (hours == 0 && minutes == 0)
            return seconds.ToString("F2") + " s";
        else if (hours == 0 && minutes != 0)
            return minutes.ToString() + ":" + seconds.ToString("F2");
        else if (hours != 0)
            return hours.ToString() + ":" + minutes.ToString() + ":" + seconds.ToString("F2");

        return "You'll never know :)";
    }
}

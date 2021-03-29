using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitSound_Manager : MonoBehaviour
{
    public static PitSound_Manager singleton = null;

    //Music that pits will play
    AudioSource music = null;

    //Number of scripts editing this manager
    int connectedSources = 0;

    [Header("MUSIC SETTINGS")]
    [SerializeField] float maxVolume = 0.7f;
    [SerializeField] float minVolume = 0.2f;

    public void ConnectSource()
    {
        connectedSources++;
    }
    public void DisconnectSource()
    {
        if (connectedSources <= 0)
        {
            connectedSources = 0;
            return;
        }


        connectedSources--;
        if (connectedSources == 0)
        {
            SetVolume(0f);
        }
    }
    private void Start()
    {
        music = GetComponent<AudioSource>();

        SetVolume(0f);
    }

    //Sets volume using magnitude float (0.0 - 1.0)
    public void SetVolume(float m)
    {
        float magnitude = (m > 1f) ? 1f : (m < 0f) ? 0f : m;

        music.volume = minVolume + magnitude * (maxVolume - minVolume);
    }
}

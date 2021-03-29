using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager singleton = null;

    //Music that this wall will play
    AudioSource rightEar = null;
    AudioSource leftEar = null;
    //This will muffle the audio
    AudioLowPassFilter leftMuffler = null;
    AudioLowPassFilter rightMuffler = null;

    //Number of scripts editing this manager
    [SerializeField] int connectedSources = 0;

    [Header("MUSIC SETTINGS")]
    [SerializeField] float maxVolume = 0.7f;
    [SerializeField] float minVolume = 0.2f;
    [SerializeField] int minFrequency = 450;
    [SerializeField] int maxFrequency = 22000;

    private void OnEnable()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            this.enabled = false;
    }
    private void OnDisable()
    {
        if (singleton == this)
            singleton = null;
    }

    private void Start()
    {
        AudioSource[] musics = GetComponentsInChildren<AudioSource>();
        leftEar = musics[0];
        rightEar = musics[1];

        leftEar.panStereo = -1f;
        rightEar.panStereo = 1f;

        leftMuffler = leftEar.GetComponent<AudioLowPassFilter>();
        rightMuffler = rightEar.GetComponent<AudioLowPassFilter>();

        SetVolume(1f, 0f);
        SetFrequency(1f, 0f);
    }

    //Keep track of how many scripts are editing this script (reset audio if = 0)
    public void ConnectSource()
    {
        connectedSources++;
    }
    public void DisconnectSource()
    {
        if (connectedSources <= 0){
            connectedSources = 0;
            return;
        }
            

        connectedSources--;
        if(connectedSources == 0)
        {
            SetVolume(1f, 0f);
            SetFrequency(1f, 0f);
        }  
    }

    //Sets volume using magnitude float (0.0 - 1.0)
    public void SetVolume(float m, float panStereo)
    {
        float magnitude = (m > 1f) ? 1f : (m < 0f) ? 0f : m;
        float leftMag = (panStereo > 0) ? (1 - panStereo) * magnitude : 1f;
        float rightMag = (panStereo < 0) ? (1 + panStereo) * magnitude : 1f;

        leftEar.volume = minVolume + leftMag * (maxVolume - minVolume);
        rightEar.volume = minVolume + rightMag * (maxVolume - minVolume);
    }
    //Sets frequency using magnitude float (0.0 - 1.0)
    public void SetFrequency(float m, float panStereo)
    {
        float magnitude = (m > 1f) ? 1f : (m < 0f) ? 0f : m;
        float leftMag = (panStereo > 0) ? (1-panStereo) * magnitude: 1f;
            //panStereo > 0f ? panStereo * magnitude : 1f - (panStereo * magnitude);
        float rightMag = (panStereo < 0) ? (1+panStereo) * magnitude : 1f;
        //panStereo < 0f ? -panStereo * magnitude : 1f + (panStereo * magnitude);

        Debug.Log("pan: " + panStereo + " mag: " + magnitude + " right: " + rightMag);

        leftMuffler.cutoffFrequency = minFrequency + leftMag * (maxFrequency - minFrequency);
        rightMuffler.cutoffFrequency = minFrequency + rightMag * (maxFrequency - minFrequency);
    }
}

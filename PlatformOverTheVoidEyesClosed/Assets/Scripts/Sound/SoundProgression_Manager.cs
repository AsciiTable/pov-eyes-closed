using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProgression_Manager : MonoBehaviour
{
    public static SoundProgression_Manager singleton = null;

    [SerializeField] ProgressionNode[] progressionList = null;
    private int currentID = -1;
    public bool IsFinished { get => currentID == progressionList.Length - 1; }

    private AudioSource music = null;

    [Header("MUSIC SETTINGS")]
    [SerializeField] float musicVolume = 0.5f;
    public float Volume { get => musicVolume; }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }
    private void OnDisable()
    {
        singleton = null;
    }
    void Start()
    {
        music = GetComponentInChildren<AudioSource>();

        for(int i = 0; i < progressionList.Length; i++)
        {
            progressionList[i].PID = i;
        }

        Progress(-1);
    }

    public void ResetProgress()
    {
        Progress(-1);
    }
    public void Progress(int id) 
    {
        if (id >= progressionList.Length || id < -1)
            return;

        //Turn on audio for new id
        if (id < progressionList.Length - 1)
        {
            music.transform.position = progressionList[id + 1].transform.position;
            Play();
        }
        else
            Mute();

        currentID = id;
    }
    public void Mute()
    {
        music.volume = 0f;
    }
    public void Play()
    {
        music.volume = musicVolume;
    }
}

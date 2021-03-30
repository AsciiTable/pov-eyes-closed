using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProgression_Manager : MonoBehaviour
{
    public static SoundProgression_Manager singleton = null;
    [Tooltip("List of progressionNodes that player needs to pass to complete the level")]
    [SerializeField] ProgressionNode[] progressionList = null;
    private int currentID = -1;
    [Tooltip("If the player dies, progress will reset to the node of these indices")]
    [SerializeField] int[] checkpoints = new int[0];
    private int checkpointIndex = -1;
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

    public void FallDown()
    {
        if (checkpointIndex == -1)
            Progress(-1);
        else if (checkpointIndex >= progressionList.Length)
            return;
        else
        {
            Progress(checkpoints[checkpointIndex] - 1);
        }
    }
    public void Progress(int id) 
    {
        //leave if id is out of range
        if (id >= progressionList.Length || id < -1)
            return;

        //Turn on audio for new id
        if (id < progressionList.Length - 1)
        {
            music.transform.position = progressionList[id + 1].transform.position;
            progressionList[id + 1].NextNode = true;
            progressionList[id].NextNode = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundProgression_Manager : MonoBehaviour
{
    public static SoundProgression_Manager singleton = null;
    [Tooltip("List of progressionNodes that player needs to pass to complete the level")]
    [SerializeField] ProgressionNode[] progressionList = null;
    [SerializeField] private int currentID = -1;
    [Tooltip("If the player dies, progress will reset to the node of these indices")]
    [SerializeField] int[] checkpoints = new int[0];
    private int checkpointIndex = -1;

    public int CurrentID { get => currentID; }
    public bool IsFinished { get => currentID >= progressionList.Length - 1; }

    private AudioSource music = null;
    private FollowNode follow = null;

    [Header("MUSIC SETTINGS")]
    [SerializeField] float musicVolume = 0.5f;
    public float Volume { get => musicVolume; }

    private void OnEnable()
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
        if(music.GetComponent<FollowNode>() != null)
            follow = music.GetComponent<FollowNode>();

        for(int i = 0; i < progressionList.Length; i++)
        {
            progressionList[i].PID = i;
        }

        Progress(-1);
    }

    public void FallDown()
    {
        if (IsFinished || checkpointIndex < 0)
            return;
        else
        {
            Progress(checkpoints[checkpointIndex]);
        }
    }
    public void Progress(int id) 
    {
        //leave if id is out of range
        if (IsFinished || id < -1)
            return;

        //Turn on audio for new id
        if (id < progressionList.Length - 1)
        {
            music.transform.position = progressionList[id + 1].transform.position;
            progressionList[id + 1].NextNode = true;
            if(id >= 0)
                progressionList[id].NextNode = false;

            if(checkpoints.Length > 0)
            {
                checkpointIndex = 0;
                while (checkpointIndex <= checkpoints.Length - 2 && checkpoints[checkpointIndex + 1] <= id)
                    checkpointIndex++;
            }
            Play();
        }

        currentID = id;
        if (follow != null)
        {
            if (currentID == progressionList.Length - 2)
                follow.enabled = false;
            else follow.enabled = true;
        }
            


    }
    public void Mute()
    {
        music.volume = 0f;
    }
    public void Play()
    {
        music.volume = GameSettingHandler.BGMVolume * musicVolume;
    }
}

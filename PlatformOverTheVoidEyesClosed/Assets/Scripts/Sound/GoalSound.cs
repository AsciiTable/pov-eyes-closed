using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSound : MonoBehaviour
{
    //Music that pits will play
    AudioSource music = null;
    Transform playerTrans = null;

    [Header("MUSIC SETTINGS")]
    [SerializeField] float maxVolume = 0.7f;
    [SerializeField] float minVolume = 0.2f;

    [Tooltip("Angle range from left and right of player to determine when stero pan starts adjusting")]
    [SerializeField] float deltaEars = 90f;
    [Tooltip("Angle range from back of player to determine when volume starts silencing")]
    [SerializeField] float deltaBack = 90f;
    [Tooltip("Range for angle when back's magnitude become 0")]
    [SerializeField] float backSuppress = 10f;
    [Tooltip("Range for angle when ear's stereo pan maxes")]
    [SerializeField] float earSuppress = 10f;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float playerAngle = playerTrans.rotation.eulerAngles.y;

        //Adjust volume (mute if facing other direction
        if (playerAngle >= 180 - deltaBack && playerAngle <= 180 + deltaBack)
        {
            float angle = playerAngle - 180;
            float magnitude = Mathf.Abs(angle / deltaBack) - (backSuppress / deltaBack);
            music.volume = minVolume + magnitude * (maxVolume - minVolume);
        }
        else
            music.volume = maxVolume;

        //Adjust stero pan (mute right ear)
        if (playerAngle >= 90 - deltaEars && playerAngle <= 90 + deltaEars)
        {
            //60 - 120
            float angle = playerAngle - 90 - earSuppress;
            float magnitude = -1 + Mathf.Abs(angle / deltaEars);
            music.panStereo = magnitude - (earSuppress / deltaBack);
        }
        //Adjust stero pan (mute left ear)
        else if (playerAngle >= 270 - deltaEars && playerAngle <= 270 + deltaEars)
        {
            float angle = playerAngle - 270 - earSuppress;
            float magnitude = 1 - Mathf.Abs(angle / deltaEars);
            music.panStereo = magnitude + (earSuppress / deltaBack);
        }
        else
            music.panStereo = 0;

    }
}

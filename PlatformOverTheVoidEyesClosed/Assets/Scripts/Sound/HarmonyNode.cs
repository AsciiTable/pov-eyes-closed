using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonyNode : MonoBehaviour
{
    Transform camTrans = null;
    AudioSource music = null;

    [Header("MUSIC SETTINGS")]
    [SerializeField] float maxVolume = 1f;
    [SerializeField] float minVolume = 0f;
    
    [Tooltip("Angle range from left and right of player to determine when stero pan starts adjusting")]
    [SerializeField] float deltaEars = 90f;
    [Tooltip("Angle range from back of player to determine when volume starts silencing")]
    [SerializeField] float deltaBack = 170f;
    [Tooltip("Range for angle when back's magnitude become 0")]
    [SerializeField] float backSuppress = 0f;
    [Tooltip("Range for angle when ear's stereo pan maxes")]
    [SerializeField] float earSuppress = 0f;

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += DirectionalMusic;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= DirectionalMusic;
    }
    void Start()
    {
        camTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        music = GetComponent<AudioSource>();
    }

    void DirectionalMusic()
    {
        //Get absolute angle using the position and direction of the player and platform
        Vector3 targetDir = transform.position - camTrans.position;
        targetDir.y = 0;
        float angle = Vector3.Angle(targetDir, camTrans.forward);

        //Set angle from absolute to 0-360
        float rCheck = Vector3.Angle(targetDir, camTrans.right);
        float lCheck = Vector3.Angle(targetDir, -camTrans.right);

        if (lCheck > rCheck)
            angle = 360 - angle;

        //Adjust volume (mute if facing other direction
        if (angle >= 180 - deltaBack && angle <= 180 + deltaBack)
        {
            float magnitude = 0;
            if (angle < 180 - backSuppress || angle > 180 + backSuppress)
            {
                float bAngle = angle - 180;
                magnitude = Mathf.Abs(bAngle / deltaBack);
            }

            music.volume = minVolume + magnitude * (maxVolume - minVolume);
        }
        else
            music.volume = maxVolume;

        //Adjust stero pan (mute right ear)
        if (angle >= 90 - deltaEars && angle <= 90 + deltaEars)
        {
            //60 - 120
            float magnitude = -1;
            if (angle < 90 - earSuppress || angle > 90 + earSuppress)
            {
                float rAngle = angle - 90 - earSuppress;
                magnitude += Mathf.Abs(rAngle / deltaEars);
            }

            music.panStereo = magnitude;
        }
        //Adjust stero pan (mute left ear)
        else if (angle >= 270 - deltaEars && angle <= 270 + deltaEars)
        {
            float magnitude = 1;
            if (angle < 270 - earSuppress || angle > 270 + earSuppress)
            {
                float lAngle = angle - 270 - earSuppress;
                magnitude -= Mathf.Abs(lAngle / deltaEars);
            }

            music.panStereo = magnitude;
        }
        else
            music.panStereo = 0;
    }
}

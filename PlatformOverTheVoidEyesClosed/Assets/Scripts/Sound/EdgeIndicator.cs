using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeIndicator : MonoBehaviour
{
    Transform camTrans = null;
    AudioSource music = null;

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
    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += SetAngle;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= SetAngle;
    }
    void Start()
    {
        camTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        music = GetComponent<AudioSource>();
    }

    void SetAngle()
    {
        //Get absolute angle using the position and direction of the player and platform
        Vector3 targetDir = transform.position - camTrans.position;
        targetDir.y = 0;
        float angle = Vector3.Angle(targetDir, camTrans.forward);

        //Set angle from absolute to 0-360
        float rCheck = Vector3.Angle(targetDir, camTrans.right);
        float lCheck = Vector3.Angle(targetDir, -camTrans.right);
        Debug.Log("angle before: " + angle);

        if (lCheck > rCheck)
            angle = 360 - angle;

        Debug.Log("angle after: " + angle);
        //Adjust volume (mute if facing other direction
        if (angle >= 180 - deltaBack && angle <= 180 + deltaBack)
        {
            float magnitude = 0;
            if(angle < 180 - backSuppress || angle > 180 + backSuppress)
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

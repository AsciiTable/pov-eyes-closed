using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitNode : MonoBehaviour
{
    private AudioSource soundFX = null;
    private Transform playerTrans = null;
    private Transform camTrans = null;

    private Vector3 origin = Vector3.zero;
    private Vector3 size = Vector3.zero;

    [Header("Music Settings")]
    [SerializeField] private float maxVolume = .5f;
    [SerializeField] float minVolume = 0f;
    [SerializeField] private float decreaseStep = 0.3f;
    [Tooltip("Angle range from left and right of player to determine when stero pan starts adjusting")]
    [SerializeField] float deltaEars = 90f;
    [Tooltip("Angle range from back of player to determine when volume starts silencing")]
    [SerializeField] float deltaBack = 90f;
    [Tooltip("Range for angle when back's magnitude become 0")]
    [SerializeField] float backSuppress = 10f;
    [Tooltip("Range for angle when ear's stereo pan maxes")]
    [SerializeField] float earSuppress = 0f;


    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += FollowPlayer;
        UpdateHandler.UpdateOccurred += DirectionalSound;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= FollowPlayer;
        UpdateHandler.UpdateOccurred -= DirectionalSound;
    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        camTrans = GameObject.FindGameObjectWithTag("MainCamera").transform;
        soundFX = GetComponent<AudioSource>();

        origin = transform.parent.position;
        size = transform.parent.localScale / 2;
        size.y = origin.y = 0f;
    }
    private void FollowPlayer()
    {
        Vector3 pos = transform.position;

        if (playerTrans.position.x >= (origin.x - size.x) && playerTrans.position.x <= (origin.x + size.x))
            pos.x = playerTrans.position.x;
        else if (playerTrans.position.x <= origin.x)
            pos.x = origin.x - size.x;
        else
            pos.x = origin.x + size.x;

        if (playerTrans.position.z >= (origin.z - size.z) && playerTrans.position.z <= (origin.z + size.z))
            pos.z = playerTrans.position.z;
        else if (playerTrans.position.z <= origin.z)
            pos.z = origin.z - size.z;
        else
            pos.z = origin.z + size.z;

        transform.position = pos;
    }
    private void DirectionalSound()
    {
        float playerAngle = camTrans.rotation.eulerAngles.y;

        //Adjust volume (mute if facing other direction
        if (playerAngle >= 180 - deltaBack && playerAngle <= 180 + deltaBack)
        {
            float angle = playerAngle - 180;
            float magnitude = Mathf.Abs(angle / deltaBack) - (backSuppress / deltaBack);
            soundFX.volume = GameSettingHandler.SFXVolume * (minVolume + magnitude * (maxVolume - minVolume));
        }
        else
            soundFX.volume = GameSettingHandler.SFXVolume * maxVolume;

        //Adjust stero pan (mute right ear)
        if (playerAngle >= 90 - deltaEars && playerAngle <= 90 + deltaEars)
        {
            //60 - 120
            float angle = playerAngle - 90 - earSuppress;
            float magnitude = -1 + Mathf.Abs(angle / deltaEars);
            soundFX.panStereo = magnitude - (earSuppress / deltaBack);
        }
        //Adjust stero pan (mute left ear)
        else if (playerAngle >= 270 - deltaEars && playerAngle <= 270 + deltaEars)
        {
            float angle = playerAngle - 270 - earSuppress;
            float magnitude = 1 - Mathf.Abs(angle / deltaEars);
            soundFX.panStereo = magnitude + (earSuppress / deltaBack);
        }
        else
            soundFX.panStereo = 0;
    }
    /*
    private float CheckHeight()
    {
        int level = Mathf.FloorToInt(playerTrans.position.y / 1.5f);

        return GameSettingHandler.SFXVolume * (1 - level * decreaseStep);
    }
    */
}

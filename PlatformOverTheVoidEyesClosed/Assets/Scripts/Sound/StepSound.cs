using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    Transform playerTrans = null;
    AudioSource music = null;

    bool horizontal = false;
    //Which direction player will look to be in front of step
    int frontDegrees = 0;

    [Header("MUSIC SETTINGS")]
    [Tooltip("How close player needs to be to hear the step")]
    [SerializeField] float range = 10f;

    [SerializeField] float maxVolume = 0.5f;
    [SerializeField] float minVolume = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        music = GetComponent<AudioSource>();

        if (transform.localScale.x > transform.localScale.z)
            horizontal = true;
        else
            horizontal = false;
        //0 z > player, 270 z < player
        //90 x > player, 180 z < player
        frontDegrees = (!horizontal) ? ((playerTrans.position.x > transform.position.x) ? 90 : 270) : ((playerTrans.position.z > transform.position.z) ? 180 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPassed())
        {
            music.volume = 0f;
            return;
        }

        float distance = Mathf.Abs(horizontal ? transform.position.z - playerTrans.position.z : transform.position.x - playerTrans.position.x);
        distance -= horizontal ? (transform.localScale.z + playerTrans.localScale.z) / 2 : (transform.localScale.x + playerTrans.localScale.x) / 2;
        
        if( distance > range)
        {
            music.volume = 0f;
            return;
        }

        float magnitude = minVolume + (maxVolume - minVolume) * (1 - distance / range);
        music.volume = magnitude;
    }

    private bool PlayerPassed()
    {
        //Wall should be in front of player (Moving on z axis)
        if (frontDegrees == 0)
        {
            return (playerTrans.position.z > transform.position.z);
        }
        //Wall should be in front of player (Moving on x axis)
        else if(frontDegrees == 90)
        {
            return (playerTrans.position.x > transform.position.x);
        }
        //Wall should be in behind of player (Moving on z axis)
        else if (frontDegrees == 180)
        {
            return (playerTrans.position.z < transform.position.z);
        }
        //Wall should be in behind of player (Moving on x axis)
        else if (frontDegrees == 270)
        {
            return (playerTrans.position.x < transform.position.x);
        }
        return false;
    }
}

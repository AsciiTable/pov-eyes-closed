using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSound : MonoBehaviour
{
    bool touching;
    bool connected;

    Transform playerTrans = null;

    //Music that this wall will play
    AudioSource music = null;
    //This will muffle the audio
    AudioLowPassFilter muffler = null;

    //Wall is at the side. Wall is at the front/back if false
    bool sideWall = false;
    //Degree of player for them to look in front of this wall
    float frontDegrees;

    [Tooltip("How close player needs to be to wall")]
    [SerializeField] private float radius = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        if (transform.localScale.z > transform.localScale.x)
            sideWall = true;
        else
            sideWall = false;
        //0 z > player, 270 z < player
        //90 x > player, 180 z < player
        frontDegrees = (sideWall) ? ((playerTrans.position.x > transform.position.x) ? 90 : 270) : ((playerTrans.position.z > transform.position.z) ? 180 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Get magnitude of volume from player position
        float magnitude = 0;
        float muffle = 0;

        if (!touching)
        {
            float distance = (sideWall) ? Mathf.Abs(transform.position.x - playerTrans.position.x) : Mathf.Abs(transform.position.z - playerTrans.position.z);
            distance -= (sideWall) ? (transform.localScale.x + playerTrans.localScale.x) * 0.5f : (transform.localScale.z + playerTrans.localScale.z) * 0.5f;

            //Disconnect script if it's out of range
            if (distance > radius || CheckPastPlayer())
            {
                if (connected)
                {
                    WallSound_Manager.singleton.DisconnectSource();
                    connected = false;
                }
                return;
            }

            magnitude = 1 + Mathf.Log(distance / radius);
            if (magnitude < 0.1f) magnitude = 0.1f;
            muffle = magnitude;
        }
        //Get Stereo Pan of volume from player rotation
        float panStereo = 0f;
        bool forward = false;

        float angle = playerTrans.rotation.eulerAngles.y - frontDegrees;
        //Make angle range -179 - 180
        if (angle <= -180) angle += 360f;

        //Make range -90 to 90
        if (angle > 90)
            angle = 180f - angle;
        else if (angle < -90) angle = -180f - angle;
        else forward = true;

        //Get magnitude of stereoPan using the given angle
        panStereo = angle / 90f;

        if (!connected) {
            WallSound_Manager.singleton.ConnectSource();
            connected = true;
        }
        WallSound_Manager.singleton.SetVolume(magnitude, panStereo);
        WallSound_Manager.singleton.SetFrequency(muffle, panStereo);
    }

    //Check if player passed this wall
    private bool CheckPastPlayer()
    {
        //0 z > player, 270 z < player
        //90 x > player, 180 z < player
        if (frontDegrees == 0 && transform.position.z < playerTrans.position.z)
            return true;
        else if(frontDegrees == 90 && transform.position.x < playerTrans.position.x)
            return true;
        else if (frontDegrees == 180  && transform.position.z > playerTrans.position.z)
            return true;
        else if (frontDegrees == 270 && transform.position.x > playerTrans.position.x)
            return true;

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        touching = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        touching = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepNode : ProgressionNode
{
    Transform playerTrans = null;
    //If x scale is larger than z scale (Player will move towards other axis)
    bool horizontal = true;
    //Checks whether player is on step platform
    bool onStep = false;
    //Checks whether player has passed this step
    bool hasProgressed = false;

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += PlayerPassed;
        UpdateHandler.UpdateOccurred += PlayerReturned;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= PlayerPassed;
        UpdateHandler.UpdateOccurred -= PlayerReturned;
    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        horizontal = (transform.localScale.x >= transform.localScale.z) ? true : false;
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.CompareTag("Player"))
            if (progressionID >= 0)
                if(col.transform.position.y >= transform.parent.position.y)
                {
                    onStep = true;
                    SoundProgression_Manager.singleton.Progress(progressionID);
                }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
            onStep = false;
    }
    private void PlayerPassed()
    {
        if (!nextNode || progressionID < 0)
            return;

        if (horizontal)
        {
            if(playerTrans.position.z > transform.position.z)
            {
                SoundProgression_Manager.singleton.Progress(progressionID);
                hasProgressed = true;
            }
                    
        }
        else if(playerTrans.position.x > transform.position.x)
        {
            SoundProgression_Manager.singleton.Progress(progressionID);
            hasProgressed = true;
        }
            
    }
    private void PlayerReturned()
    {
        if (!hasProgressed || onStep || SoundProgression_Manager.singleton.CurrentID != progressionID)
            return;

        if (horizontal)
        {
            if (playerTrans.position.z < transform.position.z)
            {
                SoundProgression_Manager.singleton.Progress(progressionID-1);
                hasProgressed = false;
            }

        }
        else if (playerTrans.position.x < transform.position.x)
        {
            SoundProgression_Manager.singleton.Progress(progressionID-1);
            hasProgressed = false;
        }
    }
}

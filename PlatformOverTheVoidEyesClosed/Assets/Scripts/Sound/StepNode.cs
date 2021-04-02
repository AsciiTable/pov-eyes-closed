using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepNode : ProgressionNode
{
    Transform playerTrans = null;
    //If x scale is larger than z scale (Player will move towards other axis)
    bool horizontal = true;

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += PassedPlayer;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= PassedPlayer;
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
                    SoundProgression_Manager.singleton.Progress(progressionID);
                }
    }
    private void PassedPlayer()
    {
        if (!nextNode)
            return;

        if (horizontal)
        {
            if(playerTrans.position.z > transform.position.z)
                    SoundProgression_Manager.singleton.Progress(progressionID);
        }
        else if(playerTrans.position.x > transform.position.x)
            SoundProgression_Manager.singleton.Progress(progressionID);
    }
}

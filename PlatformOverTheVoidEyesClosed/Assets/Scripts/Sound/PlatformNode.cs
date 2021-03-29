using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformNode : ProgressionNode
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
            if (progressionID >= 0)
                if(col.transform.position.y >= transform.position.y)
                    SoundProgression_Manager.singleton.Progress(progressionID);
    }
}

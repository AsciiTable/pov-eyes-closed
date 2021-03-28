using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMuffler : MonoBehaviour
{
    private AudioSource music;
    private Transform playerTrans;
    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float rotation = transform.eulerAngles.y - 180; 

        
    }
}

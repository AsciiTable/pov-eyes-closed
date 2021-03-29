using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    Transform playerTrans = null;
    AudioSource music = null;

    //pit is horizontal. Pit is at the verticle if false
    private bool horizontal = false;
    bool connected = false;

    [Tooltip("How close player needs to be to wall")]
    [SerializeField] private float radius = 5;

    [SerializeField] float volume = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        music = GetComponent<AudioSource>();

        horizontal = (transform.localScale.z < transform.localScale.x);
        music.volume = volume;
    }
}

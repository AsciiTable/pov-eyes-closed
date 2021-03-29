using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    Transform playerTrans = null;

    //pit is horizontal. Pit is at the verticle if false
    private bool horizontal = false;
    bool connected = false;

    [Tooltip("How close player needs to be to wall")]
    [SerializeField] private float radius = 5;

    // Start is called before the first frame update
    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;

        horizontal = (transform.localScale.z < transform.localScale.x);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (horizontal) ? Mathf.Abs(transform.position.z - playerTrans.position.x) : Mathf.Abs(transform.position.z - playerTrans.position.z);
        distance -= (horizontal) ? (transform.localScale.z + playerTrans.localScale.z) * 0.5f : (transform.localScale.x + playerTrans.localScale.x) * 0.5f;

        if(distance <= radius)
        {
            float magnitude = 1 + Mathf.Log(distance / radius);
            PitSound_Manager.singleton.SetVolume(magnitude);
            if (!connected)
            {
                PitSound_Manager.singleton.ConnectSource();
                connected = true;
            }
        }
        else
        {
            if (connected)
            {
                PitSound_Manager.singleton.DisconnectSource();
                connected = false;
            }
        }
    }
}

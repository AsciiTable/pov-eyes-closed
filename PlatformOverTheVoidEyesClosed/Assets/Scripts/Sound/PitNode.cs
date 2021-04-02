using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitNode : MonoBehaviour
{
    private AudioSource soundFX = null;
    private Transform playerTrans = null;

    private Vector3 origin = Vector3.zero;
    private Vector3 size = Vector3.zero;

    [Header("Music Settings")]
    [SerializeField] private float maxVolume = .5f;
    [SerializeField] private float decreaseStep = 0.15f;

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += FollowPlayer;
        UpdateHandler.UpdateOccurred += CheckHeight;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= FollowPlayer;
        UpdateHandler.UpdateOccurred -= CheckHeight;
    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
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

    private void CheckHeight()
    {
        int level = Mathf.FloorToInt(playerTrans.position.y / 1.5f);

        soundFX.volume = maxVolume - level * decreaseStep;
    }
}

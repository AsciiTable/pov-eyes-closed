using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNode : MonoBehaviour
{
    Transform playerTrans = null;
    enum Axis { x, y, z };

    [SerializeField] Axis axis = Axis.x;

    private void OnEnable()
    {
        UpdateHandler.UpdateOccurred += FollowPlayer;
    }
    private void OnDisable()
    {
        UpdateHandler.UpdateOccurred -= FollowPlayer;
    }

    private void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void FollowPlayer()
    {
        Vector3 follow = transform.position;

        switch (axis)
        {
            case Axis.x:
                follow.x = playerTrans.position.x;
                break;
            case Axis.y:
                follow.y = playerTrans.position.y;
                break;
            case Axis.z:
                follow.z = playerTrans.position.z;
                break;
        }

        transform.position = follow;
    }


}

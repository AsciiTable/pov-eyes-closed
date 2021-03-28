using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNode : MonoBehaviour
{
    enum Axis { x, y, z };

    [Tooltip("Axis this node is traveling on.")]
    [SerializeField] private Axis followAxis;
    [Tooltip("Blacklist axis = false. Whitelist axis = true")]
    [SerializeField] private bool whiteList = true;

    private Transform playerTrans = null;

    void Start()
    {
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float x = (followAxis == Axis.x && !whiteList) || (followAxis != Axis.x && whiteList) ? transform.position.x : playerTrans.position.x;
        float y = (followAxis == Axis.y && !whiteList) || (followAxis != Axis.y && whiteList) ? transform.position.y : playerTrans.position.y;
        float z = (followAxis == Axis.z && !whiteList) || (followAxis != Axis.z && whiteList) ? transform.position.z : playerTrans.position.z;

        transform.position = new Vector3(x, y, z);
    }
}

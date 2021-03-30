using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteStepWall : MonoBehaviour
{
    string wallTag = "";

    private void Start()
    {
        wallTag = transform.parent.tag;
    }

    private void OnTriggerEnter(Collider other)
    {
        transform.parent.tag = "Untagged";
    }
    private void OnTriggerExit(Collider other)
    {
        transform.parent.tag = wallTag;
    }
}

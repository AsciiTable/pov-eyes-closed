using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionNode : MonoBehaviour
{
    [Tooltip("[READ ONLY] Where in progression line this node lies")]
    [SerializeField] protected int progressionID = -1;
    public int PID { get => progressionID; set => progressionID = value; }
    protected bool nextNode = false;
    public bool NextNode { set => nextNode = value; }
}

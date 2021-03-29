using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionNode : MonoBehaviour
{
    [SerializeField] protected int progressionID = -1;
    public int PID { get => progressionID; set => progressionID = value; }
}

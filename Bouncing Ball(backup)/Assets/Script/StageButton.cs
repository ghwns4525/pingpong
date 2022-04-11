using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageButton : MonoBehaviour
{
    [SerializeField] private int stage = 0;

    public int Stage { get => stage; set => stage = value; }

}

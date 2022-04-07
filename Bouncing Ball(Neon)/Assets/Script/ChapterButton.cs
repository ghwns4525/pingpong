using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterButton : MonoBehaviour
{
    [SerializeField] private int chapter = 0;

    public int Chapter { get => chapter; set => chapter = value; }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int stageNum = 0;
    private int chapterNum = 0;
    private int clearNum = 0;
    public int StageNum { get => stageNum; set => stageNum = value; }
    public int ChapterNum { get => chapterNum; set => chapterNum = value; }
    public int ClearNum { get => clearNum; set => clearNum = value; }

    public static StageManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        StageManager.instance = this;
    }
    private void Update()
    {
        Debug.Log(clearNum);
    }
}

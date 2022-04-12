using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Button[] btn_chapters;
    [SerializeField] private Button[] btn_stages;

    private int stageNum = 0;
    private int chapterNum = 0;
    private int clearNum = 0;
    private int chapterClear = 0;

    public int StageNum { get => stageNum; set => stageNum = value; }
    public int ChapterNum { get => chapterNum; set => chapterNum = value; }
    public int ClearNum { get => clearNum; set => clearNum = value; }

    public static StageManager instance;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        StageManager.instance = this;
    }
    private void Start()
    {

    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "2_Chapter")
        {
            ChapterInitialSetting();
        }
        if (SceneManager.GetActiveScene().name == "3_Select")
        {
            StageInitialSetting();
        }
        Debug.Log(clearNum);
    }
    
    // 스테이지 초기 설정
    private void ChapterInitialSetting()
    {
        if (btn_chapters.Length == 0 || btn_chapters[0] == null)
        {
            GameObject go_Chapter = GameObject.Find("Chapter");
            btn_chapters = go_Chapter.GetComponentsInChildren<Button>();
        }

        for (int i = 1; i < btn_chapters.Length; i++)
        {
            for (int j = 1; j < 31; j++)
            {
                if (ScoreManager.instance.HighScore[i, j] != 0)
                {
                    chapterClear++;
                }
            }
            if (chapterClear == 30)
            {
                // 해당 chapter 버튼 활성화
                if(i != btn_chapters.Length)
                {
                    btn_chapters[i].interactable = true;
                }
            }
            else
            {
                if(i != btn_chapters.Length)
                {
                    btn_chapters[i].interactable = false;
                }
            }
            chapterClear = 0;
        }
    }
    private void StageInitialSetting()
    {
        if (btn_stages.Length == 0 || btn_stages[0] == null)
        {
            GameObject go_Stage = GameObject.Find("Stage");
            btn_stages = go_Stage.GetComponentsInChildren<Button>();
        }
        for (int j = 1; j < btn_stages.Length; j++)
        {
            if (ScoreManager.instance.HighScore[chapterNum, j] != 0)
            {
                // 해당 stage 버튼 활성화
                if (j != btn_stages.Length)
                {
                    btn_stages[j].interactable = true;
                }
            }
            else
            {
                if (j != btn_stages.Length)
                {
                    btn_stages[j].interactable = false;
                }
            }
        }

    }
}

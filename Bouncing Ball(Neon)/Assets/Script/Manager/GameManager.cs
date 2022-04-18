using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    GameObject popup_GameOver;
    [SerializeField]Ball cs_Ball;
    private bool isRetry = false;
    [SerializeField] TextMeshProUGUI txt_Score;
    [SerializeField] TextMeshProUGUI txt_GameOverScore;
    [SerializeField] TextMeshProUGUI txt_Life;

    private Clear[] clears;
    private int clearCount;
    private int currentClearCount;
    private int currentAdCount;

    private bool isClear = false;
    private bool isBuild = false;

    private GameObject[][] stagePrefabs = new GameObject[4][];
    private GameObject[] stage1Prefabs = new GameObject[30];
    private GameObject[] stage2Prefabs = new GameObject[30];
    private GameObject[] stage3Prefabs = new GameObject[30];
    private GameObject[] stage4Prefabs = new GameObject[30];
    private GameObject crnt_Stage;

    public bool IsRetry { get => isRetry; set => isRetry = value; }
    public bool IsClear { get => isClear; set => isClear = value; }
    public int ClearCount { get => clearCount; set => clearCount = value; }
    public int CurrentClearCount { get => currentClearCount; set => currentClearCount = value; }
    public bool IsBuild { get => isBuild; set => isBuild = value; }

    public static GameManager instance;

    private void Awake()
    {
        GameManager.instance = this;

        #region 생성 초기화
        stage1Prefabs = Resources.LoadAll<GameObject>("Prefab/Chapter1");
        stage2Prefabs = Resources.LoadAll<GameObject>("Prefab/Chapter2/");
        stage3Prefabs = Resources.LoadAll<GameObject>("Prefab/Chapter3/");
        stage4Prefabs = Resources.LoadAll<GameObject>("Prefab/Chapter4/");
        stagePrefabs[0] = stage1Prefabs;
        stagePrefabs[1] = stage2Prefabs;
        stagePrefabs[2] = stage3Prefabs;
        stagePrefabs[3] = stage4Prefabs;
        ChapterBuild();
        #endregion

        clears = FindObjectsOfType<Clear>();
        clearCount = clears.Length;
        currentClearCount = clearCount;
        currentAdCount = StageManager.instance.ClearNum;
    }

    // Start is called before the first frame update
    void Start()
    {
        popup_GameOver = GameObject.Find("PopUp_GameOver");
        cs_Ball = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBuild)
        {
            if (cs_Ball.IsGameOver || isClear)
            {
                popup_GameOver.SetActive(true);
            }
            else
            {
                popup_GameOver.SetActive(false);
            }

            txt_Score.text = "Score : " + cs_Ball.Score;
            txt_GameOverScore.text = "Score : " + cs_Ball.Score;
            txt_Life.text = cs_Ball.CurrentLife.ToString();

            Clear();
        }
    }

    private void Clear()
    {
        if(currentClearCount <= 0)
        {
            isClear = true;            
        }
    }

    private void AdAdd()
    {
        if (currentAdCount == StageManager.instance.ClearNum)
        {
            StageManager.instance.ClearNum++;
        }
    }

    private void StageBuild(int chapterNum)
    {
        /*        switch (StageManager.instance.StageNum)
                {
                    case 1:
                        GameObject.Instantiate<GameObject>(stagePrefabs[chapterNum][0]);
                        break;
                    case 2:
                        GameObject.Instantiate<GameObject>(stagePrefabs[chapterNum][1]);
                        break;
                    case 3:
                        GameObject.Instantiate<GameObject>(stagePrefabs[chapterNum][2]);
                        break;
                    default:
                        return;
                }*/
        crnt_Stage = GameObject.Instantiate<GameObject>(stagePrefabs[chapterNum][StageManager.instance.StageNum-1]);
    }

    private void ChapterBuild()
    {
        switch (StageManager.instance.ChapterNum)
        {
            case 1:
                StageBuild(0);
                break;
            case 2:
                StageBuild(1);
                break;
            case 3:
                StageBuild(2);
                break;
            case 4:
                StageBuild(3);
                break;
            default:
                return;
        }
        isBuild = true;
    }


    #region 버튼이벤트
    public void btn_Retry()
    {
        isRetry = true;
    }
    public void btn_Main()
    {
        isBuild = false;
        AdAdd();
        SceneManager.LoadScene("2_Chapter");
    }
    #endregion
}

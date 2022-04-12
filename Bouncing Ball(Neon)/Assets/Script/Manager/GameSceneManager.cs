using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class GameSceneManager : MonoBehaviour
{
    private GameObject txt_GO_HighScore;
    private TextMeshProUGUI txt_HighScore;
    private GameObject GO_Popup_Exit;

    private GoogleAdsManager googleAdsManager;

    private void Awake()
    {
        googleAdsManager = FindObjectOfType<GoogleAdsManager>();
        if (SceneManager.GetActiveScene().name == "3_Select")
        {
            txt_GO_HighScore = GameObject.Find("txt_HighScore");
            txt_HighScore = txt_GO_HighScore.GetComponent<TextMeshProUGUI>();
            StageManager.instance.StageNum = 1;
            GO_Popup_Exit = GameObject.Find("PopUp_Exit");
            GO_Popup_Exit.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "0_Start")
        {
            SceneManager.LoadScene("1_Main");
        }
        if(SceneManager.GetActiveScene().name == "3_Select")
        {
            if(StageManager.instance.StageNum == 0)
            {
                txt_HighScore.text = "High Score " + ScoreManager.instance.HighScore[StageManager.instance.ChapterNum, StageManager.instance.StageNum + 1].ToString();
            }
            else
            {
                txt_HighScore.text = "High Score " + ScoreManager.instance.HighScore[StageManager.instance.ChapterNum, StageManager.instance.StageNum].ToString();
            }
        }

        back();
    }

    private void back()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(SceneManager.GetActiveScene().name == "3_Select")
            {
                GO_Popup_Exit.SetActive(true);
            }
        }
    }

    #region 버튼 이벤트
    public void btn_Main()
    {
        SceneManager.LoadScene("2_Chapter");
    }
    public void btn_Start()
    {
        SceneManager.LoadScene("3_Select");
        StageManager.instance.ChapterNum = EventSystem.current.currentSelectedGameObject.GetComponent<ChapterButton>().Chapter;
    }
    public void btn_GameStart()
    {
        SceneManager.LoadScene("4_Stage");
    }
    public void btn_StageSelect()
    {
        StageManager.instance.StageNum = EventSystem.current.currentSelectedGameObject.GetComponent<StageButton>().Stage;
    }
    public void btn_Exit()
    {
        Application.Quit();
    }
    public void btn_aRewardAd()
    {
        googleAdsManager.RewardAdTest();
    }
    public void btn_bRewardAd()
    {
        googleAdsManager.RewardBAdTest();
    }
    public void btn_DataDelete()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion
}

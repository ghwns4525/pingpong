using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[,] highScore = new int[4,30];

    public int[,] HighScore { get => highScore; set => highScore = value; }

    public static ScoreManager instance;
    
    private void Awake()
    {
        ScoreManager.instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 30; j++)
            {
                if(PlayerPrefs.HasKey("HighScore_" + (i + 1) + "_" + (j + 1)))
                {
                    HighScore[i, j] = PlayerPrefs.GetInt("HighScore_" + (i + 1) + "_" + (j + 1));
                    Debug.Log("HighScore_" + (i + 1) + "_" + (j + 1) + "ºÒ·¯¿È");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

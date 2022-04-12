using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int[,] highScore = new int[5,31];

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
        for (int i = 1; i < 5; i++)
        {
            for (int j = 1; j < 31; j++)
            {
                if(PlayerPrefs.HasKey("HighScore_" + (i) + "_" + (j)))
                {
                    highScore[i, j] = PlayerPrefs.GetInt("HighScore_" + (i) + "_" + (j));                    
                }
                else
                {
                    highScore[i, j] = 0;
                }
                Debug.Log("HighScore_" + (i) + "_" + (j) + "_" +highScore[i, j]);
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}

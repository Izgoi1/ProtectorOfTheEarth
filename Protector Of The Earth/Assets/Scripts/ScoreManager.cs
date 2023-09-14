using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public Text bestScoreText;
    public static int score;

    private void Start()
    {
        BestScoreUpdate();
    }

    private void Update()
    {
        scoreText.text = score.ToString();

        if (score > PlayerPrefs.GetInt("bestScore", 0))
        {
            PlayerPrefs.SetInt("bestScore", score);
        }
    }
     
    public void BestScoreUpdate()
    {
        bestScoreText.text = $"Best Score: {PlayerPrefs.GetInt("bestScore", 0)}";
    }

    public void DeleteHighScore()
    {
        PlayerPrefs.DeleteKey("bestScore");
    }

}

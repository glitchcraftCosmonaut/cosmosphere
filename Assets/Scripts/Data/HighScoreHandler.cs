using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    [SerializeField] HighScoreUI highScoreUI;

    int highScore;

    public int HighScore
    {
        set
        {
            highScore = value;
            highScoreUI.SetHighScore(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetLatestHighScore();
    }

    void SetLatestHighScore()
    {
        HighScore = PlayerPrefs.GetInt("highscore", 0);
    }

    void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt("highscore", score);
    }

    public void SetHighScoreIfGreater(int score)
    {
        if(score > highScore)
        {
            HighScore = score;
            SaveHighScore(score);
        }
    }
}

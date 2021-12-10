using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score main;

    public static int ScoreVal
    {
        get
        {
            return main._score;
        }
        set
        {
            main._score = value;
            main.GetComponent<UnityEngine.UI.Text>().text = "" + value;
            if (value >= HighScore.HighScoreVal)
            {
                HighScore.HighScoreVal = value;
            }
        }
    }

    public int _score = 0;

    private void Awake()
    {
        main = this;
    }
}

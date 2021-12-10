using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public static HighScore main;

    public static int HighScoreVal
    {
        get
        {
            return main._highScore;
        }
        set
        {
            main._highScore = value;
            main.GetComponent<UnityEngine.UI.Text>().text = "" + value;
            PlayerPrefs.SetInt("Score", value);
        }
    }

    private int _highScore = 0;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        HighScoreVal = PlayerPrefs.GetInt("Score",0);
    }
}

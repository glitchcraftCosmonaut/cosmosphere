using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] Text highScoreText;

    public void SetHighScore(int score)
    {
        highScoreText.text = "HighScore : " + score.ToString();
    }
}

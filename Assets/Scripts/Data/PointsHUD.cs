using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsHUD : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreGameOver;

    int scores = 0;

    public int growthRate;
    

    private void Awake() 
    {
        UpdateHUD();
    }
    public int Points
    {
        get{ return scores; }
        set
        { 
            scores = value;
            UpdateHUD();
        }
    }
    
    public void ResetPoints ()
    {
        scores = 0;
        UpdateHUD ();
    }
    private void UpdateHUD()
    {
        scoreText.text = "Score : " + scores.ToString();
        scoreGameOver.text = scores.ToString();
    }
}

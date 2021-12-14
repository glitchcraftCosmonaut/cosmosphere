using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsHUD : MonoBehaviour
{
    [SerializeField] Text scoreText;

    int scores = 0;
    

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
    
    private void UpdateHUD()
    {
        scoreText.text = "Score : " + scores.ToString();
    }
}

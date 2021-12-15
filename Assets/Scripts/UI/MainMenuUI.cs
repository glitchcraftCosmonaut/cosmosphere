using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject highscorePanel;
    [SerializeField] GameObject creditPanel;
    [SerializeField] GameObject controlPanel;


    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Game");
    }
    public void ShowLeaderboards()
    {
        highscorePanel.SetActive(true);
    }
    public void CloseLeaderboards()
    {
        highscorePanel.SetActive(false);
    }
    public void ShowCredit()
    {
        creditPanel.SetActive(true);
    }
    public void CloseCredit()
    {
        creditPanel.SetActive(false);
    }

    public void ShowControl()
    {
        controlPanel.SetActive(true);
    }

    public void CloseControl()
    {
        controlPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

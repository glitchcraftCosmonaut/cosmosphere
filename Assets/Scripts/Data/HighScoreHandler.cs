using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreHandler : MonoBehaviour
{
    List<HighScoreElement> highscoreList = new List<HighScoreElement> ();
    [SerializeField] int maxCount = 7;
    [SerializeField] string filename;

    public delegate void OnHighscoreListChanged (List<HighScoreElement> list);
    public static event OnHighscoreListChanged onHighscoreListChanged;
    

    private void Start () 
    {
        LoadHighscores ();
    }

    private void LoadHighscores () 
    {
        highscoreList = FileHandler.ReadListFromJSON<HighScoreElement> (filename);

        while (highscoreList.Count > maxCount) 
        {
            highscoreList.RemoveAt (maxCount);
        }

        if (onHighscoreListChanged != null) 
        {
            onHighscoreListChanged.Invoke (highscoreList);
        }
    }

    private void SaveHighscore () 
    {
        FileHandler.SaveToJSON<HighScoreElement> (highscoreList, filename);
    }

    public void AddHighscoreIfPossible (HighScoreElement element) 
    {
        for (int i = 0; i < maxCount; i++) {
            if (i >= highscoreList.Count || element.score >= highscoreList[i].score) 
            {
                // add new high score
                highscoreList.Insert (i, element);

                while (highscoreList.Count > maxCount) 
                {
                    highscoreList.RemoveAt (maxCount);
                }

                SaveHighscore ();

                if (onHighscoreListChanged != null) 
                {
                    onHighscoreListChanged.Invoke (highscoreList);
                }

                break;
            }
        }
    }
}

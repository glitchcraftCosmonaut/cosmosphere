using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreUI : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] GameObject highscoreUIElements;
    [SerializeField] Transform elementWrapper;

    List<GameObject> uiElement = new List<GameObject>();

    private void OnEnable()
    {
        HighScoreHandler.onHighscoreListChanged += UpdateUI;
    }

    private void OnDisable()
    {
        HighScoreHandler.onHighscoreListChanged -= UpdateUI;

    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
    }

    private void UpdateUI(List<HighScoreElement> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            HighScoreElement elements = list[i];


            if(elements.score > 0)
            {
                if(i >= uiElement.Count)
                {
                    var inst = Instantiate(highscoreUIElements, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    uiElement.Add(inst);
                }

                var texts = uiElement[i].GetComponentsInChildren<Text>();
                texts[0].text = elements.playerName;
                texts[1].text = elements.score.ToString();
            }
        }
    }
}

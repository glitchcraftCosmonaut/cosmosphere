using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    // [SerializeField] public InputField nameInput;
    // [SerializeField] string filename;

    // List<HighScoreElement> entries = new List<HighScoreElement>();
    

    private void Start() 
    {
        // entries = FileHandler.ReadListFromJSON<InputEntry>(filename);
    }

    public void AddNameToList()
    {
        // entries.Add (new HighScoreElement(nameInput.text, GameController.SharedInstance.pointsHUD.Points));
        // nameInput.text = GameController.SharedInstance.playerName;
        GameController.SharedInstance.highScoreHandler.AddHighscoreIfPossible(new HighScoreElement(GameController.SharedInstance.playerName.text, GameController.SharedInstance.pointsHUD.Points));
        // FileHandler.SaveToJSON<InputEntry>(entries, filename);
    }
}

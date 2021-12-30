using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] Canvas mainMenuCanvas;

    [Header("Button")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonOption;
    [SerializeField] Button buttonQuit;


    private void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonOption.gameObject.name, OnButtonOptionClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClick);
    }

    private void OnDisable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Start()
    {
        Time.timeScale = 1f;
        GameManager.GameState = GameState.Playing;
        UIInput.Instance.SelectUI(buttonStart);
    }
    void OnButtonStartClick()
    {
        mainMenuCanvas.enabled = false;
        SceneLoader.Instance.LoadGamePlayScene();
    }

    void OnButtonOptionClick()
    {
        UIInput.Instance.SelectUI(buttonOption);
    }

    void OnButtonQuitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

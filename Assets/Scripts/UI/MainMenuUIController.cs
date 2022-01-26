using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] Canvas mainMenuCanvas;
    [SerializeField] Canvas howToPlayCanvas;

    [Header("Button")]
    [SerializeField] Button buttonStart;
    [SerializeField] Button buttonHowToPlay;
    [SerializeField] Button buttonBack;
    [SerializeField] Button buttonQuit;


    private void OnEnable()
    {
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonStart.gameObject.name, OnButtonStartClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonHowToPlay.gameObject.name, OnButtonHowToPlayClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonQuit.gameObject.name, OnButtonQuitClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(buttonBack.gameObject.name, OnButtonBackClick);
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
        howToPlayCanvas.enabled = false;
        // mainMenuCanvas.SetActive(false);
        // howToPlayCanvas.SetActive(false);
        SceneLoader.Instance.LoadGamePlayScene();
    }

    void OnButtonHowToPlayClick()
    {
        mainMenuCanvas.enabled = false;
        howToPlayCanvas.enabled = true;
        buttonBack.enabled = true;
        buttonHowToPlay.enabled = false;
        buttonStart.enabled = false;
        buttonQuit.enabled =false;
        // mainMenuCanvas.SetActive(false);
        // howToPlayCanvas.SetActive(true);
        // UIInput.Instance.SelectUI(buttonHowToPlay);
        UIInput.Instance.SelectUI(buttonBack);

    }

    void OnButtonBackClick()
    {
        mainMenuCanvas.enabled = true;
        howToPlayCanvas.enabled = false;
        buttonBack.enabled = false;
        buttonHowToPlay.enabled = true;
        buttonStart.enabled = true;
        buttonQuit.enabled =true;
        // mainMenuCanvas.SetActive(true);
        // howToPlayCanvas.SetActive(false);
        UIInput.Instance.SelectUI(buttonStart);

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

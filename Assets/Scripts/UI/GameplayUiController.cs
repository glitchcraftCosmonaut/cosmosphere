using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUiController : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] PlayerInput playerInput;

    [Header("Canvas")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menusCanvas;

    [Header("button")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button optionButton;
    [SerializeField] Button mainMenuButton;

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        resumeButton.onClick.AddListener(OnResumeButtonClick);
        optionButton.onClick.AddListener(OnOptionButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);

        // ButtonPressed.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        // ButtonPressed.buttonFunctionTable.Add(optionButton.gameObject.name, OnOptionButtonClick);
        // ButtonPressed.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        resumeButton.onClick.RemoveAllListeners();
        optionButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
    }

    void Pause()
    {
        Time.timeScale = 0f;
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        playerInput.EnablePauseInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);
    }

    void Unpause()
    {
        OnResumeButtonClick();
    }

    void OnResumeButtonClick()
    {
        Time.timeScale = 1f;
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    void OnOptionButtonClick()
    {
        //todo
        UIInput.Instance.SelectUI(optionButton);
        playerInput.EnablePauseInput();
    }

    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}

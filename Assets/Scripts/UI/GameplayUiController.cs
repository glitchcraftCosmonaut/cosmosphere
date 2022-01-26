using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUiController : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] PlayerInput playerInput;

    [Header("Audio Data")]
    [SerializeField] AudioData pauseSFX;
    [SerializeField] AudioData unpauseSFX;

    [Header("Canvas")]
    [SerializeField] Canvas hUDCanvas;
    [SerializeField] Canvas menusCanvas;

    [Header("button")]
    [SerializeField] Button resumeButton;
    // [SerializeField] Button optionButton;
    [SerializeField] Button mainMenuButton;

    int buttonPressedParameterID = Animator.StringToHash("Pressed");

    private void OnEnable()
    {
        playerInput.onPause += Pause;
        playerInput.onUnpause += Unpause;

        // resumeButton.onClick.AddListener(OnResumeButtonClick);
        // optionButton.onClick.AddListener(OnOptionButtonClick);
        // mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);

        ButtonPressedBehavior.buttonFunctionTable.Add(resumeButton.gameObject.name, OnResumeButtonClick);
        // ButtonPressedBehavior.buttonFunctionTable.Add(optionButton.gameObject.name, OnOptionButtonClick);
        ButtonPressedBehavior.buttonFunctionTable.Add(mainMenuButton.gameObject.name, OnMainMenuButtonClick);
    }

    private void OnDisable()
    {
        playerInput.onPause -= Pause;
        playerInput.onUnpause -= Unpause;

        // resumeButton.onClick.RemoveAllListeners();
        // optionButton.onClick.RemoveAllListeners();
        // mainMenuButton.onClick.RemoveAllListeners();

        ButtonPressedBehavior.buttonFunctionTable.Clear();
    }

    void Pause()
    {
        hUDCanvas.enabled = false;
        menusCanvas.enabled = true;
        GameManager.GameState = GameState.Paused;
        TimeController.Instance.Pause();
        playerInput.EnablePauseInput();
        playerInput.SwitchToDynamicUpdateMode();
        UIInput.Instance.SelectUI(resumeButton);
        AudioManager.Instance.PlaySFX(pauseSFX);
    }

    void Unpause()
    {
        // OnResumeButtonClick();
        resumeButton.Select();
        resumeButton.animator.SetTrigger(buttonPressedParameterID);
        AudioManager.Instance.PlaySFX(unpauseSFX);
    }

    void OnResumeButtonClick()
    {
        hUDCanvas.enabled = true;
        menusCanvas.enabled = false;
        GameManager.GameState = GameState.Playing;
        TimeController.Instance.Unpause();
        playerInput.EnableGameplayInput();
        playerInput.SwitchToFixedUpdateMode();
    }

    // void OnOptionButtonClick()
    // {
    //     //todo
    //     UIInput.Instance.SelectUI(optionButton);
    //     playerInput.EnablePauseInput();
    // }

    void OnMainMenuButtonClick()
    {
        menusCanvas.enabled = false;
        SceneLoader.Instance.LoadMainMenuScene();
    }
}

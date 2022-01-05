using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] PlayerInput input;

    [SerializeField] Canvas HUDCanvas;

    [SerializeField] AudioData confirmGameOverSound;

    int exitStateID = Animator.StringToHash("GameOverScreenExit");

    Canvas canvas;
    
    Animator animator;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        animator = GetComponent<Animator>();

        canvas.enabled = false;
        animator.enabled = false;
    }

    private void OnEnable()
    {
        GameManager.onGameOver += OnGameOver;

        input.onConfirmGameOver += OnConfirmGameOver;  
    }

    private void OnDisable()
    {
        GameManager.onGameOver -= OnGameOver;

        input.onConfirmGameOver -= OnConfirmGameOver;  

    }

    private void OnConfirmGameOver()
    {
        AudioManager.Instance.PlaySFX(confirmGameOverSound);
        input.DisableAllInput();
        animator.Play(exitStateID);
        SceneLoader.Instance.LoadScoringScene();
        //todo restart or main menu
    }

    private void OnGameOver()
    {
        HUDCanvas.enabled = false;
        canvas.enabled = true;
        animator.enabled = true;
        input.DisableAllInput();
    }

    void EnableGameOverScreenInput()
    {
        input.EnableGameOverScreenInput();
    }
}

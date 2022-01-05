using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Player Input")]
public class PlayerInput : 
    ScriptableObject, 
    InputActions.IGameplayActions, 
    InputActions.IPauseMenuActions, 
    InputActions.IGameOverScreenActions
{
    public event UnityAction<Vector2> onMove = delegate {};

    public event UnityAction onStopMove = delegate {};

    public event UnityAction onFire = delegate {};
    public event UnityAction onStopFire = delegate {};
    public event UnityAction onBulletTime = delegate {};
    public event UnityAction onStopBulletTime = delegate {};

    public event UnityAction onSlash = delegate {};
    public event UnityAction onOverdrive = delegate {};
    public event UnityAction onPause = delegate {};
    public event UnityAction onUnpause = delegate {};
    public event UnityAction onLaunchMissile = delegate {};
    public event UnityAction onConfirmGameOver = delegate {};
    

    InputActions inputActions;

    private void OnEnable() 
    {
        inputActions = new InputActions();

        inputActions.Gameplay.SetCallbacks(this);
        inputActions.PauseMenu.SetCallbacks(this);
        inputActions.GameOverScreen.SetCallbacks(this);
    }

    private void OnDisable() 
    {
        DisableAllInput();
    }

    void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
    {
        inputActions.Disable();
        actionMap.Enable();

        if(isUIInput)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void SwitchToDynamicUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
    public void SwitchToFixedUpdateMode() => InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInFixedUpdate;

    public void DisableAllInput() => inputActions.Disable();

    public void EnableGameplayInput() => SwitchActionMap(inputActions.Gameplay, false);

    public void EnablePauseInput() => SwitchActionMap(inputActions.PauseMenu, true);
    
    public void EnableGameOverScreenInput() => SwitchActionMap(inputActions.GameOverScreen, false);

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            onStopMove.Invoke();
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            onFire.Invoke();
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            onStopFire.Invoke();
        }
    }

    public void OnOverdrive(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onOverdrive.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onPause.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onUnpause.Invoke();
        }
    }

    public void OnLaunchMissile(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onLaunchMissile.Invoke();
        }
    }

    public void OnConfirmGameOver(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onConfirmGameOver.Invoke();
        }
    }

    public void OnBulletTime(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            onBulletTime.Invoke();
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            onStopBulletTime.Invoke();
        }
    }

    public void OnSlash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onSlash.Invoke();
        }
    }
}

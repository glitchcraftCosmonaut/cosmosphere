using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileNRGSys : Singleton<PlayerProjectileNRGSys>
{
    [SerializeField] ProjectileBar projectileBar;

    public bool available = true;
    int projectileNRG;
    WaitForSeconds waitForOverdriveInterval;
    private float overdriveInterval = 1f;
    public const int PERCENT = 1;
    public const int MAX = 100;


    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }
    void OnEnable()
    {
        PlayerProjectileActive.on += PlayerProjectileOn;
        PlayerProjectileActive.off += PlayerProjectileOff;
    }

    void OnDisable()
    {
        PlayerProjectileActive.on -= PlayerProjectileOn;
        PlayerProjectileActive.off -= PlayerProjectileOff;
    }

    void Start()
    {
        projectileBar.Initialize(projectileNRG, MAX);
        // Obtain(MAX);
    }

    public void Obtain(int value)
    {
        if (projectileNRG == MAX || !available || !gameObject.activeSelf) return;

        projectileNRG = Mathf.Clamp(projectileNRG + value, 0, MAX);
        projectileBar.UpdateStates(projectileNRG, MAX);
    }
    //BIG BUGS START HERE FIX THIS ASAP PLAYER WONT STOP SHOOTING UNTIL MINUS INT
    public void Use(int value)
    {
        projectileNRG -= value;
        projectileBar.UpdateStates(projectileNRG, MAX);

        // if player is overdriving and energy = 0
        //TODO PROJECTILE GET MINUS POINT AND DOESNT STOP SHOOTING UNTIL BUTTON UP
        //BUT WHEN BUTTON DOWN AGAIN CANNOT SHOOT
        if (projectileNRG == 0 && !available)
        {
            // player stop overdriving
            // return;
            // PlayerProjectileOff();
            PlayerOverdrive.off.Invoke();
        }
    }

    public bool IsEnough(int value) => projectileNRG >= value;

    void PlayerProjectileOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    void PlayerProjectileOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }
    IEnumerator KeepUsingCoroutine()
    {
        while (gameObject.activeSelf && projectileNRG > 0)
        {
            yield return waitForOverdriveInterval;
            Use(PERCENT);
        }
    }   
}

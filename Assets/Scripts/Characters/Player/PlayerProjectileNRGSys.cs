using UnityEngine;

public class PlayerProjectileNRGSys : Singleton<PlayerProjectileNRGSys>
{
    [SerializeField] ProjectileBar projectileBar;

    public bool available = true;
    int projectileNRG;
    public const int MAX = 200;

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
        //use this for debugging
        Obtain(MAX);
    }

    public void Obtain(int value)
    {
        if (projectileNRG == MAX || !available || !gameObject.activeSelf) return;

        projectileNRG = Mathf.Clamp(projectileNRG + value, 0, MAX);
        projectileBar.UpdateStates(projectileNRG, MAX);

        if(projectileNRG == MAX && available)
        {
            PlayerProjectileActive.on.Invoke();
        }
    }
    public void Use(int value)
    {
        // projectileNRG -= value; -->
        //stop at zero so this will not go down to minus point
        projectileNRG = Mathf.Clamp(projectileNRG - value, 0, MAX);
        projectileBar.UpdateStates(projectileNRG, MAX);

        // if nrg == 0 and not available to get nrg
        if (projectileNRG == 0 && !available)
        {
            // invoke this 
            PlayerProjectileActive.off.Invoke();
        }
    }

    public bool IsEnough(int value) => projectileNRG >= value;

    void PlayerProjectileOn()
    {
        available = false;
    }

    void PlayerProjectileOff()
    {
        available = true;
    }  
}

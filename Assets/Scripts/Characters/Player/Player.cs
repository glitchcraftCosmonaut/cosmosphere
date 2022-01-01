using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : Character
{
    #region  health System
    // [Header("HEALTH SYSTEM")]
    [SerializeField] Statsbar_HUD statsbar_HUD;
    [SerializeField] bool regenerateHealth = true;
    [SerializeField] float healthRegenerateTime;
    [SerializeField,Range(0f, 1f)] float healthRegeneratePercent;
    #endregion

    [Header("INPUT")]
    [SerializeField] PlayerInput input;

    #region movement
    [Header("MOVEMENT")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float deccelerationTime = 3f;
    
    #endregion

    #region Firing Variables
    [Header("FIRING")]
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;
    [SerializeField] GameObject projectileOverdrive;
    [SerializeField] ParticleSystem muzzleVFX;

    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;

    [SerializeField] AudioData projectileLaunchSFX;


    [SerializeField, Range(0, 2)] int weaponPower = 0;
    [SerializeField] float fireInterval = 0.2f;

    WaitForSeconds waitForFireInterval;
    #endregion
    WaitForSeconds waitForOverdriveFireInterval;
    WaitForSeconds waitHealthRegenerateTime;
    WaitForSeconds waitForDeccelerationTime;
    WaitForSeconds waitInvincibleTime;
    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    [Header("OVERDRIVING")]
    [SerializeField] float overDriveSpeedFactor = 1.2f;
    [SerializeField] float overDriveFireFactor = 1.2f;
    bool isOverdriving = false;

    Rigidbody2D playerRigidbody;
    new Collider2D collider;

    MissileSystem missile;

    float t;

    Vector2 moveDirection;
    Vector2 previousVelocity;
    readonly float slowMotionDuration = 1f;

    readonly float InvincibleTime = 1f;
    float paddingX;
    float paddingY;


    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;


    private void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        missile = GetComponent<MissileSystem>();

        playerRigidbody.gravityScale = 0;

        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;

        paddingX = size.x / 3f;
        paddingY = size.y / 3f;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitForOverdriveFireInterval = new WaitForSeconds(fireInterval / overDriveFireFactor);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
        waitForDeccelerationTime = new WaitForSeconds(deccelerationTime);
        waitInvincibleTime = new WaitForSeconds(InvincibleTime);
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        input.onMove += Move;    
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
        input.onOverdrive += Overdrive;
        input.onLaunchMissile += LaunchMissile;

        PlayerOverdrive.on += OverdriveOn;
        PlayerOverdrive.off += OverdriveOff;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;
        input.onOverdrive -= Overdrive;
        input.onLaunchMissile -= LaunchMissile;


        PlayerOverdrive.on -= OverdriveOn;
        PlayerOverdrive.off -= OverdriveOff;

    }
    // Start is called before the first frame update
    void Start()
    {
        
        statsbar_HUD.Initialize(health,maxHealth);

        input.EnableGameplayInput();
    }

    public override void RestoreHealth(float value)
    {
        base.RestoreHealth(value);
        statsbar_HUD.UpdateStates(health, maxHealth);
    }

    public override void Die()
    {
        GameManager.onGameOver?.Invoke();
        GameManager.GameState = GameState.GameOver;
        statsbar_HUD.UpdateStates(0f, maxHealth);
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        statsbar_HUD.UpdateStates(health, maxHealth);
        TimeController.Instance.BulletTime(slowMotionDuration);
        if(gameObject.activeSelf)
        {
            Move(moveDirection);
            StartCoroutine(InvincibleCoroutine());
            if(regenerateHealth)
            {
                if(healthRegenerateCoroutine != null)
                {
                    StopCoroutine(healthRegenerateCoroutine);
                }
                healthRegenerateCoroutine = StartCoroutine(HealthRegenerationCoroutine(waitHealthRegenerateTime, healthRegeneratePercent));
            }
        }
    }

    IEnumerator InvincibleCoroutine()
    {
        collider.isTrigger = true;

        yield return waitInvincibleTime;

        collider.isTrigger = false;
    }

    #region MOVE
    void Move(Vector2 moveInput)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = moveInput.normalized;
        moveCoroutine =  StartCoroutine(MoveCoroutine(accelerationTime ,moveDirection * moveSpeed));
        StopCoroutine(nameof(DeccelerationCoroutine));

        StartCoroutine(nameof(MovePositionLimitCoroutine));
    }

    void StopMove()
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveDirection = Vector2.zero;
        moveCoroutine = StartCoroutine(MoveCoroutine(deccelerationTime, moveDirection));
        StartCoroutine(nameof(DeccelerationCoroutine));
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity)
    {
        t = 0f;
        previousVelocity = playerRigidbody.velocity;
        // Quaternion previousRotation = transform.rotation

        while(t < time)
        {
            t += Time.fixedDeltaTime / time;
            playerRigidbody.velocity = Vector2.Lerp(previousVelocity, moveVelocity, t/time);

            //add moverotation variable in ienumerator
            // transform.rotation = Quaternion.Lerp(previousRotation, moveRotation, t/time);

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MovePositionLimitCoroutine()
    {
        while(true)
        {
            transform.position = Viewport.Instance.PlayerMoveablePosition(transform.position, paddingX, paddingY);

            yield return null;
        }
    }

    IEnumerator DeccelerationCoroutine()
    {
        yield return waitForDeccelerationTime;
        StopCoroutine(nameof(MovePositionLimitCoroutine));
    }
    #endregion

    #region FIRE
    void Fire()
    {
        muzzleVFX.Play();
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        muzzleVFX.Stop();
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while(true)
        {
            switch(weaponPower)
            {
                case 0:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleBottom.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile1, muzzleMiddle.position, Quaternion.identity);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile2, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(isOverdriving ? projectileOverdrive : projectile3, muzzleBottom.position, Quaternion.identity);
                    break;
                default:
                break;
            }

            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            yield return isOverdriving ? waitForOverdriveFireInterval : waitForFireInterval;
            
        }
    }
    #endregion

    #region OVERDRIVE
    void Overdrive()
    {
        if(!PlayerEnergy.Instance.IsEnough(PlayerEnergy.MAX)) return;

        PlayerOverdrive.on.Invoke();
    }

    void OverdriveOn()
    {
        isOverdriving = true;
        moveSpeed *= overDriveSpeedFactor;
        TimeController.Instance.BulletTime(slowMotionDuration, slowMotionDuration);
    }

    void OverdriveOff()
    {
        isOverdriving = false;
        moveSpeed /= overDriveSpeedFactor;

    }

    #endregion

    void LaunchMissile()
    {
        missile.Launch(muzzleMiddle);
    }
}

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
    [SerializeField] float paddingX = 0.2f;
    [SerializeField] float paddingY = 0.2f;
    #endregion

    #region Firing Variables
    [Header("FIRING")]
    [SerializeField] GameObject projectile1;
    [SerializeField] GameObject projectile2;
    [SerializeField] GameObject projectile3;

    [SerializeField] Transform muzzleMiddle;
    [SerializeField] Transform muzzleTop;
    [SerializeField] Transform muzzleBottom;


    [SerializeField, Range(0, 2)] int weaponPower = 0;
    [SerializeField] float fireInterval = 0.2f;
    WaitForSeconds waitForFireInterval;
    #endregion
    WaitForSeconds waitHealthRegenerateTime;

    Rigidbody2D playerRigidbody;

    Coroutine moveCoroutine;
    Coroutine healthRegenerateCoroutine;


    private void Awake() 
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void OnEnable() 
    {
        base.OnEnable();
        input.onMove += Move;    
        input.onStopMove += StopMove;
        input.onFire += Fire;
        input.onStopFire += StopFire;
    }

    private void OnDisable()
    {
        input.onMove -= Move;
        input.onStopMove -= StopMove;
        input.onFire -= Fire;
        input.onStopFire -= StopFire;

    }
    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody.gravityScale = 0;

        waitForFireInterval = new WaitForSeconds(fireInterval);
        waitHealthRegenerateTime = new WaitForSeconds(healthRegenerateTime);
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
        statsbar_HUD.UpdateStates(0f, maxHealth);
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        statsbar_HUD.UpdateStates(health, maxHealth);
        if(gameObject.activeSelf)
        {
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

    #region MOVE
    void Move(Vector2 moveInput)
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine =  StartCoroutine(MoveCoroutine(accelerationTime ,moveInput.normalized * moveSpeed));

        StartCoroutine(MovePositionLimitCoroutine());
    }

    void StopMove()
    {
        if(moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine(deccelerationTime, Vector2.zero));
        StopCoroutine(MovePositionLimitCoroutine());
    }

    IEnumerator MoveCoroutine(float time, Vector2 moveVelocity)
    {
        float t = 0f;

        while(t < time)
        {
            t += Time.fixedDeltaTime / time;
            playerRigidbody.velocity = Vector2.Lerp(playerRigidbody.velocity, moveVelocity, t/time);

            //add moverotation variable in ienumerator
            // transform.rotation = Quaternion.Lerp(transform.rotation, moveRotation, t/time);

            yield return null;
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
    #endregion

    #region FIRE
    void Fire()
    {
        StartCoroutine(nameof(FireCoroutine));
    }

    void StopFire()
    {
        StopCoroutine(nameof(FireCoroutine));
    }

    IEnumerator FireCoroutine()
    {
        while(true)
        {
            switch(weaponPower)
            {
                case 0:
                    PoolManager.Release(projectile1, muzzleMiddle.position, Quaternion.identity);
                    break;
                case 1:
                    PoolManager.Release(projectile1, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(projectile1, muzzleBottom.position, Quaternion.identity);
                    break;
                case 2:
                    PoolManager.Release(projectile1, muzzleMiddle.position, Quaternion.identity);
                    PoolManager.Release(projectile2, muzzleTop.position, Quaternion.identity);
                    PoolManager.Release(projectile3, muzzleBottom.position, Quaternion.identity);
                    break;
                default:
                break;
            }

            yield return waitForFireInterval;
        }
    }
    #endregion


}

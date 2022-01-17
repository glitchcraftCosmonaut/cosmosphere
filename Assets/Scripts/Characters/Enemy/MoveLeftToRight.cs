using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftToRight : MonoBehaviour
{
   public float moveSpeed = 5;
    [Header("FIRING")]
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected AudioData[] projectileLaunchSFX;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected ParticleSystem muzzleVFX;
    [SerializeField]float continuousFireDuration = 1.5f;
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    [SerializeField] protected float spawnYPos = 2f;
    float idleDelay = 4.5f;

    protected float paddingX;
    float paddingY;
    protected Vector3 targetPosition;
    List<GameObject> magazine;


    WaitForSeconds waitForContinuousFireInterval;
    WaitForSeconds waitForFireInterval;
    WaitForSeconds waitForIdleDelay;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();


    protected virtual void Awake() 
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        magazine = new List<GameObject>(projectiles.Length);

        waitForContinuousFireInterval = new WaitForSeconds(minFireInterval);
        waitForFireInterval = new WaitForSeconds(maxFireInterval);
        waitForIdleDelay = new WaitForSeconds(idleDelay);

        paddingX = size.x / 1f;
        paddingY = size.y / 2f;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveLeftToRighCoroutine));
        // StartCoroutine(nameof(ContinuousFireCoroutine));
        StartCoroutine(nameof(RandomlyFiresCoroutine));
        // StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    // private void FixedUpdate() 
    // {
    //     Vector2 pos = transform.position;

    //     pos.x -= moveSpeed * Time.fixedDeltaTime;

    //     // if(pos.x < -10)
    //     // {
    //     //     Destroy(gameObject);
    //     // }

    //     transform.position = pos;    
    // }

    IEnumerator MoveLeftToRighCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPositonFromLeft(-paddingX, paddingY + spawnYPos);

        targetPosition = Viewport.Instance.RightHalfPosition(paddingX);
        // targetPosition = new Vector3(-1.74f, transform.position.y, transform.position.z);

        while(gameObject.activeSelf)
        {
            //if not has arrive at location
            if(Vector3.Distance(transform.position, targetPosition) >= moveSpeed * Time.fixedDeltaTime)
            {
                //keep moving to target position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
                //make enemy rotate with x axis while moving
                // transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * moveRotationAngle, Vector3.right);
            }
            else
            {
                yield return waitForIdleDelay;
                //set new target position
                // targetPosition = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);
                targetPosition = Viewport.Instance.RandomEnemySpawnPositonFromLeft(-paddingX, paddingY + spawnYPos);
                if(transform.position.x < -9)
                {
                    gameObject.SetActive(false);
                    EnemyManager.Instance.RemoveFromList(gameObject);
                }
            }

            yield return waitForFixedUpdate;

        }
    }

    void LoadProjectile()
    {
        magazine.Clear();
            //launch projectile 2 or 3
        
        magazine.Add(projectiles[Random.Range(0, projectiles.Length)]);
            
    }
    IEnumerator RandomlyFiresCoroutine()
    {
        while(isActiveAndEnabled)
        {
            if(GameManager.GameState == GameState.GameOver) yield break;
            yield return waitForFireInterval;
            yield return StartCoroutine(nameof(ContinuousFireCoroutine));
        }
        
    }
    IEnumerator ContinuousFireCoroutine()
    {
        LoadProjectile();
        muzzleVFX.Play();
        float continuousFireTimer = 0f;
        if(GameManager.GameState == GameState.GameOver) yield break;


        while(continuousFireTimer < continuousFireDuration)
        {
            foreach(var projectile in magazine)
            {
                PoolManager.Release(projectile,muzzle.position);
            }

            continuousFireTimer += minFireInterval;
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);

            yield return waitForContinuousFireInterval;
        }
        muzzleVFX.Stop();
    }

}

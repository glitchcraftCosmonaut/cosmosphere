using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightLeft : MonoBehaviour
{
    public float moveSpeed = 5;
    [Header("FIRING")]
    [SerializeField] protected GameObject[] projectiles;
    [SerializeField] protected AudioData[] projectileLaunchSFX;
    [SerializeField] protected Transform muzzle;
    [SerializeField] protected ParticleSystem muzzleVFX;
    [SerializeField] protected float minFireInterval;
    [SerializeField] protected float maxFireInterval;
    protected float paddingX;
    float paddingY;
    protected Vector3 targetPosition;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();


    protected virtual void Awake() 
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;

        paddingX = size.x / 1f;
        paddingY = size.y / 2f;
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveRightToLeftCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
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

    IEnumerator MoveRightToLeftCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        while(gameObject.activeSelf)
        {
            Vector2 pos = transform.position;

            pos.x -= moveSpeed * Time.fixedDeltaTime;

            if(pos.x < -10)
            {
                gameObject.SetActive(false);
                EnemyManager.Instance.RemoveFromList(gameObject);
            }

            transform.position = pos;

            yield return waitForFixedUpdate;
        }
    }
    protected IEnumerator RandomlyFireCoroutine()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(Random.Range(minFireInterval, maxFireInterval));

            if(GameManager.GameState == GameState.GameOver) yield break;

            foreach (var projectile in projectiles)
            {
                PoolManager.Release(projectile, muzzle.position);
            }
            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
            muzzleVFX.Play();
        }
    }
}

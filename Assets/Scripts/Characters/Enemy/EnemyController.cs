using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("MOVEMENT")]
    
    [SerializeField] float moveSpeed = 2f;
    // [SerializeField] float moveRotationAngle;

    [Header("FIRING")]
    [SerializeField] GameObject[] projectiles;
    [SerializeField] AudioData[] projectileLaunchSFX;
    [SerializeField] Transform muzzle;
    [SerializeField] float minFireInterval;
    [SerializeField] float maxFireInterval;

    float paddingX;
    float paddingY;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake() 
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;

        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    private void OnEnable() 
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
        StartCoroutine(nameof(RandomlyFireCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        Vector3 targetPosition = Viewport.Instance.RandomRighHalfPosition(paddingX, paddingY);

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
                //set new target position
                targetPosition = Viewport.Instance.RandomRighHalfPosition(paddingX, paddingY);
            }

            yield return waitForFixedUpdate;

        }
    }

    private IEnumerator RandomlyFireCoroutine()
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
        }
    }
}

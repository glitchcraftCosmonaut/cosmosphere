using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElitarController : MonoBehaviour
{
    [Header("MOVEMENT")]
    
    [SerializeField] float moveSpeed = 2f;
    // // [SerializeField] float moveRotationAngle;

    // [Header("FIRING")]
    // [SerializeField] protected GameObject[] projectiles;
    // [SerializeField] protected AudioData[] projectileLaunchSFX;
    // [SerializeField] protected Transform muzzle;
    // [SerializeField] protected ParticleSystem muzzleVFX;
    // [SerializeField] protected float minFireInterval;
    // [SerializeField] protected float maxFireInterval;

    protected float paddingX;
    float paddingY;
    protected Vector3 targetPosition;

    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

    protected virtual void Awake() 
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;

        paddingX = size.x / 2f;
        paddingY = size.y / 2f;
    }

    protected virtual void OnEnable() 
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.Instance.RandomEnemySpawnPosition(paddingX, paddingY);

        targetPosition = Viewport.Instance.StayInRightToMiddlePos();

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
                targetPosition = Viewport.Instance.StayInRightToMiddlePos();
            }

            yield return waitForFixedUpdate;

        }
    }
}


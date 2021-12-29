using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;

    float ballisticAngle;

    Vector3 targetDirection;
    public IEnumerator HomingCoroutine(GameObject target)
    {
        ballisticAngle = Random.Range(minBallisticAngle,maxBallisticAngle);
        while(gameObject.activeSelf)
        {
            if(target.activeSelf)
            {
                //move to target
                targetDirection = target.transform.position - transform.position;
                //rotate to target
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg, Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f,0f, ballisticAngle);
                //move projectile
                projectile.Move();
            }
            else
            {
                //move projectile to move direction
                projectile.Move();
            }
            yield return null;
        }
    }
}

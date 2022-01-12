using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    float damage;
    int nrgValue;
    bool isNRGProjectiles;

    GameObject hitVFX;
    ParticleSystem system;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake() 
    {
        system = GetComponent<ParticleSystem>();
        hitVFX = GetComponentInParent<BulletHellSpawner>().hitVFX;
        damage = GetComponentInParent<BulletHellSpawner>().damage;
        nrgValue = GetComponentInParent<BulletHellSpawner>().nrgValue;
        isNRGProjectiles = GetComponentInParent<BulletHellSpawner>().isNRGProjectiles;
    }


    private void OnParticleCollision(GameObject collision)
    { 
        int events = system.GetCollisionEvents(collision, collisionEvents);
        if(collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if(isNRGProjectiles)
            {
                if(player.isProjectileActive == false)
                {
                    PlayerProjectileNRGSys.Instance.Obtain(nrgValue);
                    PoolManager.Release(hitVFX, collisionEvents[0].intersection, Quaternion.LookRotation(collisionEvents[0].normal));
                }
                else
                {
                    player.TakeDamage(damage);
                    PoolManager.Release(hitVFX, collisionEvents[0].intersection, Quaternion.LookRotation(collisionEvents[0].normal));
                }
            }
            else
            {
                player.TakeDamage(damage);
                PoolManager.Release(hitVFX, collisionEvents[0].intersection, Quaternion.LookRotation(collisionEvents[0].normal));
            }

        }
    }
}

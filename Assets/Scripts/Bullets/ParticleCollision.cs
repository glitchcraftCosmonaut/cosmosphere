using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    float damage;

    GameObject hitVFX;
    ParticleSystem system;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake() 
    {
        system = GetComponent<ParticleSystem>();
        hitVFX = GetComponentInParent<BulletHellSpawner>().hitVFX;
        damage = GetComponentInParent<BulletHellSpawner>().damage;
    }


    private void OnParticleCollision(GameObject collision)
    { 
        int events = system.GetCollisionEvents(collision, collisionEvents);
        Debug.Log("Hit");
        if(collision.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);
            PoolManager.Release(hitVFX, collisionEvents[0].intersection, Quaternion.LookRotation(collisionEvents[0].normal));
        }

        // for(int i = 0; i < events; i++)
        // {
        //     PoolManager.Release(hitVFX, collisionEvents[i].intersection, Quaternion.LookRotation(collisionEvents[i].normal));
        // }
    }
}

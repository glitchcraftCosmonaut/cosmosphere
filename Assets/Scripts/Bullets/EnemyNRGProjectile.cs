using System.Collections;
using UnityEngine;

public class EnemyNRGProjectile : MonoBehaviour
{
    [SerializeField] float damage = 50f;
    [SerializeField] int nrgValue = 50;
    [SerializeField] GameObject hitVFX;
    [SerializeField] protected float moveSpeed = 10f;

    [SerializeField] protected Vector2 moveDirection;
    void OnEnable() 
    {
        StartCoroutine(MoveDirectlyCoroutine());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            if(player.isProjectileActive == false)
            {
                PlayerProjectileNRGSys.Instance.Obtain(nrgValue);
                PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
                gameObject.SetActive(false);
            }
            else
            {
                player.TakeDamage(damage);
                PoolManager.Release(hitVFX, collision.GetContact(0).point, Quaternion.LookRotation(collision.GetContact(0).normal));
                gameObject.SetActive(false);
            }
            
        }
    }
    IEnumerator MoveDirectlyCoroutine()
    {
        while(gameObject.activeSelf)
        {
            Move();
            yield return null;
        }
    }
    public void Move() => transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioData[] deathSFX;

    [Header("===Material===")]
    [SerializeField] protected Material hurtMat;
    protected SpriteRenderer sp;
    protected Material defaultMat2D;
    

    [Header("HEALTH SYSTEM")]
    [SerializeField] protected float maxHealth;
    protected float health;

    // private void Awake() {
    //     sp = GetComponentInChildren<SpriteRenderer>();
    //     defaultMat2D = GetComponentInChildren<SpriteRenderer>().material;
    // }

    protected virtual void OnEnable()
    {
        health = maxHealth;
    }
    // protected virtual void OnDisable()
    // {
    //     StopCoroutine(HurtEffectController.Instance.HurtEffect());
    // }
    public virtual void TakeDamage(float damage)
    {
        if(health == 0f) return;
        StartCoroutine(HurtEffect());
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
        
    }

    public virtual void Die()
    {
        health = 0;
        AudioManager.Instance.PlayRandomSFX(deathSFX);
        PoolManager.Release(deathVFX, transform.position);
        gameObject.SetActive(false);
    }

    public virtual void RestoreHealth(float value)
    {
        if(health == maxHealth) return;
        // health += value;
        // health = Mathf.Clamp(health, 0f, maxHealth);
        health = Mathf.Clamp(health + value, 0f, maxHealth);
    }

    protected IEnumerator HealthRegenerationCoroutine(WaitForSeconds waitTime, float percent)
    {
        while(health < maxHealth)
        {
            yield return waitTime;

            RestoreHealth(maxHealth * percent);
        }
    }

    protected IEnumerator DamageOverTimeCoroutine(WaitForSeconds waitTime, float percent)
    {
        while(health > 0f)
        {
            yield return waitTime;

            TakeDamage(maxHealth * percent);
        }
    }

    protected virtual IEnumerator HurtEffect()
    {
        sp.material = hurtMat;
        yield return new WaitForSeconds(0.1f);
        sp.material = defaultMat2D;
    }

}

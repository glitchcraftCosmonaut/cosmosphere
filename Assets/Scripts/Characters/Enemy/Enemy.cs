using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;
    [SerializeField] protected int healthFactor;
    // [SerializeField] Material hurtMat;
    // private SpriteRenderer sp;
    // private Material defaultMat2D;

    LootSpawner lootSpawner;

    protected virtual void Awake()
    {
        lootSpawner = GetComponent<LootSpawner>();
        sp = GetComponentInChildren<SpriteRenderer>();
        defaultMat2D = GetComponentInChildren<SpriteRenderer>().material;
    }

    protected override void OnEnable()
    {
        SetHealth();
        sp.material = defaultMat2D;
        base.OnEnable();
    }

    // protected virtual void OnCollisionEnter2D(Collision2D other)
    // {
    //     if(other.gameObject.TryGetComponent<Player>(out Player player))
    //     {
    //         player.TakeDamage(50);
    //         Die();
    //     }
    // }

    public override void TakeDamage(float damage)
    {
        // if(gameObject.activeSelf)
        // {
        //     StartCoroutine(HurtEffect());
        // }
        base.TakeDamage(damage);
    }

    public override void Die()
    {
        StopCoroutine(nameof(HurtEffect));
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        lootSpawner.Spawn(transform.position);
        base.Die();
    }

    protected virtual void SetHealth()
    {
        maxHealth += (int)(EnemyManager.Instance.WaveNumber / healthFactor);
    }

    // IEnumerator HurtEffect()
    // {
    //     sp.material = hurtMat;
    //     yield return new WaitForSeconds(0.2f);
    //     sp.material = defaultMat2D;
    // }
}

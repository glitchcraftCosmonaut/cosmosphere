using UnityEngine;

public class Enemy : Character
{
    [SerializeField] int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;
    [SerializeField] protected int healthFactor;
    bool isPlayer;

    LootSpawner lootSpawner;

    protected virtual void Awake()
    {
        lootSpawner = GetComponent<LootSpawner>();
        isPlayer = TryGetComponent<Player>(out Player player);
    }

    protected override void OnEnable()
    {
        SetHealth();
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

    public override void Die()
    {
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
}

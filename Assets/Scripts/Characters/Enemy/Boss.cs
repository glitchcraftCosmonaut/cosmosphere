using UnityEngine;

public class Boss : Enemy
{
    BossHealthBar healthBar;
    Canvas healthBarCanvas;

    private void Awake()
    {
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);
        healthBarCanvas.enabled = true;
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<Player>(out Player player))
        {
            player.Die();
            Die();
        }
    }

    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        healthBar.UpdateStates(health, maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;
    }
}

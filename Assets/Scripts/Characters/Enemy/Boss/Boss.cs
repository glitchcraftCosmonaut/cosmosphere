using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    BossHealthBar healthBar;
    [SerializeField] string nameText;
    // [SerializeField] Material hurtMat;
    // private SpriteRenderer sp;
    // private Material defaultMat2D;
    Canvas healthBarCanvas;

    protected override void Awake()
    {
        base.Awake();
        healthBar = FindObjectOfType<BossHealthBar>();
        // sp = GetComponentInChildren<SpriteRenderer>();
        // defaultMat2D = GetComponentInChildren<SpriteRenderer>().material;
        // nameText = healthBar.bossName;
        healthBarCanvas = healthBar.GetComponentInChildren<Canvas>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        healthBar.Initialize(health, maxHealth);
        healthBar.bossName.text = nameText;
        healthBarCanvas.enabled = true;
    }
    // protected override void OnCollisionEnter2D(Collision2D other)
    // {
    //     if(other.gameObject.TryGetComponent<Player>(out Player player))
    //     {
    //         player.Die();
    //         Die();
    //     }
    // }

    public override void Die()
    {
        healthBarCanvas.enabled = false;
        base.Die();
    }

    public override void TakeDamage(float damage)
    {
        StartCoroutine(nameof(HurtEffect));
        base.TakeDamage(damage);
        healthBar.UpdateStates(health, maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;
    }

    // IEnumerator HurtEffect()
    // {
    //     sp.material = hurtMat;
    //     yield return new WaitForSeconds(0.2f);
    //     sp.material = defaultMat2D;
    // }
}

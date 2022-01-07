using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : Enemy
{
    BossHealthBar healthBar;
    [SerializeField] string nameText;
    Canvas healthBarCanvas;
    // [SerializeField] Material hurtMat;
    // private MeshRenderer mp;
    // private Material defaultMat3D;

    protected override void Awake()
    {
        base.Awake();
        // mp = GetComponentInChildren<MeshRenderer>();
        // defaultMat3D = GetComponentInChildren<MeshRenderer>().material;
        healthBar = FindObjectOfType<BossHealthBar>();
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
        // StartCoroutine(nameof(HurtEffect));
        healthBar.UpdateStates(health, maxHealth);
    }

    protected override void SetHealth()
    {
        maxHealth += EnemyManager.Instance.WaveNumber * healthFactor;
    }

    // IEnumerator HurtEffect()
    // {
    //     mp.material = hurtMat;
    //     yield return new WaitForSeconds(0.2f);
    //     mp.material = defaultMat3D;
    // }
}
using UnityEngine;

public class Enemy : Character
{
    int scorePoint = 100;
    [SerializeField] int deathEnergyBonus = 3;

    public override void Die()
    {
        ScoreManager.Instance.AddScore(scorePoint);
        PlayerEnergy.Instance.Obtain(deathEnergyBonus);
        EnemyManager.Instance.RemoveFromList(gameObject);
        base.Die();
    }
}

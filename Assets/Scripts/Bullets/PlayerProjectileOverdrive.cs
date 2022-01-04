using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    TrailRenderer trail;
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;

    protected override void Awake()
    {
        trail = GetComponentInChildren<TrailRenderer>();
    }
    protected override void OnEnable()
    {
        SetTarget(EnemyManager.Instance.RandomEnemy);
        transform.rotation = Quaternion.identity;

        if(target == null)
        {
            base.OnEnable();
        }
        else
        {
            //track target
            StartCoroutine(guidanceSystem.HomingCoroutine(target));
        }
    }

    private void OnDisable()
    {
        trail.Clear();
    }
}

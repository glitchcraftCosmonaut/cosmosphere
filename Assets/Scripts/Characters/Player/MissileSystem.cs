using System.Collections;
using UnityEngine;

public class MissileSystem : MonoBehaviour
{
    [SerializeField] int defaultAmount = 5;
    [SerializeField] float cooldownTime = 1f;
    [SerializeField] GameObject missilePrefab = null;
    [SerializeField] AudioData launchSFX = null;

    bool isReady = true;

    int amount;

    private void Awake()
    {
        amount = defaultAmount;
    }
    
    private void Start()
    {
        MissileDisplay.UpdateAmountText(amount);
    }

    public void Launch(Transform muzzleTransform)
    {
        if(amount == 0 || !isReady) return;

        isReady = false;
        PoolManager.Release(missilePrefab, muzzleTransform.position);

        AudioManager.Instance.PlayRandomSFX(launchSFX);
        amount--;

        MissileDisplay.UpdateAmountText(amount);

        if(amount == 0)
        {
            MissileDisplay.UpdateCooldownImage(1f);
        }
        else
        {
            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {
        var cooldownValue = cooldownTime;

        while(cooldownValue > 0f)
        {
            MissileDisplay.UpdateCooldownImage(cooldownValue / cooldownTime);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);

            yield return null;
        }

        isReady = true;
    }
}

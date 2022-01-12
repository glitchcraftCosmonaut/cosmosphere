using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSystem : MonoBehaviour
{
    [Header("===BlADE SYSTEM===")]
    [SerializeField] float bladeCoolDown = 1f;
    Animator slashAnimator;
    [SerializeField] AudioData slashSFX = null;
    [SerializeField] AudioData hitSFX = null;

    [SerializeField] protected float damage;

    public bool isBladeReady = true;
    bool comboPossible;
    int comboStep;

    private void Awake()
    {
        slashAnimator = GetComponent<Animator>();
    }

    #region ATTACK COMBO TODO THIS ONE NOT WORK PROPERLY
    //GOT SOME BUGS REALY BIG BUGS, THIS COMBO USING ANIMATION EVENT
    public void Attack()
    {
        // if(!isBladeReady) return;
        // isBladeReady = false;
        if(comboStep == 0)
        {
            slashAnimator.Play("PlayerSlash1");
            comboStep = 1;
            return;
        }
        if(comboStep != 0)
        {
            if(comboPossible)
            {
                comboPossible = false;
                comboStep += 1;
            }
        }
    }

    public void ComboPossible()
    {
        comboPossible = true;
    }

    public void Combo()
    {
        if(comboStep == 2)
        {
            slashAnimator.Play("PlayerSlash2");
        }
        if(comboStep == 3)
        {
            slashAnimator.Play("PlayerSlash3");
        }
    }

    public void ComboReset()
    {
        comboPossible = false;
        // isBladeReady = false;
        comboStep = 0;
        // StartCoroutine(SlashCoolDownCoroutine());
    }
    #endregion

    public void Slashing()
    {
        if(!isBladeReady) return;
        isBladeReady = false;
        slashAnimator.Play("PlayerSlash1");
        AudioManager.Instance.PlayRandomSFX(slashSFX);
        StartCoroutine(SlashCoolDownCoroutine());

    }
    IEnumerator SlashCoolDownCoroutine()
    {
        var cooldownValue = bladeCoolDown;

        while(cooldownValue > 0f)
        {
            BladeDisplayUI.UpdateCooldownImage(cooldownValue / bladeCoolDown);
            cooldownValue = Mathf.Max(cooldownValue - Time.deltaTime, 0f);            

            yield return null;
        }

        isBladeReady = true;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.TryGetComponent<Character>(out Character character))
        {
            character.TakeDamage(damage);

            // var contactPoint = collision.GetContact(0);
            AudioManager.Instance.PlayRandomSFX(hitSFX);
            // gameObject.SetActive(false);
        }
    }
}

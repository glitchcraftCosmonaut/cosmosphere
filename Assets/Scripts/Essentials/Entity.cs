using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float HP = 5.0f;
    public AudioClip DeathSound;
    public float DeathVolume;
    public bool IsDead = false;
    public int ScoreValue = 10;

    public void Damage(float Damage)
    {
        HP -= Damage;
        if (HP <= 0)
        {
            if (!IsDead)
            {
                IsDead = true;
                Die();
            }
        }
    }

    public virtual void Die()
    {
        
        Destroy(gameObject);
        PlaySound();

        Score.ScoreVal += ScoreValue;
    }

    public void PlaySound()
    {
        if (DeathSound == null) return;
        GameObject newAC = new GameObject();
        AudioSource audioSource = newAC.AddComponent<AudioSource>();
        audioSource.volume = DeathVolume;
        audioSource.PlayOneShot(DeathSound);
        StartCoroutine(DestroyOverSeconds(DeathSound.length + 0.1f, newAC));
        DontDestroyOnLoad(newAC);

    }

    public IEnumerator DestroyOverSeconds(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        Destroy(go);
    }
}

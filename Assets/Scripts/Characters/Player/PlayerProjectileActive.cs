using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProjectileActive : MonoBehaviour
{
    public static UnityAction on = delegate {};
    public static UnityAction off = delegate {};
    [SerializeField] AudioData onSFX;
    [SerializeField] AudioData offSFX;

    private void Awake() 
    {
        on += On;
        off += Off;
    }

    private void OnDestroy()
    {
        on -= On;
        off -= Off;
    }

    void On()
    {
        AudioManager.Instance.PlayRandomSFX(onSFX);
    }

    void Off()
    {
        AudioManager.Instance.PlayRandomSFX(offSFX);
    }
}

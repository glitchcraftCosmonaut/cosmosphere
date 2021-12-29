using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : Singleton<PlayerEnergy>
{
    [SerializeField] EnergyBar energyBar;
    [SerializeField] float overdriveInterval = 0.1f;

    bool available = true;
    public const int MAX = 100;
    public const int PERCENT = 1;
    int energy;

    WaitForSeconds waitForOverdriveInterval;

    protected override void Awake()
    {
        base.Awake();
        waitForOverdriveInterval = new WaitForSeconds(overdriveInterval);
    }

    private void OnEnable()
    {
        PlayerOverdrive.on += PlayerOverdriveOn;
        PlayerOverdrive.off += PlayerOverdriveOff;
    }

    private void Start()
    {
        energyBar.Initialize(energy, MAX);
    }

    public void Obtain(int value)
    {
        if(energy == MAX || !available || !gameObject.activeSelf) return;

        // energy += value;
        energy = Mathf.Clamp(energy + value, 0, MAX);
        energyBar.UpdateStates(energy,MAX);
    }

    public void Use(int value)
    {
        energy -= value;
        energyBar.UpdateStates(energy,MAX);

        if(energy == 0 && !available)
        {
            PlayerOverdrive.off.Invoke();
        }
    }

    public bool IsEnough(int value) { return energy >= value; }

    void PlayerOverdriveOn()
    {
        available = false;
        StartCoroutine(nameof(KeepUsingCoroutine));
    }

    void PlayerOverdriveOff()
    {
        available = true;
        StopCoroutine(nameof(KeepUsingCoroutine));
    }

    IEnumerator KeepUsingCoroutine()
    {
        while(gameObject.activeSelf && energy > 0)
        {
            //every 0.1 secons
            yield return waitForOverdriveInterval;

            //use 1 percent max energy , every 1 seconds use 10 percent of max energy
            //that means overdrive last 10 seconds
            Use(PERCENT);
        }
    }
}

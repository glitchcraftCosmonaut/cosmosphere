using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptablesObjects/Wave", order = 1)]
public class Wave : ScriptableObject
{

    [field : SerializeField] public int count {get; private set;}

    [field : SerializeField] public float rate{get; private set;}

    [field : SerializeField] public float waveTimer{get; private set;}

    // [field : SerializeField] public Transform[] SpawnPos{get; private set;}
    // [Header("Enemy Prefabs")]
    [field : SerializeField] public GameObject[] enemy{get; private set;}
}

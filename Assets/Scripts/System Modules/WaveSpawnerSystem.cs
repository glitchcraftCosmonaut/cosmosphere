using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WaveSpawnerSystem : MonoBehaviour
{
    [SerializeField] public Wave[] waves;
    [SerializeField] public Transform[] spawnPos;
    [SerializeField] public float timeBetweemWaves = 3f;
    [SerializeField] public bool spawn = true;

    [SerializeField] public bool wavespawn = true;

    [SerializeField] public int nextwave = 0;



    void Awake()

    {
        if (spawn)
        {
            StartCoroutine(DoSpawn());
        }
    }

    IEnumerator DoSpawn()

    {
        while (nextwave <= waves.Length)
        {
            if (wavespawn)
            {
                yield return new WaitForSeconds(timeBetweemWaves);

                for (int i = 0; i < waves[nextwave].enemy.Length; i++)
                {
                    for (int j = 0; j < waves[nextwave].count; j++)
                    {
                        yield return new WaitForSeconds(waves[nextwave].rate);
                        SpawnUnit(waves[nextwave].enemy[i], waves[nextwave]);
                    }
                }
                wavespawn = false;
            }
            yield return new WaitForSeconds(waves[nextwave].waveTimer);
            if (nextwave + 1 > waves.Length - 1)
            {
                Debug.Log("waves completed");
                spawn = false;
                //todo stage complete move to next stage

                yield break;
            }
            else
            {
                nextwave++;
                wavespawn = true;
            }
        }
    }
    void SpawnUnit(GameObject enemy, Wave _wave)
    {
        for(int i = 0; i < spawnPos.Length; i++)
            // Instantiate(enemy, _wave.spawnPos[i].position, Quaternion.identity);
            Instantiate(enemy, spawnPos[i].position, Quaternion.identity);
    }
}
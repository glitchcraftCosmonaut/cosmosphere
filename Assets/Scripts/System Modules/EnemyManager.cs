using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    public GameObject RandomEnemy => enemyList.Count == 0 ? null : enemyList[Random.Range(0, enemyList.Count)];
    public int WaveNumber => waveNumber;
    public float TimeBetweenWaves => timeBetweenWaves;
    [SerializeField] bool spawnEnemy = true;
    [SerializeField] GameObject waveUI;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float timeBetweenWaves = 1f;
    [SerializeField] float timeUIWarning = 1f;
    [SerializeField] int minEnemyAmount = 4;
    [SerializeField] int maxEnemyAmount = 10;
    [SerializeField] GameObject bossPrefab;
    [SerializeField] int bossWaveNumber;

    WaitForSeconds waitTimeBetweenSpawns;
    WaitForSeconds waitTimeBetweenWaves;
    WaitForSeconds waitUIWarning;
    WaitUntil waitUntilNoEnemy;

    int waveNumber = 1;
    int enemyAmount;

    List<GameObject> enemyList;

    protected override void Awake()
    {
        base.Awake();
        enemyList = new List<GameObject>();
        waitTimeBetweenSpawns = new WaitForSeconds(timeBetweenSpawns);
        waitTimeBetweenWaves = new WaitForSeconds(timeBetweenWaves);
        waitUIWarning = new WaitForSeconds(timeUIWarning);
        waitUntilNoEnemy = new WaitUntil(() => enemyList.Count ==0);
    }


    IEnumerator Start()
    {
        while(spawnEnemy && GameManager.GameState != GameState.GameOver)
        {
            yield return waitTimeBetweenWaves;
            yield return  StartCoroutine(nameof(RandomlySpawnCoroutine));
        }
    }

    IEnumerator RandomlySpawnCoroutine()
    {
        if(waveNumber % bossWaveNumber == 0)
        {
            waveUI.SetActive(true);
            yield return waitUIWarning;
            waveUI.SetActive(false);
            var boss = PoolManager.Release(bossPrefab);
            enemyList.Add(boss);
        }
        else
        {
            enemyAmount = Mathf.Clamp(enemyAmount, minEnemyAmount + waveNumber / bossWaveNumber, maxEnemyAmount);

            for(int i = 0; i < enemyAmount; i++)
            {
                enemyList.Add(PoolManager.Release(enemyPrefab[Random.Range(0, enemyPrefab.Length)]));

                yield return waitTimeBetweenSpawns;
            }

        }
        yield return waitUntilNoEnemy;

        waveNumber++;

    }

    public void RemoveFromList(GameObject enemy)
    {  
        enemyList.Remove(enemy);
    }
}

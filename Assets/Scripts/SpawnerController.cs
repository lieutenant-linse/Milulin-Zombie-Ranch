using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    //set Enemys with Unity Editor
    public GameObject EnemyPrefab;

    public GameObject EnemyPrefab2;


    //Enemy Game Flow preferences
    public int maxEnemys = 100;

    public int enemyCounter = 0;


    //Spawner settings
    public float Radius = 1;

    public float spawnDelay;

    private float lastSpawn;

    //three different spawning positions (on each island one)
    public Vector3 pos = new Vector3(-4.5f, 12f, 0f);
    public Vector3 pos2 = new Vector3(-33f, -7f, 0f);
    public Vector3 pos3 = new Vector3(-30f, 21f, 0f);


    void Start()
    {
        StartCoroutine(SpawnTimer());
    }

    // Update is called once per frame
    void Update()
    {
        //SpawnTimer starts only after LastSpawn + SpawnDelay
        if (Time.time > lastSpawn + spawnDelay)
        {
            StartCoroutine(SpawnTimer());
            lastSpawn = Time.time;
        }
    }

    private IEnumerator SpawnTimer()
    { 
        while (true) 
        {
            //Wait for another 2-4 Seconds for randomness
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            //Enemys spwaning in three different locations
            SpawnObjectAtRandom();

            SpawnObjectAtRandom2();

            SpawnObjectAtRandom3();
        }
    }

    //If there aren't too many Enemys on the field (MaxEnemys) then a enemy prefab is instantiated
    public void SpawnObjectAtRandom()
    {
        if (enemyCounter < maxEnemys)
        {
            Instantiate(EnemyPrefab, pos, Quaternion.identity);
            enemyCounter++;
        }
    }

    //If there aren't too many Enemys on the field (MaxEnemys) then a enemy prefab is instantiated
    public void SpawnObjectAtRandom2()
    {
        if(enemyCounter < maxEnemys)
        {
            Instantiate(EnemyPrefab2, pos2, Quaternion.identity);
            enemyCounter++;
        }
    }

    //If there aren't too many Enemys on the field (MaxEnemys) then teo enemy prefasb are instantiated
    public void SpawnObjectAtRandom3()
    {
        if (enemyCounter < maxEnemys)
        {
            Instantiate(EnemyPrefab, pos3, Quaternion.identity);
            Instantiate(EnemyPrefab2, pos3, Quaternion.identity);
            enemyCounter+=2;
        }
    }


    //A Blue Circle Around the one Spwan Area - only relevant for development
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

       
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}

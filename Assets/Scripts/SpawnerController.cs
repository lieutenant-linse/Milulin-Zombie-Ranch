using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject EnemyPrefab;

    public GameObject EnemyPrefab2;

    public int maxEnemys = 100;

    public int enemyCounter = 0;

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
        //if (Input.GetKeyDown(KeyCode.Space))
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
            yield return new WaitForSeconds(Random.Range(4f, 6f));
            SpawnObjectAtRandom();

            yield return new WaitForSeconds(Random.Range(4f, 6f));
            SpawnObjectAtRandom2();

            yield return new WaitForSeconds(Random.Range(2f, 4f));
            SpawnObjectAtRandom3();
        }
    }

    public void SpawnObjectAtRandom()
    {
        //Vector3 randomPos = Random.insideUnitCircle * Radius;

        //Debug.Log(randomPos);

        if (enemyCounter < maxEnemys)
        {
            Instantiate(EnemyPrefab, pos, Quaternion.identity);
            enemyCounter++;
        }
    }


    public void SpawnObjectAtRandom2()
    {
        if(enemyCounter < maxEnemys)
        {
            Instantiate(EnemyPrefab2, pos2, Quaternion.identity);
            enemyCounter++;
        }
    }

    public void SpawnObjectAtRandom3()
    {
        if (enemyCounter < maxEnemys)
        {
            Instantiate(EnemyPrefab, pos3, Quaternion.identity);
            Instantiate(EnemyPrefab2, pos3, Quaternion.identity);
            enemyCounter+=2;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

       
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}

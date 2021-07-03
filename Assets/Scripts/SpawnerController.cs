using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float Radius = 1;

    public Vector3 pos = new Vector3 (-4.5f, 12f, 0f);

    public float spawnDelay;

    private float lastSpawn;



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
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        SpawnObjectAtRandom();
    }

    void SpawnObjectAtRandom()
    {
        Vector3 randomPos = Random.insideUnitCircle * Radius;

        Debug.Log(randomPos);

        

        Instantiate(EnemyPrefab, pos, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

       
        Gizmos.DrawWireSphere(this.transform.position, Radius);
    }
}

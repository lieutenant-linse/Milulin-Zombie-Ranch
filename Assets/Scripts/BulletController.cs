using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);

        }

        if (collision.tag == "Enemy2")
        {
            collision.gameObject.GetComponent<EnemyController2>().Death();
            Destroy(gameObject);

        }
    }
}

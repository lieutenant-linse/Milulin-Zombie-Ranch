using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Animator playerAnim;


    public float lifeTime;

    public bool isEnemyBullet = false;

    // Here we save the position in the last update and current position of the bullet
    private Vector2 lastPos;
    private Vector2 curPos;

    private Vector2 playerPos;


    // Start is called before the first frame update
    void Start()
    {
        // Every bullet has a maximum lifetime, after which it gets destroyed
        StartCoroutine(DeathDelay());

        // The players bullets can be changed in size by collecting the respective item
        if(!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    private void Awake()
    {
        // Vllt brauche ich das hier nicht, wenn ich die Animation in den Player auslagern kann
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy bullets get shot towards the current position of the player, if it reaches the position (doesn´t move anymore) it gets destroyed
        if (isEnemyBullet)
        {
            curPos = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 10f * Time.deltaTime);
            if(curPos == lastPos)
            {
                Destroy(gameObject);
                playerAnim.SetBool("Player_Hit", false);
            }
            lastPos = curPos;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    // Enemy-Bullets only damage the player and player-bullets only damage enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Enemy" && !isEnemyBullet)
        {
            collision.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);

        }

        if (collision.tag == "Enemy2" && !isEnemyBullet)
        {
            collision.gameObject.GetComponent<EnemyController2>().Death();
            Destroy(gameObject);

        }

        if (collision.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            StartCoroutine(PlayerHitAnimation());       
        }
    }

    //versuchen in Player auszulagern?
    private IEnumerator PlayerHitAnimation()
    {

        playerAnim.SetBool("Player_Hit", true);
        yield return new WaitForSeconds(0.15f);
        playerAnim.SetBool("Player_Hit", false);
        
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Wander,
    Follow,
    Die,
    Attack
};


public class EnemyController : MonoBehaviour
{

    //necessary GameObjects / Components
    GameObject player;

    private Animator playerAnim; //defined in Awake()

    public GameObject bulletPrefab;

    public EnemyState currState = EnemyState.Wander;

    public Rigidbody2D rb_enemy;

    public Animator animator;


    //Properties for controlling Game Flow
    public float attackRange;

    public bool coolDownAttack;

    public float attackDelay;

    public float range; //between enemy and player

    public float speed;

    public float bulletSpeed;

    private bool chooseDir = false;



    //different variables for Wander and Follow State to differntiate in the animator
    private Vector2 movement;
    private Vector2 follow_movement;


    //Ludowig
    private void Awake()
    {
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        //different states for different behaviour from the Enemy
        switch (currState)
        {
            case(EnemyState.Wander):
                Wander();
                break;
            case(EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                break;
            case (EnemyState.Attack):
                Attack();
                break;

        }

        //if the enemy is close enough to the Player it changes state to follow the player
        if(IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }

        //if the enemy isn't close enough to the Player it changes state to wander around
        else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
        }


        //Defining the values for the animation of the enemy
        if (movement != Vector2.zero && currState != EnemyState.Die)
        {
            animator.SetFloat("Horizontal_Enemy", movement.x);
            animator.SetFloat("Vertical_Enemy", movement.y);

        }

        if(currState != EnemyState.Die)
        {
            animator.SetFloat("Speed_Enemy", movement.sqrMagnitude);
        }
        

        if (follow_movement != Vector2.zero && currState != EnemyState.Die)
        {
            animator.SetFloat("Horizontal_Enemy", follow_movement.x);
            animator.SetFloat("Vertical_Enemy", follow_movement.y);

            animator.SetFloat("Speed_Enemy", follow_movement.sqrMagnitude);
        }

        if(Vector3.Distance(transform.position, player.transform.position) <= attackRange && currState != EnemyState.Die && !coolDownAttack)
        {
            currState = EnemyState.Attack;
        }


    }


    //True/False if the Enemy is in Range to follow the Player - Depending on predefined range variable
    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }



    //A Coroutine to generate a random direction (-1,0,1) for the enemy in Wander-State
    private IEnumerator ChooseDirection()
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(1f, 3f)); //to make the Enemy look like it's decinding where to go next
        follow_movement = Vector2.zero;
        movement = new Vector2(Random.Range(-1,2), Random.Range(-1, 2));
        chooseDir = false;
    }


    void Wander()
    {
        //Starts the ChooseDirection() only when there isn't Coroutine already running
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        //Enemy Wanders in the direction, wich was generated before
        rb_enemy.MovePosition(rb_enemy.position + movement * speed * Time.fixedDeltaTime);


        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    //The Enemy moves towards the player
    //depending on the realtive positions of Enemy and player, the Enemy looks in different directions
    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if(transform.position.x <= player.transform.position.x && transform.position.y <= player.transform.position.y)
        {
            follow_movement.x = 1;
            follow_movement.y = 0;
        } else if (transform.position.x >= player.transform.position.x && transform.position.y >= player.transform.position.y)
            {
                follow_movement.x = -1;
                follow_movement.y = 0;
            }  else if (transform.position.x >= player.transform.position.x && transform.position.y <= player.transform.position.y)
                {
                    follow_movement.x = 0;
                    follow_movement.y = 1;
                }  else if (transform.position.x <= player.transform.position.x && transform.position.y >= player.transform.position.y)
                    {
                        follow_movement.x = 0;
                        follow_movement.y = -1;
                    }
    }


    void Attack()
    {
        if(!coolDownAttack)
        {
            StartCoroutine(RangedAttackAnimation());
            StartCoroutine(CoolDown());
        }
    }


    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.GetComponent<BulletController>().GetPlayer(player.transform);
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<BulletController>().isEnemyBullet = true;
    }


    private IEnumerator RangedAttackAnimation()
    {
        animator.SetBool("Shoot_Enemy", true);
        yield return new WaitForSeconds(0.65f);
        ShootBullet();
        yield return new WaitForSeconds(0.25f);
        animator.SetBool("Shoot_Enemy", false);
    }


    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(attackDelay);
        coolDownAttack = false;
    }


    public void Death()
    {
        animator.SetTrigger("Death_Enemy");
        currState = EnemyState.Die;
        StartCoroutine(DeathDelay());

    }


    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(0.3f);
        ScoreController.instance.AddPoint();
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerController>().enemyCounter -= 1; //important for the the maximum of Enemys on the Map at a time
        Destroy(gameObject);
    }
}


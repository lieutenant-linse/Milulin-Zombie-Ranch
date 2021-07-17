using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Different states for different kinds of actions, actions are selected in e.g. the update-method depending on the active state
public enum EnemyState2
{
    Wander,
    Follow,
    Die,
    Attack
};

public class EnemyController2 : MonoBehaviour
{

    //necessary GameObjects / Components
    GameObject player;

    private Animator playerAnim;


    public EnemyState2 currState = EnemyState2.Wander;

    public Rigidbody2D rb_enemy;

    public Animator animator;


    //Properties for controlling Game Flow
    public float attackRange;

    public bool coolDownAttack;

    public float attackDelay;

    public float range; //between enemy and player

    public float speed;

    private bool chooseDir = false;




    //different variables for Wander and Follow State to differntiate in the animator
    private Vector2 movement;
    private Vector2 follow_movement;



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
            case (EnemyState2.Wander):
                Wander();
                break;
            case (EnemyState2.Follow):
                Follow();
                break;
            case (EnemyState2.Die):
                break;
            case (EnemyState2.Attack):
                Attack();
                break;
        }

        //if the enemy is close enough to the Player it changes state to follow the player
        if (IsPlayerInRange(range) && currState != EnemyState2.Die)
        {
            currState = EnemyState2.Follow;
        }

        //if the enemy isn't close enough to the Player it changes state to wander around
        else if (!IsPlayerInRange(range) && currState != EnemyState2.Die)
        {
            currState = EnemyState2.Wander;
        }


        //Defining the values for the animation of the enemy
        if (movement != Vector2.zero && currState != EnemyState2.Die)
        {
            animator.SetFloat("Horizontal_Enemy_2", movement.x);
            animator.SetFloat("Vertical_Enemy_2", movement.y);
        }

        if(currState != EnemyState2.Die)
        {
            animator.SetFloat("Speed_Enemy_2", movement.sqrMagnitude);
        }
        

        if (follow_movement != Vector2.zero && currState != EnemyState2.Die)
        {
            animator.SetFloat("Horizontal_Enemy_2", follow_movement.x);
            animator.SetFloat("Vertical_Enemy_2", follow_movement.y);

            animator.SetFloat("Speed_Enemy_2", follow_movement.sqrMagnitude);
        }

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange && currState != EnemyState2.Die && !coolDownAttack)
        {
            currState = EnemyState2.Attack;
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
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        //new way to move randonmly to map to animation
        follow_movement = Vector2.zero;
        movement = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));


        //Quaternion nextRotation = Quaternion.Euler(randomDir);
        //transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(.5f, 2.5f));
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
            currState = EnemyState2.Follow;
        }
    }

    //The Enemy moves towards the player
    //depending on the realtive positions of Enemy and player, the Enemy looks in different directions
    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (transform.position.x <= player.transform.position.x && transform.position.y <= player.transform.position.y)
        {
            follow_movement.x = 1;
            follow_movement.y = 0;
        }
        else if (transform.position.x >= player.transform.position.x && transform.position.y >= player.transform.position.y)
        {
            follow_movement.x = -1;
            follow_movement.y = 0;
        }
        else if (transform.position.x >= player.transform.position.x && transform.position.y <= player.transform.position.y)
        {
            follow_movement.x = 0;
            follow_movement.y = 1;
        }
        else if (transform.position.x <= player.transform.position.x && transform.position.y >= player.transform.position.y)
        {
            follow_movement.x = 0;
            follow_movement.y = -1;
        }
    }

    // If the enemy is close to the player, he performs a meelee attack and damages him
    void Attack()
    {
        if (!coolDownAttack)
        {
            StartCoroutine(MeeleeAttackAnimation());
            GameController.DamagePlayer(1);
            StartCoroutine(CoolDown());
            StartCoroutine(PlayerHitAnimation());
        }
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(attackDelay);
        coolDownAttack = false;
    }

    public IEnumerator PlayerHitAnimation()
    {
        playerAnim.SetBool("Player_Hit", true);
        yield return new WaitForSeconds(0.15f);
        playerAnim.SetBool("Player_Hit", false);
    }

    private IEnumerator MeeleeAttackAnimation() 
    {
        animator.SetBool("Attack_Enemy2", true);
        yield return new WaitForSeconds(1.1f);
        animator.SetBool("Attack_Enemy2", false);
    } 

    public void Death()
    {
        animator.SetTrigger("Death_Enemy2");
        currState = EnemyState2.Die;
        StartCoroutine(DeathDelay());

    }

    // A delay in the destruction of the GameObject, so that the hot-animation of the enemy has time to be played
    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(0.3f);
        ScoreController.instance.AddPoint();
        GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnerController>().enemyCounter -= 1;
        Destroy(gameObject);
    }



}


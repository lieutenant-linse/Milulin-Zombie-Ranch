using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState2
{
    Wander,
    Follow,
    Die,
    Attack
};

public class EnemyController2 : MonoBehaviour
{

    GameObject player;

    public Animator playerAnim;


    public EnemyState2 currState = EnemyState2.Wander;

    public Rigidbody2D rb_enemy;

    public Animator animator;



    public float attackRange;

    public bool coolDownAttack;

    public float attackDelay;

    public float range;

    public float speed;

    private bool chooseDir = false;

    private bool dead = false;



    //private Vector3 randomDir;

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

        if (IsPlayerInRange(range) && currState != EnemyState2.Die)
        {
            currState = EnemyState2.Follow;
        }
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

        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange && currState != EnemyState2.Die)
        {
            currState = EnemyState2.Attack;
        }


    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

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
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection());
        }

        // transform.position += -transform.right * speed * Time.deltaTime;

        rb_enemy.MovePosition(rb_enemy.position + movement * speed * Time.fixedDeltaTime);


        if (IsPlayerInRange(range))
        {
            currState = EnemyState2.Follow;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        follow_movement.x = player.transform.position.x;
        follow_movement.y = player.transform.position.y;

    }

    void Attack()
    {
        if (!coolDownAttack)
        {
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

    private IEnumerator PlayerHitAnimation()
    {
        playerAnim.SetBool("Player_Hit", true);
        yield return new WaitForSeconds(0.5f);
        playerAnim.SetBool("Player_Hit", false);
    }

    public void Death()
    {
        animator.SetTrigger("Death_Enemy2");
        currState = EnemyState2.Die;
        StartCoroutine(DeathDelay());

    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }



}


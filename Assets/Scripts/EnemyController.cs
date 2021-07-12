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

    GameObject player;

    private Animator playerAnim;

    public GameObject bulletPrefab;


    public EnemyState currState = EnemyState.Wander;

    public Rigidbody2D rb_enemy;

    public Animator animator;



    public float attackRange;

    public bool coolDownAttack;

    public float attackDelay;

    public float range;

    public float speed;

    public float bulletSpeed;

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

        if(IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Follow;
        }
        else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
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
        movement = new Vector2(Random.Range(-1,2), Random.Range(-1, 2));
        

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
            currState = EnemyState.Follow;
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
        if(!coolDownAttack)
        {
            StartCoroutine(RangedAttackAnimation());
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            bullet.GetComponent<BulletController>().GetPlayer(player.transform);
            bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator RangedAttackAnimation()
    {
        animator.SetBool("Shoot_Enemy", true);
        yield return new WaitForSeconds(1.1f);
        animator.SetBool("Shoot_Enemy", false);
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(attackDelay);
        coolDownAttack = false;
    }


    // on Collision Death - Sheesh

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
        Destroy(gameObject);
    }





}


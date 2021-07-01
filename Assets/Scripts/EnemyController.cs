using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Wander,
    Follow,
    Die
};

public class EnemyController : MonoBehaviour
{

    GameObject player;

    public EnemyState currState = EnemyState.Wander;

    public Rigidbody2D rb_enemy;

    public Animator animator;



    public float range;

    public float speed;

    private bool chooseDir = false;

    private bool dead = false;



    //private Vector3 randomDir;

    private Vector2 movement;
    private Vector2 follow_movement;





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
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal_Enemy", movement.x);
            animator.SetFloat("Vertical_Enemy", movement.y);
        }

        animator.SetFloat("Speed_Enemy", movement.sqrMagnitude);

        if (follow_movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal_Enemy", follow_movement.x);
            animator.SetFloat("Vertical_Enemy", follow_movement.y);

            animator.SetFloat("Speed_Enemy", follow_movement.sqrMagnitude);
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



}


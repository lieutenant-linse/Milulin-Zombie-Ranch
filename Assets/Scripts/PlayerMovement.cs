using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public Vector2 movement;

    private Vector2 shooting;


    //necessary GameObjects / Components
    public Rigidbody2D rb;

    public Animator animator;

    public GameObject bulletPrefab;


    //Properties for controlling Game Flow - PLayer
    public float moveSpeed = 5f;

    public float bulletSpeed;

    public float fireDelay;

    private float lastFire;



    public bool playerDead = false;


    
    void Update()
    {
        // as long as the player is alive, the update method is executed
        if (!playerDead)
        {
            // The moveSpeed gets overwritten by the GameController, this allows the item to increase the moveSpeed
            moveSpeed = GameController.MoveSpeed;

            // Movement and movement animation (input and Animator values)
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");


            if (movement != Vector2.zero)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
            }

            animator.SetFloat("Speed", movement.sqrMagnitude);


            // Shooting and shooting animation (input and Animator values)
            shooting.x = Input.GetAxisRaw("ShootHorizontal");
            shooting.y = Input.GetAxisRaw("ShootVertical");


            animator.SetFloat("ShootHorizontal", shooting.x);
            animator.SetFloat("ShootVertical", shooting.y);

            animator.SetFloat("Shoot", shooting.sqrMagnitude);

            if ((shooting.x != 0 || shooting.y != 0) && Time.time > lastFire + fireDelay)
            {
                Shoot(shooting.x, shooting.y);
                lastFire = Time.time;
            }
        }
        else
        {
            Destroy(rb);
            StartCoroutine(GameOverDelay());
        }



    }

    void FixedUpdate()
    {
        // Position update through movement Vector
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * bulletSpeed : Mathf.Ceil(x) * bulletSpeed,
            (y < 0) ? Mathf.Floor(y) * bulletSpeed : Mathf.Ceil(y) * bulletSpeed,
            0
            );
    }

    public void DeathAnimation()
    {
        animator.SetBool("Player_Death", true);
    }
    private static IEnumerator GameOverDelay()
    {
        yield return new WaitForSeconds(3.5f);
        GameController.endscreen.SetActive(true);
    }


}

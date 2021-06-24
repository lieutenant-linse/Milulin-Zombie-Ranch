using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    public Animator animator;


    public GameObject bulletPrefab;

    public float bulletSpeed;

    public float fireDelay;

    private float lastFire;










    private Vector2 movement;





    void Update()
    {

        // Movement and movement animation 

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero) { 
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Shooting 

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");
        íf((shootHor != 0 || shootVert != 0) && Time.time > lastFire + fireDelay){

        }

    }

    void FixedUpdate()
    {
        // Position update through movement Vector
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}

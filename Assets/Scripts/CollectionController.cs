using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectionController : MonoBehaviour
{
    // Start is called before the first frame update


    public float healthChange;

    public float moveSpeedChange;

    public float bulletSizeChange;

    void Start()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //PlayerMovement.collectedAmount++;

            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            Destroy(gameObject);
        }
    }
}

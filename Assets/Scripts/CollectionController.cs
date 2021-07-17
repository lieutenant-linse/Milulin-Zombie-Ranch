using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollectionController : MonoBehaviour
{

    // The amount of heal/ buff an item applies on the player can be directly defined in the Unity-Editor
    public float healthChange;

    public float moveSpeedChange;

    public float bulletSizeChange;


    // If the player collides with an Items the respective effect is activated
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            Destroy(gameObject);
        }
    }
}

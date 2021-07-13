using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item
{
    public string name;

    public Sprite itemImage;
}

public class CollectionController : MonoBehaviour
{
    // Start is called before the first frame update

    public Item item;

    public float healthChange;

    public float moveSpeedChange;

    public float bulletSizeChange;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
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

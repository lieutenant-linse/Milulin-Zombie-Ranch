using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public static GameObject endscreen;


    public static Animator playerAnim;

    public static GameObject player;


    public static GameController instance;




    // These values apply to the player

    private static float health = 3;

    private static int maxHealth = 3;

    private static float moveSpeed = 5f;

    private static float bulletSize = 0.75f;


    // Getter/ Setter methods for the private values above
    public static float Health { get => health; set => health = value; }

    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public static float BulletSize  { get => bulletSize; set => bulletSize = value; }


    private void Awake()
    {

        if(instance = null)
        {
            instance = this;
        }


        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player");

    }

    void Start()
    {
        endscreen = GameObject.FindGameObjectWithTag("GameOverScreen");
        endscreen.SetActive(false);

        health = maxHealth;

        Time.timeScale = 1f;
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            KillPlayer();
        }
    }

    // The following three functions are used by the items to manipulate the players stats
    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, health + healAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }
    
    // 
    public static void KillPlayer()
    {
        if (!player.GetComponent<PlayerMovement>().playerDead)
        {
            player.GetComponent<PlayerMovement>().DeathAnimation();
            player.GetComponent<AudioSource>().Play();
            player.GetComponent<PlayerMovement>().playerDead = true;
        }
    }


}

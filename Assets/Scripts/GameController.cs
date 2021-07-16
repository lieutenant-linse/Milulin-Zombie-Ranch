using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameObject endscreen;

    public static Animator playerAnim;

    public static GameController instance;

    private static float health = 3;

    private static int maxHealth = 3;

    private static float moveSpeed = 5f;

    private static float bulletSize = 0.5f;


    public static float Health { get => health; set => health = value; }

    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    public static float BulletSize  { get => bulletSize; set => bulletSize = value; }

    // Hier muss dann noch eine Connection zur UI gemacht werden, also Health-Anzeige usw.


    // Start is called before the first frame update
    private void Awake()
    {
        if(instance = null)
        {
            instance = this;
        }
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

    }

    void Start()
    {
        endscreen = GameObject.FindGameObjectWithTag("GameOverScreen");
        endscreen.SetActive(false);
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if (Health <= 0)
        {
            KillPlayer();
        }
    }


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
    
    public static void KillPlayer()
    {   
        playerAnim.SetBool("Player_Death", true);
        endscreen.SetActive(true);
        Time.timeScale = 0;

        // Hier muss jetzt zum Game-Over Screen ?bergeleitet werden?


    }
}

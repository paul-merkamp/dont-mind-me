using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public int maxhealth;
    public int health;
    public int enemyType;

    public float healthPercentage;

    // enemy parameters
    public PlayerController player;
    public bool isPlayer = false;
    public bool isEnemy = false;
    public bool meleePlayer;
    public int meleeDamage;
    public float movementSpeed = 2f;
    public int maxChaseDistance;

    public AudioClip damageSound;
    public AudioClip deathSound;

    public Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;

        player = GameObject.Find("Player").GetComponent<PlayerController>();

        try
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
        catch
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Die();
        }

        if (health > maxhealth)
        {
            health = maxhealth;
        }

        healthPercentage = 1.000f * health / maxhealth;
        healthPercentage = 0.200f + (0.800f * healthPercentage);

        if (!isPlayer)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(healthPercentage, healthPercentage, healthPercentage, 1);
        }

        int stateMachine = 2;
        if (isEnemy)
        {
            if (enemyType == 0 || enemyType == 2)
            {
                switch (stateMachine)
                {
                    case 1: // idle
                        break;
                    case 2: // chase
                        try
                        {
                            ChasePlayer();
                        }
                        catch
                        {
                            throw;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (enemyType == 1)
            {
                switch (stateMachine)
                {
                    case 1: // idle
                        break;
                    case 2: // shoot at player
                        GetComponent<BulletSource>().FireBullet("left");
                        break;
                    default:
                        break;
                }
            }
        }

    }

    public void TakeDamage(int amt)
    {
        health -= amt;
        try
        {
            if (health > 0)
            {
                GetComponent<AudioSource>().PlayOneShot(damageSound, 1);
            }
            else if (health <= 0)
            {
                GetComponent<AudioSource>().PlayOneShot(deathSound, 1);
            }
        }
        catch
        {
            throw;
        }
    }

    public void Die()
    {
        if (!isPlayer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            player.GameOver();
        }
    }

    public void Move(Vector2 target)
    {
        target.Normalize();
        target *= movementSpeed;
        rb2d.velocity = target;
        if (target.x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (target.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void ChasePlayer()
    {
        Vector2 chaseDirection = (player.transform.position - transform.position);
        if (Vector2.Distance(player.transform.position, transform.position) < maxChaseDistance)
        {
            Move(chaseDirection);
        }
    }
}

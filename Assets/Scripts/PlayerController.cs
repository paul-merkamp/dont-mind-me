using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool invincible;
    public float invincibilityLength;

    public float speed = 5f;
    public float jarPenalty = 0.7f;
    public float nopeLength = 0.5f;
    public int damageJumpSpeed;
    public int damageJumpDistance;

    public string direction = "down";
    public bool walking = false;
    public bool carryingJar = false;
    public bool dead = false;

    public bool touchingBrain = false;
    public bool touchingTurret = false;

    public int[] unlockedTurrets;
    public int turretSelectedInList;

    public bool interactionDisabled = false;

    public bool inSchematicSelector = false;

    public AudioClip stepSound;
    public AudioClip takeDamageSound;
    public float stepSoundWaitTime;
    public bool readyForNextStep = true;

    public GameObject deathScreen;

    public Vector2 mvt = new Vector2();
    Vector2 jumpAwayTarget;
    Vector2 initialJumpPosition;

    Rigidbody2D rb2d;

    public Collider2D currentCollider;

    public bool movementDisabled;

    // Start is called before the first frame update
    void Start()
    {
        unlockedTurrets = new int[4];
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;

        if (!dead)
        {
            if (!Input.GetKey("w") && !Input.GetKey("s"))
            {
                mvt.y = 0;
            }

            else if (Input.GetKey("w"))
            {
                if (!inSchematicSelector)
                {
                    mvt.y = 1;
                    walking = true;
                }
            }
            else if (Input.GetKey("s"))
            {
                if (!inSchematicSelector)
                {
                    mvt.y = -1;
                    walking = true;
                }
            }

            if (!Input.GetKey("a") && !Input.GetKey("d"))
            {
                mvt.x = 0;
            }

            else if (Input.GetKey("a"))
            {
                if (!inSchematicSelector)
                {
                    mvt.x = -1;
                    direction = "left";
                    walking = true;
                }
                else if (inSchematicSelector)
                {
                    advanceSchematicSelector(-1);
                }
            }
            else if (Input.GetKey("d"))
            {
                if (!inSchematicSelector)
                {
                    mvt.x = 1;
                    direction = "right";
                    walking = true;
                }
                else if (inSchematicSelector)
                {
                    advanceSchematicSelector(1);
                }
            }

            if (!Input.GetKey("w") && !Input.GetKey("s") && !Input.GetKey("a") && !Input.GetKey("d"))
            {
                walking = false;
            }

            if (Input.GetKeyDown("e"))
            {
                if (!interactionDisabled && touchingBrain)
                {
                    if (!carryingJar)
                    {
                        if (!currentCollider.gameObject.GetComponent<BrainSpace>().SpaceLocked)
                        {
                            carryingJar = true;
                            currentCollider.gameObject.GetComponent<BrainSpace>().PickUpBrain();
                        }
                        else
                        {
                            StartCoroutine(Nope());
                        }
                    }
                    else
                    {
                        carryingJar = false;
                        currentCollider.gameObject.GetComponent<BrainSpace>().PlaceBrain();
                    }
                }
                /* else if (!interactionDisabled && touchingTurret && !inSchematicSelector)
                {
                    if (!currentCollider.gameObject.GetComponent<TurretSpace>().occupied)
                    {
                        openSchematicSelector();
                    }
                }*/
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!inSchematicSelector)
                {
                    transform.Find("Gun").gameObject.GetComponent<BulletSource>().FireBullet(direction);
                }
                else if (inSchematicSelector)
                {
                    // buy and place selected turret
                }
            }
        }

        if (Input.GetKeyDown("r"))
        {
            if (dead)
            {
                Application.LoadLevel(0);
            }
        }

        MovePlayer(mvt);

        if (direction == "left")
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = true;

            transform.Find("Gun").GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (direction == "right")
        {
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = false;

            transform.Find("Gun").GetComponent<SpriteRenderer>().flipX = false;
        }

        if (walking)
        {
            GetComponent<Animator>().SetBool("walking", true);
        }
        else { GetComponent<Animator>().SetBool("walking", false); }

        if (carryingJar)
        {
            transform.Find("HoldingJar").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("HoldingJar").gameObject.SetActive(false);
        }


        if (walking && readyForNextStep)
        {
            readyForNextStep = false;
            StartCoroutine("WalkingSound");
        }
    }

    public void MovePlayer(Vector2 v)
    {
        v.x *= speed;
        v.y *= speed;
        if (carryingJar)
        {
            v.x *= jarPenalty;
            v.y *= jarPenalty;
        }
        if (Vector2.Distance(initialJumpPosition, new Vector2(transform.position.x, transform.position.y)) < damageJumpDistance)
        {
            v.x = jumpAwayTarget.x * damageJumpSpeed;
            v.y = jumpAwayTarget.y * damageJumpSpeed;
        }
        else
        {
            initialJumpPosition = new Vector2(10000, 10000);
        }
        rb2d.velocity = v;
    }

    public void GameOver()
    {
        dead = true;
        deathScreen.SetActive(true);

        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Sprite").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("Gun").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("Shadow").GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.GetComponent<Entity>().enabled = false;
    }

    IEnumerator Nope()
    {
        transform.Find("NopeSign").gameObject.SetActive(true);
        yield return new WaitForSeconds(nopeLength);
        transform.Find("NopeSign").gameObject.SetActive(false);
    }

    IEnumerator WalkingSound()
    {
        GetComponent<AudioSource>().PlayOneShot(stepSound, 1);
        
        yield return new WaitForSeconds(stepSoundWaitTime);

        readyForNextStep = true;
    }

    public void openSchematicSelector()
    {
        inSchematicSelector = true;
        transform.Find("SchematicSelector").GetComponent<Animator>().SetBool("menuOpen", true);

        if (turretSelectedInList == 0)
        {
            transform.Find("SchematicSelector").transform.Find("turretSelectorPrevious").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("SchematicSelector").transform.Find("turretSelectorPrevious").gameObject.SetActive(true);
        }
        if (turretSelectedInList == unlockedTurrets.Length)
        {
            transform.Find("SchematicSelector").transform.Find("turretSelectorNext").gameObject.SetActive(false);
        }
        else
        {
            transform.Find("SchematicSelector").transform.Find("turretSelectorNext").gameObject.SetActive(true);
        }
    }

    public void advanceSchematicSelector(int direction)
    {
        if ((direction == -1 && (turretSelectedInList-1 >= 0)) || (direction == 1 && (turretSelectedInList+1 <= unlockedTurrets.Length))){
            transform.Find("SchematicSelector").GetComponent<Animator>().SetInteger("mvtDirection", direction);
            turretSelectedInList += direction;

            if (turretSelectedInList == 0)
            {
                transform.Find("SchematicSelector").transform.Find("turretSelectorPrevious").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("SchematicSelector").transform.Find("turretSelectorPrevious").gameObject.SetActive(true);
            }
            if (turretSelectedInList == unlockedTurrets.Length)
            {
                transform.Find("SchematicSelector").transform.Find("turretSelectorNext").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("SchematicSelector").transform.Find("turretSelectorNext").gameObject.SetActive(true);
            }

            StartCoroutine("SelectSchematic");
        }
    }

    IEnumerator SelectSchematic()
    {
        yield return new WaitForSeconds(0.1f);
        transform.Find("SchematicSelector").GetComponent<Animator>().SetInteger("mvtDirection", 0);
    }

    public void TakeDamage(int amt, Vector2 e)
    {
        gameObject.GetComponent<Entity>().TakeDamage(amt);
        initialJumpPosition = transform.position;
        jumpAwayTarget = (Vector2)transform.position - e;
        StartCoroutine("TemporaryInvincibility");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BrainBase") || other.CompareTag("BrainSpawn"))
        {
            touchingBrain = true;
        }
        else if (other.CompareTag("TurretBase"))
        {
            touchingTurret = true;
        }
        if (!invincible)
        {
            if (other.CompareTag("Enemy")) // touch enemy
            {
                if (other.GetComponent<Entity>().meleePlayer && other.GetComponent<Entity>().enemyType != 2)
                {
                    TakeDamage(other.GetComponent<Entity>().meleeDamage, other.transform.position);
                }
            }
            currentCollider = other;
        }
    }

    IEnumerator TemporaryInvincibility()
    {
        invincible = true;
        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(invincibilityLength);

        transform.Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
        invincible = false;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BrainBase") || other.CompareTag("BrainSpawn"))
        {
            touchingBrain = false;
        }
        else if (other.CompareTag("TurretBase"))
        {
            touchingTurret = false;
        }
        currentCollider = null;
    }
}

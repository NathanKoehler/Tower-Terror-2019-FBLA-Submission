using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class Enemy_S : MonoBehaviour
{
    public static Transform target;
    public static bool foundTarget;

    [SerializeField]
    AudioManager_S audioManager;

    [SerializeField]
    EnemyPistol_S enemyPistol;

    [SerializeField]
    private float invincibilitySeconds = 1;

    private AudioSource firingSound;
    private bool invincibilityFrame = false;
    private bool tooClose;
    private float howLongClose;
    private float health = 4;
    private float totalBullets;
    private float clipSize;
    private float currentBullets;
    private float reloadSpeed;
    private float fireRate;
    private float zRotation = 1;
    private float xRotation = 0;
    private bool rhand = true;
    private bool firing = false;
    private bool reloading = false;
    private bool invisible = false;
    private string reloadSound = "";
    private GameObject enemySprite;
    private GameObject enemyGunSprite;
    private Rigidbody spriteRigid;
    private int invincibilityFlashTicks;

    private float localAngle;
    private float x;
    private float y;
    private bool walking = false;



    public float updateRate = 2f; // number of path updates per second

    private Animator animator;

    private Seeker seeker;
    private Rigidbody2D rigid;
    private Vector3 dir;
    private Vector2 look;


    public Path path;


    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWPDistance = 3; // max disance from the AI to a location before it finds another location

    private int currentWaypoint = 0; // Current waypoint AI is seeking


    // Start is called before the first frame update
    void Start()
    {
        foundTarget = false;

        audioManager = GameObject.Find("Audio Manager").GetComponent<AudioManager_S>();
        enemySprite = transform.GetChild(0).gameObject;
        spriteRigid = transform.GetChild(0).GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        invincibilityFlashTicks = (int)((invincibilitySeconds - 0.28f) / 0.16f);

        // ****************************************************************

        seeker = GetComponent<Seeker>();
        rigid = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            target = transform;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete); // starts a new path to the target, returns the result to method On Path Complete
        StartCoroutine(UpdatePath());


    }

    private void Update()
    {
        if (howLongClose > 30)
        {
            StartCoroutine(TooCloseMan());
            howLongClose = 0;
        }
    }

    void FixedUpdate()
    {
        spriteRigid.MovePosition(new Vector3(0, 0, -1 * (rigid.position.y / Mathf.Tan(71.0954241574f)))); // Controls the Z of the Player Sprite

        if (target == null) { return; }
        if (path == null) { return; }

        if (foundTarget && 5 > Random.Range(0, 100))
        {
            enemyPistol.requestFire();
        }

        if (tooClose) { return; }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        dir = (path.vectorPath[currentWaypoint] - transform.position).normalized; // Direction to next waypoint
        dir *= speed * Time.fixedDeltaTime;

        rigid.AddForce(dir, fMode);
        lookAtPlayer();

        float d = (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]));
        if (d < nextWPDistance)
        {
            currentWaypoint++;
            return;
        }
    }


    // ** Methods **
    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }


    public void lookAtPlayer()
    {
        localAngle = enemyPistol.localAngle;

        if (localAngle > 0)
        {
            if (localAngle > 30)
            {
                if (localAngle < 150)
                {
                    if (localAngle <= 110 && rhand)
                    {
                        y = 1;
                        x = 0.01f;
                    }
                    else if (localAngle <= 70 && !rhand)
                    {
                        rhand = true;
                        y = 1;
                        x = 0.01f;
                    }
                    else
                    {
                        rhand = false;
                        y = 1;
                        x = -0.01f;
                    }
                }
                else
                {
                    y = 0;
                    x = -1;
                }
            }
            else
            {
                y = 0;
                x = 1;
            }
        }
        else
        {
            if (localAngle < -30)
            {
                if (localAngle > -150)
                {
                    if (localAngle >= -110 && rhand)
                    {
                        y = -1;
                        x = 0.01f;
                    }
                    else if (localAngle >= -70 && !rhand)
                    {
                        rhand = true;
                        y = -1;
                        x = 0.01f;
                    }
                    else
                    {
                        rhand = false;
                        y = -1;
                        x = -0.01f;
                    }
                }
                else
                {
                    y = 0;
                    x = -1;
                }
            }
            else
            {
                y = 0;
                x = 1;
            }
        }


        Vector2 moveInput = dir;
        moveInput = moveInput.normalized;

        look = new Vector2(x, y);

        if (moveInput.x == 0 && moveInput.y == 0)
        {
            walking = false;
            animator.SetLayerWeight(1, 0);
        }
        else
        {
            if (!walking)
            {
                walking = true;
            }


            animator.SetLayerWeight(1, 1);
        }

        if (look.x != 0 || look.y != 0)
        {
            LookAtCamera(look);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }


    public void LookAtCamera(Vector3 direction)
    {
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }


    // ** Damage **
    public void takeDamage(float damage)
    {
        if (!invincibilityFrame && health > 0)
        {
            audioManager.Play("Alien Sound");

            invincibilityFrame = true; // Wait for Coroutine to end before letting player take damage again

            health -= damage;
            StartCoroutine(damageAnimation()); // Plays the damage animation for the player and UI

            if (health <= 0)
            {
                audioManager.Play("Alien Death");
                Destroy(gameObject.transform.root.gameObject);
            }
        }
    }


    // ** Coroutine **
    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            yield return false;
        }
        else
            seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }


    IEnumerator damageAnimation()
    {
        

        enemySprite.GetComponent<SpriteRenderer>().material.SetColor("_overlappedColor", Color.white);
        yield return new WaitForSeconds(0.08f);
        enemySprite.GetComponent<SpriteRenderer>().material.SetColor("_overlappedColor", Color.black);
        for (int i = 0; i < invincibilityFlashTicks; i++) // Flashing animation
        {
            yield return new WaitForSeconds(0.08f);
            enemySprite.GetComponent<SpriteRenderer>().material.SetFloat("_visibility", 0f);
            yield return new WaitForSeconds(0.08f);
            enemySprite.GetComponent<SpriteRenderer>().material.SetFloat("_visibility", 0.9999f);
            yield return new WaitForSeconds(0.2f);

            invincibilityFrame = false;
        }
    }

    IEnumerator TooCloseMan()
    {
        tooClose = true;
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        tooClose = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            howLongClose++;
        }
        else
        {
            howLongClose = 0;
        }
    }
}

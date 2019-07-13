using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player_S : MonoBehaviour // Script for slight UI control and the player
{
    [SerializeField]
    private Canvas canvas = null;
    [SerializeField]
    private AudioManager_S audioManager = null;
    [SerializeField]
    private GameController_S gameController = null;

    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private float invincibilitySeconds = 0;


    // ** Non-Primative variables **
    private Animator animator;
    private Camera mainCamera;
    private GameObject playerSprite;
    private Rigidbody2D rigid;
    private Rigidbody spriteRigid;
    private Vector2 moveVelocity;
    private CameraShake_S cameraShake;


    // ** Arrays **
    private List<GameObject> canCollectables = new List<GameObject>();
    private List<GameObject> collectables = new List<GameObject>();


    // ** UI variables **
    private Canvas_S canvasResources;


    // ** Primative variables **
    private bool invincibilityFrame;
    private bool holdingCollectable;
    private int invincibilityFlashTicks;
    private float health = 8;


    public static Player_S self;


    void Awake()
    {
        if (self == null)
        {
            self = this;
            
            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
            GameController_S.maintainedScripts.Add(gameObject);
        }
        else Destroy(gameObject);
    }


    // Use this for initialization
    protected void Start()
    {
        gameController = GameController_S.self;

        mainCamera = Camera_S.self.GetComponent<Camera>();

        playerSprite = transform.GetChild(0).gameObject;
        spriteRigid = playerSprite.GetComponent<Rigidbody>();
        animator = playerSprite.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();

        canvasResources = canvas.GetComponent<Canvas_S>();
        cameraShake = gameController.gameObject.GetComponent<CameraShake_S>();


	invincibilityFlashTicks = (int)((invincibilitySeconds - 0.28f)/0.16f);
    }

    // Update is called once per frame
    protected void Update()
    {
        PlayerMovement();
        InputsForTesting();
    }

    private void FixedUpdate()
    {
        spriteRigid.MovePosition(new Vector3(0, 0, -1*(rigid.position.y / Mathf.Tan(71.0954241574f)))); // Controls the Z of the Player Sprite
        rigid.MovePosition(rigid.position + moveVelocity * Time.fixedDeltaTime); // Moves the Player
    }


    // ** Methods **
    private void PlayerMovement()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        
    }

    private void InputsForTesting()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        //     takeDamage(1f);

        if (Input.GetKeyDown(KeyCode.E)) // Add Collectable to Inventory
        {
            if (holdingCollectable)
            {
                GameObject collectable = collectables[collectables.Count - 1];
                setCollectable(collectable, false);
            }
            if (canCollectables.Count > 0)
                setCollectable(canCollectables[canCollectables.Count - 1], true); // Adds the MOST RECENTLY found item into Inventory
        }
    }


    // ** Get Methods **
    public bool getInvicibilityFrames()
    {
        return invincibilityFrame;   
    }

    public float getHealth()
    {
        return health;
    }


    public static Player_S local()
    {
        return self;
    }


    // ** Set Methods **
    public void takeDamage(float damage)
    {
        if (!invincibilityFrame && health > 0)
        {
	    invincibilityFrame = true; // Wait for Coroutine to end before letting player take damage again

            health -= damage;
            StartCoroutine(damageAnimation()); // Plays the damage animation for the player and UI

            if (health <= 0)
            {
                // TODO add game over features 
                gameController.GameOverScreen();
                Debug.Log("You Died");
            }
        }
    }

    public void setCanCollect(GameObject collectable, bool canCollect) // Enables the player to collect an object
    {
        if (canCollect)
        {
            canCollectables.Add(collectable);
        }
        else
        {
            canCollectables.Remove(collectable);
        }
    }

    public void setCollectable(GameObject collectable, bool collect)
    {
        if (collect)
        {
            canCollectables.Remove(collectable);
            collectables.Add(collectable);
            collectable.AddComponent<SpringJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
            audioManager.Play("Collecting Sound");

            collectable.GetComponent<Collectable_S>().setCollected(true);
            if (collectable.GetComponent<CollectableAnswer_S>() != null)
                collectable.GetComponent<CollectableAnswer_S>().ColorFlash(1, Color.white);

            holdingCollectable = true;
        }
        else
        {
            collectables.Remove(collectable);
            Destroy(collectable.GetComponent<SpringJoint2D>());
            collectable.GetComponent<Collectable_S>().setCollected(false);

            holdingCollectable = false;
        }
    }


    // ** Coroutines **
    IEnumerator damageAnimation()
    {
        canvasResources.HealthUI.setHealth();

        audioManager.Play("Hit Sound");
        //cameraShake.ShakeCamera(0.02f, 0.2f);

        playerSprite.GetComponent<SpriteRenderer>().material.SetColor("_overlappedColor", Color.white);
        canvasResources.HealthUI.activateHealthTies(false);
        yield return new WaitForSeconds(0.08f);
        playerSprite.GetComponent<SpriteRenderer>().material.SetColor("_overlappedColor", Color.black);
        canvasResources.HealthUI.activateHealthTies(true);
        for (int i = 0; i < invincibilityFlashTicks; i++) // Flashing animation
        {
	    yield return new WaitForSeconds(0.08f);
            playerSprite.GetComponent<SpriteRenderer>().material.SetFloat("_visibility", 0f);
            canvasResources.HealthUI.activateHealthTies(false);
            yield return new WaitForSeconds(0.08f);
            playerSprite.GetComponent<SpriteRenderer>().material.SetFloat("_visibility", 0.9999f);
            canvasResources.HealthUI.activateHealthTies(true);
        }
	yield return new WaitForSeconds(0.2f);
	
        invincibilityFrame = false;
    }
}
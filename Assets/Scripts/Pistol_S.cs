using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pistol_S : MonoBehaviour // Script for all weapons
{

    public static float angle;

    public Canvas canvas = null;
    public Camera cameraMain;
    public AudioManager_S audioManager;

    public Gun_S gun;

    protected AudioSource firingSound;
    protected float totalBullets;
    protected float clipSize;
    protected float currentBullets;
    protected float reloadSpeed;
    protected float fireRate;
    protected float zRotation = 1;
    protected float xRotation = 0;
    protected bool rhand = true;
    protected bool firing = false;
    protected bool reloading = false;
    protected bool invisible = false;
    protected string reloadSound = "";

    protected GameObject muzzle;
    protected GameObject gunSprite;
    protected GameObject childSprite;
    protected GameObject playerSprite;
    protected Animator animator;
    protected CameraShake_S cameraShake;
    

    // ** Canvas Variables **
    private Canvas_S canvasResources;
    private Text gunUIText;


    // Start is called before the first frame update
    void Start()
    {
        playerSprite = transform.parent.GetChild(0).gameObject;

        cameraShake = GameController_S.self.gameObject.GetComponent<CameraShake_S>();
        gunSprite = transform.GetChild(0).gameObject;
        muzzle = transform.GetChild(1).gameObject;


        firingSound = GetComponent<AudioSource>();
        gunSprite.GetComponent<SpriteRenderer>().sprite = gun.weapon; // Sets Gun Sprite to the Scriptable Object's Sprite

        muzzle.transform.localPosition.Set(gun.muzzleLocationX, gun.muzzleLocationY, 0); // Sets the Muzzle Location

        if (gun.childGameObject != null) // Tests and Creates Child of Gun
        {
            childSprite = Instantiate(gun.childGameObject, gunSprite.transform);
            gunSprite.GetComponent<GunLayerManagement_S>().setChildSprite(childSprite);
        }

        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.runtimeAnimatorController = gun.animation; // Sets the Animation Controller of the Sprite


        // Sets different aspects of the gun
        firingSound.clip = gun.firingSound;
        fireRate = gun.fireRate;
        totalBullets = gun.bulletCount - gun.clipSize;
        clipSize = gun.clipSize;
        currentBullets = gun.clipSize;
        reloadSpeed = gun.reloadSpeed;
        reloadSound = gun.reloadSound;


        canvasResources = canvas.GetComponent<Canvas_S>(); // Gets Canvas Script of the game;
        gunUIText = canvasResources.getGunUIText(); // Gets the Gun UI Text from the Canvas Script;
        gunUIText.color = Color.blue;
        gunUIText.text = "staples x " + currentBullets;
    }


    // Update is called once per frame
    void Update()
    {
        Inputs();
    }


    // FixedUpdate is called once per tick (time based)
    void FixedUpdate()
    {
        GunRotation(playerSprite.transform.position, new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5.23f));
    }


    // ** Methods **
    protected void GunRotation(Vector3 startPos, Vector3 endPos)
    {
        if (invisible) // Sets Visible if invisible
        {
            gunSprite.GetComponent<SpriteRenderer>().enabled = true;
        }

        Vector3 mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        var object_pos = Camera.main.WorldToScreenPoint(startPos);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;


        if ((rhand && (angle > 110 || angle < -110)) || (!rhand && angle < 70 && angle > -70))
        {
            invisible = true; // Sets invisible for one frame to avoid viewing transform process
            gunSprite.GetComponent<SpriteRenderer>().enabled = false;


            if (rhand == true)
            {
                xRotation = 180;
                zRotation = -1;
                transform.position = new Vector3(transform.position.x - 0.358f, transform.position.y, transform.position.z); // Weapon Flipping
                rhand = false;
            }
            else
            {
                xRotation = 0;
                zRotation = 1;
                transform.position = new Vector3(transform.position.x + 0.358f, transform.position.y, transform.position.z);
                rhand = true;
            }


        }

        transform.rotation = Quaternion.Euler(new Vector3(xRotation, 0, zRotation * angle)); // Sets Rotation of Gun
    }


    private void Inputs() // Basic Weapon-Oriented Inputs
    {
        if (!reloading) 
        {
            if (!firing && (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))) // Tests for Mouse Input)
                StartCoroutine("Firing");
            else if (Input.GetKeyDown(KeyCode.R) && currentBullets < clipSize)
                StartCoroutine(Reloading());
        }
    }


    public void Reload(bool instant) // External call (by game controller) to reload
    {
        if (instant) // Does the same thing as the Coroutine but instant
        {
            totalBullets += currentBullets;
            if (totalBullets < clipSize)
            {
                gunUIText.color = Color.Lerp(Color.red, Color.blue, currentBullets / clipSize); // The bullet count changes color based on currentBullets / clipSize
                currentBullets = totalBullets;
                totalBullets = 0;
            }
            else
            {
                gunUIText.color = Color.blue;
                totalBullets -= clipSize;
                currentBullets = clipSize;
            }
            gunUIText.text = "staples x " + currentBullets;

            firing = false; // Allows the player to fire and reload again
            reloading = false;
        }
        else if (!reloading && !firing && currentBullets < clipSize)
        {
            StartCoroutine(Reloading()); // Call to Coroutine
        }
    }



    // ** Coroutines **
    protected virtual IEnumerator Firing()
    {
        // cameraShake.ShakeCamera(0.01f, 0.1f);


        firing = true;
        if (currentBullets > 0)
        {
            animator.SetTrigger("Firing");
            canvasResources.GunUIAnimator.SetTrigger("Fire");

            firingSound.Play();

            currentBullets -= 1;
            gunUIText.color = Color.Lerp(Color.red, Color.blue, currentBullets / clipSize);
            if (currentBullets >= 0) { gunUIText.text = "staples x " + currentBullets; }
            
            // Creates the bullet and points it to a direction with an 8 angle max offset
            Instantiate(gun.bullet, new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, muzzle.transform.position.z), Quaternion.Euler(new Vector3(0, 0, angle - 8 + Random.Range(0, 16))));

            yield return new WaitForSecondsRealtime(fireRate);

            
            firing = false;
        }
        else
        {
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        reloading = true;
        currentBullets = 0;

        audioManager.Play(reloadSound);

        canvasResources.GunUIAnimator.SetTrigger("Reloading");
        gunUIText.color = Color.red;
        gunUIText.text = "reloading.";
        yield return new WaitForSeconds(reloadSpeed / 3); // Text animation (not enough code to force gunUIText to have its own script)
        gunUIText.text = "reloading..";
        yield return new WaitForSeconds(reloadSpeed / 3);
        gunUIText.text = "reloading...";
        yield return new WaitForSeconds(reloadSpeed / 3);


        totalBullets += currentBullets;
        if (totalBullets < clipSize) // If the player runs out of bullets and doesn't have enough for a full clip
        {
            gunUIText.color = Color.Lerp(Color.red, Color.blue, currentBullets / clipSize);
            currentBullets = totalBullets;
            totalBullets = 0; // Prevents totalBullets from having a negative value
        }
        else // The usual reloading
        {
            gunUIText.color = Color.blue;
            totalBullets -= clipSize;
            currentBullets = clipSize;
        }

        gunUIText.text = "staples x " + currentBullets;

        firing = false; // Allows the player to fire and reload again
        reloading = false;
    }


    // ** Set Methods **
    private void setTotalBullets(float newTotalBullets)
    {
        totalBullets = newTotalBullets;
    }

    private void setClipSize(float newClipSize)
    {
        clipSize = newClipSize;
    }

    private void setCurrentClip(float newCurrentBullets)
    {
        currentBullets = newCurrentBullets;
    }

    private void setReloadSpeed(float newReloadSpeed)
    {
        reloadSpeed = newReloadSpeed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistol_S : MonoBehaviour
{
    public static Transform player;

    [HideInInspector]
    public float localAngle = 0;

    private GameObject enemySprite;

    

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
    
    protected Animator animator;
    protected CameraShake_S cameraShake;


    // Start is called before the first frame update
    void Start()
    {
        enemySprite = transform.parent.gameObject;
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
        //firingSound.clip = gun.firingSound;
        fireRate = gun.fireRate;
        totalBullets = gun.bulletCount - gun.clipSize;
        clipSize = gun.clipSize;
        currentBullets = gun.clipSize;
        reloadSpeed = gun.reloadSpeed;
        reloadSound = gun.reloadSound;
    }


    private void FixedUpdate()
    {
        if (Enemy_S.foundTarget)
        {
            Debug.Log(Enemy_S.foundTarget);
            GunRotation(enemySprite.transform.position, player.position);
        }
    }


    // ** Methods **
    private void GunRotation(Vector3 startPos, Vector3 endPos)
    {
        if (invisible) // Sets Visible if invisible
        {
            gunSprite.GetComponent<SpriteRenderer>().enabled = true;
        }

        Vector3 mouse_pos = endPos;
        mouse_pos.z = 5.23f; //The distance between the camera and object
        var object_pos = startPos;
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        localAngle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;


        if ((rhand && (localAngle > 110 || localAngle < -110)) || (!rhand && localAngle < 70 && localAngle > -70))
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

        Debug.Log(localAngle);
        transform.rotation = Quaternion.Euler(new Vector3(xRotation, 0, zRotation * localAngle)); // Sets Rotation of Gun
    }


    public void requestFire()
    {
        if (!firing)
            StartCoroutine(Firing());
    }


    // ** Coroutines **
    private IEnumerator Firing()
    {
        firing = true;
        if (currentBullets > 0)
        {
            animator.SetTrigger("Firing");

            firingSound.Play();

            currentBullets -= 1;

            // Creates the bullet and points it to a direction with an 8 angle max offset
            Instantiate(gun.bullet, new Vector3(muzzle.transform.position.x, muzzle.transform.position.y, muzzle.transform.position.z), Quaternion.Euler(new Vector3(0, 0, localAngle - 14 + Random.Range(0, 28))));

            yield return new WaitForSecondsRealtime(fireRate);


            firing = false;
        }
    }
}

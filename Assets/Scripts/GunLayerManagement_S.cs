using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunLayerManagement_S : MonoBehaviour
{
    [SerializeField]
    private GameObject playerSprite = null;
    [SerializeField]
    private GameObject playerOutlineSprite = null;
    [SerializeField]
    private GameObject childSprite;
    [SerializeField]
    private AudioManager_S AudioManager = null;

    private Animator animator;
    private Animator outlineAnimator;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteOutlineRenderer;
    private SpriteRenderer childSpriteRenderer;
    private float angle;
    private float x;
    private float y;
    private bool walking = false;
    private bool rhand = true;
    private Vector2 look;
    private Vector2 moveVelocity;

    private Vector2 moveInput;

    private void Start()
    {
        // Finds Weapon to Manage
        spriteRenderer = GetComponent<SpriteRenderer>();

        angle = Pistol_S.angle;

        animator = playerSprite.GetComponent<Animator>();
        outlineAnimator = playerOutlineSprite.GetComponent<Animator>();
    }

    // ** Layer Management of Gun and Possible Child **
    private void FixedUpdate()
    {
        if (childSpriteRenderer != null) // Prevents setting Child Actor's Layer if there is none
        {
            if (Pistol_S.angle > 0)
            {
                spriteRenderer.sortingLayerName = "B Player";
                childSpriteRenderer.sortingLayerName = "B Player";
                if (Pistol_S.angle > 30)
                {
                    if (Pistol_S.angle < 150)
                    {
                        if (Pistol_S.angle <= 110 && rhand)
                        {
                            y = 1;
                            x = 0.01f;
                        }
                        else if (Pistol_S.angle <= 70 && !rhand)
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
                spriteRenderer.sortingLayerName = "F Player";
                childSpriteRenderer.sortingLayerName = "F Player";
                if (Pistol_S.angle < -30)
                {
                    if (Pistol_S.angle > -150)
                    {
                        if (Pistol_S.angle >= -110 && rhand)
                        {
                            y = -1;
                            x = 0.01f;
                        }
                        else if (Pistol_S.angle >= -70 && !rhand)
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
        }
        else // Messy, but the only way to use the least amount of code :: TODO find a better way
        {
            if (Pistol_S.angle > 0)
            {
                spriteRenderer.sortingLayerName = "B Player";
                if (Pistol_S.angle > 30)
                {
                    if (Pistol_S.angle < 150)
                    {
                        if (Pistol_S.angle <= 110 && rhand)
                        {
                            y = 1;
                            x = 0.01f;
                        }
                        else if (Pistol_S.angle <= 70 && !rhand)
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
                spriteRenderer.sortingLayerName = "F Player";
                if (Pistol_S.angle < -30)
                {
                    if (Pistol_S.angle > -150)
                    {
                        if (Pistol_S.angle >= -110 && rhand)
                        {
                            y = -1;
                            x = 0.01f;
                        }
                        else if (Pistol_S.angle >= -70 && !rhand)
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
                // Debug.Log("x = " + x + " y = " + y);
        }

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized;

        look = new Vector2(x, y);

        if (moveVelocity.x == 0 && moveVelocity.y == 0)
        {
            AudioManager.Stop("Footstep Loop");
            walking = false;
            animator.SetLayerWeight(1, 0);
            outlineAnimator.SetLayerWeight(1, 0);
        }
        else
        {
            if (!walking)
            {
                walking = true;
                AudioManager.Play("Footstep Loop");
            }


            animator.SetLayerWeight(1, 1);
            outlineAnimator.SetLayerWeight(1, 1);
        }

        if (look.x != 0 || look.y != 0)
        {
            LookAtCamera(look);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
            outlineAnimator.SetLayerWeight(1, 0);
        }
    }


    // ** Set Methods **
    public void setChildSprite(GameObject child)
    {
        childSprite = child;
        childSpriteRenderer = childSprite.GetComponent<SpriteRenderer>();
    }


    // ** Modifies the Animation Controller of the Player **
    public void LookAtCamera(Vector3 direction)
    {
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
        outlineAnimator.SetFloat("x", direction.x);
        outlineAnimator.SetFloat("y", direction.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCanvasAnimator_S : MonoBehaviour // Used to allow Sprite Animations in Image UI Assets
{


    public RuntimeAnimatorController controller; // set the controller you want to use in the inspector

    private Image imageCanvas;
    private SpriteRenderer fakeRenderer;
    private Animator animator;


    void Awake() // Called before the Start Method because this creates components that are called by Start Method in Player_S and Pistol_S
    {
        imageCanvas = GetComponent<Image>();
        fakeRenderer = gameObject.AddComponent<SpriteRenderer>();
        fakeRenderer.enabled = false;
        animator = gameObject.AddComponent<Animator>();

        animator.runtimeAnimatorController = controller; // set the controller
    }

    void Update()
    {

        if (animator.runtimeAnimatorController)
        {
            imageCanvas.sprite = fakeRenderer.sprite;
        }
    }

}

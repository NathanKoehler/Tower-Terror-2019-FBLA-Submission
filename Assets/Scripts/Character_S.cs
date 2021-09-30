using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character_S : MonoBehaviour {
    [SerializeField]
    private float speed;

    private Animator animator;

    private GameObject controller;

    protected Vector3 direction;

    // Use this for initialization
    protected virtual void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    public void Move()
    {
        if (direction.x != 0 || direction.y != 0 || direction.z != 0)
        {
            AnimMovement(direction);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }
    
    public void AnimMovement(Vector3 direction)
    {
        animator.SetLayerWeight(1, 1);

        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
        animator.SetFloat("z", direction.z);
    }
}

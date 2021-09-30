using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture_S : MonoBehaviour
{
    public char facingDirection;
    
    
    private float zOffset = 0; // Negative Z value offset


    void Start()
    {
        ManageLayering();
    }


    // ** Methods **
    protected void ManageLayering()
    {
        zOffset = -1 * (transform.position.y / Mathf.Tan(71.0954241574f));
        if (transform.parent.transform.CompareTag("Furniture")) // Offsets objects above others
        {
            zOffset = -1 * (transform.position.y / Mathf.Tan(71.0954241574f)) - 0.06565498f;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, zOffset); // Sets Z to allow smooth Layer Management for Sprites
    }


    // ** Get Methods **
    public char getFacingDirection()
    {
        return facingDirection;
    }


    // ** Set Methods **
}

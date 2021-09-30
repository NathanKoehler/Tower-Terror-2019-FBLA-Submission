using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staple_S : MonoBehaviour
{
    public float speed;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90f + transform.rotation.z * Mathf.PI * Mathf.Rad2Deg)); // Adds Rotation to Sprite to account for Sprite Asset
        GetComponent<Rigidbody2D>().AddRelativeForce(new Vector3(speed, 0, 0));
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Finds if the target is an enemy
        {
            //other.gameObject.dealDamage(1); //TODO Deal damage to found enemies
        }
    }
}

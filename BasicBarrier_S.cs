using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBarrier_S : MonoBehaviour
{
    [SerializeField]
    private bool inverted = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inverted && (collision.CompareTag("Bullet") || collision.CompareTag("Staple")))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inverted && (collision.CompareTag("Bullet") || collision.CompareTag("Staple")))
        {
            Destroy(collision.gameObject);
        }
    }
}

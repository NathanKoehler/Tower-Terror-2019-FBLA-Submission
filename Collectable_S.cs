using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_S : MonoBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player_S>().setCanCollect(gameObject, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collected && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player_S>().setCanCollect(gameObject, false);
        }
    }


    // ** Get Methods **
    public bool getCollected()
    {
        return collected;
    }


    // ** Set Methods **
    public void setCollected(bool newStatus)
    {
        collected = newStatus;
    }
}

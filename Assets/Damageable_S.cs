using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable_S : MonoBehaviour
{
    public string tagToFind = "Bullet";

    public Player_S player;
    public Enemy_S enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != null)
        {

        }
        if ( (enemy != null && collision.gameObject.CompareTag("Staple")) || (player != null && collision.gameObject.CompareTag("Bullet"))) 
        {
            if (player != null)
            {
                player.takeDamage(1);
            }

            if (enemy != null)
            {
                enemy.takeDamage(1);
            }
        }

    }
}

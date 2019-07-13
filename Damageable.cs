using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour { // Used for all assets that can take damage

    [SerializeField]
    private float health;

    public void dealHealth(float dealtHealth)
    {
        health -= dealtHealth;
        healthCheck();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Attack")
        {
            //collision.gameObject.GetComponent<Spell_Properties>().dealProperties(this.gameObject);
        }
    }

    private void healthCheck()
    {
        if (health <= 0)
        {
            
            Destroy(this.gameObject);
        }
    }
}

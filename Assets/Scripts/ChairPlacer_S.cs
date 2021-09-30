using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairPlacer_S : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        int randInt = Random.Range(0, transform.childCount); // Number of Chairs to Destroy
        for (int i = randInt - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(Random.Range(0, transform.childCount)).gameObject); // Destroys Chairs
        }
        Destroy(this);
    }
}

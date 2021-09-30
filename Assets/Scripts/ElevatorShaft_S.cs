//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorShaft_S : MonoBehaviour
{
    public float speed = -4;


    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, speed));
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_S : MonoBehaviour
{
    private static Elevator_S self;


    void Awake()
    {
        if (self == null)
        {
            self = this;

            DontDestroyOnLoad(gameObject); // Basic method to remain even after scene load
            GameController_S.maintainedScripts.Add(gameObject);
        }
        else Destroy(gameObject);
    }
}
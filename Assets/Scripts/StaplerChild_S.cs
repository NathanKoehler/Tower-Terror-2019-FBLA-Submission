using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaplerChild_S : MonoBehaviour
{
    void Awake() // Before the next Update
    {
        GetComponent<HingeJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
    }
}

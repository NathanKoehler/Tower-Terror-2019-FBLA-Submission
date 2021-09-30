using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocation_S : MonoBehaviour
{
    [SerializeField]
    private Vector3 desiredLocation = Vector3.zero;

    private float speed = 0.01f;

    private bool destroyOnEnd = false;

    private float fraction = 0;

    private Transform newParent = null;


    // Update is called once per frame
    void Update()
    {
        if (desiredLocation != null)
        {
            if (0.01 < Vector2.Distance(transform.position, desiredLocation))
            {
                if (fraction < 1)
                {
                    fraction += Time.deltaTime * speed;
                    transform.position = Vector3.Lerp(transform.position, desiredLocation, fraction);
                }
            }
            else
            {
                transform.position = new Vector3(desiredLocation.x, desiredLocation.y, desiredLocation.z);
                if (destroyOnEnd)
                    Destroy(gameObject);
                else
                {
                    if (newParent != null)
                        transform.SetParent(newParent);
                    Destroy(this);
                }
            }
        }
    }


    // ** Set Methods **
    public void setDesiredLocation(Vector2 desired)
    {
        desiredLocation = new Vector3(desired.x, desired.y, transform.position.z);
    }

    public void setDesiredLocation(Vector2 desired, bool destroy)
    {
        desiredLocation = new Vector3(desired.x, desired.y, transform.position.z);

        destroyOnEnd = destroy;
    }

    public void setDesiredLocation(Vector2 desired, float moveSpeed)
    {
        desiredLocation = new Vector3(desired.x, desired.y, transform.position.z);
        speed = moveSpeed;
        destroyOnEnd = false;
    }

    public void setDesiredLocation(Vector2 desired, float moveSpeed, bool destroy)
    {
        desiredLocation = new Vector3(desired.x, desired.y, transform.position.z);
        speed = moveSpeed;
        destroyOnEnd = destroy;
    }

    public void setDesiredLocation(Vector2 desired, Transform desiredParent)
    {
        desiredLocation = new Vector3(desired.x, desired.y, transform.position.z);
        destroyOnEnd = false;
        newParent = desiredParent;
    }

    public void setDesiredLocation(Vector2 desired, float moveSpeed, Transform desiredParent)
    {
        desiredLocation = new Vector3(desired.x, desired.y, transform.position.z);
        speed = moveSpeed;
        destroyOnEnd = false;
        newParent = desiredParent;
    }
}

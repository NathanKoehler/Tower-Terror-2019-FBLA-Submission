using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDecorator_S : MonoBehaviour
{
    public int type = 1;

    private GameObject tableGroup;


    // ** Methods **
    public void createTable()
    {
        Instantiate(tableGroup, transform.position, Quaternion.identity);
    }


    // ** Get Methods **
    public int GetGroup()
    {
        return type;
    }


    // ** Set Methods **
    public void SetTableGroup(GameObject tableGroup)
    {
        this.tableGroup = tableGroup;
    }
}

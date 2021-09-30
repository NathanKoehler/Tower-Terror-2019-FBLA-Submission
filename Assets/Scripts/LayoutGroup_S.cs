using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGroup_S : MonoBehaviour
{
    public GameObject[] gameObjects;


    // Start is called before the first frame update
    void Start()
    {
        configureTableGroups();
    }


    // ** Methods **
    private void configureTableGroups()
    {
        int randomInt1 = Random.Range(0, gameObjects.Length);
        int randomInt2 = Random.Range(0, gameObjects.Length);
        int randomInt3 = Random.Range(0, gameObjects.Length * 2 - 1);
        int randomInt4 = Random.Range(0, gameObjects.Length);



        for (int i = 0; i < transform.childCount; i++)
        {
            RoomDecorator_S Decorator = transform.GetChild(i).GetComponent<RoomDecorator_S>();
            int group = Decorator.GetGroup();
            if (group == 1)
                Decorator.SetTableGroup(gameObjects[randomInt1]);
            else if (group == 2)
                Decorator.SetTableGroup(gameObjects[randomInt2]);
            else if (group == 3)
                if (randomInt3 >= gameObjects.Length)
                    Decorator.SetTableGroup(gameObjects[0]);
                else
                    Decorator.SetTableGroup(gameObjects[randomInt3]);
            else if (group == 4)
            {
                Decorator.SetTableGroup(gameObjects[randomInt4]);
            }
            Decorator.createTable();
        }
    }
}

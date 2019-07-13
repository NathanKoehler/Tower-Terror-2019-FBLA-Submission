using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureParent_S : Furniture_S
{
    public GameObject[] furnitureArray;
    public static GameObject[] furnitureArrayF;
    public static GameObject[] furnitureArrayR;
    public static GameObject[] furnitureArrayB;
    public static GameObject[] furnitureArrayL;


    // Start is called before the first frame update
    private void Start()
    {
        ManageLayering();
        placeChildFurniture();
    }


    // ** Methods **
    private void placeChildFurniture()
    {
        Vector2 xy;
        if (facingDirection == 'F')
        {
            xy = new Vector2(-0.004525006f, 0.07549998f);
            furnitureArray = furnitureArrayF;
        }
        else if (facingDirection == 'R')
        {
            xy = new Vector2(-0.00257498f, 0.06424993f);
            furnitureArray = furnitureArrayR;
        }
        else if (facingDirection == 'B')
        {
            xy = new Vector2(-0.004525006f, 0.075f);
            furnitureArray = furnitureArrayB;
        }
        else if (facingDirection == 'L')
        {
            xy = new Vector2(-0.004525006f, 0.075f);
            furnitureArray = furnitureArrayL;
        }
        else
        {
            Debug.Log("Error finding Furniture Child Location"); // Will not be called if the furniture array is less
            xy = Vector2.zero;
        }

        int randInt = Random.Range(0, furnitureArray.Length + 4);
        if (Random.Range(0, furnitureArray.Length + 4) < furnitureArray.Length)
        {
            randInt = Random.Range(0, furnitureArray.Length);
            GameObject newObj = Instantiate(furnitureArray[randInt], transform);
            newObj.transform.localPosition = xy;
            newObj.transform.position.Scale(new Vector3(0.5f, 0.5f, 0));
        }
    }


    // ** Set Methods **
    public static void setAllArrays(GameObject[] f, GameObject[] b, GameObject[] r, GameObject[] l)
    {
        furnitureArrayF = f;
        furnitureArrayB = b;
        furnitureArrayR = r;
        furnitureArrayL = l;
    }

    public static void setFurnitureArrayR(GameObject[] furniture)
    {
        furnitureArrayR = furniture;
    }

    public static void setFurnitureArrayF(GameObject[] furniture)
    {
        furnitureArrayF = furniture;
    }

    public static void setFurnitureArrayB(GameObject[] furniture)
    {
        furnitureArrayB = furniture;
    }

    public static void setFurnitureArrayL(GameObject[] furniture)
    {
        furnitureArrayL = furniture;
    }
}

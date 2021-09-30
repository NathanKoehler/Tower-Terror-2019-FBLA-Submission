using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurniturePlacer_S : MonoBehaviour
{
    public GameObject[] furnitureArray;

    [SerializeField]
    private float spawnChance = 1;

    // Start is called before the first frame update
    void Start()
    {
        SpawnObject();
        Destroy(this);
    }

    
    // ** Methods **
    private void SpawnObject()
    {
        if (Random.value < spawnChance)
            Instantiate(furnitureArray[Random.Range(0, furnitureArray.Length)], transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnLocation_S : MonoBehaviour
{
    public static GameController_S gameController;

    public static List<Vector3> Spawns = new List<Vector3>();

    public Vector3[] enemySpawnPositions;

    public GameObject alien = null;


    private void Awake()
    {
        foreach (Vector3 pos in enemySpawnPositions)
        {
            Vector3 worldPos = pos + transform.position;
            Spawns.Add(worldPos);
        }
    }


    // ** Methods **
}

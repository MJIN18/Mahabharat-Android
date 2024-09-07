using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public GameObject[] powerUps;

    public PowerUpSpawner[] powerUpPoints;

    // Start is called before the first frame update
    void Start()
    {
        foreach(PowerUpSpawner powerSpawnPoint in powerUpPoints)
        {
            if(powerSpawnPoint.canSpawn)
                SpawnRandomPowerups(powerSpawnPoint);
        }
    }

    void SpawnRandomPowerups(PowerUpSpawner powerSpawner)
    {
        int index = Random.Range(0, powerSpawner.powerPoints.Length);

        int spawnCheck = Random.Range(0, 8);

        if(spawnCheck > 6)
        {
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], powerSpawner.powerPoints[index]);
        }
    }
}

[System.Serializable]
public struct PowerUpSpawner
{
    public bool canSpawn;
    public Transform[] powerPoints;
}

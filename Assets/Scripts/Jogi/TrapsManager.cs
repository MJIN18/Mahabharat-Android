using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.ParticleSystem;

public class TrapsManager : MonoBehaviour
{
    public bool movingTrapManager;

    public TrapsSpawner[] trapsSpawner;

    public GameObject[] staticTraps;

    public GameObject movingTrap;

    public GameObject[] coinWithTraps;

    public GameObject[] coinSets;

    public GameObject trapCoinSet;

    //public Transform[] trapSpawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        foreach (TrapsSpawner trap in trapsSpawner)
        {
            if (trap.canSpawn)
            {
                SpawnRandomTraps(trap);
            }
        }
    }

    public void SpawnRandomTraps(TrapsSpawner trapSpawner)
    {
        int selectRandomTwoOrOne = Random.Range(0, 4);

        if (selectRandomTwoOrOne > 2)
        {
            int first = Random.Range(0, trapSpawner.trapsPoints.Length);

            int second;
            do
            {
                second = Random.Range(0, trapSpawner.trapsPoints.Length);

            } while (second == first);

            int third;
            do
            {
                do
                {
                    third = Random.Range(0, trapSpawner.trapsPoints.Length);
                } while (third == second);

            } while (third == first);

            int spawnCheck = Random.Range(0, 5);

            Debug.Log("Check A " + spawnCheck);

            bool canSpawnCoinSet = false;
            bool canSpawnMovingTrap = false;

            if (spawnCheck > 3)
            {
                canSpawnCoinSet = true;
            }
            else if (spawnCheck > 1 && spawnCheck <= 3)
            {
                canSpawnMovingTrap = true;
            }

            Instantiate(staticTraps[Random.Range(0, staticTraps.Length)], trapSpawner.trapsPoints[first]);
            Instantiate(staticTraps[Random.Range(0, staticTraps.Length)], trapSpawner.trapsPoints[second]);

            if (canSpawnCoinSet)
                Instantiate(coinSets[Random.Range(0, coinSets.Length)], trapSpawner.trapsPoints[third]);
            if (canSpawnMovingTrap)
                Instantiate(movingTrap, trapSpawner.trapsPoints[third]);
        }
        else
        {
            int one = Random.Range(0, trapSpawner.trapsPoints.Length);

            int two;
            do
            {
                two = Random.Range(0, trapSpawner.trapsPoints.Length);

            } while (two == one);

            int spawnCheck = Random.Range(0, 5);

            Debug.Log("Check B " +  spawnCheck);

            bool canSpawnCoinSet = false;
            bool canSpawnTrapCoinSet = false;
            bool canSpawnMovingTrap = false;

            if(spawnCheck >= 4)
            {
                canSpawnCoinSet = true;
            }
            else if(spawnCheck > 2 && spawnCheck <= 3)
            {
                canSpawnTrapCoinSet = true;
            }
            else if(spawnCheck > 1 && spawnCheck <= 2)
            {
                canSpawnMovingTrap = true;
            }

            Instantiate(staticTraps[Random.Range(0, staticTraps.Length)], trapSpawner.trapsPoints[one]);

            if(canSpawnCoinSet)
                Instantiate(coinSets[Random.Range(0, coinSets.Length)], trapSpawner.trapsPoints[two]);
            if (canSpawnTrapCoinSet)
                Instantiate(trapCoinSet, trapSpawner.trapsPoints[two]);
            if(canSpawnMovingTrap)
                Instantiate(movingTrap, trapSpawner.trapsPoints[two]);
        }
    }
}

[System.Serializable]
public struct TrapsSpawner
{
    public bool canSpawn;
    public Transform[] trapsPoints;
}

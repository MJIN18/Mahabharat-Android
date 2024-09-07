using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LvlGenerationSC : MonoBehaviour
{
    public static LvlGenerationSC instance;

    public Transform[] coinBlock;

    [SerializeField] Transform Starting_block1;
    [SerializeField] List<Transform> blockLists;
    [HideInInspector] public Transform Block1_endpointPosition;
    Transform chooseLevelPart;

   public  Transform epp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Block1_endpointPosition = Starting_block1.Find("EndPoint");
        int startingSpawnLevelParts = 1;

        for (int i = 0; i < startingSpawnLevelParts; i++)
        {
            spawnLevelParts(Block1_endpointPosition);
        }

    //    InvokeRepeating("SpawnerCheck", 6f,6f);
    }

   public  void SpawnerCheck()
    {
      
        spawnLevelParts(epp);
    }

    public void spawnLevelParts(Transform endPointPosition)
    {
        int spawnCheck = Random.Range(0, 5);

        if(spawnCheck >= 4)
        {
            chooseLevelPart = coinBlock[Random.Range(0, coinBlock.Length)];
        }
        else
        {
            chooseLevelPart = blockLists[Random.Range(0, blockLists.Count)];
        }

        Transform endpoint_starating_block = SpawnBlock(chooseLevelPart, endPointPosition);
    }
    private Transform SpawnBlock(Transform levelpart, Transform spawningPoint)
    {
        Transform B = Instantiate(levelpart, spawningPoint.position, Quaternion.identity);
        return B;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNpcSpawner : MonoBehaviour
{
    public GameObject[] mainNPCs;

    public Transform[] mainNpcPoint;

    public Transform[] sideNpcPoint;

    // Start is called before the first frame update
    void Start()
    {
        //int mainNpcIndex = Random.Range(0, mainNPCs.Length);

        SpawnMainNPC();
    }

    public void SpawnMainNPC()
    {

        for (int i = 0; i < mainNpcPoint.Length; i++)
        {
            if(mainNpcPoint[i].childCount > 0)
            {
                foreach (Transform child in mainNpcPoint[i])
                {
                    Destroy(child.gameObject);
                }
            }
            Instantiate(mainNPCs[(int)LevelManager.instance.stage], mainNpcPoint[i]);
        }



        for (int i = 0; i < sideNpcPoint.Length; i++)
        {
            if(sideNpcPoint[i].childCount > 0)
            {
                foreach (Transform child in sideNpcPoint[i])
                {
                    Destroy(child.gameObject);
                }
            }
            Instantiate(mainNPCs[(int)LevelManager.instance.stage], sideNpcPoint[i]);
        }
    }
}

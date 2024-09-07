using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsSpawnManager : MonoBehaviour
{
    public GameObject spawnPos;

    public GameObject[] TrapSets;

    /* private void Start()
     {
         Debug.Log("TrapsSpawnManagerGotCalled");
         //Below is used when we are using the instantiation method to spawn the platform
         Instantiate(TrapSets[Random.Range(0,TrapSets.Length)],spawnPos.transform);
     }*/

 /*   private void Start()
    {
        OnEnable();
    }*/

    private void OnEnable()
    {
        //GameManager.instance.ShowDubug("TrapsSpawnManagerGotCalled");
        Instantiate(TrapSets[Random.Range(0, TrapSets.Length)], spawnPos.transform);
    }
}

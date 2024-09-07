using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLvl : MonoBehaviour
{
    private Transform endpointPOS;

    private void Awake()
    {
        endpointPOS = gameObject.transform.parent.GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("SavePlayer"))
        {
            Debug.Log("Triggererererered");
            LvlGenerationSC.instance.spawnLevelParts(endpointPOS);

        }
    }


}

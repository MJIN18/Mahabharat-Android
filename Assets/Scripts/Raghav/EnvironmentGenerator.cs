using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    //   public GameObject[] AllEnvironments;

    //  public Environment environment;
    public GameObject EndPoint;
      int n;

    public GameObject[] AllEnvironments;

    private void Start()
    {
        //     StartCoroutine(GenerateEnvironment());

        InvokeRepeating("GenerateEnvironment", 0f, 6f);

  //      Debug.Log("Name of the prefab======" + environment.EnvironmentPrefab[0]);
    }

   /* IEnumerator GenerateEnvironment()
    {
        yield return null;
    }*/

    public void GenerateEnvironment()
    {
        n = Random.Range(0, 3);
        //    EndPoint = environment.EnvironmentPrefab[n];
        //   environment.EnvironmentPrefab[n].transform.position = EndPoint.transform.position;

   //     Instantiate(environment.EnvironmentPrefab[n] , EndPoint.transform.position , Quaternion.identity);


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlatformPosition : MonoBehaviour
{
    public GameObject EndPoint;
    public GameObject[] AllEnvironments;

    public TrapsSpawnManager trapsSpawnManager;

    int n;

    private void OnTriggerEnter(Collider other)
    {
        //      if (other.gameObject.tag == "Environment")

        Debug.Log("otherrrrrrr=======" + other.gameObject.tag);
    //    if(other.gameObject.tag == "Player")
  //  if(other.name.Contains("Player_Arjun"))
    //    {
         //   n = Random.Range(0, 3);
            //      EndPoint = AllEnvironments[n];
            Debug.Log("Value of N=============" + n);
      /*      if (AllEnvironments[n].gameObject.activeInHierarchy)
            {
                if(n == 0)
                {
                    n = 1;
                }else if(n == 1)
                {
                    n = 0;
                }else if(n == 2)
                {
                    n = 0;
                }
            }*/

       /*     AllEnvironments[n].gameObject.SetActive(true);
            AllEnvironments[n].transform.GetChild(2).GetComponent<TrapsSpawnManager>().enabled = true;
            AllEnvironments[n].transform.position = EndPoint.transform.position;*/



            Debug.Log("Parent Name ===========" + EndPoint.transform.parent);
       // }
    }
}

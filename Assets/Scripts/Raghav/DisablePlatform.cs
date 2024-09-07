using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlatform : MonoBehaviour
{

    //int destroyFirst = 0;

    int z = 0;

    bool setPosition = true;
    GameObject FE;

    public GameObject EndPoint;
    public Vector3 endPointPos;

    private void OnTriggerEnter(Collider other)
    {
        //GameManager.instance.ShowDubug("Other Name=======" + other.tag);
     /*   if (other.tag == "Environment")
        {
            /*  while(other.gameObject.transform.GetChild(2).childCount > 0)
                {
                    Destroy(other.gameObject.transform.GetChild(2).GetChild(0).gameObject);
                }*/
     /*       if (destroyFirst == 0)
            {
                FE = other.gameObject;
                other.gameObject.SetActive(false);
                destroyFirst = 1;
                Invoke("DestroyFirstEnvironment", 2f);
            } */
      /*      else 
            { 
             
                foreach (Transform child in other.gameObject.transform.GetChild(2).transform)
            {
                Destroy(child.gameObject);
            }

            other.gameObject.transform.GetChild(2).GetComponent<TrapsSpawnManager>().enabled = false;
                Debug.Log("GameObject Name Other=========" + other.gameObject.name);
            other.gameObject.SetActive(false);
            }
        }*/
     /*   else if (other.tag == "Traps" || other.tag == "Magnet" || other.tag == "Invisible" || other.tag == "Shield" || other.tag == "Coin")
        {
            Destroy(other.gameObject);
            Debug.Log("other Parent Name=======" + other.transform.root + "Parent ======" + other.transform.parent);
        }*/
        
    /*    if(other.gameObject.name.Contains("EnvironmentVer (1)"))
        {
            Destroy(other.gameObject);
        }*/

        if(other.tag == "Environment")
        {
            endPointPos = other.GetComponent<PlatformMotion>().endPoint.position;
            other.GetComponent<PlatformMotion>().npcSpawner.SpawnMainNPC();
            other.transform.position = endPointPos;

            if (setPosition == true)
            {
               // Platform Speed = -0.13
                switch (z)
                {
                    case 0:
                        {
                            // Environment 1
                            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                            z = 1;
                            break;
                        }

                    case 1:
                        {
                            // Environment 2
                            z = 2;
                            break;
                        }

                    case 2:
                        {
                            // Environment 3
                            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                            z = 0;

                            setPosition = false;
                            break;
                        }
                }
            }

    //        
            foreach (Transform child in other.gameObject.transform.GetChild(2).transform)
            {
                Destroy(child.gameObject);
            }
            Debug.Log(other.name);
            other.gameObject.transform.GetChild(2).GetComponent<TrapsSpawnManager>().enabled = false;
            other.gameObject.transform.GetChild(2).GetComponent<TrapsSpawnManager>().enabled = true;

            Debug.Log(other.gameObject.transform.GetChild(3).name);
            other.gameObject.transform.GetChild(3).GetComponent<PowersSpawnManager>().enabled = false;
            other.gameObject.transform.GetChild(3).GetComponent<PowersSpawnManager>().enabled = true;
        }
        else if (other.tag == "Traps" || other.tag == "Magnet" || other.tag == "Invisible" || other.tag == "Shield" || other.tag == "Coin")
        {
            Destroy(other.transform.parent.gameObject);
            //GameManager.instance.ShowDubug("other Root Name=======" + other.transform.root + "Parent ======" + other.transform.parent);
        }
    }

 /*   public void DestroyFirstEnvironment()
    {
        Destroy(FE);
        CancelInvoke("DestroyFirstEnvironment");
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
         if (other.gameObject.tag == "Environment")
         {
             Destroy(other.gameObject);

         }
    }
}

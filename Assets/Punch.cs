using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Traps")
        {
            UIScript.Instance.smallExplosion.SetActive(true);

            UIScript.Instance.smallExplosion.transform.position = other.transform.position;

   //         PlayerController.instance.Punch.SetActive(false);

            UIScript.Instance.StopParticleEffect(1);

            Destroy(other.gameObject);
        }
    }
}

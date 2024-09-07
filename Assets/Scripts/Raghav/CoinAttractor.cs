using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAttractor : MonoBehaviour
{
    public float speed = 5f;
    
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Coin")
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position,transform.position, speed * Time.deltaTime);
        }
    }
}

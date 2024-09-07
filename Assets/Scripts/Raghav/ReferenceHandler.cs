using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceHandler : MonoBehaviour
{
    public static ReferenceHandler instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public CoinCollector coinCollector;
    public CoinAttractor coinAttractor;
}

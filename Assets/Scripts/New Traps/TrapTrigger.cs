using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    public TrapType trapType;

    public TargetLocker targetLocker;
    public ArrowTrap arrowTrap;

    private void Start()
    {
        //
        //targetLocker.TrapReset();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "SavePlayer")
        {
            switch (trapType)
            {
                case TrapType.ROCK:
                    targetLocker.LaunchTrap();
                    break;

                case TrapType.FLAME:
                    arrowTrap.ActivateTrap();
                    break;
            }
        }
    }
}

public enum TrapType
{
    FLAME,
    ROCK
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPlusTrapManager : MonoBehaviour
{
    public GameObject[] movingTraps;
    public Transform trapsPoint;

    public int triggerDistance;

    PlayerController player;

    GameObject myTrap;

    TrapsBehaviour myTrapsBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        myTrap = Instantiate(movingTraps[Random.Range(0, movingTraps.Length)], trapsPoint);
        myTrapsBehaviour = myTrap.GetComponentInChildren<TrapsBehaviour>();
        myTrapsBehaviour.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            player = PlayerController.instance;
        }
        float dis = Vector3.Distance(transform.position, player.transform.position);

        if(dis < triggerDistance && !myTrapsBehaviour.enabled)
        {
            myTrapsBehaviour.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "SavePlayer")
        {
            myTrapsBehaviour.enabled = true;
        }
    }
}

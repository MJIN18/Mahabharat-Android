using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject[] NPCs;

    public Transform[] spawnPointBack;
    public Transform[] spawnPointFront;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNPC());
    }

    IEnumerator SpawnNPC()
    {
        yield return new WaitForSeconds(Random.Range(5,10));

        int Chance = Random.Range(0,5);

        if(Chance == 3)
        {
            TrapsBehaviour NPC_A = Instantiate(NPCs[Random.Range(0,NPCs.Length)], spawnPointBack[0].position, spawnPointBack[0].rotation).transform.GetChild(0).GetComponent<TrapsBehaviour>();
            TrapsBehaviour NPC_B = Instantiate(NPCs[Random.Range(0, NPCs.Length)], spawnPointBack[1].position, spawnPointBack[1].rotation).transform.GetChild(0).GetComponent<TrapsBehaviour>();

            TrapsBehaviour NPC_C = Instantiate(NPCs[Random.Range(0, NPCs.Length)], spawnPointFront[0].position, spawnPointFront[0].rotation).transform.GetChild(0).GetComponent<TrapsBehaviour>();
            NPC_C.moveSpeed = NPC_C.moveSpeed * 2f;

            TrapsBehaviour NPC_D = Instantiate(NPCs[Random.Range(0, NPCs.Length)], spawnPointFront[1].position, spawnPointFront[1].rotation).transform.GetChild(0).GetComponent<TrapsBehaviour>();
            NPC_D.moveSpeed = NPC_D.moveSpeed * 2f;
        }
        else if(Chance <= 4)
        {
            for (int i = 0; i < spawnPointBack.Length; i++)
            {
                TrapsBehaviour NPC_A = Instantiate(NPCs[Random.Range(0, NPCs.Length)], spawnPointBack[i].position, spawnPointBack[i].rotation).transform.GetChild(0).GetComponent<TrapsBehaviour>();

                TrapsBehaviour NPC_B = Instantiate(NPCs[Random.Range(0, NPCs.Length)], spawnPointFront[i].position, spawnPointFront[i].rotation).transform.GetChild(0).GetComponent<TrapsBehaviour>();
                NPC_B.moveSpeed = NPC_B.moveSpeed * 2f;
            }
        }

        StartCoroutine(SpawnNPC());
    }
}

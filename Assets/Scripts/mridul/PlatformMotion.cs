using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMotion : MonoBehaviour
{
    public Transform endPoint;

    public Vector3 moveDirection;
    public float platformSpeed = -0.1f;

    public MainNpcSpawner npcSpawner;


    private void Start()
    {
     //   StartCoroutine(IncreasePlatformSpeed());
    }


    private void FixedUpdate()
    {
        transform.Translate(moveDirection, Space.World);
        moveDirection.z = platformSpeed;
    }

    IEnumerator IncreasePlatformSpeed()
    {
        yield return new WaitForSeconds(10f);
        platformSpeed = -0.20f;

     /*   yield return new WaitForSeconds(10f);
        platformSpeed = -0.20f;*/

  /*      yield return new WaitForSeconds(10f);
        platformSpeed = -0.25f;

        yield return new WaitForSeconds(10f);
        platformSpeed = -0.27f;*/
    }
}

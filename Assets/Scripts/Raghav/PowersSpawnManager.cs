using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowersSpawnManager : MonoBehaviour
{
//    public Transform spawningPlace;


    public GameObject[] PowerSets;

    public GameObject[] AllPowers;

    float Timer = 2;

 //   GameObject[] a;

    int n = 0;

    int generator = 0;

 //   public GameObject[] trapSets;

  /*  private void Start()
    {
        //       Instantiate(trapSets[Random.Range(0, trapSets.Length)], spawningPlace);
        
        StartCoroutine(GeneratePower());
        Debug.Log("Yeah calling it");
    }*/

    private void OnEnable()
    {
        //       Instantiate(trapSets[Random.Range(0, trapSets.Length)], spawningPlace);

        StartCoroutine(GeneratePower());
        //GameManager.instance.ShowDubug("Yeah calling it");
    }


    IEnumerator GeneratePower()
    {
        n = Random.Range(0, 3);
        generator = Random.Range(0, 8);
        //    Timer = Random.Range(5, 10);
        //Define Timer Here
        //GameManager.instance.ShowDubug("The value of Generator=========" + generator);
        if (generator == 0 || generator == 1 || generator == 5 /*|| generator == 7*/)
        {
            yield return new WaitForSeconds(Timer);
            //     Transform b = a[0].transform.GetChild(0).transform;
            //GameManager.instance.ShowDubug("Yup power Generated");

            GameObject g = AllPowers[Random.Range(0, AllPowers.Length)];

            //   Instantiate(AllPowers[Random.Range(0, AllPowers.Length)], PowerSets[n].transform.GetChild(n).transform);

            /*   if (g.name.Contains("Shield"))
               {

               }*/

            Instantiate(g, PowerSets[n].transform.GetChild(n).transform);

            StopCoroutine(GeneratePower());
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpController : MonoBehaviour
{
    public bool invisibleOn;

    [SerializeField] float timer;

    public GameObject ShieldInHand;
    public GameObject Attractor;
    public GameObject Arjun;
    public GameObject Punch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPower(int p)
    {

        //Setting ShieldOn Layer to ignore the collision betwwen player and traps or obstacles


        switch (p)
        {

            case 0:
                {
                    //Magnet
                    StartCoroutine(StopPower(p));
                    break;
                }

            case 1:
                {
                    //Invisible
                    //this.transform.GetChild(1).GetComponent<CapsuleCollider>().enabled = false;

                    //If shield is on it won't be used if invisible on as well
                    invisibleOn = true;
                    UIScript.Instance.buff.SetActive(true);
                    this.transform.gameObject.layer = 11;
                    Arjun.layer = 11;
                    Debug.Log("Changing Layer====" + this.transform.gameObject.layer);
                    ShieldInHand.GetComponent<BoxCollider>().enabled = false;


                    StartCoroutine(StopPower(p));
                    break;
                }

            case 2:
                {
                    //Shield
                    Debug.Log("value of invisible on=========" + invisibleOn);
                    if (!invisibleOn)
                    {
                        ShieldInHand.SetActive(true);
                        Arjun.tag = "SavePlayer";

                        //ShieldCollider.GetComponent<BoxCollider>().enabled = true;

                        this.transform.gameObject.layer = 11;
                        Arjun.layer = 11;
                    }

                    transform.GetComponent<Animator>().SetLayerWeight(1, 1);

                    /*    this.transform.GetChild(1).tag = "SavePlayer";
                          this.transform.GetChild(1).GetComponent<CapsuleCollider>().isTrigger= true;*/
                    break;
                }

            case 3:
                {

                    break;
                }


        }
    }

    IEnumerator StopPower(int p)
    {
        yield return new WaitForSeconds(timer);


        switch (p)
        {

            case 0:
                {
                    //Magnet
                    Attractor.GetComponent<BoxCollider>().enabled = false;
                    UIScript.Instance.buffMagnet.SetActive(false);
                    break;
                }

            case 1:
                {
                    //Invisible

                    //If shield is on it won't be used if invisible on as well
                    invisibleOn = false;

                    //Setting Player Layer back
                    this.transform.gameObject.layer = 7;
                    this.transform.GetChild(1).gameObject.layer = 7;
                    UIScript.Instance.fistButton.GetComponent<Button>().interactable = true;
                    ShieldInHand.GetComponent<BoxCollider>().enabled = true;

                    UIScript.Instance.buff.SetActive(false);
                    break;
                }

                  case 2:
                      {
                    //  Shield

                   
           //               ShieldinHand.SetActive(false);
                          break;
                      }

        }

    }
}

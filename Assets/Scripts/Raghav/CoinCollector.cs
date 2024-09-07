using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{

 //   public static CoinCollector instance;

 //   private void Awake()
 //   {
       /* if (instance != null)
        {
            Destroy(this.gameObject);
        }*/
     //   else
     //   {
    //        instance = this;
       // }
   // }


    private void OnTriggerEnter(Collider other)
    {
      
        if(other.tag == "Player")
        {
            UIScript uiScript = UIScript.Instance;

            uiScript.coinAudioSource.PlayOneShot(uiScript.coinAudioClip[0]);

            uiScript.collectedCurrency[0].amount = uiScript.collectedCurrency[0].amount + 1;

            uiScript.coins.SetText(uiScript.collectedCurrency[0].amount.ToString());

            //PlayerPrefs.SetInt("TotalCoins", UIScript.Instance.c);

            Destroy(this.gameObject);
        }
    }
}

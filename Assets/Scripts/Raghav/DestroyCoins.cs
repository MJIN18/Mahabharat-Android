using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCoins : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Coin")
        {
            UIScript uiScript = UIScript.Instance;

            uiScript.coinAudioSource.PlayOneShot(uiScript.coinAudioClip[0]);

            uiScript.coinCollector.SetActive(true);

            uiScript.coinCollector.transform.position = this.transform.position;

            uiScript.StopParticleEffect(2);

            uiScript.collectedCurrency[0].amount = uiScript.collectedCurrency[0].amount + 1;

            uiScript.coins.SetText(uiScript.collectedCurrency[0].amount.ToString());

            PlayerPrefs.SetInt("TotalCoins", uiScript.collectedCurrency[0].amount);
            
            Destroy(other.gameObject);
        }
    }
}

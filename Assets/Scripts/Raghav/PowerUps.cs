using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps : MonoBehaviour
{

    int i = 0;

    private void OnTriggerEnter(Collider other)
    {
        //  if (other.tag == "HitBox")
        //   {
        
      

        Debug.Log("Player Name======== and this tag" + "name======" + other.name + "=========" + other.tag + "=======" + this.tag);

        if ((other.tag == "Player" || other.tag == "SavePlayer") && this.tag == "Magnet")
        {
            PlayerControllerNew.instance.powerUpController.Attractor.GetComponent<BoxCollider>().enabled = true;
            //      PlayerController.instance.StartPower(0);

            i = 0;
            UIScript.Instance.buffMagnet.SetActive(true);
            UIScript.Instance.buffMagnet.transform.SetParent(PlayerControllerNew.instance.powerUpController.Arjun.transform);
            UIScript.Instance.buffMagnet.transform.position = PlayerControllerNew.instance.powerUpController.Arjun.transform.position;
            powerUpCommonCode();

        }
        else if ((other.tag == "Player" || other.tag == "SavePlayer") && this.tag == "Invisible")
        {
            //      PlayerController.instance.StartPower(1);

            i = 1;
            Debug.Log("Value of i=========" + i);
            //UIScript.Instance.buff.SetActive(true);
            UIScript.Instance.fistButton.GetComponent<Button>().interactable = false;
            UIScript.Instance.buff.transform.SetParent(PlayerControllerNew.instance.powerUpController.Arjun.transform);
            UIScript.Instance.buff.transform.position = PlayerControllerNew.instance.powerUpController.Arjun.transform.position;

            powerUpCommonCode();

        }
        else if ((other.tag == "Player" || other.tag == "SavePlayer" /*|| other.tag == "Untagged"*/) && this.tag == "Shield")
        {
            //         PlayerController.instance.StartPower(2);

            i = 2;
            PlayerControllerNew.instance.powerUpController.invisibleOn = false;

            powerUpCommonCode();
        }
        else if ((other.tag == "Player" || other.tag == "SavePlayer" || other.tag == "Untagged") && this.tag == "Fist")
        {
            //         PlayerController.instance.StartPower(3);

            UIScript.Instance.fistPowerMethod();
            i = 3;
            powerUpCommonCode();
        }


    }
    //  }

    public void powerUpCommonCode()
    {
        UIScript.Instance.coinAudioSource.PlayOneShot(UIScript.Instance.coinAudioClip[1]);
        UIScript.Instance.pickUpEffect.SetActive(true);

        UIScript.Instance.pickUpEffect.transform.position = transform.position;
        UIScript.Instance.pickUpEffect.transform.rotation = transform.rotation;

        UIScript.Instance.StopParticleEffect(0);


        PlayerControllerNew.instance.powerUpController.StartPower(i);
        Destroy(this.gameObject.transform.parent.gameObject);
    }

  
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionToDeath : MonoBehaviour
{

    public GameObject[] Allenvironment;
    public GameObject breakParticle;

    public bool isFire;

    private void Start()
    {
        if (isFire)
        {
            //gameObject.layer = 11;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collided with" + other.gameObject.name + " With tag = " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            RewardedAdButton.instance.LoadAd();
            PlayerControllerNew playerController = other.transform.root.GetComponent<PlayerControllerNew>();
            LevelManager.instance.player = playerController;
            if (playerController == null)
            {
                return;
            }

            playerController._animation.Play("Die");
            playerController.isDead = true;
            //playerController.touchField.gameObject.SetActive(false);
            //playerController.buttonUI.gameObject.SetActive(false);
            if(SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySFX("Death");
            }
            Allenvironment = GameObject.FindGameObjectsWithTag("Environment");
            foreach (GameObject obj in Allenvironment)
            {
                obj.transform.GetComponent<PlatformMotion>().platformSpeed = 0;
            }

            UIScript.Instance.GameOver(2);
        }
    }
}

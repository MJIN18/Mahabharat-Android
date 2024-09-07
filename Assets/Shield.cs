using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Coming Inside in"+other.gameObject.tag);
        if(other.gameObject.tag == "Traps")
        {
            CollisionToDeath col = other.gameObject.GetComponent<CollisionToDeath>();
            if (col.isFire)
            {
                return;
            }

            Debug.Log("Coming Outside in");
            CollisionToDeath collisionToDeath = other.gameObject.GetComponent<CollisionToDeath>();
            if(collisionToDeath.breakParticle != null)
            {
                GameObject effect = Instantiate(collisionToDeath.breakParticle, other.transform);
            }

            //Triggering shield layer mask
            PlayerControllerNew.instance.GetComponent<Animator>().SetTrigger("ShieldSaver");

            //UIScript.Instance.smallExplosion.SetActive(true);

            //UIScript.Instance.smallExplosion.transform.position = other.transform.position;

            GameObject breakParticle = other.transform.GetComponent<CollisionToDeath>().breakParticle;
            if(breakParticle != null)
            {
                Instantiate(breakParticle, other.transform.position, Quaternion.identity);
            }

            if (UIScript.Instance.shieldBreak.activeInHierarchy)
            {
                ParticleSystem particle = UIScript.Instance.shieldBreak.GetComponent<ParticleSystem>();
                particle.Play();
            }
            else
            {
                UIScript.Instance.shieldBreak.SetActive(true);
            }

            UIScript.Instance.shieldBreak.transform.position = other.transform.position;

            UIScript.Instance.StopParticleEffect(1);

            PlayerControllerNew.instance.powerUpController.ShieldInHand.SetActive(false);

            PlayerControllerNew.instance.powerUpController.Arjun.tag = "Player";
            PlayerControllerNew.instance.powerUpController.gameObject.layer = 7;
            PlayerControllerNew.instance.powerUpController.Arjun.layer = 7;
            PlayerControllerNew.instance._animation.SetLayerWeight(1, 0);

            UIScript.Instance.coinAudioSource.PlayOneShot(UIScript.Instance.coinAudioClip[2]);

            Destroy(other.gameObject);
            
  //          PlayerController.instance.ShieldinHand.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
   //     PlayerController.instance.Arjun.tag = "HitBox";
    }
}

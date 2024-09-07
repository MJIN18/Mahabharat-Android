using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject defaultSkin;
    public RuntimeAnimatorController Controller;
    public ControlMode controlMode;

    public Transform spawnPoint;

    public CutSceneCartBehaviour csc;

 //   public Transform mainCam;

 //   Transform CamPos;

    public Transform target;
    Transform from;

  //  Vector3 playerPos;

    GameObject player = null;

    // Start is called before the first frame update
    void Start()
    {

        //    CamPos = mainCam.transform;

        //     Debug.Log("CamPos========" + CamPos.transform.position);

   //     InvokeRepeating("SetMainCameraPosition", 2f, 10f);

   //     GameObject player = null;
        if (GameManager.instance == null)
        {
            player = Instantiate(defaultSkin, spawnPoint);
            //player.GetComponent<CharacterController>().enabled = true;
            Animator anim = player.GetComponent<Animator>();
            //anim.runtimeAnimatorController = Controller;
            //anim.enabled = true;
            //PlayerControllerNew controller = player.GetComponent<PlayerControllerNew>();
            //controller.enabled = true;
            //controller.mode = GameManager.instance.controlMode;
            //controller.laneDistance = 2;
            //player.GetComponent<PlayerInput>().enabled = true;
            //player.GetComponent<PowerUpController>().enabled = true;
            from = player.transform;
            csc._Player = player.transform;
            csc.playerAnim = anim;

   //         playerPos = from.transform.position;
        }
        else
        {
            //player = Instantiate(GameManager.instance.currentSkin.skin, transform.position, transform.rotation);
            GameManager.instance.UseArmorChild(GameManager.instance.selectedArmor, ArmorType.GAMEPLAY, spawnPoint, ref player);
            //player.GetComponent<CharacterController>().enabled = true;
            Animator anim = player.GetComponent<Animator>();
            //anim.runtimeAnimatorController = GameManager.instance.playerController;
            //anim.enabled = true;
            //PlayerControllerNew controller = player.GetComponent<PlayerControllerNew>();
            //controller.enabled = true;
            //controller.mode = GameManager.instance.controlMode;
            //controller.laneDistance = 2;
            //player.GetComponent<PlayerInput>().enabled = true;
            //player.GetComponent<PowerUpController>().enabled = true;
            from = player.transform;
            csc._Player = player.transform;
            csc.playerAnim = anim;

            GameManager.instance.ShowDubug("Checking the spawn player============" + from.transform);

     //       playerPos = from.transform.position;
        }
    }

 /*   private void Update()
    {
        //       player.transform.position = from.transform.position;

        mainCam.transform = CamPos.transform;
    }*/

  /*  void SetMainCameraPosition()
    {
        mainCam.transform.position = new Vector3(mainCam.transform.position.x, 3.49f, -3.01f);
        Debug.Log("mainCam.transform.position" + mainCam.transform.position);
    }*/
   
}

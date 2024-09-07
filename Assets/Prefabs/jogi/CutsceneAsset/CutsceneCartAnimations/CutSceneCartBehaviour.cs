using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutSceneCartBehaviour : MonoBehaviour
{
    public GameObject cam01;
    public GameObject cam02;
    public GameObject cam03;

    public FollowZMovement fzm;
    public string turnParam;
    public string jumpOverParam;

    public bool spawn;

    public Transform _Player;

    public Animator playerAnim;
    public Animator _KrishnaAnim;
    public Animator[] _horseAnim;

    public void CamChange01()
    {
        cam01.SetActive(false);
        cam02.SetActive(true);
    }

    public void CamChange02()
    {
        cam02.SetActive(false);
        cam03.SetActive(true);
    }

    public void StopCart()
    {
        _KrishnaAnim.SetTrigger("StopCart");
        foreach (Animator anim in _horseAnim)
        {
            anim.Play("StartTurn");
        }
    }

    public void TurnStopHorses()
    {
        foreach (Animator anim in _horseAnim)
        {
            anim.SetTrigger("TurnToStop");
        }
    }

    public void TriggerTurn()
    {
        playerAnim.SetTrigger(turnParam);
    }

    public void EnablePlayerCam()
    {
        _Player.GetComponent<PlayerControllerNew>().playerCamera.SetActive(true);
        fzm.target = _Player.GetComponent<PlayerControllerNew>().playerCamera.transform;
    }

    public void TriggerJumpOver()
    {
        playerAnim.SetTrigger(jumpOverParam);
        EnablePlayerCam();
    }

    public void EnableControls()
    {
        PlayerControllerNew controller = _Player.GetComponent<PlayerControllerNew>();
        //controller.playerCamera.SetActive(true);
        //_Player.GetComponent<CharacterController>().enabled = true;
        controller.enabled = true;
        if (GameManager.instance != null)
            controller.mode = GameManager.instance.controlMode;
        controller.laneDistance = 2;
        _Player.GetComponent<PlayerInput>().enabled = true;
        _Player.GetComponent<PowerUpController>().enabled = true;
        _Player.SetParent(null);
        _Player.localPosition = new Vector3(0f, 0f, 66.4f);
        _Player.localRotation = Quaternion.identity;
        _Player.GetComponent<CharacterController>().enabled = true;
        fzm.enabled = true;
    }
}

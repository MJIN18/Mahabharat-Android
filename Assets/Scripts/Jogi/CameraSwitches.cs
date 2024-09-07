using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitches : MonoBehaviour
{
    public GameObject _Cam01;
    public GameObject _Cam02;
    public GameObject _MainCam01;
    public GameObject _MainCam02;
    public GameObject chariot;
    public PlayerSpawnManager psm;

    public GameObject _PlayButton;

    public void OnPlayButtonClick()
    {
        psm.SpawnPlayer();
        _PlayButton.SetActive(false);
        chariot.SetActive(true);
        _Cam01.SetActive(false);
        _Cam02.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Triggered");
            _MainCam01.SetActive(false);
            _MainCam02.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

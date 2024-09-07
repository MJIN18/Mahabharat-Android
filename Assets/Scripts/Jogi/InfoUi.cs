using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoUi : MonoBehaviour
{
    public string _key;

    public int _index;

    public float _currentTimeScale;

    public GameObject[] _infos;
    public GameObject _closeButton;
    public Button prevButton;
    public Button nextButton;
    public GameObject _infoUi;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(_key))
        {
            ShowInfoUI();
        }
    }

    public void ShowInfoUI()
    {
        _currentTimeScale = Time.timeScale;
        Time.timeScale = 0;
        PlayerPrefs.SetInt(_key, 1);
        _closeButton.SetActive(false);
        _index = 0;
        _infos[0].SetActive(true);
        _infos[1].SetActive(false);
        _infos[2].SetActive(false);
        prevButton.interactable = false;
        nextButton.interactable = true;
        _infoUi.SetActive(true);
    }

    public void NextScreen()
    {
        _infos[_index].SetActive(false);
        _index++;
        _infos[_index].SetActive(true);
        if(_index > 0)
        {
            prevButton.interactable = true;
        }
        if (_index == _infos.Length - 1)
        {
            nextButton.interactable = false;
            _closeButton.SetActive(true);
        }
    }

    public void PrevScreen()
    {
        _infos[_index].SetActive(false);
        _index--;
        _infos[_index].SetActive(true);
        if (_index == 0)
        {
            prevButton.interactable = false;
        }
        if (_index < _infos.Length - 1)
        {
            nextButton.interactable = true;
            _closeButton.SetActive(false);
        }
    }

    public void CloseInfoScreens()
    {
        if(_currentTimeScale == 0)
        {
            _currentTimeScale = 1;
        }
        Time.timeScale = _currentTimeScale;
        _index = 0;
        _infoUi.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance;

    public Character _character;

    public CharacterDatabase characterDB;

    public TMP_Text characterNameText;
    public Transform CharacterPOSInCharacterPnl;
    public Transform CharacterPOSInMainMenu;

    [HideInInspector] public GameObject playerGO;
    [HideInInspector] public int getPlayerId;

    private int SelectedCharacter = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;            
        }
    }

    void Start()
    {
        characterDB = GameManager.instance.characterDatabase;

        int playerID = 0;
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            playerID = PlayerSaveData.Instance.GetPlayerData();
        }
        Character ch = characterDB.GetCharacter(playerID);

        //GameObject cha = Instantiate(ch.CharacterModel, CharacterPOSInMainMenu);
        //cha.GetComponent<PlayerController>().enabled= false;
        //cha.GetComponent<Animator>().enabled= false;
        //cha.transform.localScale = new Vector3(165, 165, 165);

    }

   

  public void NextOption()
    {
        SelectedCharacter++;

        if(SelectedCharacter >= characterDB.CharacterCount)
        {
                SelectedCharacter= 0;
        }

        UpdateCharacter(SelectedCharacter);
    }


    public void BackOption()
    {
        SelectedCharacter--;

        if (SelectedCharacter < 0)
        {
            SelectedCharacter = characterDB.CharacterCount - 1;
        }

        UpdateCharacter(SelectedCharacter);
    }

    public void SelectOption()
    {
        PlayerSaveData.Instance.SavePlayerData(getPlayerId);

    }

    public void UpdateCharacter(int selectedCharacter) 
    {
        Character character = characterDB.GetCharacter(selectedCharacter); // access characters index value from character database(Scriptableobject) 

        //Debug.Log("Show  ======"+ CharacterPOSInCharacterPnl.GetChild(0).gameObject);

        if(CharacterPOSInCharacterPnl.childCount > 0) // if there is a character GAmeObject present, destroy it (so new gameobject can be shown there) 
        {
            for (int i = 0; i < CharacterPOSInCharacterPnl.childCount; i++)
            {
                Destroy(CharacterPOSInCharacterPnl.GetChild(i).gameObject);

            }         
            //Debug.Log("Destroy======" + CharacterPOSInCharacterPnl.GetChild(0).gameObject);
        }

        //playerGO = character.CharacterModel; // access the Gameobject from character Database of particular index value
        
        ////Debug.Log("Playergo======" + playerGO);

        //GameObject PP = Instantiate(playerGO, CharacterPOSInCharacterPnl);
        ////PP.GetComponent<PlayerController>().enabled = false;
        ////PP.GetComponent<Animator>().enabled = false;
        ////PP.transform.localScale = new Vector3(165, 165, 165);
        //Debug.Log("Playergo 1======" + PP);

        //getPlayerId = character.ID;
        //characterNameText.text = character.CharacterName;
    }
}

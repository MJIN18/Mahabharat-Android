using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public Character[] _character;

    public int CharacterCount
    {
        get
        {
            return _character.Length;
        }
    }

    public Character GetCharacter(int index)
    {
        return _character[index];
    }



}

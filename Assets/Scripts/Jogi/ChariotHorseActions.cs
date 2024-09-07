using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotHorseActions : MonoBehaviour
{
    private Animator _myAnim;

    public int horseAction;

    private void Start()
    {
        _myAnim = GetComponent<Animator>();
    }

    public void SetHorseAction()
    {
        horseAction = Random.Range(0, 4);

        if (_myAnim != null)
            _myAnim.SetInteger("HorseAction", horseAction);
    }

    public void ResetHorseAction(int actionCode)
    {
        if (_myAnim != null)
            _myAnim.SetInteger("HorseAction", actionCode);
    }
}

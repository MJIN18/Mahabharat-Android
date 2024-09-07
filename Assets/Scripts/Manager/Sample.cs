using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Sample : MonoBehaviour
{
    public void OnValueChange(TMP_InputField o)
    {
        GameManager.instance.ShowDubug(string.Format("Sample:OnValueChange[{1}] ({0})", o.text, o.name));
    }
    public void OnEndEdit(TMP_InputField o)
    {
        GameManager.instance.ShowDubug(string.Format("Sample:OnEndEdit[{1}] ({0})", o.text, o.name));
    }
}

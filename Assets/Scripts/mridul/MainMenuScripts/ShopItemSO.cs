using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="shopMenu",menuName ="Scriptable Obects/New Shop Item")]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public string description;
    public int baseCost;
}
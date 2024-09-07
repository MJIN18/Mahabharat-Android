using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    public int Coins;
    public ShopItemSO[] shopItems;
    public ShopTemplate[] shopTemplates;
    public GameObject[] shopTemplateGO;
    public Button[] PurchaseItemBtn;
    private void Start()
    {
        for (int i = 0 ; i < shopTemplateGO.Length; i++)
        {
            if (!shopTemplateGO[i].activeInHierarchy)
            {
                shopTemplateGO[i].SetActive(true);
            }
        }
    }

    public void LoadPanels()
    {
        for(int i = 0; i < shopItems.Length; i++)
        {
            shopTemplates[i].titleTxt.text = shopItems[i].title;
            shopTemplates[i].descriptionTxt.text= shopItems[i].description;
            shopTemplates[i].CostTxt.text = "Coins : " + shopItems[i].baseCost.ToString();
        }
    }

    public void CheckPurchasableItems()
    {
        for(int i = 0; i < shopItems.Length; i++) 
        {
            if (Coins >= shopItems[i].baseCost)  // Have enougf money to purchase item
            {
                PurchaseItemBtn[i].interactable = true;
            }
            else
            {
                PurchaseItemBtn[i].interactable = false;
            }
        }
    }

    public void PurchaseItems(int BtnNo)
    {
        if(Coins >= shopItems[BtnNo].baseCost)
        {
            Coins = Coins - shopItems[BtnNo].baseCost;
            Debug.Log(BtnNo);
            Debug.Log(Coins);
            CheckPurchasableItems();
            // Unlock Item
        }
    }

}

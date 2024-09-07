using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ShopTemplate : MonoBehaviour
{
    public TMP_Text titleTxt;
    public TMP_Text descriptionTxt;
    public TMP_Text CostTxt;
    public int Cost;
    public Item item;
    public bool purchased;
    public Button purchaseButton;
    public TextMeshProUGUI buttonText;
    public Image icon;

    public bool isInAppPurchase;
    
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        titleTxt.text = item.itemName;

        if(descriptionTxt != null)
        {
            descriptionTxt.text = item.description;
        }

        if(isInAppPurchase)
        {

            CostTxt.text = "$ " + Cost.ToString();
        }
        else
        {
            CostTxt.text = Cost.ToString();
        }

        icon.sprite = item.icon;

        if( purchased)
        {
            purchaseButton.onClick.RemoveAllListeners();
            //buttonText.text = "SOLD";
        }
        else
        {
            //buttonText.text = "BUY";
        }
    }

    //public void Purchase()
    //{
    //    if(gameManager.coins >= Cost)
    //    {
    //        if(item.type == ItemType.SKIN)
    //        {
    //            gameManager.coins = gameManager.coins - Cost;
    //            gameManager.characterDatabase._character[item.itemID].unlocked = true;
    //            purchaseButton.onClick.RemoveAllListeners();
    //            buttonText.text = "SOLD";
    //            purchased = true;
    //        }
    //    }
    //    else
    //    {
    //        gameManager.ShowOffer();
    //    }
    //}
}

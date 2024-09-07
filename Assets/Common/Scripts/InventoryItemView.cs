using System;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples
{
    public class InventoryItemView : MonoBehaviour
    {
        public Sprite swordSprite;
        public Sprite shieldSprite;

        private GameObject armor;

        public string _id;

        [Header("Tarality")]
        public Sprite ironArmor;
        public Sprite bronzeArmor;
        public Sprite silverArmor;
        public Sprite goldArmor;

        public Armor iron;
        public Armor bronze;
        public Armor silver;
        public Armor gold;

        Image m_IconImage;

        void Awake()
        {
            m_IconImage = GetComponentInChildren<Image>();
        }

        public void SetKey(string key)
        {
            _id = key;

            switch (key)
            {
                case "SWORD":
                    m_IconImage.sprite = swordSprite;
                    break;

                case "SHIELD":
                    m_IconImage.sprite = shieldSprite;
                    break;

                case "IRON_ARMOR":
                    m_IconImage.sprite = iron.icon;
                    armor = iron.armor;
                    break;

                case "BRONZE_ARMOR":
                    m_IconImage.sprite = bronze.icon;
                    armor = bronze.armor;
                    break;

                case "SILVER_ARMOR":
                    m_IconImage.sprite = silver.icon;
                    armor = silver.armor;
                    break;

                case "GOLD_ARMOR":
                    m_IconImage.sprite = gold.icon;
                    armor = gold.armor;
                    break;

                default:
                    m_IconImage.sprite = shieldSprite;
                    break;
            }
        }

        public void ChooseArmor()
        {
            GameManager.instance.UseArmor(_id, ArmorType.DISPLAY, GameObject.Find("DisplaySpot").transform,ref GameManager.instance.prevbody);

            GameManager.instance._SR.objectToRotatr = GameManager.instance.prevbody.transform;

            //Transform armorPoint = GameManager.instance.previewBody;
            //GameObject fullBody = GameManager.instance._Body;

            //if (armorPoint.childCount > 0)
            //{
            //    for (int i = 0; i < armorPoint.childCount; i++)
            //    {
            //        Destroy(armorPoint.GetChild(i).gameObject);
            //    }
            //}

            //if (fullBody.activeInHierarchy)
            //{
            //    fullBody.SetActive(false);
            //}

            //GameManager.instance.selectedArmor = armor;
            //Instantiate(armor, armorPoint);
            //fullBody.SetActive(true);
            //GetComponent<Button>().interactable = false;
        }
    }
}

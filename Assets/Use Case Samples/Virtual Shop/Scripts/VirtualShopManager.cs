using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Services.Samples.VirtualShop
{
    public class VirtualShopManager : MonoBehaviour
    {

        [Header("Sprites")]
        public Sprite _sprite;
        public Sprite _default;
        public EconomySprites[] economySprites;

        public static VirtualShopManager instance { get; private set; }

        public Dictionary<string, VirtualShopCategory> virtualShopCategories { get; private set; }

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this);
            }
            else
            {
                instance = this;
            }
        }

        void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        public void Initialize()
        {
            virtualShopCategories = new Dictionary<string, VirtualShopCategory>();

            foreach (var categoryConfig in RemoteConfigManager.instance.virtualShopConfig.categories)
            {
                var virtualShopCategory = new VirtualShopCategory(categoryConfig);
                virtualShopCategories[categoryConfig.id] = virtualShopCategory;
            }
        }



        public Sprite GetSpriteByID(string id)
        {
            Sprite _spritee = _default;

            for (int i = 0; i < economySprites.Length; i++)
            {
                if (economySprites[i].Id == id)
                {
                    _spritee = economySprites[i]._Sprite;
                    break;
                }
            }

            return _spritee;
        }
    }
}

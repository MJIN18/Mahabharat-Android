using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;
using GAMEDEV.Save;
using UnityEngine.UI;

namespace Unity.Services.Samples.VirtualShop
{
    public class VirtualShopSceneManager : MonoBehaviour
    {
        public static VirtualShopSceneManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        const int k_EconomyPurchaseCostsNotMetStatusCode = 10504;

        public VirtualShopSampleView virtualShopSampleView;

        //async void Start()
        //{
        //    //await InitializeShopAsync();
        //}

        public async void InitializeStoreAsync()
        {
            await InitializeShopAsync();
        }

        public async Task InitializeShopAsync()
        {
            try
            {
                //GameManager.instance.ShowDubug("A");
                await UnityServices.InitializeAsync();
                //GameManager.instance.ShowDubug("B");
                // Check that scene has not been unloaded while processing async wait to prevent throw.
                if (this == null) return;

                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    //await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    //if (this == null) return;
                    MenuManager.instance.GoToSignInPage();
                    return;
                }

                GameManager.instance.ShowDubug($"Player id:{AuthenticationService.Instance.PlayerId}");
                //GameManager.instance.ShowDubug("C");
                await GameManager.instance.UpdateCurrencies();
                await EconomyManager.instance.RefreshEconomyConfiguration();
                if (this == null) return;
                //GameManager.instance.ShowDubug("D");
                EconomyManager.instance.InitializeVirtualPurchaseLookup();
                //GameManager.instance.ShowDubug("E");
                // Note: We want these methods to use the most up to date configuration data, so we will wait to
                // call them until the previous two methods (which update the configuration data) have completed.
                await Task.WhenAll(AddressablesManager.instance.PreloadAllEconomySprites(),
                    //RemoteConfigManager.instance.FetchConfigs(),
                    EconomyManager.instance.RefreshCurrencyBalances());
                if (this == null) return;
                //GameManager.instance.ShowDubug("F");
                // Read all badge addressables
                // Note: must be done after Remote Config values have been read (above).
                await AddressablesManager.instance.PreloadAllShopBadgeSprites(
                    RemoteConfigManager.instance.virtualShopConfig.categories);
                //GameManager.instance.ShowDubug("G");
                // Initialize all shops.
                // Note: must be done after all other initialization has completed (above).
                VirtualShopManager.instance.Initialize();
                //GameManager.instance.ShowDubug("H");
                virtualShopSampleView.Initialize(VirtualShopManager.instance.virtualShopCategories);
                //GameManager.instance.ShowDubug("I");
                var firstCategoryId = RemoteConfigManager.instance.virtualShopConfig.categories[0].id;
                if (!VirtualShopManager.instance.virtualShopCategories.TryGetValue(
                        firstCategoryId, out var firstCategory))
                {
                    throw new KeyNotFoundException($"Unable to find shop category {firstCategoryId}.");
                }
                //GameManager.instance.ShowDubug("J");
                virtualShopSampleView.ShowCategory(firstCategory);

                GameManager.instance.ShowDubug("Initialization and sign in complete.");

                EnablePurchases();
            }
            catch (Exception e)
            {
                GameManager.instance.ShowDubug("K");
                Debug.LogException(e);
            }
            GameManager.instance.ShowDubug("L");
        }

        void EnablePurchases()
        {
            GameManager.instance.ShowDubug("M");
            virtualShopSampleView.SetInteractable();
            GameManager.instance.ShowDubug("N");
        }

        public void OnCategoryButtonClicked(string categoryId)
        {
            var virtualShopCategory = VirtualShopManager.instance.virtualShopCategories[categoryId];
            virtualShopSampleView.ShowCategory(virtualShopCategory);
        }

        public async Task OnPurchaseClicked(VirtualShopItem virtualShopItem, Button button)
        {
            try
            {
                var result = await EconomyManager.instance.MakeVirtualPurchaseAsync(virtualShopItem.id);

                

                GameManager gm = GameManager.instance;

                string rewardId = virtualShopItem.rewards[0].id;
                string costId = virtualShopItem.costs[0].id;

                switch (rewardId)
                {
                    case "COIN":
                        await gm.UpdateTokensAndCoins(gm.username, virtualShopItem.rewards[0].amount, -virtualShopItem.costs[0].amount);
                        break;
                    case "TARALITY_TOKEN":
                        await gm.UpdateTokensAndCoins(gm.username, -virtualShopItem.costs[0].amount, virtualShopItem.rewards[0].amount);
                        break;
                    default:
                        switch (costId)
                        {
                            case "COIN":
                                await gm.UpdateTokensAndCoins(gm.username, -virtualShopItem.costs[0].amount, 0);
                                break;
                            case "TARALITY_TOKEN":
                                await gm.UpdateTokensAndCoins(gm.username, 0, -virtualShopItem.costs[0].amount);
                                break;
                        }
                        break;
                }


                await gm.UpdateCurrencies();

                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
                if (this == null) return;


                GameManager.instance.ShowDubug("This is purchase result " + result.Rewards);

                ShowRewardPopup(result.Rewards, virtualShopItem, button);
            }
            catch (EconomyException e)
                when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
            {
                virtualShopSampleView.ShowVirtualPurchaseFailedErrorPopup();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void OnGainCurrencyDebugButtonClicked()
        {
            try
            {
                await EconomyManager.instance.GrantDebugCurrency("TARALITY_TOKEN", 100);
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void OnUseCurrencyDebugButtonClicked(int value)
        {
            try
            {
                await EconomyManager.instance.GrantDebugCurrency("TARALITY_TOKEN", -value);
                if (this == null) return;

                await EconomyManager.instance.RefreshCurrencyBalances();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public async void SaveEarnedCurrency(Currency[] currencies)
        {
            foreach (Currency currency in currencies)
            {
                try
                {
                    await EconomyManager.instance.GrantDebugCurrency(currency.id, currency.amount);
                    if (this == null) return;

                    await EconomyManager.instance.RefreshCurrencyBalances();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        void ShowRewardPopup(Rewards rewards, VirtualShopItem virtualShopItem, Button button)
        {
            var addressablesManager = AddressablesManager.instance;

            var rewardDetails = new List<RewardDetail>();
            foreach (var inventoryReward in rewards.Inventory)
            {
                rewardDetails.Add(new RewardDetail
                {
                    id = inventoryReward.Id,
                    quantity = inventoryReward.Amount,
                    sprite = VirtualShopManager.instance.GetSpriteByID(inventoryReward.Id)
                });

                GameManager.instance.ShowDubug("The Name Is Andi mandi " + inventoryReward.Id + " & " + virtualShopItem.id);
                SaveItemData(virtualShopItem.id, 1);
                CloudSaveSystem.Interaction(button, false);
            }

            foreach (var currencyReward in rewards.Currency)
            {
                GameManager.instance.ShowDubug("The Name Is Andi mandi " + currencyReward.Id + " & " + virtualShopItem.id);
                rewardDetails.Add(new RewardDetail
                {
                    id = currencyReward.Id,
                    quantity = currencyReward.Amount,
                    sprite = VirtualShopManager.instance.GetSpriteByID(currencyReward.Id)
                });
            }

            virtualShopSampleView.ShowRewardPopup(rewardDetails);
        }

        public async void ReviveWithToken()
        {
            try
            {
                await OnReviveWithTokenClicked("REVIVE_01");
                //LevelManager.instance.OnRevive();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }



        public async Task OnReviveWithTokenClicked(string purchaseID)
        {
            try
            {
                var result = await EconomyManager.instance.MakeVirtualPurchaseAsync(purchaseID);
                if (this == null) return;

                GameManager.instance.ShowDubug("This is this " + this);
                GameManager.instance.ShowDubug("This is result " + this);

                UIScript uiScript = UIScript.Instance;

                if (uiScript.currentPurchaseIndex < uiScript.revivalCosts.Length)
                {
                    uiScript.currentPurchaseIndex += 1;
                }

                LevelManager.instance.OnRevive();

                await EconomyManager.instance.RefreshCurrencyBalances();
                if (this == null) return;

                LevelManager.instance.OnRevive();

                //ShowRewardPopup(result.Rewards, virtualShopItem, button);
            }
            catch (EconomyException e)
                when (e.ErrorCode == k_EconomyPurchaseCostsNotMetStatusCode)
            {
                GameManager.instance.ShowDubug("This is EconomyException " + e);
                //virtualShopSampleView.ShowVirtualPurchaseFailedErrorPopup();
                UIScript.Instance.ShowWarningPop(true);
            }
            catch (Exception e)
            {

                GameManager.instance.ShowDubug("This is EconomyException else " + e);
                Debug.LogException(e);
            }
        }

        async void SaveItemData(string id, int value)
        {
            await CloudSaveSystem.SaveDataAsync(id, 1);
        }
    }
}

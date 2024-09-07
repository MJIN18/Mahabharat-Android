using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unity.Services.Samples.VirtualShop
{
    public class InventoryPopupView : MonoBehaviour
    {
        public async Task Show()
        {
            gameObject.SetActive(true);

            await EconomyManager.instance.RefreshInventory();
        }

        public void OnCloseButtonPressed()
        {
            UIController.instance.ButtonSound();

            Close();
        }

        void Close()
        {
            gameObject.SetActive(false);
        }
    }
}

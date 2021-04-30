using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

[RequireComponent(typeof(AdsManager))]
public class RemoveAds : MonoBehaviour
{
    private SkinsManager sManager;
    private AdsManager ads;
    [Header("Remove Ads Shop:")]
    public GameObject removeAds;
    public Button iapNoAdsBtn;

    private void Awake()
    {
        DisableRestorePurchaseButton();
        ads = GetComponent<AdsManager>();
        sManager = GetComponentInChildren<SkinsManager>();
        sManager.rAds = this;
    }
    public void OpenCloseShop()
    {
        iapNoAdsBtn.interactable = !ads.noAds;
        removeAds.SetActive(!removeAds.activeSelf);

        EditorManager.instance.ExitEM();

        if (sManager.skinsLayout.activeSelf)
            sManager.skinsLayout.SetActive(false);
    }


    #region IAP Methods:
    private string noAds = "com.bestgames.partyslide.noads";
    public GameObject restorePurchaseBtn;
    //If purchase went through >:)
    public void OnPurchaseComplete(Product product)
    {
        if(product.definition.id == noAds)
        {
            ads.noAds = true;
            iapNoAdsBtn.interactable = !ads.noAds;
            //Save noAds in SaveData!!!
            SaveData.instance.noAds = ads.noAds;
            SaveData.instance.SaveGame();
        }
    }

    //For all the wrong reasons >:C
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log($"Purchase of {product.definition.id} failed for a reason: {reason}");
    }

    public void DisableRestorePurchaseButton()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            restorePurchaseBtn.SetActive(false);
    }
    #endregion
}

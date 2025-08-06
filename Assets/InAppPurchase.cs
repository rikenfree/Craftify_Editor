using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppPurchase : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    private static string noadsPurchase = "com.craftify.editor.formcpe.noads";

    private void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (!IsInitialized())
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(noadsPurchase, ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
        }
    }
    private bool IsInitialized()
    {
        return storeController != null && extensionProvider != null;
    }
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        storeController = controller;
        extensionProvider = extensions;

        Debug.Log("Available items:");
        foreach (var item in storeController.products.all)
        {
            Debug.Log(item.receipt);
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                    item.metadata.localizedTitle,
                    item.metadata.localizedDescription,
                    item.metadata.isoCurrencyCode,
                    item.metadata.localizedPrice.ToString(),
                    item.metadata.localizedPriceString,
                    item.transactionID,
                    item.receipt
                    }));
            }
            else
            {
                Debug.Log("Item Not Purchase");
            }
        }

    }

    public void LimitedNoAds()
    {
        BuyProductID(noadsPurchase);
    }

    private void BuyProductID(string productId)
    {
        if (!IsInitialized())
        {
            Debug.LogError("Purchasing not initialized.");
            return;
        }

        Product product = storeController.products.WithID(productId);
        if (product != null && product.availableToPurchase)
        {
            Debug.Log($"Purchasing product: {product.definition.id}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogError("Product not found or not available for purchase.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");
            IAppleExtensions extension = extensionProvider.GetExtension<IAppleExtensions>();
            extension.RestoreTransactions(delegate (bool result)
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("Failed to initialize purchasing: " + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError("Purchase of " + product.definition.id + " failed: " + failureReason);
    }

    public void OnPurchaseComplete(Product product)
    {
        Debug.Log("Purchase of " + product.definition.id + " successful.");
        // Grant purchased items to the player and consume them if needed
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, noadsPurchase, StringComparison.Ordinal))
        {
            Debug.Log("Remove Ads Purchase Successful");

            // Save "no ads" preference
            PlayerPrefs.SetInt("NoAds", 1);
            PlayerPrefs.Save();

            // Immediately remove any active banner ads
            if (SuperStarSdk.SuperStarAd.Instance != null)
            {
                SuperStarSdk.SuperStarAd.Instance.HideBannerAd();
            }

            // Optional: If you want to reload ad settings without restarting
            if (SuperStarSdk.SuperStarAd.Instance != null)
            {
                SuperStarSdk.SuperStarAd.Instance.Setup();
            }
        }


        return PurchaseProcessingResult.Complete;
    }
}

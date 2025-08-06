using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class InAppPurchase : MonoBehaviour, IStoreListener
{
    public static InAppPurchase Instance;

    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    private static string noadsPurchase = "com.noads.subscription";

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            await InitializeUnityGamingServices();

            if (storeController == null)
                InitializePurchasing();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task InitializeUnityGamingServices()
    {
        try
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            Debug.Log("Unity Gaming Services Initialized Successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to initialize Unity Gaming Services: " + e.Message);
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
            return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(noadsPurchase, ProductType.Subscription);

        Debug.Log("Initializing Unity IAP...");
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return storeController != null && storeExtensionProvider != null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("IAP Initialization Successful.");
        storeController = controller;
        storeExtensionProvider = extensions;

        if (IsSubscribed())
            GrantNoAds();
        else
            RevokeNoAds();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("IAP Initialization Failed: " + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP Initialization Failed: {error} - {message}");
    }

    // Call this from button OnClick
    public void LimitedNoAds()
    {
        BuyProductID(noadsPurchase);
    }

    private void BuyProductID(string productId)
    {
        if (!IsInitialized())
        {
            Debug.LogError("Purchasing not initialized. Retrying...");
            InitializePurchasing();
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

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        if (string.Equals(purchaseEvent.purchasedProduct.definition.id, noadsPurchase, StringComparison.Ordinal))
        {
            Debug.Log("Monthly No Ads Subscription Purchased");
            GrantNoAds();
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase of {product.definition.id} failed: {failureReason}");
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.LogError("RestorePurchases failed. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started...");
            var apple = storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions(result =>
            {
                Debug.Log("RestorePurchases result: " + result);
                if (result && IsSubscribed())
                    GrantNoAds();
                else
                    RevokeNoAds();
            });
        }
        else
        {
            Debug.Log("RestorePurchases not supported on this platform.");
        }
    }

    public bool IsSubscribed()
    {
        if (!IsInitialized())
            return PlayerPrefs.GetInt("NoAds", 0) == 1;

        Product product = storeController.products.WithID(noadsPurchase);

        if (product != null && product.hasReceipt)
        {
            Debug.Log("Valid subscription receipt found.");
            return true;
        }

        return false;
    }

    private void GrantNoAds()
    {
        PlayerPrefs.SetInt("NoAds", 1);
        PlayerPrefs.Save();
        Debug.Log("No Ads Granted");

        if (SuperStarSdk.SuperStarAd.Instance != null)
        {
            SuperStarSdk.SuperStarAd.Instance.HideBannerAd();
            SuperStarSdk.SuperStarAd.Instance.Setup();
        }
    }

    private void RevokeNoAds()
    {
        PlayerPrefs.SetInt("NoAds", 0);
        PlayerPrefs.Save();
        Debug.Log("No Ads Revoked");

        if (SuperStarSdk.SuperStarAd.Instance != null)
        {
            SuperStarSdk.SuperStarAd.Instance.ShowBannerAd();
        }
    }
}

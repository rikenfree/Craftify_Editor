using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using SuperStarSdk;

public class IAPManager : MonoBehaviour, IStoreListener
{
    private static IAPManager instance = null;

  

    public static IAPManager Instance
    {
        get { return instance; }
    }


   

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

   
    public string NonConsumableNoAds;

  
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
       
        // If we haven't set up the Unity Purchasing reference
        if (m_StoreController == null)
        {
            // Begin to configure our connection to Purchasing
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        // If we have already connected to Purchasing ...
        if (IsInitialized())
        {
            // ... we are done here.
            return;
        }

        CreateBuilder();
    }

    public void CreateBuilder()
    {
        // Create a builder, first passing in a suite of Unity provided stores.
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        // Continue adding the non-consumable product.
      
        builder.AddProduct(NonConsumableNoAds, ProductType.NonConsumable);



        //Debug.Log("kProductIDNonConsumable: " + kProductIDNonConsumable);
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    public void BuyNonConsumable()
    {
        if (!IsInitialized())
        {
            Debug.Log("Not Initialized.CreateBuilder");
            CreateBuilder();
        }
      
        BuyProductID(NonConsumableNoAds);
    }

   


    void BuyProductID(string productId)
    {
        // If Purchasing has been initialized ...
        //Debug.Log("productId" + productId);
        if (IsInitialized())
        {
            // ... look up the Product reference with the general product identifier and the Purchasing 
            // system's products collection.
            Product product = m_StoreController.products.WithID(productId);

            // If the look up found a product for this device's store and that product is ready to be sold ... 
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                // asynchronously.
                m_StoreController.InitiatePurchase(product);
            }
            // Otherwise ...
            else
            {
                // ... report the product look-up failure situation  
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                //#if UNITY_IOS
                //                GameController.gcInstance.IAPPreloaderScreen.SetActive(false);
                //#endif
            }
        }
        // Otherwise ...
        else
        {
            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
            // retrying initiailization.
            Debug.Log("BuyProductID FAIL. Not initialized.");
            //#if UNITY_IOS
            //            GameController.gcInstance.IAPPreloaderScreen.SetActive(false);
            //#endif
        }
    }


    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
    public void RestorePurchases()
    {
        // If Purchasing has not yet been set up ...
        if (!IsInitialized())
        {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            // InitializePurchasing();
            return;
        }

        // If we are running on an Apple device ... 
        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");
            //GameController.gcInstance.IAPPreloaderScreen.SetActive(true);
            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) =>
            {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });

        }
        // Otherwise ...
        else
        {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
            //#if UNITY_IOS
            //            GameController.gcInstance.IAPPreloaderScreen.SetActive(false);
            //#endif
        }
    }


    //  
    // --- IStoreListener
    //

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

        Product product = controller.products.WithID(NonConsumableNoAds);
        if (product != null && product.hasReceipt)
        {
            // Owned Non Consumables and Subscriptions should always have receipts.
            // So here the Non Consumable product has already been bought.
            //itemBought = true;
            Debug.LogError("Item Is Already Boughted with id:" + NonConsumableNoAds);
            /*if (SuperStarAd.Instance.NOADS ==1)
            {
               
                Debug.LogError("Already purchased no ads");
            }
            else
            {
                SuperStarAd.Instance.HideBannerAd();
                SuperStarAd.Instance.NOADS = 1;
            }*/
        }
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        //#if UNITY_IOS
        //        GameController.gcInstance.IAPPreloaderScreen.SetActive(false);
        //#endif
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //false
        //GameController.gcInstance.IAPPreloaderScreen.SetActive(false);
        Debug.Log("PurchaseProcessingResult Called");
        // Or ... a non-consumable product has been purchased by this user.
        if (String.Equals(args.purchasedProduct.definition.id, NonConsumableNoAds, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));


            /*if (SuperStarAd.Instance.NOADS == 1)
            {

                Debug.LogError("Already purchased no ads");
                //  AdmobManager.Instance.DestrotyBannerAd();
            }
            else
            {
                SuperStarAd.Instance.HideBannerAd();
                SuperStarAd.Instance.NOADS = 1;
            }*/
            // TODO: The non-consumable item has been successfully purchased, grant this item to the player.


        }
      
        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            //StoreManager.instance.OpenResultScreen("Ooops!!Purchase failed...");
        }

        // Return a flag indicating whether this product has completely been received, or if the application needs 
        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
        // saving purchased products to the cloud, and when that save is delayed. 
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //false
        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
        // this reason with the user to guide their troubleshooting actions.
        //GameController.gcInstance.IAPPreloaderScreen.SetActive(false);
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        //StoreManager.instance.OpenResultScreen("Ooops!!Purchase failed...");
    }

    public string GetPrice(string productID)
    {
        if (m_StoreController == null) InitializePurchasing();
        return m_StoreController.products.WithID(productID).metadata.localizedPriceString;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
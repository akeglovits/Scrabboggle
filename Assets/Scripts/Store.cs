using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.UI;

public class Store : MonoBehaviour, IStoreListener{

    private IStoreController controller;
    private IExtensionProvider extensions;

    private IAppleExtensions appleExtension;

    private int coins;
    private string type;
    private string subtype;

    private string productId;

    void Start(){
        type = "";

        #if UNITY_IOS
            GameObject.Find("Store-Restore-Purchases").GetComponent<Image>().enabled = true;
        #endif
        if(controller != null && extensions != null){
            return;
        }else{
            InitializeStore();
        }

    }


    public void InitializeStore () {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct("500_coins_0", ProductType.Consumable);
        builder.AddProduct("1300_coins_0", ProductType.Consumable);
        builder.AddProduct("2800_coins_0", ProductType.Consumable);
        builder.AddProduct("6000_coins_0", ProductType.Consumable);
        builder.AddProduct("8500_coins_0", ProductType.Consumable);
        builder.AddProduct("20000_coins_0", ProductType.Consumable);
        builder.AddProduct("500_coins_1", ProductType.Consumable);
        builder.AddProduct("1300_coins_1", ProductType.Consumable);
        builder.AddProduct("2800_coins_1", ProductType.Consumable);
        builder.AddProduct("6000_coins_1", ProductType.Consumable);
        builder.AddProduct("8500_coins_1", ProductType.Consumable);
        builder.AddProduct("20000_coins_1", ProductType.Consumable);
        builder.AddProduct("vip_month_0", ProductType.Subscription);
        builder.AddProduct("ad_month_0", ProductType.Subscription);
        builder.AddProduct("lives_month_0", ProductType.Subscription); 

        UnityPurchasing.Initialize (this, builder);
    }

    public void restorePurchasesButton(){

        appleExtension.RestoreTransactions (result => {
        if (result) {
            Debug.Log("Transactions Restored!");
        } else {
            Debug.Log("Restoration Failed!");
        }
    });
    }


    public void purchaseCoinsButton(int productCoins) {

     string productId = productCoins + "_coins_"+PlayerPrefs.GetInt("VIP",0);
    controller.InitiatePurchase(productId);

    type = "coins";
    coins = productCoins;

    }

    public void purchaseSubButton(string subscriptiontype) {

     string productId = subscriptiontype + "_month_"+PlayerPrefs.GetInt("VIP",0);
    controller.InitiatePurchase(productId);

    type = "subscription";
    subtype = subscriptiontype;

    }


    // Called when Unity IAP is ready to make purchases.
    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
        this.controller = controller;
        this.extensions = extensions;

        appleExtension = extensions.GetExtension<IAppleExtensions>();

        foreach(var item in controller.products.all){

            if(item.definition.type == ProductType.Subscription){
            if(item.receipt != null){
                
                SubscriptionManager subscriptionManager = new SubscriptionManager(item, null);
                var isSubscribed = subscriptionManager.getSubscriptionInfo().isSubscribed();
                var product = subscriptionManager.getSubscriptionInfo().getProductId();

                Debug.Log(product + ":" + isSubscribed);

                if(isSubscribed == Result.True){
                    if(product == "vip_month_0"){
                        
                        PlayerPrefs.SetInt("VIP", 1);
                PlayerPrefs.SetInt("VIPSUB", 1);

                StoreCoins.VIPPricing();

                    }else if(product == "ad_month_0"){

                        PlayerPrefs.SetInt("ADS", 1);
                PlayerPrefs.SetInt("ADSSUB", 1);

                    }else if(product == "lives_month_0"){

                        PlayerPrefs.SetInt("Lives", 5);
                PlayerPrefs.SetInt("UNLIMITEDLIVES", 1);
                PlayerPrefs.SetInt("LIVESSUB", 1);

                    }
                }else{

                    if(product == "vip_month_0"){
                        
                PlayerPrefs.SetInt("VIPSUB", 0);
                    if(PlayerPrefs.GetInt("VipCoin", 0) == 0){
                        PlayerPrefs.SetInt("VIP", 0);
                    }

                    }else if(product == "ad_month_0"){

                PlayerPrefs.SetInt("ADSSUB", 0);

                        if(PlayerPrefs.GetInt("AdsCoin", 0) == 0){
                            PlayerPrefs.SetInt("ADS", 0);
                        }

                    }else if(product == "lives_month_0"){

                        PlayerPrefs.SetInt("LIVESSUB", 0);

                        if(PlayerPrefs.GetInt("LivesCoin", 0) == 0){
                            PlayerPrefs.SetInt("UNLIMITEDLIVES", 0);
                        }

                    }

                }

            }else{

                string id = item.definition.id;
                if(id == "vip_month_0"){
                        
                PlayerPrefs.SetInt("VIPSUB", 0);
                    if(PlayerPrefs.GetInt("VipCoin", 0) == 0){
                        PlayerPrefs.SetInt("VIP", 0);
                    }

                    }else if(id == "ad_month_0"){

                PlayerPrefs.SetInt("ADSSUB", 0);

                        if(PlayerPrefs.GetInt("AdsCoin", 0) == 0){
                            PlayerPrefs.SetInt("ADS", 0);
                        }

                    }else if(id == "lives_month_0"){

                        PlayerPrefs.SetInt("LIVESSUB", 0);

                        if(PlayerPrefs.GetInt("LivesCoin", 0) == 0){
                            PlayerPrefs.SetInt("UNLIMITEDLIVES", 0);
                        }

                    }

                
                }
            }
        }

        DataControl.DC.setAccountData();
    }

    
    /// Called when Unity IAP encounters an unrecoverable initialization error.
    /// Note that this will not be called if Internet is unavailable; Unity IAP
    /// will attempt initialization until it becomes available.
    
    public void OnInitializeFailed (InitializationFailureReason error)
    {
        Debug.Log("Initialization Failed");
    }


    /// Called when a purchase completes.
    ///
    /// May be called at any time after OnInitialized().

    public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
    {

        bool validPurchase = true; // Presume valid for platforms with no R.V.

    // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
    // Prepare the validator with the secrets we prepared in the Editor
    // obfuscation window.
    var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
        AppleTangle.Data(), Application.identifier);

    try {
        // On Google Play, result has a single product ID.
        // On Apple stores, receipts contain multiple products.

        var result = validator.Validate(e.purchasedProduct.receipt);
        // For informational purposes, we list the receipt(s)
        Debug.Log("Receipt is valid. Contents:");
        foreach (IPurchaseReceipt productReceipt in result) {
            Debug.Log(productReceipt.productID);
            Debug.Log(productReceipt.purchaseDate);
            Debug.Log(productReceipt.transactionID);

            productId = productReceipt.productID;

            GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
    if (null != google) {
        // This is Google's Order ID.
        // Note that it is null when testing in the sandbox
        // because Google's sandbox does not provide Order IDs.
        Debug.Log(google.orderID);
        Debug.Log(google.purchaseState);
        Debug.Log(google.purchaseToken);
    }

    AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
    if (null != apple) {
        Debug.Log(apple.originalTransactionIdentifier);
        Debug.Log(apple.subscriptionExpirationDate);
        Debug.Log(apple.cancellationDate);
        Debug.Log(apple.quantity);
    }
        }
    } catch (IAPSecurityException) {
        Debug.Log("Invalid receipt, not unlocking content");
        validPurchase = false;
    }
#endif

    if (validPurchase) {
        
        if(type == "coins"){

        Coins.C.addCoins(coins);

        }else{

            if(subtype == "vip" || productId == "vip_month_0"){
                PlayerPrefs.SetInt("VIP", 1);
                PlayerPrefs.SetInt("VIPSUB", 1);

                GameObject.Find("Lives-Time").GetComponent<Text>().enabled = false;

                if(GameObject.Find("Lives-Time-2") != null){
                    GameObject.Find("Lives-Time-2").GetComponent<Text>().enabled = false;
                }

                StoreCoins.VIPPricing();
            }else if(subtype == "ad" || productId == "ad_month_0"){
                PlayerPrefs.SetInt("ADS", 1);
                PlayerPrefs.SetInt("ADSSUB", 1);
            }else if(subtype == "lives" || productId == "lives_month_0"){
                PlayerPrefs.SetInt("Lives", 5);
                PlayerPrefs.SetInt("UNLIMITEDLIVES", 1);
                PlayerPrefs.SetInt("LIVESSUB", 1);

                GameObject.Find("Lives-Time").GetComponent<Text>().enabled = false;

                if(GameObject.Find("Lives-Time-2") != null){
                    GameObject.Find("Lives-Time-2").GetComponent<Text>().enabled = false;
                }
            }
        }
    }

        DataControl.DC.setAccountData();

        return PurchaseProcessingResult.Complete;
    }

    /// <summary>
    /// Called when a purchase fails.
    /// </summary>
    public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
    {
        Debug.Log("Purchased Failed");
    }
}
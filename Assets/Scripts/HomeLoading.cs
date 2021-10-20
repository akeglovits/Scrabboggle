using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using AppodealAppTracking.Unity.Api;
using AppodealAppTracking.Unity.Common;
using ConsentManager.Api;
using ConsentManager.Common;
using Facebook.Unity;

public class HomeLoading : MonoBehaviour, IConsentFormListener, IConsentInfoUpdateListener, IAppodealAppTrackingTransparencyListener{
    // Start is called before the first frame update

    public static HomeLoading HL;
    private bool didSignIn;

    private string appkey;

    private bool titleFinished;

    private Consent appodealConsent;
    void Start()
    {

        #if UNITY_IOS
            appkey = "455f12f5b1ddd69295b908ff4553275b49fc10017c25a6bd";
        #elif UNITY_ANDROID
            appkey = "eae837acf5e98617b324a4aeb2b9004f5be2841c16f15475";
        #else
            appkey = "unrecognized_platform";
        #endif

        HL = this;

        titleFinished = false;

        if (!FB.IsInitialized) {
            FB.Init(initCallback, onHideUnity);
        } else {
            // Already initialized
            FB.ActivateApp();
        }

        ConsentManager.Api.ConsentManager consentManager = ConsentManager.Api.ConsentManager.getInstance();

        consentManager.requestConsentInfoUpdate(appkey, this);

        // Get current ShouldShow status
        Consent.ShouldShow consentShouldShow = consentManager.shouldShowConsentDialog();

        if (consentShouldShow == Consent.ShouldShow.TRUE){

            StartCoroutine(showConsentForm());

        }else{

            appodealConsent = consentManager.getConsent();

            AppodealAppTrackingTransparency.RequestTrackingAuthorization(this);

            Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, consentManager.getConsent());

        StartCoroutine(loadHomeScreen());

        }

        

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
  var dependencyStatus = task.Result;
  if (dependencyStatus == Firebase.DependencyStatus.Available) {
    // Create and hold a reference to your FirebaseApp,
    // where app is a Firebase.FirebaseApp property of your application class.
       Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

    didSignIn = true;
    

  } else {
    UnityEngine.Debug.LogError(System.String.Format(
      "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
    // Firebase Unity SDK is not safe to use here.
  }
});
    Debug.Log("Made It");
    StartCoroutine(tileShow());
    }


    private void initCallback (){
        if (FB.IsInitialized) {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        } else {
            Debug.Log("Something went wrong to Initialize the Facebook SDK");
        }
    }


    void OnApplicationPause (bool pauseStatus){
    // Check the pauseStatus to see if we are in the foreground
    // or background
    if (!pauseStatus) {
        //app resume
        if (FB.IsInitialized) {
            FB.ActivateApp();
        } else {
            //Handle FB.Init
            FB.Init( () => {
            FB.ActivateApp();
        });
        }
    }
    }

    private void onHideUnity(bool isGameShown){
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
        Time.timeScale = 1;
        }
    }


    public IEnumerator showConsentForm(){

    ConsentForm consentForm = new ConsentForm.Builder().withListener(this).build();
    consentForm?.load();

    while(!consentForm.isLoaded()){
        yield return new WaitForSeconds(.01f);
    }

        consentForm.showAsDialog();
    }

    #region ConsentInfoUpdateListener

public void onConsentInfoUpdated(Consent consent) { 
    print("onConsentInfoUpdated");
    appodealConsent = consent;
    }

public void onFailedToUpdateConsentInfo(ConsentManagerException error) { print($"onFailedToUpdateConsentInfo Reason: {error.getReason()}");}

#endregion

    #region ConsentFormListener

public void onConsentFormLoaded() { print("ConsentFormListener - onConsentFormLoaded");}

public void onConsentFormError(ConsentManagerException exception) { print($"ConsentFormListener - onConsentFormError, reason - {exception.getReason()}");}

public void onConsentFormOpened() { print("ConsentFormListener - onConsentFormOpened");}

public void onConsentFormClosed(Consent consent) { 

    appodealConsent = consent;

        AppodealAppTrackingTransparency.RequestTrackingAuthorization(this);

        Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, consent);

        StartCoroutine(loadHomeScreen());
        
    }

#endregion


public void AppodealAppTrackingTransparencyListenerNotDetermined(){ 
    Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, appodealConsent);
    StartCoroutine(loadHomeScreen());

    }
public void AppodealAppTrackingTransparencyListenerRestricted(){ 
   Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, appodealConsent);
   StartCoroutine(loadHomeScreen());

    }
public void AppodealAppTrackingTransparencyListenerDenied() { 
    Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, appodealConsent);
    StartCoroutine(loadHomeScreen());

}
public void AppodealAppTrackingTransparencyListenerAuthorized() { 
    Appodeal.initialize(appkey, Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO, appodealConsent);
    StartCoroutine(loadHomeScreen());

}

public IEnumerator tileShow(){

    for(int i = 0; i < 11; i++){
        GameObject currentTile = GameObject.Find("Title").transform.GetChild(i).gameObject;

        StartCoroutine(tileIndividual(currentTile));

        yield return new WaitForSeconds(.2f);
    }

    yield return new WaitForSeconds(.5f);

    titleFinished = true;
}

public IEnumerator tileIndividual(GameObject currentTile){

    while(currentTile.transform.localScale.y < .7f){
            currentTile.transform.localScale += new Vector3(.1f, .1f, .1f);
            yield return new WaitForSeconds(.1f);
        }

       currentTile.transform.localScale = new Vector3(.7f,.7f,.7f); 
    
    while(currentTile.transform.localScale.y > .5f){
            currentTile.transform.localScale -= new Vector3(.1f, .1f, .1f);
            yield return new WaitForSeconds(.1f);
        }

    currentTile.transform.localScale = new Vector3(.5f,.5f,.5f);
}

public IEnumerator loadHomeScreen(){

    while(!didSignIn || !titleFinished){
        yield return new WaitForSeconds(.01f);
    }
    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SignIn");

        while (!asyncLoad.isDone)
        {

            for(int i = 0; i < 11; i++){
                GameObject currentTile = GameObject.Find("Title").transform.GetChild(i).gameObject;

                StartCoroutine(tileIndividual(currentTile));

                yield return new WaitForSeconds(.2f);
            }
        }
}

}

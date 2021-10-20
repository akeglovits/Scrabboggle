using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class GameEndAd : MonoBehaviour, IInterstitialAdListener
{

    public static GameEndAd GEA;

    private bool adFinished;

    public static bool scoreUpdated;


    // Start is called before the first frame update
    void Start()
    {
        GEA = this;

        adFinished = false;
        scoreUpdated = false;

        Appodeal.setInterstitialCallbacks(this);
    }


#region Interstitial callback handlers
public void onInterstitialLoaded(bool isPrecache) { print("Interstitial loaded"); } // Called when interstitial was loaded (precache flag shows if the loaded ad is precache)
public void onInterstitialFailedToLoad() { print("Interstitial failed"); } // Called when interstitial failed to load
public void onInterstitialShowFailed() { 
    if(PlayerPrefs.GetInt("Mute", 0) == 0){
    GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = false;
    } 

    if(SceneManager.GetActiveScene().name == "MultiPlay"){
        adFinished = true;
        StartCoroutine(goHome());
    }

    } // Called when interstitial was loaded, but cannot be shown (internal network errors, placement settings, or incorrect creative)
public void onInterstitialShown() { print("Interstitial opened"); } // Called when interstitial is shown
public void onInterstitialClosed() { 
    if(PlayerPrefs.GetInt("Mute", 0) == 0){
        GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = false;
    }

    if(SceneManager.GetActiveScene().name == "MultiPlay"){
        adFinished = true;
        StartCoroutine(goHome());
    }

 } // Called when interstitial is closed
public void onInterstitialClicked() { print("Interstitial clicked"); } // Called when interstitial is clicked
public void onInterstitialExpired() { print("Interstitial expired"); } // Called when interstitial is expired and can not be shown
#endregion


public void showGameEndAd(){

    
        if (Appodeal.isLoaded(Appodeal.INTERSTITIAL) && PlayerPrefs.GetInt("ADS", 0) == 0 && PlayerPrefs.GetInt("VIP", 0) == 0)
        {
            GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = true;
            Appodeal.show(Appodeal.INTERSTITIAL);
        }else{

            if(SceneManager.GetActiveScene().name == "MultiPlay"){
                adFinished = true;
                StartCoroutine(goHome());
            }
        }
}

public IEnumerator goHome(){

    GameObject.Find("Loading-Panel").transform.SetAsLastSibling();

    while(!adFinished || !scoreUpdated){

        for(int i = 1; i <= 8; i++){
				GameObject.Find("Loading-Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/loading-circle-"+i);

				yield return new WaitForSeconds(.2f);
			}        
    }

    PlayerPrefs.SetInt("OpenGame" , 1);
    PlayerPrefs.SetString("HomePageTab", "Multi-Play");
    
    
    PlayerPrefs.SetString("NextScene", "Home");

    SceneManager.LoadScene("LoadingScreen");
}
}
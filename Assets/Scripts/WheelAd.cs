using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;

public class WheelAd : MonoBehaviour, IRewardedVideoAdListener
{

    // Start is called before the first frame update
    void Start()
    {
        Appodeal.setRewardedVideoCallbacks(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region Rewarded Video callback handlers 
public void onRewardedVideoLoaded(bool isPrecache) { print("Video loaded"); } //Called when rewarded video was loaded (precache flag shows if the loaded ad is precache). 
public void onRewardedVideoFailedToLoad() { print("Video failed"); } // Called when rewarded video failed to load 
public void onRewardedVideoShowFailed() { 
    
    if(PlayerPrefs.GetInt("Mute", 0) == 0){
    GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = false;
    } 
 } // Called when rewarded video was loaded, but cannot be shown (internal network errors, placement settings, or incorrect creative) 
public void onRewardedVideoShown() { print("Video shown"); } // Called when rewarded video is shown 
public void onRewardedVideoClicked() { print("Video clicked"); } // Called when reward video is clicked 
public void onRewardedVideoClosed(bool finished) { 
    
    if(PlayerPrefs.GetInt("Mute", 0) == 0){
    GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = false;
    }

    if(finished){
        WheelFunctions.WF.spinWheel();
    }
 } // Called when rewarded video is closed 
public void onRewardedVideoFinished(double amount, string name) { 
    float rotation = Random.Range(0f,360f);
    GameObject.Find("Wheel").transform.Rotate(0f,0f,rotation,Space.Self);

    GameObject.Find("Wheel-Button").GetComponent<Button>().interactable = false;
 } // Called when rewarded video is viewed until the end 
public void onRewardedVideoExpired() { print("Video expired"); } //Called when rewarded video is expired and can not be shown 
#endregion

    public void showAd(){

        

        if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO) && PlayerPrefs.GetInt("VIP", 0) == 0){
            GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = true;
            Appodeal.show(Appodeal.REWARDED_VIDEO);
        }else{
            float rotation = Random.Range(0f,360f);
            GameObject.Find("Wheel").transform.Rotate(0f,0f,rotation,Space.Self);
            GameObject.Find("Wheel-Button").GetComponent<Button>().interactable = false;
            WheelFunctions.WF.spinWheel();
        }
    }
}

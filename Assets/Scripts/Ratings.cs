using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ratings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DateTime currentTime = System.DateTime.Now;
        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("LastTimePlayed",System.DateTime.Now.AddDays(-2).ToBinary().ToString()));
        DateTime lastDate = System.DateTime.FromBinary(tempTime);
        TimeSpan timeDiff = currentTime.Subtract(lastDate);

        if(currentTime.DayOfYear == lastDate.DayOfYear && timeDiff.TotalDays < 2){
           
        }else if(((currentTime.DayOfYear == 1 && (lastDate.DayOfYear == 365 || lastDate.DayOfYear == 366)) || currentTime.DayOfYear == lastDate.DayOfYear + 1) && timeDiff.TotalDays < 2){
            PlayerPrefs.SetInt("ConsecutiveDays",PlayerPrefs.GetInt("ConsecutiveDays",0) + 1);
            PlayerPrefs.SetInt("FirstLogIn", 1);
        }else{
            PlayerPrefs.SetInt("ConsecutiveDays",1);
        }

        if(PlayerPrefs.GetInt("ConsecutiveDays", 0) % 3 == 0 && currentTime.DayOfYear > lastDate.DayOfYear){
            openRating();
        }

        PlayerPrefs.SetString("LastTimePlayed",System.DateTime.Now.ToBinary().ToString());

    }

    public void gotoAppStore(){
        PlayerPrefs.SetInt("LeftRating", 1);

        #if UNITY_ANDROID
        Application.OpenURL("market://details?id=com.BAMfam.Scrabboggle");
        #elif UNITY_IOS
        Application.OpenURL("itms-apps://itunes.apple.com/app/id1549248401");
        #endif

        closeRating();
    }

    public static void openRating(){
        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Rating-Panel").transform.SetAsLastSibling();

        PlayerPrefs.SetInt("FirstLogIn", 0);
    }

    public void closeRating(){
        GameObject.Find("Rating-Panel").transform.SetAsFirstSibling();
        GameObject.Find("Block-Background").transform.SetAsFirstSibling();
        
    }

    public void dontAskAgain(){
        PlayerPrefs.SetInt("LeftRating", 1);
        closeRating();
    }
}

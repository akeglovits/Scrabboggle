using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreCoins : MonoBehaviour
{
    // Start is called before the first frame update

    private string purchaseType;
    private int purchaseSubtype;

    private int coinCost;
    private int hourTime;
    void Start()
    {
        if(PlayerPrefs.GetInt("UNLIMITEDLIVES", 0) == 1 && PlayerPrefs.GetInt("LIVESSUB", 0) == 0){
            StartCoroutine(unlimitedLifeTimer());
        }

        if(PlayerPrefs.GetInt("ADS", 0) == 1 && PlayerPrefs.GetInt("ADSSUB", 0) == 0){
            StartCoroutine(adsTimer());
        }

        if(PlayerPrefs.GetInt("VIP", 0) == 1){
            VIPPricing();

            if(PlayerPrefs.GetInt("VIPSUB", 0) == 0){
                StartCoroutine(VIPTimer());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void VIPPricing(){

        GameObject.Find("3-Hrs-Lives-Amount").GetComponent<Text>().text = "350";
        GameObject.Find("6-Hrs-Lives-Amount").GetComponent<Text>().text = "600";
        GameObject.Find("9-Hrs-Lives-Amount").GetComponent<Text>().text = "800";
        GameObject.Find("12-Hrs-Lives-Amount").GetComponent<Text>().text = "1,000";

        GameObject.Find("3-Hrs-Ads-Amount").GetComponent<Text>().text = "1,500";
        GameObject.Find("6-Hrs-Ads-Amount").GetComponent<Text>().text = "2,400";
        GameObject.Find("9-Hrs-Ads-Amount").GetComponent<Text>().text = "3,250";
        GameObject.Find("12-Hrs-Ads-Amount").GetComponent<Text>().text = "4,000";

        GameObject.Find("3-Power-Amount").GetComponent<Text>().text = "250";
        GameObject.Find("6-Power-Amount").GetComponent<Text>().text = "450";
        GameObject.Find("9-Power-Amount").GetComponent<Text>().text = "650";
        GameObject.Find("12-Power-Amount").GetComponent<Text>().text = "800";
        GameObject.Find("15-Power-Amount").GetComponent<Text>().text = "950";
        GameObject.Find("18-Power-Amount").GetComponent<Text>().text = "1,050";

        GameObject.Find("500-Coins-Cash-Text").GetComponent<Text>().text = "$0.99";
        GameObject.Find("1300-Coins-Cash-Text").GetComponent<Text>().text = "$1.99";
        GameObject.Find("2800-Coins-Cash-Text").GetComponent<Text>().text = "$4.99";
        GameObject.Find("6000-Coins-Cash-Text").GetComponent<Text>().text = "$9.99";
        GameObject.Find("8500-Coins-Cash-Text").GetComponent<Text>().text = "$11.99";
        GameObject.Find("20000-Coins-Cash-Text").GetComponent<Text>().text = "$24.99";
    }

    public static void removeVIPPricing(){

        GameObject.Find("3-Hrs-Lives-Amount").GetComponent<Text>().text = "700";
        GameObject.Find("6-Hrs-Lives-Amount").GetComponent<Text>().text = "1,200";
        GameObject.Find("9-Hrs-Lives-Amount").GetComponent<Text>().text = "1,600";
        GameObject.Find("12-Hrs-Lives-Amount").GetComponent<Text>().text = "2,000";

        GameObject.Find("3-Hrs-Ads-Amount").GetComponent<Text>().text = "3,000";
        GameObject.Find("6-Hrs-Ads-Amount").GetComponent<Text>().text = "4,800";
        GameObject.Find("9-Hrs-Ads-Amount").GetComponent<Text>().text = "6,500";
        GameObject.Find("12-Hrs-Ads-Amount").GetComponent<Text>().text = "8,000";

        GameObject.Find("3-Power-Amount").GetComponent<Text>().text = "500";
        GameObject.Find("6-Power-Amount").GetComponent<Text>().text = "900";
        GameObject.Find("9-Power-Amount").GetComponent<Text>().text = "1,300";
        GameObject.Find("12-Power-Amount").GetComponent<Text>().text = "1,600";
        GameObject.Find("15-Power-Amount").GetComponent<Text>().text = "1,900";
        GameObject.Find("18-Power-Amount").GetComponent<Text>().text = "2,100";

        GameObject.Find("500-Coins-Cash-Text").GetComponent<Text>().text = "$1.99";
        GameObject.Find("1300-Coins-Cash-Text").GetComponent<Text>().text = "$4.99";
        GameObject.Find("2800-Coins-Cash-Text").GetComponent<Text>().text = "$9.99";
        GameObject.Find("6000-Coins-Cash-Text").GetComponent<Text>().text = "$19.99";
        GameObject.Find("8500-Coins-Cash-Text").GetComponent<Text>().text = "$24.99";
        GameObject.Find("20000-Coins-Cash-Text").GetComponent<Text>().text = "$49.99";
    }

    public void purchaseLives(int coins){

        if(PlayerPrefs.GetInt("VIP", 0) == 1){
            coinCost = coins/2;
        }else{
            coinCost = coins;
        }

        if(PlayerPrefs.GetInt("Coins", 0) < coinCost){
            HomePage.HP.openNotEnoughCoins();
        }else{
            HomePage.HP.openAreYouSure();
            purchaseType = "Lives";
        }
    }

    public void purchaseVip(int coins){

            coinCost = coins;

        if(PlayerPrefs.GetInt("Coins", 0) < coinCost){
            HomePage.HP.openNotEnoughCoins();
        }else{
            HomePage.HP.openAreYouSure();
            purchaseType = "VIP";
        }
    }

    public void purchaseAds(int coins){

        if(PlayerPrefs.GetInt("VIP", 0) == 1){
            coinCost = coins/2;
        }else{
            coinCost = coins;
        }

        if(PlayerPrefs.GetInt("Coins", 0) < coinCost){
            HomePage.HP.openNotEnoughCoins();
        }else{
            HomePage.HP.openAreYouSure();
            purchaseType = "Ads";
        }
    }


    public void purchasePower(int coins){

        if(PlayerPrefs.GetInt("VIP", 0) == 1){
            coinCost = coins/2;
        }else{
            coinCost = coins;
        }

        if(PlayerPrefs.GetInt("Coins", 0) < coinCost){
            HomePage.HP.openNotEnoughCoins();
        }else{
            HomePage.HP.openAreYouSure();
            purchaseType = "Power";
        }
    }

    public void setHours(int hours){
        hourTime = hours;
    }

    public void processCoinPurchase(){

        long tempTime;
        DateTime lastDate;
        DateTime newTime;
        bool expired = true;
        if(purchaseType == "Lives"){
            tempTime = Convert.ToInt64(PlayerPrefs.GetString("UNLIMITEDLIVESEND",System.DateTime.Now.ToBinary().ToString())); 
            lastDate = System.DateTime.FromBinary(tempTime);

            if(lastDate.Ticks < System.DateTime.Now.Ticks){
                newTime = System.DateTime.Now.AddHours(hourTime);
            }else{
                newTime = lastDate.AddHours(hourTime);
                expired = false;
            }
            PlayerPrefs.SetString("UNLIMITEDLIVESEND", newTime.ToBinary().ToString());
            PlayerPrefs.SetInt("UNLIMITEDLIVES", 1);
            PlayerPrefs.SetInt("LivesCoin", 1);
            PlayerPrefs.SetInt("Lives", 5);
            if(expired){
                StartCoroutine(unlimitedLifeTimer());
            }
        }else if(purchaseType == "ADS"){

            tempTime = Convert.ToInt64(PlayerPrefs.GetString("ADSEND",System.DateTime.Now.ToBinary().ToString())); 
            lastDate = System.DateTime.FromBinary(tempTime);

            if(lastDate.Ticks < System.DateTime.Now.Ticks){
                newTime = System.DateTime.Now.AddHours(hourTime);
            }else{
                newTime = lastDate.AddHours(hourTime);
                expired = false;
            }
            PlayerPrefs.SetString("ADSEND", newTime.ToBinary().ToString());
            PlayerPrefs.SetInt("AdsCoin", 1);
            PlayerPrefs.SetInt("ADS", 1);
            if(expired){
                StartCoroutine(adsTimer());
            }

        }else if(purchaseType == "VIP"){

            tempTime = Convert.ToInt64(PlayerPrefs.GetString("VIPEND",System.DateTime.Now.ToBinary().ToString())); 
            lastDate = System.DateTime.FromBinary(tempTime);

            if(lastDate.Ticks < System.DateTime.Now.Ticks){
                newTime = System.DateTime.Now.AddHours(hourTime);
            }else{
                newTime = lastDate.AddHours(hourTime);
                expired = false;
            }
            PlayerPrefs.SetString("VIPEND", newTime.ToBinary().ToString());
            PlayerPrefs.SetInt("VipCoin", 1);
            PlayerPrefs.SetInt("VIP", 1);
            if(expired){
                StartCoroutine(adsTimer());
            }

        }else{

            PlayerPrefs.SetInt("Clock", PlayerPrefs.GetInt("Clock", 3) + hourTime);
            PlayerPrefs.SetInt("Sword", PlayerPrefs.GetInt("Sword", 3) + hourTime);
            PlayerPrefs.SetInt("Tornado", PlayerPrefs.GetInt("Tornado", 3) + hourTime);
        }

        Coins.C.removeCoins(coinCost);

        HomePage.HP.closeAreYouSure();

        DataControl.DC.setAccountData();
    }

    public IEnumerator unlimitedLifeTimer(){

        GameObject.Find("Lives-Time").GetComponent<Text>().enabled = true;

        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("UNLIMITEDLIVESEND",System.DateTime.Now.ToBinary().ToString()));
        DateTime newTime = System.DateTime.FromBinary(tempTime);

        TimeSpan timeDiff = newTime - System.DateTime.Now;
        while(timeDiff.TotalSeconds > 0){

            yield return new WaitForSeconds(.1f);
            timeDiff = newTime - System.DateTime.Now;

            string seconds = "00";
            if(timeDiff.Seconds < 10){
                seconds = "0"+timeDiff.Seconds.ToString();
            }else{
                seconds = timeDiff.Seconds.ToString();
            }

            GameObject.Find("Lives-Time").GetComponent<Text>().text = timeDiff.Hours.ToString() + ":" + timeDiff.Minutes.ToString() + ":" + seconds;
        }

        PlayerPrefs.SetInt("LivesCoin", 0);

        GameObject.Find("Lives-Time").GetComponent<Text>().enabled = false;

        if(PlayerPrefs.GetInt("LIVESSUB", 0) == 0){
            PlayerPrefs.SetInt("UNLIMITEDLIVES", 0);
        }
    }


    public IEnumerator adsTimer(){

        GameObject.Find("Ads-Label").GetComponent<Text>().enabled = true;
        GameObject.Find("Ads-Time").GetComponent<Text>().enabled = true;

        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("ADSEND",System.DateTime.Now.ToBinary().ToString()));
        DateTime newTime = System.DateTime.FromBinary(tempTime);

        TimeSpan timeDiff = newTime - System.DateTime.Now;
        while(timeDiff.TotalSeconds > 0){

            yield return new WaitForSeconds(.1f);
            timeDiff = newTime - System.DateTime.Now;

            string seconds = "00";
            if(timeDiff.Seconds < 10){
                seconds = "0"+timeDiff.Seconds.ToString();
            }else{
                seconds = timeDiff.Seconds.ToString();
            }

            GameObject.Find("Ads-Time").GetComponent<Text>().text = timeDiff.Hours.ToString() + ":" + timeDiff.Minutes.ToString() + ":" + seconds;
        }

        PlayerPrefs.SetInt("AdsCoin", 0);

        GameObject.Find("Ads-Label").GetComponent<Text>().enabled = false;
        GameObject.Find("Ads-Time").GetComponent<Text>().enabled = false;

        if(PlayerPrefs.GetInt("ADSSUB", 0) == 0){
            PlayerPrefs.SetInt("ADS", 0);
        }
    }


    public IEnumerator VIPTimer(){
        
        GameObject.Find("Vip-Label").GetComponent<Text>().enabled = true;
        GameObject.Find("Vip-Time").GetComponent<Text>().enabled = true;

        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("VIPEND",System.DateTime.Now.ToBinary().ToString()));
        DateTime newTime = System.DateTime.FromBinary(tempTime);

        TimeSpan timeDiff = newTime - System.DateTime.Now;
        while(timeDiff.TotalSeconds > 0){

            yield return new WaitForSeconds(.1f);
            timeDiff = newTime - System.DateTime.Now;

            string seconds = "00";
            if(timeDiff.Seconds < 10){
                seconds = "0"+timeDiff.Seconds.ToString();
            }else{
                seconds = timeDiff.Seconds.ToString();
            }

            GameObject.Find("Vip-Time").GetComponent<Text>().text = timeDiff.Hours.ToString() + ":" + timeDiff.Minutes.ToString() + ":" + seconds;
        }

        PlayerPrefs.SetInt("VipCoin", 0);

        GameObject.Find("Vip-Label").GetComponent<Text>().enabled = false;
        GameObject.Find("Vip-Time").GetComponent<Text>().enabled = false;

        if(PlayerPrefs.GetInt("VIPSUB", 0) == 0){
            PlayerPrefs.SetInt("VIP", 0);

            removeVIPPricing();
        }
    }
}

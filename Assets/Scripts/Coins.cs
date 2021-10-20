using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{

    public static Coins C;
    // Start is called before the first frame update
    void Start()
    {

        C = this; 

        GameObject.Find("Coins").GetComponent<Text>().text = PlayerPrefs.GetInt("Coins", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addCoins(int amount){

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + amount);

        if(amount > 50){
            GameObject.Find("Coins").GetComponent<Text>().text = PlayerPrefs.GetInt("Coins", 0).ToString();
        }else{

        StartCoroutine(coinAdd(amount));
        }
    }

    public IEnumerator coinAdd(int amount){

        for(int i = 1; i <= amount; i++){
            int currentcoins = Convert.ToInt32(GameObject.Find("Coins").GetComponent<Text>().text);
            currentcoins++;
            GameObject.Find("Coins").GetComponent<Text>().text = currentcoins.ToString();

            yield return new WaitForSeconds(.01f);
        }
    }

    public void removeCoins(int amount){

        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) - amount);

        GameObject.Find("Coins").GetComponent<Text>().text = PlayerPrefs.GetInt("Coins", 0).ToString();

        //StartCoroutine(coinSubtract(amount));
    }

    public IEnumerator coinSubtract(int amount){

        for(int i = 1; i <= amount; i++){
            int currentcoins = Convert.ToInt32(GameObject.Find("Coins").GetComponent<Text>().text);
            currentcoins--;
            GameObject.Find("Coins").GetComponent<Text>().text = currentcoins.ToString();

            yield return new WaitForSeconds(.01f);
        }
    }
}

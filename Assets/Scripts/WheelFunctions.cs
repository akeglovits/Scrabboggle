using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelFunctions : MonoBehaviour
{

public static WheelFunctions WF;
    public static string currentSlice;
    public static int amount;
    public static string type;

    public AudioClip wheelPrize;
    // Start is called before the first frame update
    void Start()
    {
        WF = this;

        StartCoroutine(buttonTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spinWheel(){
        StartCoroutine(wheelSpin());
    }

    public IEnumerator wheelSpin(){

        
        float spin = 35f;

        while(spin > 0f){
            GameObject.Find("Wheel").transform.Rotate(0f,0f,-spin,Space.Self);
            yield return new WaitForSeconds(.05f);

            spin -= .25f;
        }

        GameObject.Find("Wheel-Particles").GetComponent<ParticleSystem>().Play();

        
            GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(wheelPrize);
        

        if(PlayerPrefs.GetInt("Vibrate", 1) == 1){
            Handheld.Vibrate();
        }

        if(type == "Coins"){
            Coins.C.addCoins(amount);
        }else{
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives",5) + amount);
        }

        PlayerPrefs.SetString("LastWheelSpin",System.DateTime.Now.ToBinary().ToString());
        StartCoroutine(buttonTimer());

        DataControl.DC.setAccountData();

    }


    public IEnumerator buttonTimer(){

        GameObject.Find("Wheel-Button-Time").GetComponent<Text>().enabled = true;
        GameObject.Find("Wheel-Button").GetComponent<Button>().interactable = false;

        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("LastWheelSpin",System.DateTime.Now.AddMinutes(-20).ToBinary().ToString()));
        DateTime lastDate = System.DateTime.FromBinary(tempTime);
        DateTime newTime = lastDate.AddMinutes(10);

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

            GameObject.Find("Wheel-Button-Time").GetComponent<Text>().text = timeDiff.Minutes.ToString() + ":" + seconds;
        }

        GameObject.Find("Wheel-Button-Time").GetComponent<Text>().enabled = false;
        GameObject.Find("Wheel-Button").GetComponent<Button>().interactable = true;
    }
}

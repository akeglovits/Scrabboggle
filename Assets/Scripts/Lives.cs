using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    // Start is called before the first frame update
    public static Lives L;

    
    void Start()
    {
        L = this;



        if(PlayerPrefs.GetInt("Lives", 5) < 5){
            checkAddedLives();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("UNLIMITEDLIVES", 0) == 0 && PlayerPrefs.GetInt("VIP", 0) == 0){
            GameObject.Find("Lives").GetComponent<Text>().enabled = true;
            GameObject.Find("Lives").GetComponent<Text>().text = PlayerPrefs.GetInt("Lives", 5).ToString() + "/5";
            GameObject.Find("Lives-Infinity").GetComponent<Image>().enabled = false;

            if(GameObject.Find("Lives-2") != null){
                GameObject.Find("Lives-2").GetComponent<Text>().enabled = true;
            GameObject.Find("Lives-2").GetComponent<Text>().text = PlayerPrefs.GetInt("Lives", 5).ToString() + "/5";
            GameObject.Find("Lives-Infinity-2").GetComponent<Image>().enabled = false;
            }
        }else{
            GameObject.Find("Lives").GetComponent<Text>().enabled = false;
            GameObject.Find("Lives-Infinity").GetComponent<Image>().enabled = true;

            if(GameObject.Find("Lives-2") != null){
                GameObject.Find("Lives-2").GetComponent<Text>().enabled = false;
            GameObject.Find("Lives-Infinity-2").GetComponent<Image>().enabled = true;
            }

            
        }
    }

    public void checkAddedLives(){

        
        DateTime currentTime = System.DateTime.Now;
        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("LastLifeAdded",System.DateTime.Now.ToBinary().ToString()));
        DateTime lastDate = System.DateTime.FromBinary(tempTime);
        TimeSpan timeDiff = currentTime.Subtract(lastDate);

        if(PlayerPrefs.GetInt("Lives", 5) < 5){
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives", 5) + (int)(Mathf.Floor((float)timeDiff.TotalMinutes / 20f)));
            if(PlayerPrefs.GetInt("Lives", 5) > 5){
                PlayerPrefs.SetInt("Lives", 5);
            }

            DateTime newTime = lastDate.AddMinutes((double)(Mathf.Floor((float)timeDiff.TotalMinutes / 20f)) * 20f);
            PlayerPrefs.SetString("LastLifeAdded", newTime.ToBinary().ToString());

            if(PlayerPrefs.GetInt("Lives", 5) < 5){
                StartCoroutine(lifeTimer());
            }
        }

    }

    public IEnumerator lifeTimer(){

        GameObject.Find("Lives-Time").GetComponent<Text>().enabled = true;

        if(GameObject.Find("Lives-Time-2") != null){
            GameObject.Find("Lives-Time-2").GetComponent<Text>().enabled = true;
        }

        long tempTime = Convert.ToInt64(PlayerPrefs.GetString("LastLifeAdded",System.DateTime.Now.ToBinary().ToString()));
        DateTime lastDate = System.DateTime.FromBinary(tempTime);
        DateTime newTime = lastDate.AddMinutes(20);

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

            GameObject.Find("Lives-Time").GetComponent<Text>().text = timeDiff.Minutes.ToString() + ":" + seconds;

            if(GameObject.Find("Lives-Time-2") != null){
                GameObject.Find("Lives-Time-2").GetComponent<Text>().text = timeDiff.Minutes.ToString() + ":" + seconds;
            }
        }

        addLife();
    }

    public void loseLife(){

        if(PlayerPrefs.GetInt("UNLIMITEDLIVES", 0) == 0 && PlayerPrefs.GetInt("VIP", 0) == 0){
            if(PlayerPrefs.GetInt("Lives", 5) == 5){
                PlayerPrefs.SetString("LastLifeAdded", System.DateTime.Now.ToBinary().ToString());

                    StartCoroutine(lifeTimer());
                
            }

            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives", 5) - 1);
        }
    }

    public void addLife(){

         GameObject.Find("Lives-Time").GetComponent<Text>().enabled = false;

         if(GameObject.Find("Lives-Time-2") != null){
            GameObject.Find("Lives-Time-2").GetComponent<Text>().enabled = false;
        }

        PlayerPrefs.SetString("LastLifeAdded", System.DateTime.Now.ToBinary().ToString());

        if(PlayerPrefs.GetInt("Lives", 5) < 5){
            PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives", 5) + 1);
            StartCoroutine(lifeTimer());
        }
    }
}

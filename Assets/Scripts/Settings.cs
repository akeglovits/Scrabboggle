using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Mute", 0) == 1){
            GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = true;
            GameObject.Find("Settings-Mute-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-on");
        }

        if(PlayerPrefs.GetInt("Vibrate", 1) == 0){
            GameObject.Find("Settings-Vibrate-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-off");
        }

        if(PlayerPrefs.GetInt("Sounds", 1) == 0){
            GameObject.Find("Settings-Sounds-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-off");
            GameObject.Find("Game-Sounds").GetComponent<AudioSource>().mute = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleMute(){

        if(PlayerPrefs.GetInt("Mute", 0) == 0){
            PlayerPrefs.SetInt("Mute", 1);
            GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = true;
            GameObject.Find("Settings-Mute-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-on");
        }else{
            PlayerPrefs.SetInt("Mute", 0);
            GameObject.Find("Background-Music").GetComponent<AudioSource>().mute = false;
            GameObject.Find("Settings-Mute-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-off");
        }
    }

    public void toggleVibrate(){

        if(PlayerPrefs.GetInt("Vibrate", 1) == 0){
            PlayerPrefs.SetInt("Vibrate", 1);
            GameObject.Find("Settings-Vibrate-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-on");
        }else{
            PlayerPrefs.SetInt("Vibrate", 0);
            GameObject.Find("Settings-Vibrate-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-off");
        }
    }

    public void toggleSounds(){

        if(PlayerPrefs.GetInt("Sounds", 1) == 0){
            PlayerPrefs.SetInt("Sounds", 1);
            GameObject.Find("Settings-Sounds-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-on");
            GameObject.Find("Game-Sounds").GetComponent<AudioSource>().mute = false;
        }else{
            PlayerPrefs.SetInt("Sounds", 0);
            GameObject.Find("Settings-Sounds-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/setting-off");
            GameObject.Find("Game-Sounds").GetComponent<AudioSource>().mute = true;
        }
    }


    public void openSettingsInGame(){

        GameplayFunctions.timerGo = false;
        StartCoroutine(panelOpen("Settings-Panel"));
    }

    public void closeSettingsInGame(){

        StartCoroutine(panelClose("Settings-Panel"));
    }


    public IEnumerator panelOpen(string panel){
        GameObject panelObject = GameObject.Find(panel);
        float scale = .1f;
        float scale2 = .03f;

        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        panelObject.transform.SetAsLastSibling();

        while(panelObject.transform.localScale.y < 1.1f){
            panelObject.transform.localScale += new Vector3(scale,scale,scale);
            yield return new WaitForSeconds(.02f);
        }

        yield return new WaitForSeconds(.05f);

        while(panelObject.transform.localScale.y > 1f){
            panelObject.transform.localScale -= new Vector3(scale2,scale2,scale2);
            yield return new WaitForSeconds(.02f);
        }


        panelObject.transform.localScale = new Vector3(1f,1f,1f);
    }


    public IEnumerator panelClose(string panel){
        GameObject panelObject = GameObject.Find(panel);
        float scale = .1f;
        float scale2 = .03f;
        while(panelObject.transform.localScale.y < 1.1f){
            panelObject.transform.localScale += new Vector3(scale2,scale2,scale2);
            yield return new WaitForSeconds(.02f);
        }

        yield return new WaitForSeconds(.1f);

        while(panelObject.transform.localScale.y > 0){
            panelObject.transform.localScale -= new Vector3(scale,scale,scale);
            yield return new WaitForSeconds(.02f);
        }

        panelObject.transform.localScale = new Vector3(0f,0f,0f);

        panelObject.transform.SetAsFirstSibling();
        GameObject.Find("Block-Background").transform.SetAsFirstSibling();

        GameplayFunctions.GPF.startTimer();
    }
}

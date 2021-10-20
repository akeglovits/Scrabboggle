using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayFunctions : MonoBehaviour
{

    public static GameplayFunctions GPF;
    public static bool creatingWord;
    public static int timer;
    public static bool timerGo;
    public static int totalScore;

    public GameObject scoreflash;
    // Start is called before the first frame update
    void Start()
    {

        GPF = this;
        
        creatingWord = false;
        timerGo = false;
        totalScore = 0;
        timer = 120;

        if(SceneManager.GetActiveScene().name == "SinglePlay" && PlayerPrefs.GetInt("FirstGame", 0) == 0){
            // Instructions will open in Instruction Script
        }else{
            startTimer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Total-Score").GetComponent<Text>().text = totalScore.ToString();

        string seconds = "00";
        if(timer % 60 < 10){
            seconds = "0" + (timer % 60);
        }else{
            seconds = (timer % 60).ToString();
        }
        GameObject.Find("Timer").GetComponent<Text>().text = Mathf.Floor(timer / 60).ToString() + ":" + seconds;
    }

    public void flashScoreCall(int score){

        StartCoroutine(flashScore(score, scoreflash));
    }
    public IEnumerator flashScore(int score, GameObject addedscore){

        addedscore.GetComponent<Text>().text = "+"+score.ToString();

        GameObject newScore = Instantiate(addedscore, GameObject.Find("Score-Add").transform, false);

        yield return new WaitForSeconds(.2f);

        newScore.GetComponent<Text>().color = Color.green;

        yield return new WaitForSeconds(.2f);

        newScore.GetComponent<Text>().color = Color.white;

        yield return new WaitForSeconds(.2f);

        newScore.GetComponent<Text>().color = Color.green;

        yield return new WaitForSeconds(.2f);

        newScore.GetComponent<Text>().color = Color.white;

        yield return new WaitForSeconds(.2f);

        newScore.GetComponent<Text>().color = Color.green;

        yield return new WaitForSeconds(.2f);

        Destroy(newScore);
    }

    public IEnumerator RunTimer(){

        while(timerGo && timer > 0){

            yield return new WaitForSeconds(1f);

            timer--;
        }

        if(timerGo){
            if(SceneManager.GetActiveScene().name == "SinglePlay"){
                SinglePlaySetup.gameOver();
            }else{
                MultiPlaySetup.gameOver();
            }
        }
    }


    public void startTimer(){
        timerGo = true;
        StartCoroutine(RunTimer());
    }
}

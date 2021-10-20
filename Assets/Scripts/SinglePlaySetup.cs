using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SinglePlaySetup : MonoBehaviour
{   

    private List<string> DL;
    private List<string> TL;
    private List<string> DW;
    private List<string> TW;

    private static int currentLevel;
    private static int currentStage;
    private static int goal;
    private static bool changeHomeLevel;

    public static bool gameEnded;

    
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        currentStage = PlayerPrefs.GetInt("CurrentStage", 1);

        changeHomeLevel = false;

        gameEnded = false;

        goal = (currentStage * 50) + 25;

        if(currentLevel == 1){
            DL = new List<string>{"Tile-1-1", "Tile-2-1", "Tile-3-1", "Tile-4-1"};
            TL = new List<string>{"Tile-1-4", "Tile-2-4", "Tile-3-4", "Tile-4-4"};
            DW = new List<string>{"Tile-1-2", "Tile-1-3"};
            TW = new List<string>{"Tile-4-2", "Tile-4-3"};
        }else if(currentLevel == 2){
            DL = new List<string>{"Tile-2-2", "Tile-3-3"};
            TL = new List<string>{"Tile-2-3", "Tile-3-2"};
            DW = new List<string>{"Tile-1-1", "Tile-4-4"};
            TW = new List<string>{"Tile-1-4", "Tile-4-1"};
        }else if(currentLevel == 3){
            TL = new List<string>{"Tile-2-2", "Tile-3-3"};
            DL = new List<string>{"Tile-2-3", "Tile-3-2"};
            TW = new List<string>{"Tile-1-1", "Tile-4-4"};
            DW = new List<string>{"Tile-1-4", "Tile-4-1"};
        }else if(currentLevel == 4){
            DL = new List<string>{"Tile-2-2"};
            TL = new List<string>{"Tile-3-3"};
            DW = new List<string>{"Tile-1-1"};
            TW = new List<string>{"Tile-4-4"};
        }else if(currentLevel == 5){
            DL = new List<string>{"Tile-3-2"};
            TL = new List<string>{"Tile-2-3"};
            DW = new List<string>{"Tile-1-4"};
            TW = new List<string>{"Tile-4-1"};
        }else if(currentLevel == 6){
            DL = new List<string>{"Tile-1-1"};
            TL = new List<string>{"Tile-4-4"};
            DW = new List<string>{"Tile-1-4"};
            TW = new List<string>{"Tile-4-1"};
        }else if(currentLevel == 7){
            DL = new List<string>{"Tile-2-2"};
            TL = new List<string>{"Tile-3-3"};
            DW = new List<string>{"Tile-2-3"};
            TW = new List<string>{"Tile-3-2"};
        }else if(currentLevel == 8){
            DL = new List<string>{};
            TL = new List<string>{};
            DW = new List<string>{"Tile-2-3"};
            TW = new List<string>{"Tile-3-2"};
        }else if(currentLevel == 9){
            DL = new List<string>{"Tile-2-2"};
            TL = new List<string>{"Tile-3-3"};
            DW = new List<string>{};
            TW = new List<string>{};
        }else{
            DL = new List<string>();
            TL = new List<string>();
            DW = new List<string>();
            TW = new List<string>();
        }

        setMultipliers(DL, "DL");
        setMultipliers(TL, "TL");
        setMultipliers(DW, "DW");
        setMultipliers(TW, "TW");
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Clock-Amount").GetComponent<Text>().text = PlayerPrefs.GetInt("Clock", 3).ToString();
        GameObject.Find("Sword-Amount").GetComponent<Text>().text = PlayerPrefs.GetInt("Sword", 3).ToString();
        GameObject.Find("Tornado-Amount").GetComponent<Text>().text = PlayerPrefs.GetInt("Tornado", 3).ToString();

        if(PlayerPrefs.GetInt("Clock", 3) == 0 || WordCheck.swordActive || WordCheck.tornadoActive){
            GameObject.Find("Clock-Button").GetComponent<Button>().interactable = false;
        }else{
            GameObject.Find("Clock-Button").GetComponent<Button>().interactable = true;
        }

        if(PlayerPrefs.GetInt("Sword", 3) == 0 || WordCheck.swordActive || WordCheck.tornadoActive){
            GameObject.Find("Sword-Button").GetComponent<Button>().interactable = false;
        }else{
            GameObject.Find("Sword-Button").GetComponent<Button>().interactable = true;
        }

        if(PlayerPrefs.GetInt("Tornado", 3) == 0 || WordCheck.swordActive || WordCheck.tornadoActive){
            GameObject.Find("Tornado-Button").GetComponent<Button>().interactable = false;
        }else{
            GameObject.Find("Tornado-Button").GetComponent<Button>().interactable = true;
        }
    }

    public void setMultipliers(List<string> tiles, string multiplier){

        foreach(string tile in tiles){
        GameObject currentTile = GameObject.Find(tile);

            currentTile.tag = multiplier;
            currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-"+multiplier);
            currentTile.transform.GetChild(2).gameObject.GetComponent<Text>().text = multiplier;
        }
    }

    void OnDisable(){

        if(!gameEnded){
            if(PlayerPrefs.GetInt("UNLIMITEDLIVES", 0) == 0 && PlayerPrefs.GetInt("VIP", 0) == 0){
                if(PlayerPrefs.GetInt("Lives", 5) == 5){
                    PlayerPrefs.SetString("LastLifeAdded", System.DateTime.Now.ToBinary().ToString());
                }

                PlayerPrefs.SetInt("Lives", PlayerPrefs.GetInt("Lives", 5) - 1);
            }

            DataControl.DC.setAccountData();

        }
    }

    public static void gameOver(){

        gameEnded = true;
        if(GameplayFunctions.totalScore >= goal && currentLevel == PlayerPrefs.GetInt("Level", 1) && currentStage == PlayerPrefs.GetInt("Stage", 1)){
            changeHomeLevel = true;
            if(PlayerPrefs.GetInt("Level", 1) == 10){
                PlayerPrefs.SetInt("Level", 1);
                PlayerPrefs.SetInt("Stage", PlayerPrefs.GetInt("Stage", 1) + 1);
            }else{
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
            }

            GameObject.Find("Game-Over-Coins-Text").GetComponent<Text>().text = "+" + (currentLevel * currentStage * 5).ToString();

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + (currentLevel * currentStage * 5));
        }else if(GameplayFunctions.totalScore >= goal){

            GameObject.Find("Game-Over-Coins-Text").GetComponent<Text>().text = "+" + (currentLevel * 5).ToString();

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + (currentLevel * 5));
        }

        DataControl.DC.setAccountData();

        if(GameplayFunctions.totalScore >= goal + 400){
            GameObject.Find("Game-Over-Text").GetComponent<Text>().text = "INCREDIBLE!";
        }else if(GameplayFunctions.totalScore >= goal + 300){
            GameObject.Find("Game-Over-Text").GetComponent<Text>().text = "WONDERFUL!";
        }else if(GameplayFunctions.totalScore >= goal + 200){
            GameObject.Find("Game-Over-Text").GetComponent<Text>().text = "FANTASTIC!";
        }else if(GameplayFunctions.totalScore >= goal + 100){
            GameObject.Find("Game-Over-Text").GetComponent<Text>().text = "AMAZING!";
        }else if(GameplayFunctions.totalScore >= goal){
            GameObject.Find("Game-Over-Text").GetComponent<Text>().text = "YOU DID IT!";
        }else{
            GameObject.Find("Game-Over-Text").GetComponent<Text>().text = "NICE TRY!";
            GameObject.Find("Game-Over-Text").GetComponent<Text>().color = Color.red;


            
            Lives.L.loseLife();
        }

        GameObject.Find("Score-Text").GetComponent<Text>().text = GameplayFunctions.totalScore.ToString();
        GameObject.Find("Goal-Text").GetComponent<Text>().text = goal.ToString();
        GameObject.Find("Best-Word-Text").GetComponent<Text>().text = WordCheck.bestWord.ToString() + " (" + WordCheck.bestScore + ")";

        if(GameplayFunctions.totalScore >= goal){
            GameObject.Find("Game-Over-Lives").SetActive(false);
        }else{
            if(PlayerPrefs.GetInt("UNLIMITEDLIVES",0) == 1 || PlayerPrefs.GetInt("VIP", 0) == 1){
                GameObject.Find("Game-Over-Lives").SetActive(false);
            }
            GameObject.Find("Game-Over-Coins").SetActive(false);
            GameObject.Find("Next-Level-Button-Text").GetComponent<Text>().text = "RETRY";
        }

        GameEndAd.GEA.showGameEndAd();

        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Game-Over-Panel").transform.SetAsLastSibling();
    }


    public void nextLevel(){

        if(GameplayFunctions.totalScore >= goal){
            if(PlayerPrefs.GetInt("CurrentLevel") == 10){
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.SetInt("CurrentStage", PlayerPrefs.GetInt("CurrentStage", 1) + 1);
            }else{
                PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel", 1) + 1);
            }
        }

        if(PlayerPrefs.GetInt("Lives", 5) > 0){
        PlayerPrefs.SetString("NextScene", "SinglePlay");

        SceneManager.LoadScene("LoadingScreen");
        }else{

            openNotEnough();
        }

    }

    public void goHome(){

        if(changeHomeLevel){
            PlayerPrefs.SetInt("LevelCompleted", 1);
        }

        PlayerPrefs.SetString("NextScene", "Home");

        SceneManager.LoadScene("LoadingScreen");
    }

    public void openNotEnough(){

        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Not-Enough-Panel").transform.SetAsLastSibling();
    }

    public void closeNotEnough(){

        GameObject.Find("Not-Enough-Panel").transform.SetAsFirstSibling();
        GameObject.Find("Game-Over-Panel").transform.SetAsLastSibling();
    }

    
}

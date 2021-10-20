using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiPlaySetup : MonoBehaviour
{   

    private List<string> DL;
    private List<string> TL;
    private List<string> DW;
    private List<string> TW;

    private static int currentRound;
    public static bool gameEnded;
    // Start is called before the first frame update
    void Start()
    {

        gameEnded = false;

        currentRound = PlayerPrefs.GetInt("CurrentMultiRound", 1);

        if(currentRound == 1){
            DL = new List<string>{"Tile-2-2", "Tile-3-3"};
            TL = new List<string>{"Tile-1-4", "Tile-3-4"};
            DW = new List<string>{"Tile-4-2"};
            TW = new List<string>();
        }else if(currentRound == 2){
            DL = new List<string>{"Tile-2-3", "Tile-3-2"};
            TL = new List<string>{"Tile-1-2", "Tile-4-3"};
            DW = new List<string>{"Tile-4-1"};
            TW = new List<string>{"Tile-1-4"};
        }else if(currentRound == 3){
            TL = new List<string>{"Tile-1-4", "Tile-3-3"};
            DL = new List<string>{"Tile-2-2", "Tile-4-3"};
            DW = new List<string>{"Tile-1-3", "Tile-4-2"};
            TW = new List<string>{"Tile-3-4", "Tile-2-1"};
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
        
    }

    public void setMultipliers(List<string> tiles, string multiplier){

        foreach(string tile in tiles){
        GameObject currentTile = GameObject.Find(tile);

            currentTile.tag = multiplier;
            currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-"+multiplier);
            currentTile.transform.GetChild(2).gameObject.GetComponent<Text>().text = multiplier;
        }
    }

    public static void gameOver(){

        gameEnded = true;

        if(GameplayFunctions.totalScore > PlayerPrefs.GetInt("BestRound", 0)){
            PlayerPrefs.SetInt("BestRound", GameplayFunctions.totalScore);
        }

        FirebaseGameMoves.FGM.registerMove();

        DataControl.DC.setAccountData();

        GameEndAd.GEA.showGameEndAd();


    }

}

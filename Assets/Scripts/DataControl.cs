using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class DataControl : MonoBehaviour
{

    public static DataControl DC;
    private string password;

    public bool dataLoaded;

    private FirebaseFirestore root;

    public static List<string> one;
    public static List<string> two;
    public static List<string> three;
    public static List<string> four;
    public static List<string> five;
    public static List<string> eight;
    public static List<string> ten;


    // Start is called before the first frame update
    void Start()
    {
        DC = this;

        dataLoaded = false;

        root = FirebaseFirestore.DefaultInstance;

        one = new List<string> {"A", "E", "I", "L", "N", "O", "R", "S", "T", "U"};

        two = new List<string> {"D", "G"};

        three = new List<string> {"B", "C", "M", "P"};

        four = new List<string> {"F", "H", "V", "W", "Y"};

        five = new List<string> {"K"};

        eight = new List<string> {"J", "X"};

        ten = new List<string> {"Q", "Z"};
    }

    // Update is called once per frame
    void Update()
    {

    }

    public async void getAccountData(){

        DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).GetSnapshotAsync();

        if(snapshot.Exists){
		Dictionary<string, object> userData = snapshot.ToDictionary();

            PlayerPrefs.SetString("ID",(string)userData["FirebaseID"]);
            PlayerPrefs.SetInt("MultiLevel",Convert.ToInt32(userData["MultiLevel"]));
            PlayerPrefs.SetInt("Wins",Convert.ToInt32(userData["Wins"]));
            PlayerPrefs.SetInt("Loses",Convert.ToInt32(userData["Loses"]));
            PlayerPrefs.SetInt("Draws",Convert.ToInt32(userData["Draws"]));
            PlayerPrefs.SetInt("Stage",Convert.ToInt32(userData["Stage"]));
            PlayerPrefs.SetInt("Level",Convert.ToInt32(userData["Level"]));
            PlayerPrefs.SetInt("XP",Convert.ToInt32(userData["XP"]));
            PlayerPrefs.SetString("BestWord",(string)userData["BestWord"]);
            PlayerPrefs.SetInt("BestWordScore",Convert.ToInt32(userData["BestWordScore"]));
            PlayerPrefs.SetInt("BestRound",Convert.ToInt32(userData["BestRound"]));
            PlayerPrefs.SetInt("Coins", Convert.ToInt32(userData["Coins"]));
            PlayerPrefs.SetInt("Sword", Convert.ToInt32(userData["Sword"]));
            PlayerPrefs.SetInt("Clock", Convert.ToInt32(userData["Clock"]));
            PlayerPrefs.SetInt("Tornado", Convert.ToInt32(userData["Tornado"]));
            PlayerPrefs.SetInt("ADS", Convert.ToInt32(userData["ADS"]));
            PlayerPrefs.SetInt("VIP", Convert.ToInt32(userData["VIP"]));
            PlayerPrefs.SetInt("UNLIMITEDLIVES", Convert.ToInt32(userData["UNLIMITEDLIVES"]));
            PlayerPrefs.SetInt("AdsCoin", Convert.ToInt32(userData["AdsCoin"]));
            PlayerPrefs.SetInt("VipCoin", Convert.ToInt32(userData["VipCoin"]));
            PlayerPrefs.SetInt("LivesCoin", Convert.ToInt32(userData["LivesCoin"]));
            PlayerPrefs.SetInt("ADSSUB", Convert.ToInt32(userData["ADSSUB"]));
            PlayerPrefs.SetInt("VIPSUB", Convert.ToInt32(userData["VIPSUB"]));
            PlayerPrefs.SetInt("LIVESSUB", Convert.ToInt32(userData["LIVESSUB"]));
            PlayerPrefs.SetString("LastLifeAdded",(string)userData["LastLifeAdded"]);
            PlayerPrefs.SetString("UNLIMITEDLIVESEND",(string)userData["UNLIMITEDLIVESEND"]);
            PlayerPrefs.SetString("ADSEND",(string)userData["ADSEND"]);
            PlayerPrefs.SetString("VIPEND",(string)userData["VIPEND"]);
            PlayerPrefs.SetString("LastWheelSpin",(string)userData["LastWheelSpin"]);
            PlayerPrefs.SetInt("FirstGame", Convert.ToInt32(userData["FirstGame"]));
            
            List<string> tokens = snapshot.GetValue<List<string>>("NotificationToken");

            PlayerLists.notificationTokens = new List<string>(tokens);
            

		}

        

        dataLoaded = true;

		}


public async void setAccountData () { 

    string firebaseid = PlayerPrefs.GetString("ID","");
    string username = PlayerPrefs.GetString("Username","");
    int multilevel = PlayerPrefs.GetInt("MultiLevel",1);
    int wins = PlayerPrefs.GetInt("Wins",0);
    int loses = PlayerPrefs.GetInt("Loses",0);
    int draws = PlayerPrefs.GetInt("Draws",0);
    int stage = PlayerPrefs.GetInt("Stage",1);
    int level = PlayerPrefs.GetInt("Level",1);
    int xp = PlayerPrefs.GetInt("XP",0);
    string bestword = PlayerPrefs.GetString("BestWord","");
    int bestwordscore = PlayerPrefs.GetInt("BestWordScore",0);
    int bestround = PlayerPrefs.GetInt("BestRound",0);
    int coins = PlayerPrefs.GetInt("Coins", 0);
    int lives = PlayerPrefs.GetInt("Lives", 5);
    int sword = PlayerPrefs.GetInt("Sword", 3);
    int clock = PlayerPrefs.GetInt("Clock", 3);
    int tornado = PlayerPrefs.GetInt("Tornado", 3);
    int ads = PlayerPrefs.GetInt("ADS", 0);
    int vip = PlayerPrefs.GetInt("VIP", 0);
    int unlimitedlives = PlayerPrefs.GetInt("UNLIMITEDLIVES", 0);
    int adscoin = PlayerPrefs.GetInt("AdsCoin", 0);
    int vipcoin = PlayerPrefs.GetInt("VipCoin", 0);
    int livescoin = PlayerPrefs.GetInt("LivesCoin", 0);
    int adssub = PlayerPrefs.GetInt("ADSSUB", 0);
    int vipsub = PlayerPrefs.GetInt("VIPSUB", 0);
    int livessub = PlayerPrefs.GetInt("LIVESSUB", 0);
    string lastlifeadded = PlayerPrefs.GetString("LastLifeAdded", System.DateTime.Now.ToBinary().ToString());
    string unlimitedlivesend = PlayerPrefs.GetString("UNLIMITEDLIVESEND", System.DateTime.Now.ToBinary().ToString());
    string adsend = PlayerPrefs.GetString("ADSEND", System.DateTime.Now.ToBinary().ToString());
    string vipend = PlayerPrefs.GetString("VIPEND", System.DateTime.Now.ToBinary().ToString());
    string lastwheelspin = PlayerPrefs.GetString("LastWheelSpin", System.DateTime.Now.ToBinary().ToString());
    int firstgame = PlayerPrefs.GetInt("FirstGame", 0);

    if(!PlayerLists.notificationTokens.Contains(PlayerPrefs.GetString("NotificationToken", ""))){
        PlayerLists.notificationTokens.Add(PlayerPrefs.GetString("NotificationToken", ""));
    }

    Debug.Log(PlayerPrefs.GetString("Username"));
    await FirebaseFirestore.DefaultInstance.Collection("Users").Document(username).SetAsync(new Dictionary<string, object>(){
          {"FirebaseID", firebaseid},
          {"MultiLevel", multilevel},
          {"Wins", wins},
          {"Loses", loses},
          {"Draws", draws},
          {"Stage", stage},
          {"Level", level},
          {"XP", xp},
          {"BestWord", bestword},
          {"BestWordScore", bestwordscore},
          {"BestRound", bestround},
          {"Coins", coins},
          {"Lives", lives},
          {"Sword", sword},
          {"Clock", clock},
          {"Tornado", tornado},
          {"ADS", ads},
          {"VIP", vip},
          {"UNLIMITEDLIVES", unlimitedlives},
          {"AdsCoin", adscoin},
          {"VipCoin", vipcoin},
          {"LivesCoin", livescoin},
          {"ADSSUB", adssub},
          {"VIPSUB", vipsub},
          {"LIVESSUB", livessub},
          {"LastLifeAdded", lastlifeadded},
          {"UNLIMITEDLIVESEND", unlimitedlivesend},
          {"ADSEND", adsend},
          {"VIPEND", vipend},
          {"LastWheelSpin", lastwheelspin},
          {"NotificationToken", PlayerLists.notificationTokens},
          {"FirstGame", firstgame}
        });

        dataLoaded = true;
}

private int convertString(string stringToConvert){

    if(string.IsNullOrEmpty(stringToConvert)){
        return 0;
    }else{
        return Convert.ToInt32(stringToConvert);
    }
}

    public void createGameButton(string username, string round, string p1r1, string p1r2, string p1r3, string p2r1, string p2r2, string p2r3, GameObject clone, int turn){

        if(username == "Unknown"){

            string newLetter = "?";
            string newNumber = "0";

            username = "Waiting For Opponent";

            
            int p1score = convertString(p1r1) + convertString(p1r2) + convertString(p1r3);

            int p2score = 0;

            if(string.IsNullOrEmpty(p1r1)){
                p2score = 0;
            }else if(string.IsNullOrEmpty(p1r2)){
                p2score = convertString(p2r1);
            }else if(string.IsNullOrEmpty(p1r3)){
                p2score = convertString(p2r1) + convertString(p2r2);
            }else{
                p2score = convertString(p2r1) + convertString(p2r2) + convertString(p2r3);
            }

            clone.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = newLetter;
            clone.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = newNumber;
            clone.transform.GetChild(1).gameObject.GetComponent<Text>().text = username;

            clone.transform.GetChild(2).gameObject.GetComponent<Text>().text = p1score.ToString() + "-" + p2score.ToString();

    if(convertString(round) < 4){
	    clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "ROUND " + round;
        clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = new Color32(50,50,50,255);
        clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = false;
    }else{
        if(p1score > p2score){
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "YOU WON!";
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = Color.green;
            clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = true;
        }else if(p1score < p2score){
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "YOU LOST!";
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = Color.red;
            clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = true;
        }else{
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "DRAW!";
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = new Color32(50,50,50,255);
            clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = false;
        }
    }
    

    GameObject newButton = Instantiate(clone, new Vector3(10000,10000,0), Quaternion.identity, GameObject.Find("Active-Games").transform);

    newButton.GetComponent<Button>().onClick.AddListener(() => HomePage.HP.openActiveGamePanel(username, newLetter, newNumber, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3));

    if(newButton.transform.GetSiblingIndex() == 0){
        newButton.transform.localPosition = new Vector3(0, 440, 0);
    }else{
        GameObject lastButton = newButton.transform.parent.GetChild(newButton.transform.GetSiblingIndex() - 1).gameObject;

        newButton.transform.localPosition = new Vector3(0, lastButton.transform.localPosition.y - 200, 0);
    }


        }else{

            string newLetter = username.Substring(0,1).ToUpper();
            string newNumber = "";

            if(one.Contains(newLetter)){
                newNumber = "1";
            }else if(two.Contains(newLetter)){
                newNumber = "2";
            }else if(three.Contains(newLetter)){
                newNumber = "3";
            }else if(four.Contains(newLetter)){
                newNumber = "4";
            }else if(five.Contains(newLetter)){
                newNumber = "5";
            }else if(eight.Contains(newLetter)){
                newNumber = "8";
            }else{
                newNumber = "10";
            }

    int p1score = convertString(p1r1) + convertString(p1r2) + convertString(p1r3);

    int p2score = 0;

            if(string.IsNullOrEmpty(p1r1)){
                p2score = 0;
            }else if(string.IsNullOrEmpty(p1r2)){
                p2score = convertString(p2r1);
            }else if(string.IsNullOrEmpty(p1r3)){
                p2score = convertString(p2r1) + convertString(p2r2);
            }else{
                p2score = convertString(p2r1) + convertString(p2r2) +convertString(p2r3);
            }


    clone.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = newLetter;
    clone.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>().text = newNumber;
    clone.transform.GetChild(1).gameObject.GetComponent<Text>().text = username;

    clone.transform.GetChild(2).gameObject.GetComponent<Text>().text = p1score.ToString() + "-" + p2score.ToString();

    if(convertString(round) < 4){
	    clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "ROUND " + round;
        clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = new Color32(50,50,50,255);
        clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = false;
    }else{
        if(p1score > p2score){
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "YOU WON!";
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = Color.green;
            clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = true;
        }else if(p1score < p2score){
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "YOU LOST!";
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = Color.red;
            clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = true;
        }else{
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().text = "DRAW!";
            clone.transform.GetChild(3).gameObject.GetComponent<Text>().color = new Color32(50,50,50,255);
            clone.transform.GetChild(3).gameObject.GetComponent<Shadow>().enabled = false;
        }
    }



    

    GameObject newButton;

    if(turn == 1 || turn == 2){

    newButton = Instantiate(clone, new Vector3(10000,10000,0), Quaternion.identity, GameObject.Find("Active-Games").transform);



    if(newButton.transform.GetSiblingIndex() == 0){
        newButton.transform.localPosition = new Vector3(0, 0, 0);
    }else{
        GameObject lastButton = newButton.transform.parent.GetChild(newButton.transform.GetSiblingIndex() - 1).gameObject;

        newButton.transform.localPosition = new Vector3(0, lastButton.transform.localPosition.y - 200, 0);
    }
}else{

    newButton = Instantiate(clone, new Vector3(10000,10000,0), Quaternion.identity, GameObject.Find("Recent-Games").transform);



    if(newButton.transform.GetSiblingIndex() == 0){
        newButton.transform.localPosition = new Vector3(0, 0, 0);
    }else{
        GameObject lastButton = newButton.transform.parent.GetChild(newButton.transform.GetSiblingIndex() - 1).gameObject;

        newButton.transform.localPosition = new Vector3(0, lastButton.transform.localPosition.y - 200, 0);
    }

}

newButton.GetComponent<Button>().onClick.AddListener(() => HomePage.HP.openActiveGamePanel(username, newLetter, newNumber, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3));

}
}


public void createFriendButton(string username, string status, GameObject clone){

            

            string newLetter = username.Substring(0,1).ToUpper();
            string newNumber = "";

            if(one.Contains(newLetter)){
                newNumber = "1";
            }else if(two.Contains(newLetter)){
                newNumber = "2";
            }else if(three.Contains(newLetter)){
                newNumber = "3";
            }else if(four.Contains(newLetter)){
                newNumber = "4";
            }else if(five.Contains(newLetter)){
                newNumber = "5";
            }else if(eight.Contains(newLetter)){
                newNumber = "8";
            }else{
                newNumber = "10";
            }

            clone.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = newLetter;
            clone.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = newNumber;
            clone.transform.GetChild(1).gameObject.GetComponent<Text>().text = username;

            GameObject newButton;

            if(status == "1"){
                newButton = Instantiate(clone, new Vector3(10000,10000,0), Quaternion.identity, GameObject.Find("Current-List").transform);
            }else{
                newButton = Instantiate(clone, new Vector3(10000,10000,0), Quaternion.identity, GameObject.Find("Pending-List").transform);
            }



            if(newButton.transform.GetSiblingIndex() == 0){
                newButton.transform.localPosition = new Vector3(0, 0, 0);
            }else{
                GameObject lastButton = newButton.transform.parent.GetChild(newButton.transform.GetSiblingIndex() - 1).gameObject;

                newButton.transform.localPosition = new Vector3(0, lastButton.transform.localPosition.y - 200, 0);
            }

            newButton.GetComponent<Button>().onClick.AddListener(() => HomePage.HP.openFriendPanel(username));
    }

}

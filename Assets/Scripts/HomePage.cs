using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Functions;

public class HomePage : MonoBehaviour
{

    public static HomePage HP;
    private int stage;
    private int level;

    private FirebaseFunctions functions;

    public static bool addFriendPanelOpen;

    private int currentstage;
    private int currentlevel;
    private int stagepage;

    private Vector3 friendoriginal;
    private Vector3 singleoriginal;
    private Vector3 multioriginal;
    private Vector3 wheeloriginal;
    private Vector3 storeoriginal;
    private List<string> DL;
    private List<string> TL;
    private List<string> DW;
    private List<string> TW;

    private string currentStoreTab;

    public static List<string> one;
    public static List<string> two;
    public static List<string> three;
    public static List<string> four;
    public static List<string> five;
    public static List<string> eight;
    public static List<string> ten;

    public List<string> activeGames = new List<string>();
    public List<string> recentGames = new List<string>();
    public List<string> friends = new List<string>();
    public List<string> pendinginvites = new List<string>();
    public List<string> pendingrequests = new List<string>();
    // Start is called before the first frame update
    void Start()
    {

        HP = this;

        addFriendPanelOpen = false;

        functions = FirebaseFunctions.DefaultInstance;

        friendoriginal = GameObject.Find("Friends").transform.localPosition;
        singleoriginal = GameObject.Find("Single-Play").transform.localPosition;
        multioriginal = GameObject.Find("Multi-Play").transform.localPosition;
        wheeloriginal = GameObject.Find("Wheel-Page").transform.localPosition;
        storeoriginal = GameObject.Find("Store-Play").transform.localPosition;

        currentStoreTab = "Store-Coins";

        stage = PlayerPrefs.GetInt("Stage", 1);
        level = PlayerPrefs.GetInt("Level", 1);

        one = new List<string> {"A", "E", "I", "L", "N", "O", "R", "S", "T", "U"};

        two = new List<string> {"D", "G"};

        three = new List<string> {"B", "C", "M", "P"};

        four = new List<string> {"F", "H", "V", "W", "Y"};

        five = new List<string> {"K"};

        eight = new List<string> {"J", "X"};

        ten = new List<string> {"Q", "Z"};

        stagepage = stage;

        currentstage = stage;
        currentlevel = level;

        if(stage == 1){
            GameObject.Find("Prev-Page").GetComponent<Image>().enabled = false;
        }


        GameObject.Find("Multi-Level-Slider").GetComponent<Slider>().maxValue = (float)PlayerPrefs.GetInt("MultiLevel", 1) * 1000f;
        GameObject.Find("Multi-Level-Slider").GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("XP", 0);
        GameObject.Find("Multi-Level").GetComponent<Text>().text = PlayerPrefs.GetInt("MultiLevel",1).ToString();

        GameObject.Find("Profile-Button").GetComponent<Button>().onClick.AddListener(() => openFriendPanel(PlayerPrefs.GetString("Username")));

        setSingleLevels();

        StartCoroutine(rotateTrophyBackground());
        StartCoroutine(smallWheelRotate());
        StartCoroutine(coinShine());
        StartCoroutine(wheelSignFlash());

        if(PlayerPrefs.GetString("HomePageTab", "Single") == "Single"){

            GameObject.Find("Single-Play").transform.localPosition = new Vector3(0,0,0);

            GameObject.Find("Single-Play").transform.SetSiblingIndex(4);

            GameObject.Find("Bottom-Highlight").transform.localPosition = GameObject.Find("Single-Button").transform.localPosition;

            if(PlayerPrefs.GetInt("LevelCompleted", 0) == 1){
                if(currentlevel == 1){
                    currentstage--;
                    stagepage--;
                    currentlevel = 10;
                    setSingleLevels();
                    StartCoroutine(getTrophy());
                }else{
                    currentlevel--;
                    setSingleLevels();
                    StartCoroutine(nextLevel());
                }
            }
        }else{

            GameObject.Find("Multi-Play").transform.localPosition = new Vector3(0,0,0);

            GameObject.Find("Multi-Play").transform.SetSiblingIndex(4);

            GameObject.Find("Bottom-Highlight").transform.localPosition = GameObject.Find("Multi-Button").transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setSingleLevels(){

        GameObject.Find("Trophy-Background").GetComponent<Image>().enabled = false;
        GameObject.Find("Stage-Trophy").GetComponent<Image>().color = new Color32(255,255,255,130);
        GameObject.Find("Stage-Trophy-Text").GetComponent<Text>().text = currentstage.ToString();

        for(int i = 1; i <= 10; i++){
            GameObject.Find("Level-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-inactive");
            GameObject.Find("Level-"+i).GetComponent<Button>().interactable = false;

            if(i == 4 || i == 7 || i == 9){
            GameObject.Find("Path-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/curved-line");
            }else if(i != 10){
                GameObject.Find("Path-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/straight-line");
            }
        }

        GameObject.Find("Stage").GetComponent<Text>().text = "STAGE " + currentstage;

        for(int i = 1; i < currentlevel; i++){
            GameObject.Find("Level-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-completed-"+(currentstage % 5));
            GameObject.Find("Level-"+i).GetComponent<Button>().interactable = true;

            if(i == 4 || i == 7 || i == 9){
            GameObject.Find("Path-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/curved-line-completed");
            }else{
                GameObject.Find("Path-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/straight-line-completed");
            }
        }

        GameObject.Find("Level-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-active");

        GameObject.Find("Level-"+currentlevel).GetComponent<Button>().interactable = true;
    }

    public void setNewPage(){

        if(stagepage == stage){
            setSingleLevels();
        }else{

            GameObject.Find("Trophy-Background").GetComponent<Image>().enabled = true;
            GameObject.Find("Stage-Trophy").GetComponent<Image>().color = new Color32(255,255,255,255);
            GameObject.Find("Stage-Trophy-Text").GetComponent<Text>().text = stagepage.ToString();

        for(int i = 1; i <= 10; i++){
            GameObject.Find("Level-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-completed-"+(stagepage % 5));
            GameObject.Find("Level-"+i).GetComponent<Button>().interactable = true;

            if(i == 4 || i == 7 || i == 9){
            GameObject.Find("Path-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/curved-line-completed");
            }else if(i != 10){
                GameObject.Find("Path-"+i).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/straight-line-completed");
            }
        }

        GameObject.Find("Stage").GetComponent<Text>().text = "STAGE " + stagepage;

        }
    }

    public IEnumerator nextLevel(){

        PlayerPrefs.SetInt("LevelCompleted", 0);

        GameObject.Find("Level-Particles").transform.localPosition = GameObject.Find("Level-"+currentlevel).transform.localPosition;

        yield return new WaitForSeconds(.5f);

        GameObject.Find("Level-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-completed-"+(currentstage % 5).ToString());

        GameObject.Find("Level-Particles").GetComponent<ParticleSystem>().Play();

        GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("level-complete"));

        yield return new WaitForSeconds(1f);
        for(int i = 1; i <= 4; i++){
            if(currentlevel == 4 || currentlevel == 7 || currentlevel == 9){
                GameObject.Find("Path-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/curved-line-completed-"+i);
            }else{
                GameObject.Find("Path-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/straight-line-completed-"+i);
            }

            yield return new WaitForSeconds(.05f);
        }

        if(currentlevel == 4 || currentlevel == 7 || currentlevel == 9){
                GameObject.Find("Path-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/curved-line-completed");
            }else{
                GameObject.Find("Path-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/straight-line-completed");
            }

            currentlevel++;

            yield return new WaitForSeconds(.5f);

        GameObject.Find("Level-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-active");

        GameObject.Find("Level-"+currentlevel).GetComponent<Button>().interactable = true;

        
    }

    public IEnumerator getTrophy(){

        PlayerPrefs.SetInt("LevelCompleted", 0);

        yield return new WaitForSeconds(.5f);

        GameObject.Find("Level-"+currentlevel).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/level-completed-"+(currentstage % 5));

        yield return new WaitForSeconds(2f);

        GameObject.Find("Trophy-Background").GetComponent<Image>().enabled = true;
        GameObject.Find("Stage-Trophy").GetComponent<Image>().color = new Color32(255,255,255,255);

        GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("level-complete"));

        currentlevel = level;
        currentstage = stage;
        stagepage = stage;
        yield return new WaitForSeconds(3f);

        setSingleLevels();

        

    }

    public IEnumerator rotateTrophyBackground(){
        while(true){
            GameObject.Find("Trophy-Background").transform.Rotate(0f,0f,2f, Space.Self);
            yield return new WaitForSeconds(.05f);
        }
    }

    public void nextStagePage(){
        stagepage++;

        setNewPage();

        if(stagepage == stage){
            GameObject.Find("Next-Page").GetComponent<Image>().enabled = false;
        }else{
            GameObject.Find("Next-Page").GetComponent<Image>().enabled = true;
        }

        GameObject.Find("Prev-Page").GetComponent<Image>().enabled = true;
    }

    public void prevStagePage(){
        stagepage--;

        setNewPage();

        if(stagepage == 1){
            GameObject.Find("Prev-Page").GetComponent<Image>().enabled = false;
        }else{
            GameObject.Find("Prev-Page").GetComponent<Image>().enabled = true;
        }

        GameObject.Find("Next-Page").GetComponent<Image>().enabled = true;
    }

    public void changeTab(string tab){

        GameObject.Find(tab+"-Play").transform.SetSiblingIndex(4);
        PlayerPrefs.SetString("HomePageTab", tab);

        StartCoroutine(moveHighlight(tab,"Bottom"));
    }

    public IEnumerator moveHighlight(string tab, string bartype){

        float scale = 150f;
        GameObject newtab = GameObject.Find(tab+"-Button");
        GameObject highlight = GameObject.Find(bartype+"-Highlight");

        while(highlight.transform.localPosition.x != newtab.transform.localPosition.x){

            highlight.transform.localPosition = Vector2.MoveTowards(highlight.transform.localPosition, newtab.transform.localPosition, scale);

            yield return new WaitForSeconds(.05f);
        }

        highlight.transform.localPosition = newtab.transform.localPosition;
    }

    public void showSinglePanel(int levelnum){

        PlayerPrefs.SetInt("CurrentStage", stagepage);
        PlayerPrefs.SetInt("CurrentLevel", levelnum);

        GameObject.Find("Panel-Stage-Text").GetComponent<Text>().text = stagepage.ToString();
        GameObject.Find("Panel-Level-Text").GetComponent<Text>().text = levelnum.ToString();
        GameObject.Find("Panel-Goal-Text").GetComponent<Text>().text = "GOAL: "+((stagepage * 50) + 25).ToString();

        for(int i = 1; i <= 4; i++){
            for(int j = 1; j <= 4; j++){
                GameObject.Find("Tile-"+i+"-"+j).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back");
            }
        }

        if(levelnum == 1){
            DL = new List<string>{"Tile-1-1", "Tile-2-1", "Tile-3-1", "Tile-4-1"};
            TL = new List<string>{"Tile-1-4", "Tile-2-4", "Tile-3-4", "Tile-4-4"};
            DW = new List<string>{"Tile-1-2", "Tile-1-3"};
            TW = new List<string>{"Tile-4-2", "Tile-4-3"};
        }else if(levelnum == 2){
            DL = new List<string>{"Tile-2-2", "Tile-3-3"};
            TL = new List<string>{"Tile-2-3", "Tile-3-2"};
            DW = new List<string>{"Tile-1-1", "Tile-4-4"};
            TW = new List<string>{"Tile-1-4", "Tile-4-1"};
        }else if(levelnum == 3){
            TL = new List<string>{"Tile-2-2", "Tile-3-3"};
            DL = new List<string>{"Tile-2-3", "Tile-3-2"};
            TW = new List<string>{"Tile-1-1", "Tile-4-4"};
            DW = new List<string>{"Tile-1-4", "Tile-4-1"};
        }else if(levelnum == 4){
            DL = new List<string>{"Tile-2-2"};
            TL = new List<string>{"Tile-3-3"};
            DW = new List<string>{"Tile-1-1"};
            TW = new List<string>{"Tile-4-4"};
        }else if(levelnum == 5){
            DL = new List<string>{"Tile-3-2"};
            TL = new List<string>{"Tile-2-3"};
            DW = new List<string>{"Tile-1-4"};
            TW = new List<string>{"Tile-4-1"};
        }else if(levelnum == 6){
            DL = new List<string>{"Tile-1-1"};
            TL = new List<string>{"Tile-4-4"};
            DW = new List<string>{"Tile-1-4"};
            TW = new List<string>{"Tile-4-1"};
        }else if(levelnum == 7){
            DL = new List<string>{"Tile-2-2"};
            TL = new List<string>{"Tile-3-3"};
            DW = new List<string>{"Tile-2-3"};
            TW = new List<string>{"Tile-3-2"};
        }else if(levelnum == 8){
            DL = new List<string>{};
            TL = new List<string>{};
            DW = new List<string>{"Tile-2-3"};
            TW = new List<string>{"Tile-3-2"};
        }else if(levelnum == 9){
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

        StartCoroutine(panelOpen("Single-Play-Panel"));

    }

    public void setMultipliers(List<string> tiles, string multiplier){

        foreach(string tile in tiles){
        GameObject currentTile = GameObject.Find(tile);
            currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-"+multiplier);
        }
    }

    public void playSingleGame(){

        if(PlayerPrefs.GetInt("Lives", 5) > 0){
        PlayerPrefs.SetString("NextScene", "SinglePlay");

        SceneManager.LoadScene("LoadingScreen");
        }else{
            openNotEnough();
        }
    }

    public void playMultiGame(){
        PlayerPrefs.SetInt("CurrentMultiRandom", 0);
        PlayerPrefs.SetInt("Rematch", 0);
        PlayerPrefs.SetString("NextScene", "MultiPlay");

        SceneManager.LoadScene("LoadingScreen");
    }

    public async void playRematchGame(){
        PlayerPrefs.SetInt("CurrentMultiRandom", 0);
        PlayerPrefs.SetInt("Rematch", 1);
        PlayerPrefs.SetInt("CurrentMultiRound", 1);
        PlayerPrefs.SetString("NextScene", "MultiPlay");

        StartCoroutine(rematchLoading());

        await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("ActiveGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
      
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document(PlayerPrefs.GetString("CurrentMultiUsername")).SetAsync(new Dictionary<string, object>(){
      
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        //SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

        GameEndAd.scoreUpdated = true;

        await sendMessage("New Game To Play", " started a new game with you!", PlayerPrefs.GetString("CurrentMultiUsername"), PlayerPrefs.GetString("Username"));

        SceneManager.LoadScene("LoadingScreen");
    }

    public IEnumerator rematchLoading(){

        GameObject.Find("Loading-Panel").transform.SetAsLastSibling();

    while(true){

        for(int i = 1; i <= 8; i++){
				GameObject.Find("Loading-Panel-Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/loading-circle-"+i);

				yield return new WaitForSeconds(.2f);
			}        
    }
    }

    public void playRandomGame(){
        if(PlayerLists.activeGames.Contains("Unknown")){

            StartCoroutine(panelOpen("Waiting-Panel"));

        }else{
            PlayerPrefs.SetInt("CurrentMultiRandom", 1);
            PlayerPrefs.SetInt("Rematch", 0);
            PlayerPrefs.SetInt("CurrentMultiRound", 1);
            PlayerPrefs.SetString("NextScene", "MultiPlay");

            SceneManager.LoadScene("LoadingScreen");
        }
    }

    public void closeWaitingPanel(){

        StartCoroutine(panelClose("Waiting-Panel"));
    }

    public void closeSinglePanel(){

        StartCoroutine(panelClose("Single-Play-Panel"));

    }

    private int convertString(string stringToConvert){

        if(string.IsNullOrEmpty(stringToConvert)){
            return 0;
        }else{
            return Convert.ToInt32(stringToConvert);
        }
    }

    public async void openActiveGamePanel(string username, string newLetter, string newNumber, string p1r1, string p1r2, string p1r3, string p2r1, string p2r2, string p2r3){

        GameObject.Find("P2-Tile").GetComponent<Button>().onClick.RemoveAllListeners();

        GameObject.Find("P2-Tile").GetComponent<Button>().onClick.AddListener(() => HomePage.HP.closeActivePanel(true));
        GameObject.Find("P2-Tile").GetComponent<Button>().onClick.AddListener(() => HomePage.HP.openFriendPanel(username));

        if(newNumber == "0"){

            PlayerPrefs.SetString("CurrentMultiID", "");
            PlayerPrefs.SetString("CurrentMultiUsername", username);

        }else{

            DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(username).GetSnapshotAsync();

        Dictionary<string, object> documentDictionary = snapshot.ToDictionary();

        PlayerPrefs.SetString("CurrentMultiID", documentDictionary["FirebaseID"].ToString());
        PlayerPrefs.SetString("CurrentMultiUsername", username);
        newNumber = documentDictionary["MultiLevel"].ToString();

        }


        string firstLetter = PlayerPrefs.GetString("Username").Substring(0,1).ToUpper();
        string firstNumber = PlayerPrefs.GetInt("MultiLevel", 1).ToString();

        int p1total = convertString(p1r1) + convertString(p1r2) +convertString(p1r3);

        int p2total = 0;

        if(string.IsNullOrEmpty(p1r1)){
            p2total = 0;
        }else if(string.IsNullOrEmpty(p1r2)){
            p2total = convertString(p2r1);
        }else if(string.IsNullOrEmpty(p1r3)){
            p2total = convertString(p2r1) + convertString(p2r2);
        }else{
            p2total = convertString(p2r1) + convertString(p2r2) +convertString(p2r3);
        }
        // Fill Text at the top of Panel

        GameObject.Find("Active-Game-Text").GetComponent<Text>().text = "PLAYING WITH " + username;

        GameObject.Find("P1-Tile").transform.GetChild(0).gameObject.GetComponent<Text>().text = firstLetter;


        GameObject.Find("P1-Tile").transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = firstNumber;

        GameObject.Find("P1-Username").GetComponent<Text>().text = PlayerPrefs.GetString("Username");



        GameObject.Find("P2-Tile").transform.GetChild(0).gameObject.GetComponent<Text>().text = newLetter;

        GameObject.Find("P2-Tile").transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = newNumber;
        
        GameObject.Find("P2-Username").GetComponent<Text>().text = username;


        // Fill in scores

        GameObject.Find("P1-Total").GetComponent<Text>().text = p1total.ToString();
        GameObject.Find("P2-Total").GetComponent<Text>().text = p2total.ToString();

        GameObject.Find("P1R1").GetComponent<Text>().text = p1r1;
        GameObject.Find("P2R1").GetComponent<Text>().text = p2r1;

        GameObject.Find("P1R2").GetComponent<Text>().text = p1r2;
        GameObject.Find("P2R2").GetComponent<Text>().text = p2r2;

        GameObject.Find("P1R3").GetComponent<Text>().text = p1r3;
        GameObject.Find("P2R3").GetComponent<Text>().text = p2r3;

        GameObject.Find("P1-Total").GetComponent<Text>().color = new Color32(50,50,50,255);
        GameObject.Find("P2-Total").GetComponent<Text>().color = new Color32(50,50,50,255);

        //toggle Round 1 text

        if(String.IsNullOrEmpty(p1r1)){

            PlayerPrefs.SetInt("CurrentMultiRound", 1);

            GameObject.Find("Round-1-Group").transform.GetChild(3).gameObject.GetComponent<Text>().enabled = true;

            GameObject.Find("Active-Play-Button").GetComponent<Image>().enabled = true;

            

        }else if(!String.IsNullOrEmpty(p1r1) && String.IsNullOrEmpty(p2r1)){

            for(int i = 0; i < 3; i++){
                GameObject.Find("Round-1-Group").transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
            }
            GameObject.Find("Active-Game-Status").GetComponent<Text>().enabled = true;

        }else{

            if(convertString(p1r1) > convertString(p2r1)){
                GameObject.Find("P1R1").GetComponent<Text>().color = Color.green;
                GameObject.Find("P2R1").GetComponent<Text>().color = Color.red;

            }else if(convertString(p1r1) < convertString(p2r1)){
                GameObject.Find("P1R1").GetComponent<Text>().color = Color.red;
                GameObject.Find("P2R1").GetComponent<Text>().color = Color.green;
            }else{
                GameObject.Find("P1R1").GetComponent<Text>().color = new Color32(50,50,50,255);
                GameObject.Find("P2R1").GetComponent<Text>().color = new Color32(50,50,50,255);
            }

            for(int i = 0; i < 3; i++){
                GameObject.Find("Round-1-Group").transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
            }
        }

        // toggle round 2 text (if round 1 has been completed)

        if(!String.IsNullOrEmpty(p1r1) && !String.IsNullOrEmpty(p2r1)){

            if(String.IsNullOrEmpty(p1r2)){

                PlayerPrefs.SetInt("CurrentMultiRound", 2);

            GameObject.Find("Round-2-Group").transform.GetChild(3).gameObject.GetComponent<Text>().enabled = true;

            GameObject.Find("Active-Play-Button").GetComponent<Image>().enabled = true;

            }else if(!String.IsNullOrEmpty(p1r2) && String.IsNullOrEmpty(p2r2)){

                for(int i = 0; i < 3; i++){
                    GameObject.Find("Round-2-Group").transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
                }
                GameObject.Find("Active-Game-Status").GetComponent<Text>().enabled = true;

            }else{

                if(convertString(p1r2) > convertString(p2r2)){
                    GameObject.Find("P1R2").GetComponent<Text>().color = Color.green;
                    GameObject.Find("P2R2").GetComponent<Text>().color = Color.red;

                }else if(convertString(p1r2) < convertString(p2r2)){
                    GameObject.Find("P1R2").GetComponent<Text>().color = Color.red;
                    GameObject.Find("P2R2").GetComponent<Text>().color = Color.green;
                }else{
                    GameObject.Find("P1R2").GetComponent<Text>().color = new Color32(50,50,50,255);
                    GameObject.Find("P2R2").GetComponent<Text>().color = new Color32(50,50,50,255);
                }

                for(int i = 0; i < 3; i++){
                    GameObject.Find("Round-2-Group").transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
                }
            }
        }


        // toggle round 3 text (if round 1 & 2 have been completed)

        if(!String.IsNullOrEmpty(p1r1) && !String.IsNullOrEmpty(p2r1) && !String.IsNullOrEmpty(p1r2) && !String.IsNullOrEmpty(p2r2)){

            if(String.IsNullOrEmpty(p1r3)){

                PlayerPrefs.SetInt("CurrentMultiRound", 3);

            GameObject.Find("Round-3-Group").transform.GetChild(3).gameObject.GetComponent<Text>().enabled = true;

            GameObject.Find("Active-Play-Button").GetComponent<Image>().enabled = true;

            }else if(!String.IsNullOrEmpty(p1r3) && String.IsNullOrEmpty(p2r3)){

                for(int i = 0; i < 3; i++){
                    GameObject.Find("Round-3-Group").transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
                }
                GameObject.Find("Active-Game-Status").GetComponent<Text>().enabled = true;

            }else{

                if(convertString(p1r3) > convertString(p2r3)){
                    GameObject.Find("P1R3").GetComponent<Text>().color = Color.green;
                    GameObject.Find("P2R3").GetComponent<Text>().color = Color.red;

                }else if(convertString(p1r3) < convertString(p2r3)){
                    GameObject.Find("P1R3").GetComponent<Text>().color = Color.red;
                    GameObject.Find("P2R3").GetComponent<Text>().color = Color.green;
                }else{
                    GameObject.Find("P1R3").GetComponent<Text>().color = new Color32(50,50,50,255);
                    GameObject.Find("P2R3").GetComponent<Text>().color = new Color32(50,50,50,255);
                }


                if(p1total > p2total){
                    GameObject.Find("P1-Total").GetComponent<Text>().color = Color.green;
                    GameObject.Find("P2-Total").GetComponent<Text>().color = Color.red;

                }else if(p1total < p2total){
                    GameObject.Find("P1-Total").GetComponent<Text>().color = Color.red;
                    GameObject.Find("P2-Total").GetComponent<Text>().color = Color.green;
                }else{
                    GameObject.Find("P1-Total").GetComponent<Text>().color = new Color32(50,50,50,255);
                    GameObject.Find("P2-Total").GetComponent<Text>().color = new Color32(50,50,50,255);
                }


                for(int i = 0; i < 3; i++){
                    GameObject.Find("Round-3-Group").transform.GetChild(i).gameObject.GetComponent<Text>().enabled = true;
                }

                if(PlayerLists.activeGames.Contains(username)){

                    if(p1total > p2total){
                    GameObject.Find("Active-Recent-Status").GetComponent<Text>().color = Color.green;
                    GameObject.Find("Active-Recent-Status").GetComponent<Text>().text = PlayerPrefs.GetString("Username") + " WINS!";

                }else if(p1total < p2total){
                    GameObject.Find("Active-Recent-Status").GetComponent<Text>().color = Color.red;
                    GameObject.Find("Active-Recent-Status").GetComponent<Text>().text = username + " WINS!";
                }

                    GameObject.Find("Active-Recent-Status").GetComponent<Text>().enabled = true;
                }else{

                    PlayerPrefs.SetInt("CurrentMultiRound", 1);

                    GameObject.Find("Active-Rematch-Button").GetComponent<Image>().enabled = true;
                    GameObject.Find("Active-Rematch-Button-Text").GetComponent<Text>().enabled = true;
                    
                }
            }
        }

        StartCoroutine(panelOpen("Active-Game-Panel"));


    }

    public void closeActivePanel(bool instant){

        if(instant){
            GameObject.Find("Active-Game-Panel").transform.localScale = new Vector3(0f,0f,0f);
            GameObject.Find("Active-Game-Panel").transform.SetAsFirstSibling();
            GameObject.Find("Block-Background").transform.SetAsFirstSibling();
        }else{
        StartCoroutine(panelClose("Active-Game-Panel"));
        }

        for(int i = 1; i <= 3; i++){
            for(int j = 0; j <= 3; j++){
                GameObject.Find("Round-"+i+"-Group").transform.GetChild(j).gameObject.GetComponent<Text>().enabled = false;
            }
        }

        GameObject.Find("Active-Play-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Active-Rematch-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Active-Rematch-Button-Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Active-Recent-Status").GetComponent<Text>().enabled = false;
        GameObject.Find("Active-Game-Status").GetComponent<Text>().enabled = false;

        
    }


    public async void openFriendPanel(string username){

        string id;
        string newLetter;
        string level;
        string wins;
        string loses;
        string draws;
        string bestWord;
        string bestScore;
        string bestRound;

        if(username == PlayerPrefs.GetString("Username")){

            id = PlayerPrefs.GetString("ID");
            newLetter = PlayerPrefs.GetString("Username").Substring(0,1).ToUpper();
            level = PlayerPrefs.GetInt("MultiLevel",1).ToString();
            wins = PlayerPrefs.GetInt("Wins", 0).ToString();
            loses = PlayerPrefs.GetInt("Loses", 0).ToString();
            draws = PlayerPrefs.GetInt("Draws",0).ToString();
            bestWord = PlayerPrefs.GetString("BestWord", "");
            bestScore = PlayerPrefs.GetInt("BestWordScore",0).ToString();
            bestRound = PlayerPrefs.GetInt("BestRound", 0).ToString();
            

        }else{

        DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(username).GetSnapshotAsync();

        Dictionary<string, object> documentDictionary = snapshot.ToDictionary();



        id = documentDictionary["FirebaseID"].ToString();
        newLetter = username.Substring(0,1).ToUpper();
        level = documentDictionary["MultiLevel"].ToString();
        wins = documentDictionary["Wins"].ToString();
        loses = documentDictionary["Loses"].ToString();
        draws = documentDictionary["Draws"].ToString();
        bestWord = documentDictionary["BestWord"].ToString();
        bestScore = documentDictionary["BestWordScore"].ToString();
        bestRound = documentDictionary["BestRound"].ToString();

        }

        PlayerPrefs.SetString("CurrentMultiID", id);
        PlayerPrefs.SetString("CurrentMultiUsername", username);
        PlayerPrefs.SetInt("CurrentMultiRound", 1);

        GameObject.Find("Friend-Panel-Tile").transform.GetChild(0).gameObject.GetComponent<Text>().text = newLetter;
        GameObject.Find("Friend-Panel-Tile").transform.GetChild(1).GetChild(0).gameObject.GetComponent<Text>().text = level;

        GameObject.Find("Friend-Panel-Username-Text").GetComponent<Text>().text = username;

        GameObject.Find("Friend-Best-Word").GetComponent<Text>().text = bestWord + " (" + bestScore + ")";
        GameObject.Find("Friend-Best-Round").GetComponent<Text>().text = bestRound;

        GameObject.Find("Friend-Wins").GetComponent<Text>().text = wins;
        GameObject.Find("Friend-Loses").GetComponent<Text>().text = loses;
        GameObject.Find("Friend-Draws").GetComponent<Text>().text = draws;

        if(Convert.ToSingle(wins) + Convert.ToSingle(loses) == 0f){

        GameObject.Find("Wins-Slider").GetComponent<Slider>().maxValue = 2f;
        GameObject.Find("Loses-Slider").GetComponent<Slider>().maxValue = 2f;

        GameObject.Find("Wins-Slider").GetComponent<Slider>().value = 1f;
        GameObject.Find("Loses-Slider").GetComponent<Slider>().value = 1f;

        }else{

        GameObject.Find("Wins-Slider").GetComponent<Slider>().maxValue = Convert.ToSingle(wins) + Convert.ToSingle(loses);
        GameObject.Find("Loses-Slider").GetComponent<Slider>().maxValue = Convert.ToSingle(wins) + Convert.ToSingle(loses);

        GameObject.Find("Wins-Slider").GetComponent<Slider>().value = Convert.ToSingle(wins);
        GameObject.Find("Loses-Slider").GetComponent<Slider>().value = Convert.ToSingle(loses);

        }

        GameObject.Find("Friend-Panel-Friend-Id").GetComponent<Text>().text = "YOUR FRIEND ID: " + id;

        if(PlayerLists.activeGames.Contains(username) && !PlayerLists.pendingrequests.Contains(username)){
            GameObject.Find("Friend-Panel-Game-Status").GetComponent<Text>().enabled = true;
        }else if(PlayerLists.friends.Contains(username)){
            GameObject.Find("Friend-Panel-Play-Button").GetComponent<Image>().enabled = true;
        }else if(PlayerLists.pendingrequests.Contains(username)){
            GameObject.Find("Friend-Panel-Accept-Button").GetComponent<Image>().enabled = true;
            GameObject.Find("Friend-Panel-Accept-Button-Text").GetComponent<Text>().enabled = true;
            GameObject.Find("Friend-Panel-Deny-Button").GetComponent<Image>().enabled = true;
            GameObject.Find("Friend-Panel-Deny-Button-Text").GetComponent<Text>().enabled = true;
        }else if(PlayerLists.pendinginvites.Contains(username)){
            GameObject.Find("Friend-Panel-Request-Sent").GetComponent<Text>().enabled = true;
        }else if(id != PlayerPrefs.GetString("ID")){
            GameObject.Find("Friend-Panel-Add-Button").GetComponent<Image>().enabled = true;
            GameObject.Find("Friend-Panel-Add-Button-Text").GetComponent<Text>().enabled = true;
        }


        if(PlayerLists.friends.Contains(id)){
            GameObject.Find("Friend-Panel-Delete-Button").GetComponent<Image>().enabled = true;
            GameObject.Find("Friend-Panel-Delete-Button-Text").GetComponent<Text>().enabled = true;
        }else if(id == PlayerPrefs.GetString("ID")){
            GameObject.Find("Friend-Panel-Friend-Id").GetComponent<Text>().enabled = false;
            GameObject.Find("Friend-Panel-Settings-Button").GetComponent<Image>().enabled = true;
        }

        StartCoroutine(panelOpen("Friend-Panel"));

        
    }

    public void closeFriendPanel(){

        StartCoroutine(panelClose("Friend-Panel"));

        GameObject.Find("Friend-Panel-Delete-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Friend-Panel-Delete-Button-Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Friend-Id").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Game-Status").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Request-Sent").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Play-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Friend-Panel-Accept-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Friend-Panel-Accept-Button-Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Deny-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Friend-Panel-Deny-Button-Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Add-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Friend-Panel-Add-Button-Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Friend-Panel-Settings-Button").GetComponent<Image>().enabled = false;
    }

    public void openAddFriendPanel(){

        addFriendPanelOpen = true;

        StartCoroutine(panelOpen("Add-Friend-Panel"));
    }

    public void closeAddFriendPanel(){

        addFriendPanelOpen = false;

        StartCoroutine(panelClose("Add-Friend-Panel"));

    }

    public void closeAddFriendPanelInstant(){

        addFriendPanelOpen = false;

        GameObject.Find("Add-Friend-Panel").transform.SetAsFirstSibling();
        GameObject.Find("Add-Friend-Panel").transform.localScale = new Vector3(0f,0f,0f);

    }

    public void openFriendNotFound(){

        StartCoroutine(panelOpen("Friend-Not-Found-Panel"));
    }

    public void closeFriendNotFound(){

        StartCoroutine(panelClose("Friend-Not-Found-Panel"));

    }

    public void openNotEnoughCoins(){

        StartCoroutine(panelOpen("Not-Enough-Coins-Panel"));
    }

    public void closeNotEnoughCoins(){

        StartCoroutine(panelClose("Not-Enough-Coins-Panel"));
    }


    public void openAreYouSure(){

        StartCoroutine(panelOpen("Are-You-Sure-Panel"));
    }

    public void closeAreYouSure(){

        StartCoroutine(panelClose("Are-You-Sure-Panel"));
    }

    public void openSettings(){

        StartCoroutine(panelOpen("Settings-Panel"));
    }

    public void closeSettings(){

        StartCoroutine(panelClose("Settings-Panel"));
    }

    public void openMultiSettings(){

        StartCoroutine(panelOpen("Multi-Settings-Panel"));
    }

    public void closeMultiSettings(){

        StartCoroutine(panelClose("Multi-Settings-Panel"));
    }

    public void openSignOut(){

        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Are-You-Sure-Sign-Out-Panel").transform.SetAsLastSibling();
    }

    public void closeSignOut(){

        GameObject.Find("Are-You-Sure-Sign-Out-Panel").transform.SetAsFirstSibling();
        GameObject.Find("Multi-Settings-Panel").transform.SetAsLastSibling();
    }

    public void openLinkEmail(){

        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Link-Email-Panel").transform.SetAsLastSibling();
    }

    public void closeLinkEmail(){

        GameObject.Find("Link-Email-Panel").transform.SetAsFirstSibling();
        GameObject.Find("Multi-Settings-Panel").transform.SetAsLastSibling();

        GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
    }


    public void openNotEnough(){

        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Not-Enough-Panel").transform.SetAsLastSibling();
    }

    public void closeNotEnough(){

        GameObject.Find("Not-Enough-Panel").transform.SetAsFirstSibling();
        GameObject.Find("Single-Play-Panel").transform.SetAsLastSibling();
    }


    public void changeGameTab(string tab){

        string otherTab = "Recent";
        if(tab == "Recent"){
            otherTab = "Active";
            
        }
        StartCoroutine(moveHighlight(tab, "Toggle"));
        GameObject.Find(tab+"-Button-Text").GetComponent<Text>().color = Color.white;
        GameObject.Find(tab+"-Button-Text").GetComponent<Shadow>().enabled = true;

        GameObject.Find(otherTab+"-Button-Text").GetComponent<Text>().color = Color.black;
        GameObject.Find(otherTab+"-Button-Text").GetComponent<Shadow>().enabled = false;

        GameObject.Find(tab+"-Games").transform.SetSiblingIndex(3);
        GameObject.Find(otherTab+"-Games").transform.SetAsFirstSibling();

        GameObject.Find("Multi-Play").GetComponent<ScrollRect>().content = GameObject.Find(tab+"-Games").GetComponent<RectTransform>();
    }


    public void changeFriendsTab(string tab){

        string otherTab = "Pending";
        if(tab == "Pending"){
            otherTab = "Current";
        }
        StartCoroutine(moveHighlight(tab, "Friends"));
        GameObject.Find(tab+"-Button-Text").GetComponent<Text>().color = Color.white;
        GameObject.Find(tab+"-Button-Text").GetComponent<Shadow>().enabled = true;

        GameObject.Find(otherTab+"-Button-Text").GetComponent<Text>().color = Color.black;
        GameObject.Find(otherTab+"-Button-Text").GetComponent<Shadow>().enabled = false;

        GameObject.Find(tab+"-List").transform.SetSiblingIndex(3);
        GameObject.Find(otherTab+"-List").transform.SetAsFirstSibling();

        GameObject.Find("Friends").GetComponent<ScrollRect>().content = GameObject.Find(tab+"-List").GetComponent<RectTransform>();
    }

    public void changeStoreTab(string tab){

        StartCoroutine(moveHighlight(tab, "Store"));

        GameObject oldTab = GameObject.Find("Store-Play").transform.GetChild(5).gameObject;

        GameObject.Find(tab+"-List").transform.SetSiblingIndex(5);

        currentStoreTab = tab;
        StartCoroutine(storeTabImage(tab));

        if(oldTab != GameObject.Find(tab+"-List")){
            oldTab.transform.SetAsFirstSibling();
        }
    }

    public IEnumerator storeTabImage(string currentTab){

        GameObject currentImage = GameObject.Find(currentTab+"-Image");
        float scale = .02f;
        while(currentTab == currentStoreTab){

            while(currentImage.transform.localScale.y < 1.1f && currentTab == currentStoreTab){

                currentImage.transform.localScale += new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(.1f);
            }

            while(currentImage.transform.localScale.y > 1f && currentTab == currentStoreTab){

                currentImage.transform.localScale -= new Vector3(scale,scale,scale);
                yield return new WaitForSeconds(.1f);
            }
            
        }

        currentImage.transform.localScale = new Vector3(1f,1f,1f);
    }

    public void openPage(string page){
            StartCoroutine(pageSlideIn(page));
    }

    public IEnumerator pageSlideIn(string page){

        float scale = 150f;
         GameObject pagePanel = GameObject.Find(page);
         pagePanel.transform.SetSiblingIndex(4);
        while(pagePanel.transform.localPosition.x != 0){

            pagePanel.transform.localPosition = Vector3.MoveTowards(pagePanel.transform.localPosition, new Vector3(0,0,0), scale);

            yield return new WaitForSeconds(.05f);
        }

        pagePanel.transform.localPosition = new Vector3(0,0,0);

        if(page != "Single-Play"){
            
            
            GameObject.Find("Single-Play").transform.SetAsFirstSibling();

            if(page == "Wheel-Page"){
                GameObject.Find("Single-Play").transform.localPosition = storeoriginal;
            }else{
                GameObject.Find("Single-Play").transform.localPosition = singleoriginal;
            }
        }

        if(page != "Multi-Play"){

            GameObject.Find("Multi-Play").transform.SetAsFirstSibling();

            if(page == "Friends" || page == "Store-Play"){
                GameObject.Find("Multi-Play").transform.localPosition = singleoriginal;
            }else{
                GameObject.Find("Multi-Play").transform.localPosition = multioriginal;
            }
        }

        if(page != "Store-Play"){

            GameObject.Find("Store-Play").transform.SetAsFirstSibling();
            GameObject.Find("Store-Play").transform.localPosition = storeoriginal;

        }

        if(page != "Friends"){

            GameObject.Find("Friends").transform.SetAsFirstSibling();
            GameObject.Find("Friends").transform.localPosition = friendoriginal;

        }

        if(page != "Wheel-Page"){

            GameObject.Find("Wheel-Page").transform.SetAsFirstSibling();
            GameObject.Find("Wheel-Page").transform.localPosition = wheeloriginal;

        }

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

        if(panel == "Multi-Settings-Panel"){
            GameObject.Find("Block-Background").transform.SetAsLastSibling();
            GameObject.Find("Friend-Panel").transform.SetAsLastSibling();
        }
    }

    public IEnumerator smallWheelRotate(){

        while(true){
            GameObject.Find("Small-Wheel-Button").transform.Rotate(0f,0f,-15f, Space.Self);
            yield return new WaitForSeconds(.05f);
        }
    }

    public IEnumerator coinShine(){

        while(true){
            for(int i = 1; i <= 5; i++){
                GameObject.Find("Coin-Store-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/coin-store-"+i);
                yield return new WaitForSeconds(.1f);
            }

            GameObject.Find("Coin-Store-Button").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/coin-store-1");

            yield return new WaitForSeconds(1f);
        }
    }


    public IEnumerator wheelSignFlash(){

        while(true){
            for(int i = 1; i <= 2; i++){
                GameObject.Find("Wheel-Title").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/wheel-title-"+i);
                yield return new WaitForSeconds(.5f);
            }
        }
    }

    private Task<string> sendMessage(string title, string message, string firebaseid, string username) {
  // Create the arguments to the callable function.
  var data = new Dictionary<string, object>();
  data["Title"] = title;
  data["Message"] = message;
  data["FirebaseID"] = firebaseid;
  data["Username"] = username;
  
  return functions.GetHttpsCallable("sendMessage").CallAsync(data).ContinueWith((task) => {
    return "Completed";
  });
}
}

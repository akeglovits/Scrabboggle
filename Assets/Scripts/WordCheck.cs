using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordCheck : MonoBehaviour
{

    public static WordCheck WC;

    public static WordGameDict wgd;

    public static bool tornadoActive;
    public static bool swordActive;
    public static string word;
    public static int wordScore;
    public static int wordMultiplier;
    public static List<GameObject> tiles = new List<GameObject>();
    public static string bestWord;
    public static int bestScore;

    public static AudioClip correctWord;
    public static AudioClip wrongWord;
    // Start is called before the first frame update
    void Start()
    {

        WC = this;
        tornadoActive = false;
        swordActive = false;

        correctWord = Resources.Load<AudioClip>("word-correct");
        wrongWord = Resources.Load<AudioClip>("word-wrong");
    wgd = new WordGameDict("dictionary");

    word = "";
    wordScore = 0;
    wordMultiplier = 1;
    bestWord = "";
    bestScore = 0;

    
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Current-Word").GetComponent<Text>().text = word;

        
    }

    public static void addLetter(GameObject tile){

            string multiplier = tile.tag;
            string letter = tile.transform.GetChild(0).gameObject.GetComponent<Text>().text;
            int number = Int32.Parse(tile.transform.GetChild(1).gameObject.GetComponent<Text>().text);

            tiles.Add(tile);
            word = word + letter;

            if(multiplier == "DL"){
                wordScore += (2 * number);
            }else if(multiplier == "TL"){
                wordScore += (3 * number);
            }else if(multiplier == "DW"){
                wordScore += number;
                wordMultiplier *= 2;
            }else if(multiplier == "TW"){
                wordScore += number;
                wordMultiplier *= 3;
            }else{
                wordScore += number;
            }

            if(!swordActive && !tornadoActive){
                if(tiles.Count > 1){
                    addLine(tile, tiles[tiles.Count-2]);
                
                }else{
                    addLine(tile, tile);
                }
            }
    }

    public static void removeLetter(GameObject tile){

            string multiplier = tile.tag;
            string letter = tile.transform.GetChild(0).gameObject.GetComponent<Text>().text;
            int number = Int32.Parse(tile.transform.GetChild(1).gameObject.GetComponent<Text>().text);

            

            tiles.Remove(tile);
            word = word.Substring(0, word.Length - 1);

            if(multiplier == "DL"){
                wordScore -= (2 * number);
            }else if(multiplier == "TL"){
                wordScore -= (3 * number);
            }else if(multiplier == "DW"){
                wordScore -= number;
                wordMultiplier /= 2;
            }else if(multiplier == "TW"){
                wordScore -= number;
                wordMultiplier /= 3;
            }else{
                wordScore -= number;
            }

            removeLine(tile, tiles[tiles.Count-1]);

    }

    public static void addLine(GameObject currentTile, GameObject lastTile){

        currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-glow");

        if(GameObject.Find(currentTile.name+"-"+lastTile.name) != null){

            GameObject.Find(currentTile.name+"-"+lastTile.name).GetComponent<Image>().enabled = true;

        }else if(GameObject.Find(lastTile.name+"-"+currentTile.name) != null){

            GameObject.Find(lastTile.name+"-"+currentTile.name).GetComponent<Image>().enabled = true;

        }
    }

    public static void removeLine(GameObject currentTile, GameObject lastTile){


        if(currentTile.tag == "NONE"){
        currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back");
        }else{
            currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-"+currentTile.tag);
        }

        if(GameObject.Find(currentTile.name+"-"+lastTile.name) != null){

            GameObject.Find(currentTile.name+"-"+lastTile.name).GetComponent<Image>().enabled = false;

        }else if(GameObject.Find(lastTile.name+"-"+currentTile.name) != null){

            GameObject.Find(lastTile.name+"-"+currentTile.name).GetComponent<Image>().enabled = false;

        }
    }


    public static void checkValidWord(){

        if(wgd.CheckWord(word, 2) || tornadoActive || swordActive){
            
            GameplayFunctions.totalScore += (wordScore * wordMultiplier);

            GameplayFunctions.GPF.flashScoreCall(wordScore * wordMultiplier);


                GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(correctWord);

            if((wordScore * wordMultiplier) > bestScore && !swordActive && !tornadoActive){
                bestScore = (wordScore * wordMultiplier);
                bestWord = word;
            }

            if((wordScore * wordMultiplier) > PlayerPrefs.GetInt("BestWordScore", 0) && !swordActive && !tornadoActive){
                PlayerPrefs.SetInt("BestWordScore", (wordScore * wordMultiplier));
                PlayerPrefs.SetString("BestWord", word);
            }

            word = "";
            wordScore = 0;
            wordMultiplier = 1;

            NewLetters.NL.fillLetters(tiles);

        }else{

            word = "";
            wordScore = 0;
            wordMultiplier = 1;

            if(tiles.Count > 1){

                if(PlayerPrefs.GetInt("Vibrate", 1) == 1){
                    Handheld.Vibrate();
                }

                GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(wrongWord);
            }
        }

        for(int i = 1; i <= tiles.Count; i++){

            if(i == tiles.Count){
                removeLine(tiles[tiles.Count - i], tiles[tiles.Count - i]);
            }else{
                removeLine(tiles[tiles.Count - i], tiles[tiles.Count - i - 1]);
            }
        }

        tiles.Clear();
    }


    public void clockButton(){

        if(GameplayFunctions.timer > 0){
            GameplayFunctions.timer += 30;
            PlayerPrefs.SetInt("Clock", PlayerPrefs.GetInt("Clock", 3) - 1);
        }
    }

    public void swordButton(){

        swordActive = true;
    }

    public void swordSlice(string tile){

        addLetter(GameObject.Find(tile));

        StartCoroutine(swordSliceAnimation(tile));
    }


    public IEnumerator swordSliceAnimation(string tile){

        List<string> tilesTemp = new List<string>();

        tilesTemp.Add(tile);

       string row = tile.Substring(5,1);
       string column = tile.Substring(7,1);

       GameObject.Find("Sword-Slice-Panel").transform.SetAsLastSibling();

        byte alpha = 0;

        while(alpha < 255f){

            GameObject.Find("Sword-Slice-Panel").GetComponent<Image>().color = new Color32(255,255,255,alpha);

            alpha += 51;

            yield return new WaitForSeconds(.1f);
        }

        GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("sword-noise"));

        for(int i = 1; i <= 4; i++){
            string tilename = "Tile-"+row+"-"+i;

            if(tilename != tile){
                tilesTemp.Add(tilename);
                addLetter(GameObject.Find(tilename));
            }

            GameObject.Find(tilename+"-Row-Slice").GetComponent<Image>().enabled = true;
        }

        yield return new WaitForSeconds(.05f);


        while(alpha > 0f){

            GameObject.Find("Sword-Slice-Panel").GetComponent<Image>().color = new Color32(255,255,255,alpha);

            alpha -= 51;

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.5f);


        while(alpha < 255f){

            GameObject.Find("Sword-Slice-Panel").GetComponent<Image>().color = new Color32(255,255,255,alpha);

            alpha += 51;

            yield return new WaitForSeconds(.1f);
        }

        GameObject.Find("Game-Sounds").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("sword-noise"));

        for(int i = 1; i <= 4; i++){
            string tilename = "Tile-"+i+"-"+column;

            if(tilename != tile){
                tilesTemp.Add(tilename);
                addLetter(GameObject.Find(tilename));
            }

            GameObject.Find(tilename+"-Column-Slice").GetComponent<Image>().enabled = true;
        }

        yield return new WaitForSeconds(.05f);


        while(alpha > 0f){

            GameObject.Find("Sword-Slice-Panel").GetComponent<Image>().color = new Color32(255,255,255,alpha);

            alpha -= 51;

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(.5f);

        foreach(string slicedtile in tilesTemp){

            GameObject.Find(slicedtile+"-Row-Slice").GetComponent<Image>().enabled = false;
            GameObject.Find(slicedtile+"-Column-Slice").GetComponent<Image>().enabled = false;
        }

        tilesTemp.Clear();

        checkValidWord();

        GameObject.Find("Sword-Slice-Panel").transform.SetAsFirstSibling();

        

        yield return new WaitForSeconds(.02f);

        swordActive = false;

    }

    public void tornadoButton(){

        tornadoActive = true;
        PlayerPrefs.SetInt("Tornado", PlayerPrefs.GetInt("Tornado", 3) - 1);

        StartCoroutine(tornadoSpin());

    }

    public IEnumerator tornadoSpin(){

        GameObject.Find("Game-Sounds").GetComponent<AudioSource>().Play();

        for(int i = 1; i <= 4; i++){
            for(int j = 1; j <= 4; j++){
                addLetter(GameObject.Find("Tile-"+i+"-"+j));
            }
        }
        float size = 1f;
        while(size > .05f){

            for(int i = 1; i <= 4; i++){
                for(int j = 1; j <= 4; j++){
                GameObject currentTile = GameObject.Find("Tile-"+i+"-"+j);
                currentTile.transform.localScale -= new Vector3(.05f, .05f, .05f);
                currentTile.transform.Rotate(0f,0f,30f, Space.Self);
            
            }
        }

        size -= .05f;
        yield return new WaitForSeconds(.05f);

        }

        for(int i = 1; i <= 4; i++){
                for(int j = 1; j <= 4; j++){
                GameObject currentTile = GameObject.Find("Tile-"+i+"-"+j);
                currentTile.transform.localRotation = Quaternion.identity;
            
            }
        }

        GameObject.Find("Game-Sounds").GetComponent<AudioSource>().Stop();

        checkValidWord();

        yield return new WaitForSeconds(.02f);

        tornadoActive = false;
    }


}

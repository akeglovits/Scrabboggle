using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Instructions : MonoBehaviour
{

    public static Instructions I;
    public static bool instructionsOpen;

    public GameObject pointer;
    public Vector3 pointerOriginal;

    public GameObject pointer2;

    public GameObject pointer3;

    private int currentPanel;

    public GameObject instruction1;
    public GameObject instruction2;
    public GameObject instruction3;
    public GameObject instruction4;
    public GameObject instruction5;
    public GameObject instruction6;
    public GameObject instruction7;

    //private List<bool> panelRunning = new List<bool>();
    private List<bool> panelVisible = new List<bool>();
    // Start is called before the first frame update
    void Start()
    {
        I = this;
        instructionsOpen = false;
        pointerOriginal = pointer.transform.localPosition;

        currentPanel = 1;

        for(int i = 0; i < 7; i++){

            if(i == 0){
                //panelRunning.Add(true);
                panelVisible.Add(true);
            }else{
                //panelRunning.Add(false);
                panelVisible.Add(false);
            }
        }

        if(PlayerPrefs.GetInt("FirstGame", 0) == 0){
            openInstructions();
            PlayerPrefs.SetInt("FirstGame",1);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public IEnumerator Instruction1wordSwipe(){

        //panelRunning[0] = true;

        pointer.transform.localPosition = pointerOriginal;
        pointer.transform.localScale = new Vector3(1f,1f,1f);

        GameObject.Find("Instruction-1-Tile-2-2").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back");
        GameObject.Find("Instruction-1-Tile-2-3").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back");
        GameObject.Find("Instruction-1-Tile-1-3").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back");
        GameObject.Find("Instruction-1-Tile-1-4").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back");

        GameObject.Find("Instruction-1-Current-Word").GetComponent<Text>().text = "";

        GameObject nextTile = GameObject.Find("Instruction-1-Tile-2-3");
        while(pointer.transform.localScale.y > .7f /*&& panelVisible[0]*/){
            
            pointer.transform.localScale -= new Vector3(.1f,.1f,.1f);
            yield return new WaitForSeconds(.02f);
        }

        GameObject.Find("Instruction-1-Tile-2-2").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-glow");

        GameObject.Find("Instruction-1-Current-Word").GetComponent<Text>().text = "F";

        yield return new WaitForSeconds(.05f);

        while(pointer.transform.localPosition.x < nextTile.transform.localPosition.x  /*&& panelVisible[0]*/){

            pointer.transform.localPosition = Vector2.MoveTowards(pointer.transform.localPosition, new Vector2(nextTile.transform.localPosition.x, pointer.transform.localPosition.y), 50f);

            yield return new WaitForSeconds(.02f);

        }

        nextTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-glow");

        GameObject.Find("Instruction-1-Current-Word").GetComponent<Text>().text = "FL";

        nextTile = GameObject.Find("Instruction-1-Tile-1-3");


        while(pointer.transform.localPosition.y < nextTile.transform.localPosition.y /*&& panelVisible[0]*/){

            pointer.transform.localPosition = Vector2.MoveTowards(pointer.transform.localPosition, new Vector2(pointer.transform.localPosition.x, nextTile.transform.localPosition.y), 50f);

            yield return new WaitForSeconds(.02f);

        }

        nextTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-glow");

        GameObject.Find("Instruction-1-Current-Word").GetComponent<Text>().text = "FLA";

        nextTile = GameObject.Find("Instruction-1-Tile-1-4");

        while(pointer.transform.localPosition.x < nextTile.transform.localPosition.x /*&& panelVisible[0]*/){

            pointer.transform.localPosition = Vector2.MoveTowards(pointer.transform.localPosition, new Vector2(nextTile.transform.localPosition.x, pointer.transform.localPosition.y), 50f);

            yield return new WaitForSeconds(.02f);

        }

        nextTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-glow");

        GameObject.Find("Instruction-1-Current-Word").GetComponent<Text>().text = "FLAT";

        yield return new WaitForSeconds(.2f);

        if(panelVisible[0]){
            StartCoroutine(Instruction1wordSwipe());
        }else{
            //panelRunning[0] = false;
        }

    }

    public IEnumerator Instruction2DoubleClick(){

        //panelRunning[2] = true;

        GameObject tile = GameObject.Find("Instruction-3-Tile-1");
        GameObject letter = tile.transform.GetChild(0).gameObject;
        GameObject number = tile.transform.GetChild(1).gameObject;

        pointer3.transform.localScale = new Vector3(1f,1f,1f);

        tile.transform.localScale = new Vector3(1f,1f,1f);
        letter.GetComponent<Text>().text = "Q";
        number.GetComponent<Text>().text = "10";

        yield return new WaitForSeconds(.2f);


        while(pointer3.transform.localScale.y > .5f /*&& panelVisible[2]*/){
            
            pointer3.transform.localScale -= new Vector3(.2f,.2f,.2f);
            yield return new WaitForSeconds(.01f);
        }

        while(pointer3.transform.localScale.y < 1f /*&& panelVisible[2]*/){
            
            pointer3.transform.localScale += new Vector3(.2f,.2f,.2f);
            yield return new WaitForSeconds(.01f);
        }

        while(pointer3.transform.localScale.y > .5f /*&& panelVisible[2]*/){
            
            pointer3.transform.localScale -= new Vector3(.2f,.2f,.2f);
            yield return new WaitForSeconds(.01f);
        }

        while(pointer3.transform.localScale.y < 1f /*&& panelVisible[2]*/){
            
            pointer3.transform.localScale += new Vector3(.2f,.2f,.2f);
            yield return new WaitForSeconds(.01f);
        }


        while(tile.transform.localScale.y > 0f /*&& panelVisible[2]*/){
            
            tile.transform.localScale -= new Vector3(.2f,.2f,.2f);
            yield return new WaitForSeconds(.02f);
        }

        letter.GetComponent<Text>().text = "S";
        number.GetComponent<Text>(). text = "1";


        while(tile.transform.localScale.y < 1f /*&& panelVisible[2]*/){
            
            tile.transform.localScale += new Vector3(.2f,.2f,.2f);
            yield return new WaitForSeconds(.02f);
        }

        yield return new WaitForSeconds(.5f);


        if(panelVisible[2]){
            StartCoroutine(Instruction2DoubleClick());
        }else{
            //panelRunning[2] = false;
        }


    }

    public IEnumerator Instruction4Sword(){

        //panelRunning[4] = true;

        for(int i = 1; i <= 4; i++){
            GameObject.Find("Instruction-4-Tile-2-"+i+"-Row-Slice").GetComponent<Image>().enabled = false;
            GameObject.Find("Instruction-4-Tile-"+i+"-3-Column-Slice").GetComponent<Image>().enabled = false;

            GameObject.Find("Instruction-4-Tile-2-"+i).transform.localScale = new Vector3(1f,1f,1f);
            GameObject.Find("Instruction-4-Tile-"+i+"-3").transform.localScale = new Vector3(1f,1f,1f);
        }

        pointer2.transform.localScale = new Vector3(1f,1f,1f);

        yield return new WaitForSeconds(.2f);

        while(pointer2.transform.localScale.y > .5f /*&& panelVisible[4]*/){
            
            pointer2.transform.localScale -= new Vector3(.1f,.1f,.1f);
            yield return new WaitForSeconds(.02f);
        }

        while(pointer2.transform.localScale.y < 1f /*&& panelVisible[4]*/){
            
            pointer2.transform.localScale += new Vector3(.1f,.1f,.1f);
            yield return new WaitForSeconds(.02f);
        }



        for(int i = 1; i <= 4; i++){
            GameObject.Find("Instruction-4-Tile-2-"+i+"-Row-Slice").GetComponent<Image>().enabled = true;
        }

        yield return new WaitForSeconds(.2f);

        for(int i = 1; i <= 4; i++){
            GameObject.Find("Instruction-4-Tile-"+i+"-3-Column-Slice").GetComponent<Image>().enabled = true;
        }

        yield return new WaitForSeconds(.2f);

        float scale = 1f;
        while(scale > 0f /*&& panelVisible[4]*/){

            for(int i = 1; i <= 4; i++){
                GameObject.Find("Instruction-4-Tile-2-"+i).transform.localScale -= new Vector3(.2f,.2f,.2f);
                GameObject.Find("Instruction-4-Tile-"+i+"-3").transform.localScale -= new Vector3(.2f,.2f,.2f);
            }

            scale -= .2f;
            yield return new WaitForSeconds(.02f);
        }


        if(panelVisible[4]){
            StartCoroutine(Instruction4Sword());
        }else{
            //panelRunning[4] = false;
        }


    }


    public IEnumerator Instruction5Tornado(){

        //panelRunning[5] = true;

        for(int i = 1; i <= 4; i++){
                for(int j = 1; j <= 4; j++){
                GameObject currentTile = GameObject.Find("Instruction-5-Tile-"+i+"-"+j);
                currentTile.transform.localScale = new Vector3(1f, 1f, 1f);
                currentTile.transform.localRotation = Quaternion.identity;
            
            }
        }

        yield return new WaitForSeconds(.5f);

        float size = 1f;
        while(size > .05f){

            for(int i = 1; i <= 4; i++){
                for(int j = 1; j <= 4; j++){
                GameObject currentTile = GameObject.Find("Instruction-5-Tile-"+i+"-"+j);
                currentTile.transform.localScale -= new Vector3(.05f, .05f, .05f);
                currentTile.transform.Rotate(0f,0f,30f, Space.Self);
            
            }
        }

        size -= .05f;
        yield return new WaitForSeconds(.05f);

        }

        for(int i = 1; i <= 4; i++){
                for(int j = 1; j <= 4; j++){
                GameObject currentTile = GameObject.Find("Instruction-5-Tile-"+i+"-"+j);
                currentTile.transform.localRotation = Quaternion.identity;
            
            }
        }

        if(panelVisible[5]){
            StartCoroutine(Instruction5Tornado());
        }else{
            //panelRunning[5] = false;
        }

        
    }

    public void openInstructions(){


        if(!instructionsOpen){
            
            instructionsOpen = true;
            GameplayFunctions.timerGo = false;
            StartCoroutine(panelOpen("Instruction-Panel"));

        }
        

    }

    public void closeInstructions(){

        if(PlayerPrefs.GetInt("FirstGame", 0) == 0){
            PlayerPrefs.SetInt("FirstGame",1);
        }

        StopAllCoroutines();

        StartCoroutine(panelClose("Instruction-Panel"));
        
    }

    public void nextPanel(){

        StopAllCoroutines();
        if(currentPanel == 1){
            instruction1.SetActive(false);
            panelVisible[0] = false;
            instruction2.SetActive(true);
            panelVisible[1] = true;
            currentPanel++;

            GameObject.Find("Instruction-Back").GetComponent<Image>().enabled = true;
        }else if(currentPanel == 2){

            //if(!panelRunning[2]){
            instruction2.SetActive(false);
            panelVisible[1] = false;
            instruction3.SetActive(true);
            panelVisible[2] = true;
            currentPanel++;

            StartCoroutine(Instruction2DoubleClick());
            //}

        }else if(currentPanel == 3){

            instruction3.SetActive(false);
            panelVisible[2] = false;
            instruction4.SetActive(true);
            panelVisible[3] = true;

            currentPanel++;
            

        }else if(currentPanel == 4){

            //if(!panelRunning[4]){
            instruction4.SetActive(false);
            panelVisible[3] = false;
            instruction5.SetActive(true);
            panelVisible[4] = true;

            StartCoroutine(Instruction4Sword());
            currentPanel++;
            //}

        }else if(currentPanel == 5){

            //if(!panelRunning[5]){
            instruction5.SetActive(false);
            panelVisible[4] = false;
            instruction6.SetActive(true);
            panelVisible[5] = true;

            StartCoroutine(Instruction5Tornado());
            currentPanel++;
            //}

        }else{
            instruction6.SetActive(false);
            panelVisible[5] = false;
            instruction7.SetActive(true);
            panelVisible[6] = true;

            GameObject.Find("Instruction-Next").GetComponent<Image>().enabled = false;

            currentPanel++;

        }

        GameObject.Find("Instruction-Page").GetComponent<Text>().text = currentPanel.ToString()+"/7";
    }


    public void backPanel(){

        StopAllCoroutines();
        
        if(currentPanel == 2){

            //if(!panelRunning[0]){
            instruction2.SetActive(false);
            panelVisible[1] = false;
            instruction1.SetActive(true);
            panelVisible[0] = true;
            currentPanel--;

            StartCoroutine(Instruction1wordSwipe());

            GameObject.Find("Instruction-Back").GetComponent<Image>().enabled = false;

            //}

        }else if(currentPanel == 3){
            instruction3.SetActive(false);
            panelVisible[2] = false;
            instruction2.SetActive(true);
            panelVisible[1] = true;
            currentPanel--;

        }else if(currentPanel == 4){

            //if(!panelRunning[2]){
            instruction4.SetActive(false);
            panelVisible[3] = false;
            instruction3.SetActive(true);
            panelVisible[2] = true;

            currentPanel--;

            StartCoroutine(Instruction2DoubleClick());
            //}

        }else if(currentPanel == 5){

            instruction5.SetActive(false);
            panelVisible[4] = false;
            instruction4.SetActive(true);
            panelVisible[3] = true;

            currentPanel--;
            

        }else if(currentPanel == 6){

            //if(!panelRunning[4]){
            instruction6.SetActive(false);
            panelVisible[5] = false;
            instruction5.SetActive(true);
            panelVisible[4] = true;

            StartCoroutine(Instruction4Sword());
            currentPanel--;
            //}

        }else{

            //if(!panelRunning[5]){
            instruction7.SetActive(false);
            panelVisible[6] = false;
            instruction6.SetActive(true);
            panelVisible[5] = true;

            StartCoroutine(Instruction5Tornado());

            GameObject.Find("Instruction-Next").GetComponent<Image>().enabled = true;

            currentPanel--;
            //}
        }

        GameObject.Find("Instruction-Page").GetComponent<Text>().text = currentPanel.ToString()+"/7";
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

        StartCoroutine(Instruction1wordSwipe());
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

        instruction1.SetActive(true);
        instruction2.SetActive(false);
        instruction3.SetActive(false);
        instruction4.SetActive(false);
        instruction5.SetActive(false);
        instruction6.SetActive(false);

        for(int i = 0; i < 6; i++){
            if(i == 0){
                panelVisible[i] = true;
            }else{
                panelVisible[i] = false;
            }
        }

        currentPanel = 1;

        GameObject.Find("Instruction-Page").GetComponent<Text>().text = "1/7";
        GameObject.Find("Instruction-Next").GetComponent<Image>().enabled = true;
        GameObject.Find("Instruction-Back").GetComponent<Image>().enabled = false;

        GameplayFunctions.GPF.startTimer();

        /*while(panelRunning[0] || panelRunning[2] || panelRunning[4] || panelRunning[5]){
            yield return new WaitForSeconds(.01f);
        }*/

        instructionsOpen = false;
    }
}

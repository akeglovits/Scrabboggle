using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreateWord : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerUpHandler, IPointerClickHandler
{

    private bool dragging;
    private int row;
    private int column;

    private float lastClick;
    // Start is called before the first frame update
    void Start()
    {
        dragging = false;
        lastClick = Time.time;
        row = Int32.Parse(this.gameObject.name.Substring(5,1));
        column = Int32.Parse(this.gameObject.name.Substring(7,1));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData){

        if(WordCheck.swordActive){
            WordCheck.WC.swordSlice(this.gameObject.name);
            PlayerPrefs.SetInt("Sword", PlayerPrefs.GetInt("Sword", 3) - 1);
        }else if(Time.time - lastClick < .2f){
            NewLetters.NL.fillLetters(new List<GameObject>{this.gameObject});
        }else{
            lastClick = Time.time;
        }
    }

    public void OnPointerDown(PointerEventData eventData){

        if(!GameplayFunctions.creatingWord){
        GameplayFunctions.creatingWord = true;
        dragging = true;
        WordCheck.addLetter(this.gameObject);
        }
    }

    public void OnPointerUp(PointerEventData eventData){
        
        if(dragging){
        WordCheck.checkValidWord();
        GameplayFunctions.creatingWord = false;
        dragging = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData){

        if(GameplayFunctions.creatingWord){
        if(WordCheck.tiles.Count > 1){
            if(WordCheck.tiles[WordCheck.tiles.Count - 2] == this.gameObject){

                WordCheck.removeLetter(WordCheck.tiles[WordCheck.tiles.Count - 1]);
            
            }else if(!WordCheck.tiles.Contains(this.gameObject)){
                int lastRow =  Int32.Parse(WordCheck.tiles[WordCheck.tiles.Count - 1].name.Substring(5,1));
                int lastCol =  Int32.Parse(WordCheck.tiles[WordCheck.tiles.Count - 1].name.Substring(7,1));


                if((lastRow == row || lastRow == row + 1 || lastRow == row - 1) && (lastCol == column || lastCol == column + 1 || lastCol == column - 1)){

                   WordCheck.addLetter(this.gameObject);
                }
            }
        }else if (WordCheck.tiles.Count == 1){

            if(!WordCheck.tiles.Contains(this.gameObject)){
                int lastRow =  Int32.Parse(WordCheck.tiles[WordCheck.tiles.Count - 1].name.Substring(5,1));
                int lastCol =  Int32.Parse(WordCheck.tiles[WordCheck.tiles.Count - 1].name.Substring(7,1));


                if((lastRow == row || lastRow == row + 1 || lastRow == row - 1) && (lastCol == column || lastCol == column + 1 || lastCol == column - 1)){

                   WordCheck.addLetter(this.gameObject);
                }
            }
        }else{
                WordCheck.addLetter(this.gameObject);
        }
        }
    }

}

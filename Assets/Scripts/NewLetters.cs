using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewLetters : MonoBehaviour
{

    public static NewLetters NL;

    public static List<string> alphabet;
    public static List<string> one;
    public static List<string> two;
    public static List<string> three;
    public static List<string> four;
    public static List<string> five;
    public static List<string> eight;
    public static List<string> ten;
    public static List<string> scrabbleBag;

    public static List<GameObject> firstFill = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        NL = this;

        scrabbleBag = new List<string> {"A", "A", "A", "A", "A", "A", "A", "A", "A", "B", "B", "C", "C", "D", "D", "D", "D", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "E", "F", "F", "G", "G", "G", "H", "H", "I", "I", "I", "I", "I", "I", "I", "I", "I", "J", "K", "L", "L", "L", "L", "M", "M", "N", "N", "N", "N", "N", "N", "O", "O", "O", "O", "O", "O", "O", "O", "P", "P", "Q", "R", "R", "R", "R", "R", "R", "S", "S", "S", "S", "S", "S", "T", "T", "T", "T", "T", "T", "U", "U", "U", "U", "U", "U", "V", "V", "W", "W", "X", "Y", "Y", "Z"};

        alphabet = new List<string>(scrabbleBag);

        one = new List<string> {"A", "E", "I", "L", "N", "O", "R", "S", "T", "U"};

        two = new List<string> {"D", "G"};

        three = new List<string> {"B", "C", "M", "P"};

        four = new List<string> {"F", "H", "V", "W", "Y"};

        five = new List<string> {"K"};

        eight = new List<string> {"J", "X"};

        ten = new List<string> {"Q", "Z"};

        firstFill = new List<GameObject>();

        for(int i = 1; i <= 4; i++){
            for(int j = 1; j <= 4; j++){
                firstFill.Add(GameObject.Find("Tile-"+i+"-"+j));
            }
        }

        fillLetters(firstFill);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void refreshBoard(){

        fillLetters(firstFill);
    }

    public void fillLetters(List<GameObject> tiles){

        foreach (GameObject tile in tiles)
        {
            string newLetter = "";
            string newNumber = "0";

            int random = Random.Range(0,alphabet.Count);
            
                newLetter = alphabet[random];

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

            StartCoroutine(changeTile(tile, newLetter, newNumber));
            

            alphabet.RemoveAt(random);

            if(alphabet.Count == 0){
                alphabet = new List<string>(scrabbleBag);
            }

        }
    }

    public IEnumerator changeTile(GameObject tile, string letter, string number){

        float scale = .5f;
        while(tile.transform.localScale.z > 0){
            tile.transform.localScale -= new Vector3(scale,scale,scale);

            yield return new WaitForSeconds(.02f);
        }

        tile.transform.GetChild(0).gameObject.GetComponent<Text>().text = letter;
        tile.transform.GetChild(1).gameObject.GetComponent<Text>().text = number;

        yield return new WaitForSeconds(.02f);

        while(tile.transform.localScale.z < 1){
            tile.transform.localScale += new Vector3(scale,scale,scale);

            yield return new WaitForSeconds(.02f);
        }

        tile.transform.localScale = new Vector3(1,1,1);
    }
}

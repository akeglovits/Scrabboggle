using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{

    public static string word;
    public static List<GameObject> tiles = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        word = "";
        StartCoroutine(loadSceneAsync());
    }

    // Update is called once per frame
       void Update()
    {
        GameObject.Find("Current-Word").GetComponent<Text>().text = word;
    }

    public void addLetter(GameObject tile){

        string letter = tile.transform.GetChild(0).gameObject.GetComponent<Text>().text;

            tiles.Add(tile);
            word = word + letter;

            if(tiles.Count > 1){
                addLine(tile, tiles[tiles.Count-2]);
                
            }else{
                addLine(tile, tile);
            }
    }

    public void addLine(GameObject currentTile, GameObject lastTile){

        currentTile.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/tile-back-glow");

        if(GameObject.Find(currentTile.name+"-"+lastTile.name) != null){

            GameObject.Find(currentTile.name+"-"+lastTile.name).GetComponent<Image>().enabled = true;

        }else if(GameObject.Find(lastTile.name+"-"+currentTile.name) != null){

            GameObject.Find(lastTile.name+"-"+currentTile.name).GetComponent<Image>().enabled = true;

        }
    }

    public void removeLine(GameObject currentTile, GameObject lastTile){


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

    public IEnumerator loadSceneAsync(){

        StartCoroutine(loadingPanel());

        string scene = PlayerPrefs.GetString("NextScene", "Home");

        PlayerPrefs.SetString("NextScene", "Home");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);


        while (!asyncLoad.isDone)
        {
            yield return new WaitForSeconds(.01f);
        }
    }

    public IEnumerator loadingPanel(){

        while (true)
        {
            for(int i = 1; i <= 10; i++){
                yield return new WaitForSeconds(.2f);

                if(i == 1){
                    addLetter(GameObject.Find("Tile-1-1"));
                }else if(i == 2){
                    addLetter(GameObject.Find("Tile-2-1"));
                }else if(i == 3){
                    addLetter(GameObject.Find("Tile-3-1"));
                }else if(i == 4){
                    addLetter(GameObject.Find("Tile-4-1"));
                }else if(i == 5){
                    addLetter(GameObject.Find("Tile-4-2"));
                }else if(i == 6){
                    addLetter(GameObject.Find("Tile-4-3"));
                }else if(i == 7){
                    addLetter(GameObject.Find("Tile-4-4"));
                }else{
                    word = word + ".";
                }
            }

            yield return new WaitForSeconds(.2f);
            for(int i = 1; i <= tiles.Count; i++){

            if(i == tiles.Count){
                removeLine(tiles[tiles.Count - i], tiles[tiles.Count - i]);
            }else{
                removeLine(tiles[tiles.Count - i], tiles[tiles.Count - i - 1]);
            }
        }
        word = "";
        tiles.Clear();
        }
    }
}

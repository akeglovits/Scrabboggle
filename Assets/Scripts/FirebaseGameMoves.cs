using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Functions;

/*
using Firebase.Database;
using Firebase.Unity;
*/

public class FirebaseGameMoves : MonoBehaviour
{
  public static FirebaseGameMoves FGM;

  //private FirebaseDatabase rootDatabase;

  private FirebaseFirestore root;

  private FirebaseFunctions functions;
    // Start is called before the first frame update
    void Start()
    {
      FGM = this;

      //rootDatabase = FirebaseDatabase.GetInstance("https://scrabboggle-default-rtdb.firebaseio.com/");
      //rootDatabase = FirebaseDatabase.DefaultInstance;
      root = FirebaseFirestore.DefaultInstance;

      functions = FirebaseFunctions.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable(){
      if(!MultiPlaySetup.gameEnded){
        if(PlayerPrefs.GetInt("CurrentMultiRandom", 0) == 0){
          quitGameSendOpponent();
          quitGameSendUser();
          quitGameSendMessage();
        }
          

      }
    }

    public void registerMove(){

        if(PlayerPrefs.GetInt("CurrentMultiRandom", 0) == 1){
          checkRandomGames();
        }else if(PlayerPrefs.GetInt("Rematch", 0) == 1){
          sendGameMove();
        }else{
          sendGameMove();
        }

        Debug.Log("Move Registered");
    }


// For Firestore


async void sendRematch(){
       
        await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("ActiveGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document(PlayerPrefs.GetString("CurrentMultiUsername")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        //SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

        GameEndAd.scoreUpdated = true;

        await sendMessage("New Game To Play", " started a new game with you!", PlayerPrefs.GetString("CurrentMultiUsername"), PlayerPrefs.GetString("Username"));

    }

    async void sendGameMove(){


        await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("ActiveGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document(PlayerPrefs.GetString("CurrentMultiUsername")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        GameEndAd.scoreUpdated = true;

        await sendMessage("Round Completed", " just played another round!", PlayerPrefs.GetString("CurrentMultiUsername"), PlayerPrefs.GetString("Username"));
    }

    async void quitGameSendOpponent(){

      await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("ActiveGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

    }

    async void quitGameSendUser(){

        await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document(PlayerPrefs.GetString("CurrentMultiUsername")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);
    }

    async void quitGameSendMessage(){

      await sendMessage("Round Completed", " just played another round!", PlayerPrefs.GetString("CurrentMultiUsername"), PlayerPrefs.GetString("Username"));
    }

    async void checkRandomGames(){

      List<object> gameObjects = new List<object>(PlayerLists.activeGames);
      gameObjects.Add("Unknown");


      QuerySnapshot snapshot = await root.Collection("RandomGames").WhereNotIn(Firebase.Firestore.FieldPath.DocumentId,gameObjects).Limit(1).GetSnapshotAsync();

      int exists = 0;
      string doc = "";
    foreach(DocumentSnapshot document in snapshot.Documents){
         Dictionary<string, object> documentDictionary = document.ToDictionary();

            acceptRandomGame(document.Id, documentDictionary["Score"].ToString());

            exists = 1;
            doc = document.Id;
          }

          if(exists == 0){
            
            sendRandomGame();
            
          }else{

            await root.Collection("RandomGames").Document(doc).DeleteAsync();
          }
 
    }

    async void sendRandomGame(){


              PlayerPrefs.SetString("CurrentMultiUsername", "");

              await root.Collection("RandomGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
          {"Score", GameplayFunctions.totalScore.ToString()}
        }, SetOptions.MergeAll);
              PlayerPrefs.SetInt("CurrentMultiRound", 1);

              await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document("Unknown").SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

                //SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

                GameEndAd.scoreUpdated = true;
    }

    async void acceptRandomGame(string opponent, string score){


             PlayerPrefs.SetString("CurrentMultiUsername", opponent);
              PlayerPrefs.SetInt("Rematch", 0);
                //SQLiteGameMoves.SGM.acceptRandom(PlayerPrefs.GetString("CurrentMultiUsername"), score);

              

              await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("ActiveGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        { PlayerPrefs.GetString("CurrentMultiUsername")+"1", score.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        });

        await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document(PlayerPrefs.GetString("CurrentMultiUsername")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+PlayerPrefs.GetInt("CurrentMultiRound", 1), GameplayFunctions.totalScore.ToString() },
        { PlayerPrefs.GetString("CurrentMultiUsername")+"1", score.ToString() },
        {"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

        await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("ActiveGames").Document("Unknown").DeleteAsync();

              PlayerPrefs.SetInt("CurrentMultiRound", 1);

           //SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

           GameEndAd.scoreUpdated = true;

           await sendMessage("Game Request Answered", " answered your game request!", PlayerPrefs.GetString("CurrentMultiUsername"), PlayerPrefs.GetString("Username"));
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



// For Realtime Database

/*

    async void sendRematch(){
       
        await rootDatabase.GetReference("Rematch").Child(PlayerPrefs.GetString("CurrentMultiID")).Child(PlayerPrefs.GetString("ID")).Child("Score").SetValueAsync(GameplayFunctions.totalScore.ToString());

        await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "Rematch", true}
        
       
        }, SetOptions.MergeAll);

        SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

        GameEndAd.scoreUpdated = true;

    }

    async void sendGameMove(){


        await rootDatabase.GetReference("GameMoves").Child(PlayerPrefs.GetString("CurrentMultiID")).Child(PlayerPrefs.GetString("ID")).Child("Round").SetValueAsync(PlayerPrefs.GetInt("CurrentMultiRound").ToString());
        await rootDatabase.GetReference("GameMoves").Child(PlayerPrefs.GetString("CurrentMultiID")).Child(PlayerPrefs.GetString("ID")).Child("Score").SetValueAsync(GameplayFunctions.totalScore.ToString());

        await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "GameMove", true}
        
       
        }, SetOptions.MergeAll);

        SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

        GameEndAd.scoreUpdated = true;
    }

    async void checkRandomGames(){


      DataSnapshot snapshot = await rootDatabase.GetReference("RandomGames").LimitToFirst(1).GetValueAsync();

          if(snapshot.Value == null){
            
            sendRandomGame();
            
          }else{

            acceptRandomGame(snapshot.Key, snapshot.Child("Score").Value.ToString());

            await rootDatabase.GetReference("RandomGames").Child(snapshot.Key).RemoveValueAsync();
          }
    }

    async void sendRandomGame(){


              PlayerPrefs.SetString("CurrentMultiID", "");

              await rootDatabase.GetReference("RandomGames").Child(PlayerPrefs.GetString("ID")).Child("Score").SetValueAsync(GameplayFunctions.totalScore.ToString());

              PlayerPrefs.SetInt("CurrentMultiRound", 1);

                SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

                GameEndAd.scoreUpdated = true;
    }

    async void acceptRandomGame(string opponent, string score){


             PlayerPrefs.SetString("CurrentMultiID", opponent);
              PlayerPrefs.SetInt("Rematch", 0);
                SQLiteGameMoves.SGM.acceptRandom(PlayerPrefs.GetString("CurrentMultiID"), score);

              

              await rootDatabase.GetReference("GameMoves").Child(PlayerPrefs.GetString("CurrentMultiID")).Child(PlayerPrefs.GetString("ID")).Child("Round").SetValueAsync(PlayerPrefs.GetInt("CurrentMultiRound").ToString());
              await rootDatabase.GetReference("GameMoves").Child(PlayerPrefs.GetString("CurrentMultiID")).Child(PlayerPrefs.GetString("ID")).Child("Score").SetValueAsync(GameplayFunctions.totalScore.ToString());

              await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "GameMove", true}
        
       
        }, SetOptions.MergeAll);

              PlayerPrefs.SetInt("CurrentMultiRound", 1);

           SQLiteGameMoves.SGM.newGameMove(GameplayFunctions.totalScore);

           GameEndAd.scoreUpdated = true;
    }

    */
}

using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Messaging;
using Firebase.Functions;
/*
using Firebase.Database;
using Firebase.Unity;
*/

public class FirebaseListeners : MonoBehaviour
{

  public static FirebaseListeners FBL;

  private FirebaseFunctions functions;

    private FirebaseFirestore root;

    public GameObject friendScroll;
    public GameObject gameScroll;

    private bool gamesFinished;
    private bool recentGamesFinished;

    //private bool rematchFinished;
    //private bool friendsFinished;

    public static bool sqlLoadDone;

    //private bool gamesRead;
    //private bool rematchRead;
    //private bool friendsRead;

    void Start()
    {

      FBL = this;

      functions = FirebaseFunctions.DefaultInstance;

      gamesFinished = false;
      //rematchFinished = false;
      recentGamesFinished = false;
      //friendsFinished = false;

      //gamesRead = false;
      //rematchRead = false;
      //friendsRead = false;

      sqlLoadDone = false;

      StartCoroutine(startSQLLoad());
      root = FirebaseFirestore.DefaultInstance;
      initializeDatabase();

      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public async void initializeDatabase(){
      if(!string.IsNullOrEmpty(PlayerPrefs.GetString("ID"))){
      gameMovesListener();
      //recentGamesListener();
      //rematchListener();
      //friendsListener();

      Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
  Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;

  if(string.IsNullOrEmpty(PlayerPrefs.GetString("NotificationToken"))){

        string token = await Firebase.Messaging.FirebaseMessaging.GetTokenAsync();

        PlayerPrefs.SetString("NotificationToken", token);

      if(!PlayerLists.notificationTokens.Contains(token)){
        PlayerLists.notificationTokens.Add(token);
      }

      await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
        {"NotificationToken", PlayerLists.notificationTokens}
      }, SetOptions.MergeAll);

      
      }

      }
    }


    public async void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
      
      PlayerPrefs.SetString("NotificationToken", token.Token);

      if(!PlayerLists.notificationTokens.Contains(token.Token)){
        PlayerLists.notificationTokens.Add(token.Token);
      }

      await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
        {"NotificationToken", PlayerLists.notificationTokens}
      }, SetOptions.MergeAll);

      
}

public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
  UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
}


private Task<string> removeBadge() {
  // Create the arguments to the callable function.
  var data = new Dictionary<string, object>();
  data["FirebaseID"] = PlayerPrefs.GetString("Username");
  
  return functions.GetHttpsCallable("removeBadge").CallAsync(data).ContinueWith((task) => {
    return "Completed";
  });
}




/*

    public void initializeDatabase(){

      
      DocumentReference docRef = root.Collection("Users").Document(PlayerPrefs.GetString("ID")).Collection("Changes").Document("MoveChanges");
docRef.Listen(snapshot => {
    Dictionary<string, object> movechange = snapshot.ToDictionary();
    foreach (KeyValuePair<string, object> change in movechange) {

        if((bool)change.Value){
          if(change.Key == "GameMove"){
            gameMovesChildAdded();
          }else if(change.Key == "Rematch"){
            rematchChildAdded();
          }else{
            friendsChildAdded();
          }
        }

        if(change.Key == "GameMove"){
            gamesRead = true;
          }else if(change.Key == "Rematch"){
            rematchRead = true;
          }else{
            friendsRead = true;
          }
    }
});

    if(!gamesRead){
      gamesFinished = true;
    }

    if(!rematchRead){
      rematchFinished = true;
    }

    if(!friendsRead){
      friendsFinished = true;
    }

      }
*/

    // For Firestore 


    public async void gameMovesListener(){

      PlayerLists.activeGameMoves.Clear();

      await removeBadge();

      QuerySnapshot snapshot = await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").OrderBy("LastPlayed").GetSnapshotAsync();

    foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
  {

    Dictionary<string, object> game = documentSnapshot.ToDictionary();

    string p1r1 = "";
    string p2r1 = "";
    string p1r2 = "";
    string p2r2 = "";
    string p1r3 = "";
    string p2r3 = "";


    if(documentSnapshot.ContainsField(PlayerPrefs.GetString("Username")+"1")){
      p1r1 = game[PlayerPrefs.GetString("Username")+"1"].ToString();
    }

    if(documentSnapshot.ContainsField(documentSnapshot.Id+"1")){
      p2r1 = game[documentSnapshot.Id+"1"].ToString();
    }

    if(documentSnapshot.ContainsField(PlayerPrefs.GetString("Username")+"2")){
      p1r2 = game[PlayerPrefs.GetString("Username")+"2"].ToString();
    }

    if(documentSnapshot.ContainsField(documentSnapshot.Id+"2")){
      p2r2 = game[documentSnapshot.Id+"2"].ToString();
    }
    
    if(documentSnapshot.ContainsField(PlayerPrefs.GetString("Username")+"3")){
      p1r3 = game[PlayerPrefs.GetString("Username")+"3"].ToString();
    }

    if(documentSnapshot.ContainsField(documentSnapshot.Id+"3")){
      p2r3 = game[documentSnapshot.Id+"3"].ToString();
    }

    if(!string.IsNullOrEmpty(p1r3) && !string.IsNullOrEmpty(p2r3)){

      SQLite.SQL.moveToRecent(documentSnapshot.Id,p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    }else{

      string round = "0";

      if(string.IsNullOrEmpty(p1r1) || string.IsNullOrEmpty(p2r1)){
        round = "1";
      }else if(string.IsNullOrEmpty(p1r2) || string.IsNullOrEmpty(p2r2)){
        round = "2";
      }else{
        round = "3";
      }

      if((!string.IsNullOrEmpty(p1r1) && string.IsNullOrEmpty(p2r1)) || (!string.IsNullOrEmpty(p1r2) && string.IsNullOrEmpty(p2r2)) || (!string.IsNullOrEmpty(p1r3) && string.IsNullOrEmpty(p2r3))){

    SQLite.SQL.incomingGameMove(documentSnapshot.Id, "Opponent", round, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    
    }else{

      SQLite.SQL.incomingGameMove(documentSnapshot.Id, "Mine", round, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    }

  }

  }

  gamesFinished = true;

      recentGamesListener();
    }

  public async void recentGamesListener(){

    PlayerLists.recentGameMoves.Clear();

    QuerySnapshot snapshot = await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("RecentGames").OrderBy("LastPlayed").GetSnapshotAsync();


    foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
  {

    Dictionary<string, object> game = documentSnapshot.ToDictionary();
    
    string p1r1 = game[PlayerPrefs.GetString("Username")+"1"].ToString();
    string p2r1 = game[documentSnapshot.Id+"1"].ToString();
    string p1r2 = game[PlayerPrefs.GetString("Username")+"2"].ToString();
    string p2r2 = game[documentSnapshot.Id+"2"].ToString();
    string p1r3 = game[PlayerPrefs.GetString("Username")+"3"].ToString();
    string p2r3 = game[documentSnapshot.Id+"3"].ToString();
    

      SQLite.SQL.incomingRecentGame(documentSnapshot.Id, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);

    if(!(bool)game["XPAdded"]){
      SQLite.SQL.XPUpdate(documentSnapshot.Id, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    }
  }

  recentGamesFinished = true;

    }


    public async void returnHomeGameUpdate(){


      QuerySnapshot snapshot = await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").WhereGreaterThanOrEqualTo("LastPlayed", long.Parse(PlayerPrefs.GetString("LastUpdate","0"))).OrderBy("LastPlayed").GetSnapshotAsync();

    foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
  {

    Dictionary<string, object> game = documentSnapshot.ToDictionary();

    string p1r1 = "";
    string p2r1 = "";
    string p1r2 = "";
    string p2r2 = "";
    string p1r3 = "";
    string p2r3 = "";


    if(documentSnapshot.ContainsField(PlayerPrefs.GetString("Username")+"1")){
      p1r1 = game[PlayerPrefs.GetString("Username")+"1"].ToString();
    }

    if(documentSnapshot.ContainsField(documentSnapshot.Id+"1")){
      p2r1 = game[documentSnapshot.Id+"1"].ToString();
    }

    if(documentSnapshot.ContainsField(PlayerPrefs.GetString("Username")+"2")){
      p1r2 = game[PlayerPrefs.GetString("Username")+"2"].ToString();
    }

    if(documentSnapshot.ContainsField(documentSnapshot.Id+"2")){
      p2r2 = game[documentSnapshot.Id+"2"].ToString();
    }
    
    if(documentSnapshot.ContainsField(PlayerPrefs.GetString("Username")+"3")){
      p1r3 = game[PlayerPrefs.GetString("Username")+"3"].ToString();
    }

    if(documentSnapshot.ContainsField(documentSnapshot.Id+"3")){
      p2r3 = game[documentSnapshot.Id+"3"].ToString();
    }

    if(!string.IsNullOrEmpty(p1r3) && !string.IsNullOrEmpty(p2r3)){

      SQLite.SQL.moveToRecent(documentSnapshot.Id,p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);

    }else{

      string round = "0";

      if(string.IsNullOrEmpty(p1r1) || string.IsNullOrEmpty(p2r1)){
        round = "1";
      }else if(string.IsNullOrEmpty(p1r2) || string.IsNullOrEmpty(p2r2)){
        round = "2";
      }else{
        round = "3";
      }

      if((!string.IsNullOrEmpty(p1r1) && string.IsNullOrEmpty(p2r1)) || (!string.IsNullOrEmpty(p1r2) && string.IsNullOrEmpty(p2r2)) || (!string.IsNullOrEmpty(p1r3) && string.IsNullOrEmpty(p2r3))){

    SQLite.SQL.incomingGameMove(documentSnapshot.Id, "Opponent", round, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    
    }else{

      SQLite.SQL.incomingGameMove(documentSnapshot.Id, "Mine", round, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    }

  }

  }
      
      SQLite.SQL.updateGames();

      PlayerPrefs.SetString("LastUpdate", System.DateTime.UtcNow.Ticks.ToString());


    }


    public async void returnHomeRecentGamesUpdate(){

      await removeBadge();

    QuerySnapshot snapshot = await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("RecentGames").WhereGreaterThanOrEqualTo("LastPlayed", long.Parse(PlayerPrefs.GetString("LastUpdate","0"))).OrderBy("LastPlayed").GetSnapshotAsync();


    foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
  {

    if(PlayerLists.activeGames.Contains(documentSnapshot.Id)){
      PlayerLists.activeGames.Remove(documentSnapshot.Id);
		List<List<string>> tempList = new List<List<string>>(PlayerLists.activeGameMoves);

		foreach(List<string> tempgame in tempList){
			if(tempgame[0] == documentSnapshot.Id){
				PlayerLists.activeGameMoves.Remove(tempgame);
			}
		}
    }

    Dictionary<string, object> game = documentSnapshot.ToDictionary();
    
    string p1r1 = game[PlayerPrefs.GetString("Username")+"1"].ToString();
    string p2r1 = game[documentSnapshot.Id+"1"].ToString();
    string p1r2 = game[PlayerPrefs.GetString("Username")+"2"].ToString();
    string p2r2 = game[documentSnapshot.Id+"2"].ToString();
    string p1r3 = game[PlayerPrefs.GetString("Username")+"3"].ToString();
    string p2r3 = game[documentSnapshot.Id+"3"].ToString();
    

      SQLite.SQL.incomingRecentGame(documentSnapshot.Id, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);

    if(!(bool)game["XPAdded"]){
      SQLite.SQL.XPUpdate(documentSnapshot.Id, p1r1,p2r1,p1r2,p2r2,p1r3,p2r3);
    }
  }

  

  returnHomeGameUpdate();

  

    }


    /*public void rematchListener(){

      
  root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("Rematch").Listen(snapshot => {

  foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
  {

    Dictionary<string, object> move = documentSnapshot.ToDictionary();

    SQLite.SQL.incomingRematch(documentSnapshot.Id, move["Score"].ToString());

    deleteDocument("Rematch", documentSnapshot.Id);

  }

  });

  rematchFinished = true;

    }*/


   /*public void friendsListener(){


   root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("FriendRequests").Document("Friends").Listen(snapshot => {


    Dictionary<string, object> request = snapshot.ToDictionary();
    
    if(SceneManager.GetActiveScene().name == "Home" && sqlLoadDone){
    SQLite.SQL.updateCurrentFriends();
  }


  });

    friendsFinished = true;

    }*/


    public async void deleteDocument(string collection, string id){

      await root.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection(collection).Document(id).DeleteAsync();

      Debug.Log("Collection: " + collection + " ID: " + id + " Deleted!");
    }

    public IEnumerator startSQLLoad(){

      while(!gamesFinished || !recentGamesFinished){
        yield return new WaitForSeconds(.01f);
      }

      SQLite.SQL.updateGames();

    }



    // Realtime Database Listeners

/*
    async void gameMovesChildAdded() {

      DatabaseReference gameMovesref = FirebaseDatabase.DefaultInstance.GetReference("GameMoves").Child(PlayerPrefs.GetString("ID"));
      DataSnapshot snapshot = await gameMovesref.GetValueAsync();

      foreach(DataSnapshot s in snapshot.Children){
      string round = s.Child("Round").Value.ToString();
      string score = s.Child("Score").Value.ToString();

      SQLite.SQL.incomingGameMove(s.Key, round, score);
      }

      await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "GameMove", false}
        
       
        }, SetOptions.MergeAll);
      
      

      await gameMovesref.SetValueAsync(null);

      gamesFinished = true;
        //send message
    }

    async void rematchChildAdded() {

      DatabaseReference rematchref = FirebaseDatabase.DefaultInstance.GetReference("Rematch").Child(PlayerPrefs.GetString("ID"));
      DataSnapshot snapshot = await rematchref.GetValueAsync();

      foreach(DataSnapshot s in snapshot.Children){
      string score = s.Child("Score").Value.ToString();

      SQLite.SQL.incomingRematch(s.Key, score);

      }

      await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "Rematch", false}
        
       
        }, SetOptions.MergeAll);
      
      

      await rematchref.SetValueAsync(null);

      rematchFinished = true;

        //send message
    }

    async void friendsChildAdded() {

      DatabaseReference friendref = FirebaseDatabase.DefaultInstance.GetReference("FriendRequests").Child(PlayerPrefs.GetString("ID"));

      DataSnapshot snapshot = await friendref.GetValueAsync();
      
      foreach(DataSnapshot s in snapshot.Children){
      string username = s.Child("Username").Value.ToString();
      string status = s.Child("Status").Value.ToString();

      SQLite.SQL.incomingFriendRequest(s.Key, username, status);

      }
      await root.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "Friend", false}
        
       
        }, SetOptions.MergeAll);

      

      await friendref.SetValueAsync(null);

      friendsFinished = true;

        //send message
    }

*/
}

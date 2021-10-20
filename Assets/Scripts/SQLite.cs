using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class SQLite : MonoBehaviour
{
    // Start is called before the first frame update

	public static SQLite SQL;

	//private string connection;
	//private IDbConnection dbcon;

	public GameObject activeGamePrefab;
	public GameObject friendButtonPrefab;

	public GameObject yourTurnText;
	public GameObject opponentTurnText;
	public GameObject noActiveGamesText;
	public GameObject noRecentGamesText;
	public GameObject pendingInvitesText;
	public GameObject pendingRequestsText;
	public GameObject noCurrentFriendsText;
	public GameObject noPendingFriendsText;

	private int totalCount;
	public bool startLoading;
    void Start()
    {

		SQL = this;

		totalCount = 0;
		startLoading = true;
		StartCoroutine(loadingSpinner());
        // Create database
		//connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";
		
		// Open connection
		//dbcon = new SqliteConnection(connection);
		//dbcon.Open();

		// Create table
		//IDbCommand dbcmd;
		//dbcmd = dbcon.CreateCommand();

		//dbcmd.CommandText = "DROP TABLE active_games";
		//dbcmd.ExecuteNonQuery();

		//dbcmd.CommandText = "DROP TABLE completed_games";
		//dbcmd.ExecuteNonQuery();

		//dbcmd.CommandText = "DROP TABLE friends";
		//dbcmd.ExecuteNonQuery();
		
		/*dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS active_games (id INTEGER PRIMARY KEY, p1 TEXT, p2 TEXT, p1r1 INTEGER, p1r2 INTEGER, p1r3 INTEGER, p2r1 INTEGER, p2r2 INTEGER, p2r3 INTEGER, lastplay INTEGER )";
		dbcmd.ExecuteNonQuery();

        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS completed_games (id INTEGER PRIMARY KEY, p1 TEXT, p2 TEXT, p1r1 INTEGER, p1r2 INTEGER, p1r3 INTEGER, p2r1 INTEGER, p2r2 INTEGER, p2r3 INTEGER, gameended INTEGER )";
		dbcmd.ExecuteNonQuery();

        dbcmd.CommandText = "CREATE TABLE IF NOT EXISTS friends (id INTEGER PRIMARY KEY, username STRING, status INTEGER )";
		dbcmd.ExecuteNonQuery();*/


		//updateGames();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	/*void OnDisable(){
		dbcon.Close();
	}

	void OnApplicationQuit(){
		dbcon.Close();
	}*/

	public void updateGames(){

		if(!startLoading){
		GameObject.Find("Loading-Games").transform.SetSiblingIndex(3);

		startLoading = true;
		StartCoroutine(loadingSpinner());
		}

		int count = 0;
		PlayerLists.activeGames.Clear();


		opponentTurnText.transform.SetSiblingIndex(2);
		

		//GameObject.Find("No-Active-Text").GetComponent<Text>().enabled = false;
		noActiveGamesText.SetActive(false);

		//GameObject.Find("Your-Turn").GetComponent<Image>().enabled = true;
		//GameObject.Find("Your-Turn-Text").GetComponent<Text>().enabled = true;

		yourTurnText.SetActive(true);

		for(int i = 3; i < GameObject.Find("Active-Games").transform.childCount; i++){
			Destroy(GameObject.Find("Active-Games").transform.GetChild(i).gameObject);
		}

		/*while(GameObject.Find("Active-Games").transform.childCount > 3){
				Destroy(GameObject.Find("Active-Games").transform.GetChild(3).gameObject);
			}*/

			foreach(List<string> game in PlayerLists.activeGameMoves)
		{		
			
			if(game[1] == "Mine"){
			DataControl.DC.createGameButton(game[0], game[2], game[3], game[4], game[5], game[6], game[7], game[8], activeGamePrefab, 1);

			PlayerLists.activeGames.Add(game[0]);

			count++;
			totalCount++;
			}
		}

		

		// Read and print all values in table
		/*IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT p2, CASE WHEN p1r1 IS NULL OR p2r1 IS NULL THEN 1 WHEN p1r2 IS NULL OR p2r2 IS NULL THEN 2 ELSE 3 END, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3 FROM active_games WHERE (p1r1 IS NULL AND p2r1 IS NOT NULL) OR (p1r2 IS NULL AND p2r2 IS NOT NULL) OR (p1r3 IS NULL AND p2r3 IS NOT NULL) ORDER BY lastplay DESC";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		while (reader.Read())
		{		
			
			DataControl.DC.createGameButton(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), activeGamePrefab, 1);

			PlayerLists.activeGames.Add(reader[0].ToString());

			count++;
			totalCount++;
		}*/

		

		if(count == 0){
			//GameObject.Find("Your-Turn").GetComponent<Image>().enabled = false;
			//GameObject.Find("Your-Turn-Text").GetComponent<Text>().enabled = false;

			yourTurnText.SetActive(false);

		}


		updateOpponentsTurn();
	}

	public void updateOpponentsTurn(){

		//GameObject.Find("Opponents-Turn").GetComponent<Image>().enabled = true;
			//GameObject.Find("Opponents-Turn-Text").GetComponent<Text>().enabled = true;
			opponentTurnText.SetActive(true);

		//if(GameObject.Find("Your-Turn").GetComponent<Image>().enabled){
		if(yourTurnText.activeSelf){
			//GameObject.Find("Opponents-Turn").transform.localPosition = new Vector3(0, GameObject.Find("Active-Games").transform.GetChild(GameObject.Find("Active-Games").transform.childCount - 1).localPosition.y - 200, 0);

			opponentTurnText.transform.SetAsLastSibling();
		}

		int count = 0;

		foreach(List<string> game in PlayerLists.activeGameMoves)
		{		
			
			if(game[1] == "Opponent"){
			DataControl.DC.createGameButton(game[0], game[2], game[3], game[4], game[5], game[6], game[7], game[8], activeGamePrefab, 2);

			PlayerLists.activeGames.Add(game[0]);

			count++;
			totalCount++;
			}
		}
		

		// Read and print all values in table
		/*IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT p2, CASE WHEN p1r1 IS NULL OR p2r1 IS NULL THEN 1 WHEN p1r2 IS NULL OR p2r2 IS NULL THEN 2 ELSE 3 END, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3 FROM active_games WHERE (p1r1 IS NOT NULL AND p2r1 IS NULL) OR (p1r2 IS NOT NULL AND p2r2 IS NULL) OR (p1r3 IS NOT NULL AND p2r3 IS NULL) ORDER BY lastplay DESC";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		while (reader.Read())
		{		
			
			DataControl.DC.createGameButton(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), activeGamePrefab, 2);

			PlayerLists.activeGames.Add(reader[0].ToString());

			count++;
			totalCount++;
			
		}*/

		if(count == 0){

			//GameObject.Find("Opponents-Turn").GetComponent<Image>().enabled = false;
			//GameObject.Find("Opponents-Turn-Text").GetComponent<Text>().enabled = false;

			opponentTurnText.SetActive(false);

		}

		if(totalCount == 0){

			//GameObject.Find("No-Active-Text").transform.localPosition = new Vector3(0,150,0);
			//GameObject.Find("No-Active-Text").GetComponent<Text>().enabled = true;

			noActiveGamesText.SetActive(true);
		}

		updateRecentGames();
	}

	public void updateRecentGames(){

		PlayerLists.recentGames.Clear();

		for(int i = 1; i < GameObject.Find("Recent-Games").transform.childCount; i++){
			Destroy(GameObject.Find("Recent-Games").transform.GetChild(i).gameObject);
		}

		/*while(GameObject.Find("Recent-Games").transform.childCount > 1){
				Destroy(GameObject.Find("Recent-Games").transform.GetChild(1).gameObject);
			}*/

		//GameObject.Find("No-Recent-Text").GetComponent<Text>().enabled = false;
		noRecentGamesText.SetActive(false);

		int count = 0;

		foreach (List<string> game in PlayerLists.recentGameMoves)
		{		
			
			
			DataControl.DC.createGameButton(game[0], "4", game[1], game[2], game[3], game[4], game[5], game[6], activeGamePrefab, 3);
			
			PlayerLists.recentGames.Add(game[0]);
			
			count++;
		}

		/*

		// Read and print all values in table
		IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT p2, 4, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3 FROM completed_games ORDER BY gameended DESC";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		while (reader.Read())
		{		
			
			DataControl.DC.createGameButton(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), activeGamePrefab, 3);

			PlayerLists.recentGames.Add(reader[0].ToString());
			
			count++;
		}

		*/

		if(count == 0){
			//GameObject.Find("No-Recent-Text").transform.localPosition = new Vector3(0,150,0);
			//GameObject.Find("No-Recent-Text").GetComponent<Text>().enabled = true;

			noRecentGamesText.SetActive(true);
		}


		updateCurrentFriends();

		

	}


	public IEnumerator loadingSpinner(){

		while(startLoading){

			for(int i = 1; i <= 8; i++){
				GameObject.Find("Loading-Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/loading-circle-"+i);

				yield return new WaitForSeconds(.2f);
			}
		}
	}

	public async void updateCurrentFriends(){

		//GameObject.Find("No-Current-Text").GetComponent<Text>().enabled = false;
		noCurrentFriendsText.SetActive(false);

		//GameObject.Find("No-Pending-Text").GetComponent<Text>().enabled = false;
		noPendingFriendsText.SetActive(false);

		//GameObject.Find("Pending-Requests").GetComponent<Image>().enabled = true;
		//GameObject.Find("Pending-Requests-Text").GetComponent<Text>().enabled = true;
		pendingRequestsText.SetActive(true);

		//GameObject.Find("Pending-Invites").GetComponent<Image>().enabled = true;
		//GameObject.Find("Pending-Invites-Text").GetComponent<Text>().enabled = true;
		pendingInvitesText.SetActive(true);

		pendingInvitesText.transform.SetSiblingIndex(2);
		
		
		if(!startLoading){
		GameObject.Find("Loading-Games").transform.SetSiblingIndex(3);

		startLoading = true;
		StartCoroutine(loadingSpinner());
		}
		

			PlayerLists.friends.Clear();
			PlayerLists.pendingrequests.Clear();
			PlayerLists.pendinginvites.Clear();

			for(int i = 1; i < GameObject.Find("Current-List").transform.childCount; i++){
			Destroy(GameObject.Find("Current-List").transform.GetChild(i).gameObject);
		}

		for(int i = 3; i < GameObject.Find("Pending-List").transform.childCount; i++){
			Destroy(GameObject.Find("Pending-List").transform.GetChild(i).gameObject);
		}

			/*while(GameObject.Find("Current-List").transform.childCount > 1){
				Destroy(GameObject.Find("Current-List").transform.GetChild(1).gameObject);
			}

			while(GameObject.Find("Pending-List").transform.childCount > 3){
				Destroy(GameObject.Find("Pending-List").transform.GetChild(3).gameObject);
			}*/

		DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("FriendRequests").Document("Friends").GetSnapshotAsync();

		if(snapshot.Exists){
		Dictionary<string, object> friendRecords = snapshot.ToDictionary();

		foreach(KeyValuePair<string, object> friend in friendRecords){

			if(friend.Value.ToString() == "1"){
				PlayerLists.friends.Add(friend.Key);
			}else if(friend.Value.ToString() == "2"){
				PlayerLists.pendingrequests.Add(friend.Key);
			}else if(friend.Value.ToString() == "3"){
				PlayerLists.pendinginvites.Add(friend.Key);
			}

		}

		}

		if(PlayerLists.friends.Count > 0){
				foreach(string user in PlayerLists.friends){
					DataControl.DC.createFriendButton(user, "1", friendButtonPrefab);
				}
			}else{
				//GameObject.Find("No-Current-Text").transform.localPosition = new Vector3(0,150,0);
				//GameObject.Find("No-Current-Text").GetComponent<Text>().enabled = true;
				noCurrentFriendsText.SetActive(true);
			}

		if(PlayerLists.pendingrequests.Count > 0){
				foreach(string user in PlayerLists.pendingrequests){
					DataControl.DC.createFriendButton(user, "2", friendButtonPrefab);
				}

				//GameObject.Find("Pending-Invites").transform.localPosition = new Vector3(0, GameObject.Find("Pending-List").transform.GetChild(GameObject.Find("Pending-List").transform.childCount - 1).localPosition.y - 200, 0);

				pendingInvitesText.transform.SetAsLastSibling();

			}else{
				//GameObject.Find("Pending-Requests").GetComponent<Image>().enabled = false;
				//GameObject.Find("Pending-Requests-Text").GetComponent<Text>().enabled = false;
				pendingRequestsText.SetActive(false);

			}

		

		
		if(PlayerLists.pendinginvites.Count > 0){
				foreach(string user in PlayerLists.pendinginvites){
					DataControl.DC.createFriendButton(user, "3", friendButtonPrefab);
				}
			}else{
				//GameObject.Find("Pending-Invites").GetComponent<Image>().enabled = false;
				//GameObject.Find("Pending-Invites-Text").GetComponent<Text>().enabled = false;
				pendingInvitesText.SetActive(false);
			}

		if(PlayerLists.pendingrequests.Count == 0 && PlayerLists.pendinginvites.Count == 0){

			//GameObject.Find("No-Pending-Text").transform.localPosition = new Vector3(0,150,0);
			//GameObject.Find("No-Pending-Text").GetComponent<Text>().enabled = true;
			noPendingFriendsText.SetActive(true);
		}


		GameObject.Find("Loading-Games").transform.SetAsFirstSibling();
		startLoading = false;

		FirebaseListeners.sqlLoadDone = true;

	}

	/*public void updateCurrentFriends(){

		GameObject.Find("No-Current-Text").GetComponent<Text>().enabled = false;

		IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT username FROM friends WHERE status = 1 ORDER BY username";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		int count = 0;
		while (reader.Read())
		{		
			
			DataControl.DC.createFriendButton(reader[0].ToString(), "1", friendButtonPrefab);

			PlayerLists.friends.Add(reader[0].ToString());
			
			count++;
		}

		if(count == 0){
			GameObject.Find("No-Current-Text").transform.localPosition = new Vector3(0,150,0);
			GameObject.Find("No-Current-Text").GetComponent<Text>().enabled = true;
		}

		updatePendingFriends();
	}

		public void updatePendingFriends(){

			GameObject.Find("No-Pending-Text").GetComponent<Text>().enabled = false;

			GameObject.Find("Pending-Requests").GetComponent<Image>().enabled = true;
			GameObject.Find("Pending-Requests-Text").GetComponent<Text>().enabled = true;

			GameObject.Find("Pending-Invites").GetComponent<Image>().enabled = true;
			GameObject.Find("Pending-Invites-Text").GetComponent<Text>().enabled = true;

			IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT username, status FROM friends WHERE status IN (2,3) ORDER BY status, username";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		int count = 0;
		int requestcount = 0;
		int invitecount = 0;
		string laststatus = "0";
		while (reader.Read())
		{		
			
			if(laststatus == "0" && reader[1].ToString() == "3"){

			GameObject.Find("Pending-Requests").GetComponent<Image>().enabled = false;
			GameObject.Find("Pending-Requests-Text").GetComponent<Text>().enabled = false;

			}else if(laststatus == "2" && reader[1].ToString() == "3"){
			
				GameObject.Find("Pending-Invites").transform.localPosition = new Vector3(0, GameObject.Find("Pending-List").transform.GetChild(GameObject.Find("Pending-List").transform.childCount - 1).localPosition.y - 200, 0);
			}
			DataControl.DC.createFriendButton(reader[0].ToString(), reader[1].ToString(), friendButtonPrefab);

			if(reader[1].ToString() == "2"){
				PlayerLists.pendingrequests.Add(reader[0].ToString());
				requestcount++;
			}else{
				PlayerLists.pendinginvites.Add(reader[0].ToString());
				invitecount++;
			}
			count++;

			laststatus = reader[1].ToString();
		}

		if(requestcount == 0){

			GameObject.Find("Pending-Requests").GetComponent<Image>().enabled = false;
			GameObject.Find("Pending-Requests-Text").GetComponent<Text>().enabled = false;
		}

		if(invitecount == 0){

			GameObject.Find("Pending-Invites").GetComponent<Image>().enabled = false;
			GameObject.Find("Pending-Invites-Text").GetComponent<Text>().enabled = false;
		}

		if(count == 0){

			GameObject.Find("No-Pending-Text").transform.localPosition = new Vector3(0,150,0);
			GameObject.Find("No-Pending-Text").GetComponent<Text>().enabled = true;
		}


		GameObject.Find("Loading-Games").transform.SetAsFirstSibling();
		startLoading = false;
	}*/

	public void incomingGameMove(string opponent, string turn, string round, string p1r1, string p2r1, string p1r2, string p2r2, string p1r3, string p2r3){
		List<List<string>> tempList = new List<List<string>>(PlayerLists.activeGameMoves);

		foreach(List<string> game in tempList){
			if(game[0] == opponent){
				PlayerLists.activeGameMoves.Remove(game);
			}
		}

		PlayerLists.activeGameMoves.Insert(0,new List<string>{opponent, turn, round, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3});
	}

	public async void moveToRecent(string opponent, string p1r1, string p2r1, string p1r2, string p2r2, string p1r3, string p2r3){

		PlayerLists.activeGames.Remove(opponent);
		List<List<string>> tempList = new List<List<string>>(PlayerLists.activeGameMoves);

		foreach(List<string> game in tempList){
			if(game[0] == opponent){
				PlayerLists.activeGameMoves.Remove(game);
			}
		}
		string ticksbefore = System.DateTime.UtcNow.Ticks.ToString();
		await FirebaseFirestore.DefaultInstance.Collection("Users").Document(opponent).Collection("RecentGames").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+"1", p1r1},
		{ PlayerPrefs.GetString("Username")+"2", p1r2},
		{ PlayerPrefs.GetString("Username")+"3", p1r3},
        { opponent+"1", p2r1},
		{ opponent+"2", p2r2},
		{ opponent+"3", p2r3},
		{"XPAdded", false},
		{"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

		await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("RecentGames").Document(opponent).SetAsync(new Dictionary<string, object>(){
      
        { PlayerPrefs.GetString("Username")+"1", p1r1},
		{ PlayerPrefs.GetString("Username")+"2", p1r2},
		{ PlayerPrefs.GetString("Username")+"3", p1r3},
        { opponent+"1", p2r1},
		{ opponent+"2", p2r2},
		{ opponent+"3", p2r3},
		{"XPAdded", false},
		{"LastPlayed", System.DateTime.UtcNow.Ticks}
       
        }, SetOptions.MergeAll);

		await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("ActiveGames").Document(opponent).DeleteAsync();

		await FirebaseFirestore.DefaultInstance.Collection("Users").Document(opponent).Collection("ActiveGames").Document(PlayerPrefs.GetString("Username")).DeleteAsync();

		PlayerPrefs.SetString("LastUpdate", ticksbefore);
		FirebaseListeners.FBL.returnHomeRecentGamesUpdate();


	}

	public void incomingRecentGame(string opponent, string p1r1, string p2r1, string p1r2, string p2r2, string p1r3, string p2r3){

		List<List<string>> tempList = new List<List<string>>(PlayerLists.recentGameMoves);

		foreach(List<string> game in tempList){
			if(game[0] == opponent){
				PlayerLists.recentGameMoves.Remove(game);
			}
		}

		PlayerLists.recentGameMoves.Insert(0,new List<string>{opponent, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3});
	}


	public async void XPUpdate(string opponent, string p1r1, string p2r1, string p1r2, string p2r2, string p1r3, string p2r3){

		int p1total = convertString(p1r1)+convertString(p1r2)+convertString(p1r3);
		int p2total = convertString(p2r1)+convertString(p2r2)+convertString(p2r3);


		PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP", 0) + p1total);

		if(p1total > p2total){
			PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins", 0) + 1);
			PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP", 0) + p1total);
		}else if(p1total < p2total){
			PlayerPrefs.SetInt("Loses", PlayerPrefs.GetInt("Loses", 0) + 1);
		}else{
			PlayerPrefs.SetInt("Draws", PlayerPrefs.GetInt("Draws", 0) + 1);
		}

		levelUpdate();

		await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("RecentGames").Document(opponent).SetAsync(new Dictionary<string, object>(){
      
        {"XPAdded", true}
       
        }, SetOptions.MergeAll);

		
		
	}
	
	/*public void incomingGameMove(string opponent, string round, string score){

		string roundToUpdate = "p2r"+round;

		IDbCommand dbcmd = dbcon.CreateCommand();

		if(PlayerLists.activeGames.Contains(opponent)){
			dbcmd.CommandText = "UPDATE active_games SET "+roundToUpdate+" = "+score+", lastplay = "+System.DateTime.Now.Ticks+" WHERE p2 = '"+opponent+"'";

		}else{
			dbcmd.CommandText = "UPDATE active_games SET "+roundToUpdate+" = "+score+", lastplay = "+System.DateTime.Now.Ticks+" WHERE p2 IS NULL";
		}
		
		dbcmd.ExecuteNonQuery();

		IDbCommand cmnd_read = dbcon.CreateCommand();
		IDataReader reader;
		string query ="SELECT p2, 4, p1r1, p1r2, p1r3, p2r1, p2r2, p2r3 FROM completed_games WHERE p2 = '"+ opponent+"' ORDER BY gameended DESC";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		while (reader.Read())
		{		
			
			if(reader[4].ToString().Length > 0 && reader[7].ToString().Length > 0){

				IDbCommand dbcmd2 = dbcon.CreateCommand();

				if(PlayerLists.recentGames.Contains(reader[0].ToString())){
					dbcmd2.CommandText = "UPDATE completed_games SET p1r1 = "+reader[2].ToString()+", p1r2 = "+reader[3].ToString()+", p1r3 = "+reader[4].ToString()+", p2r1 = "+reader[5].ToString()+", p2r2 = "+reader[6].ToString()+", p2r3 = "+reader[7].ToString()+", gameended = "+System.DateTime.Now.Ticks+" WHERE p2 = '"+opponent+"'";
				}else{

					dbcmd2.CommandText = "INSERT INTO completed_games (p1,p2,p1r1,p1r2,p1r3,p2r1,p2r2,p2r3,gameended) VALUES ('"+PlayerPrefs.GetString("ID")+"','"+opponent+"',"+reader[2].ToString()+","+reader[3].ToString()+","+reader[4].ToString()+","+reader[5].ToString()+","+reader[6].ToString()+","+reader[7].ToString()+","+System.DateTime.Now.Ticks+")";

				}

				dbcmd2.ExecuteNonQuery();

				XPUpdate(Convert.ToInt32(reader[2]),Convert.ToInt32(reader[3]),Convert.ToInt32(reader[4]),Convert.ToInt32(reader[5]),Convert.ToInt32(reader[6]),Convert.ToInt32(reader[7]));
			}

			
		}

		updateGames();

	}*/

	/*public void incomingRematch(string opponent, string score){

		IDbCommand dbcmd = dbcon.CreateCommand();

		if(PlayerLists.activeGames.Contains(opponent)){

			dbcmd.CommandText = "UPDATE active_games SET p2r1 = "+score+", lastplay = "+System.DateTime.Now.Ticks+" WHERE p2 = '"+opponent+"'";
		}else{
			dbcmd.CommandText = "INSERT INTO active_games (p1,p2,p2r1,lastplay) VALUES ('"+PlayerPrefs.GetString("ID")+"','"+opponent+"',"+score+","+System.DateTime.Now.Ticks+")";
		}
		dbcmd.ExecuteNonQuery();


		updateGames();

	}*/

	/*public void incomingFriendRequest(string username, string status){

		IDbCommand dbcmd = dbcon.CreateCommand();

		if(PlayerLists.friends.Contains(username)){
			dbcmd.CommandText = "UPDATE friends SET status = "+status+" WHERE username = '"+username+"'";

		}else{
			dbcmd.CommandText = "INSERT INTO friends (username,status) VALUES ('"+username+"',"+status+")";
		}
		
		dbcmd.ExecuteNonQuery();

		updateCurrentFriends();

	}*/

	private int convertString(string stringToConvert){

    if(string.IsNullOrEmpty(stringToConvert)){
        return 0;
    }else{
        return Convert.ToInt32(stringToConvert);
    }
}

	/*public void XPUpdate(int p1r1, int p1r2, int p1r3, int p2r1, int p2r2, int p2r3){

		int p1total = convertString(p1r1)+convertString(p1r2)+convertString(p1r3);
		int p2total = convertString(p2r1)+convertString(p2r2)+convertString(p2r3);

		PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP", 0) + p1total);

		if(p1total > p2total){
			PlayerPrefs.SetInt("Wins", PlayerPrefs.GetInt("Wins", 0) + 1);
			PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP", 0) + p1total);
		}else if(p1total < p2total){
			PlayerPrefs.SetInt("Loses", PlayerPrefs.GetInt("Loses", 0) + 1);
		}else{
			PlayerPrefs.SetInt("Draws", PlayerPrefs.GetInt("Draws", 0) + 1);
		}
 

		levelUpdate();
		
	}*/

	public void levelUpdate(){

		if(PlayerPrefs.GetInt("XP", 0) >= PlayerPrefs.GetInt("MultiLevel", 1) * 1000){
				PlayerPrefs.SetInt("XP", PlayerPrefs.GetInt("XP",0) - (PlayerPrefs.GetInt("MultiLevel", 1) * 1000));
				PlayerPrefs.SetInt("MultiLevel", PlayerPrefs.GetInt("MultiLevel", 1) + 1);

				levelUpdate();
				return;
			}

		GameObject.Find("Multi-Level-Slider").GetComponent<Slider>().maxValue = (float)PlayerPrefs.GetInt("MultiLevel", 1) * 1000f;
        GameObject.Find("Multi-Level-Slider").GetComponent<Slider>().value = (float)PlayerPrefs.GetInt("XP", 0);
        GameObject.Find("Multi-Level").GetComponent<Text>().text = PlayerPrefs.GetInt("MultiLevel",1).ToString();

		DataControl.DC.setAccountData();
	}
}

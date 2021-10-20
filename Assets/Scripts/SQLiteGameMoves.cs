using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SQLiteGameMoves : MonoBehaviour
{

    public static SQLiteGameMoves SGM;
    private string connection;
	private IDbConnection dbcon;

    
    // Start is called before the first frame update
    void Start()
    {

        SGM = this;
        connection = "URI=file:" + Application.persistentDataPath + "/" + "My_Database";
		
		// Open connection
		dbcon = new SqliteConnection(connection);
		dbcon.Open();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable(){
		dbcon.Close();
	}

	void OnApplicationQuit(){
		dbcon.Close();
	}

    public void newGameMove(int score){

        IDbCommand dbcmd = dbcon.CreateCommand();

        string roundToUpdate = "p1r" + PlayerPrefs.GetInt("CurrentMultiRound");

		if(PlayerLists.activeGames.Contains(PlayerPrefs.GetString("CurrentMultiUsername")) && PlayerPrefs.GetString("CurrentMultiUsername").Length > 0){
			dbcmd.CommandText = "UPDATE active_games SET "+roundToUpdate+" = "+score+", lastplay = "+System.DateTime.Now.Ticks+" WHERE p2 = '"+PlayerPrefs.GetString("CurrentMultiUsername")+"'";

		}else if(PlayerPrefs.GetString("CurrentMultiUsername") == "" && PlayerLists.activeGames.Contains("")){
			dbcmd.CommandText = "UPDATE active_games SET "+roundToUpdate+" = "+score+", lastplay = "+System.DateTime.Now.Ticks+" WHERE p2 IS NULL";
		}else{
            dbcmd.CommandText = "INSERT INTO active_games (p1,p2,p1r1,lastplay) VALUES ('"+PlayerPrefs.GetString("ID")+"','"+PlayerPrefs.GetString("CurrentMultiUsername")+"',"+score+","+System.DateTime.Now.Ticks+")";
        }
		
		dbcmd.ExecuteNonQuery();
    }

    public void acceptRandom(string opponent, string score){

		IDbCommand dbcmd = dbcon.CreateCommand();

			dbcmd.CommandText = "INSERT INTO active_games (p1,p2,p2r1,lastplay) VALUES ('"+PlayerPrefs.GetString("ID")+"','"+opponent+"',"+score+","+System.DateTime.Now.Ticks+")";

		dbcmd.ExecuteNonQuery();

	}
}

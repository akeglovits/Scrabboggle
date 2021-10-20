using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;
using Firebase.Functions;
/*
using Firebase.Database;
using Firebase.Unity;
*/

public class FirebaseFriendRequest : MonoBehaviour
{
    private FirebaseFunctions functions;
    // Start is called before the first frame update
    void Start()
    {
        functions = FirebaseFunctions.DefaultInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// For Firestore

public async void sendFriendRequest(int status){

        int sendstatus = 2;
        if(status == 5){
            PlayerPrefs.SetString("CurrentMultiUsername", "");
            sendstatus = 2;
        }else{
            sendstatus = status;
        }
        
        FirebaseFirestore friendref = FirebaseFirestore.DefaultInstance;

        await friendref.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiUsername")).Collection("FriendRequests").Document("Friends").SetAsync(new Dictionary<string, object>(){

            {PlayerPrefs.GetString("Username"), sendstatus}
        }, SetOptions.MergeAll);

            if(sendstatus == 2){
            sendstatus = 3;
            }

        await friendref.Collection("Users").Document(PlayerPrefs.GetString("Username")).Collection("FriendRequests").Document("Friends").SetAsync(new Dictionary<string, object>(){

            {PlayerPrefs.GetString("CurrentMultiUsername"), sendstatus}
        }, SetOptions.MergeAll);


        SQLite.SQL.updateCurrentFriends();

            if(sendstatus == 1){

                HomePage.HP.closeFriendPanel();

                await sendMessage("Friend Request Accepted", " accepted your friend request!", PlayerPrefs.GetString("CurrentMultiUsername"),PlayerPrefs.GetString("Username"));
            }else if(sendstatus == 3){

                GameObject.Find("Friend-Panel-Add-Button").GetComponent<Image>().enabled = false;
                GameObject.Find("Friend-Panel-Add-Button-Text").GetComponent<Text>().enabled = false;
                GameObject.Find("Friend-Panel-Request-Sent").GetComponent<Text>().enabled = true;

                await sendMessage("New Friend Request", " sent you a friend request!", PlayerPrefs.GetString("CurrentMultiUsername"), PlayerPrefs.GetString("Username"));
            }else if(sendstatus == 4){

                HomePage.HP.closeFriendPanel();
            }

            
        
    }


public async void addFriend(){

        string name = GameObject.Find("Add-Friend-Input").GetComponent<Text>().text;

              DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(name).GetSnapshotAsync();


          if(snapshot.Exists){

            HomePage.HP.closeAddFriendPanelInstant();
            HomePage.HP.openFriendPanel(name);

          }else{
            
            GameObject.Find("Friend-Not-Found-Text").GetComponent<Text>().text = "USERNAME ("+name+") WAS NOT FOUND!";
            HomePage.HP.closeAddFriendPanelInstant();
            HomePage.HP.openFriendNotFound();
            
          }

          GameObject.Find("Add-Friend-Input").GetComponent<Text>().text = "";
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


//For Realtime Database


/*
    public async void sendFriendRequest(int status){

        int sendstatus = 2;
        if(status == 5){
            PlayerPrefs.SetString("CurrentMultiID", "");
            sendstatus = 2;
        }else{
            sendstatus = status;
        }

        await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("CurrentMultiID")).Collection("Changes").Document("MoveChanges").SetAsync(new Dictionary<string, object>(){
      
        { "Friend", true}
        
       
        }, SetOptions.MergeAll);
        
        DatabaseReference friendref = FirebaseDatabase.DefaultInstance.GetReference("FriendRequests");

        await friendref.Child(PlayerPrefs.GetString("CurrentMultiID")).Child(PlayerPrefs.GetString("ID")).Child("Status").SetValueAsync(sendstatus);

        if(sendstatus == 2){
            sendstatus = 3;
        }

        SQLite.SQL.incomingFriendRequest(PlayerPrefs.GetString("CurrentMultiID"), "", sendstatus.ToString());
        
    }

*/
}

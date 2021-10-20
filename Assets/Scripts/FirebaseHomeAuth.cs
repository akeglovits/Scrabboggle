using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Firestore;
using Firebase.Extensions;

public class FirebaseHomeAuth : MonoBehaviour
{
    // Start is called before the first frame update

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public GameObject emailInput;
    public GameObject passwordInput;
    public GameObject linkEmailButton;
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        if(PlayerPrefs.GetString("Email","") != ""){

           linkEmailButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void signOut(){

        

        PlayerLists.notificationTokens.Remove(PlayerPrefs.GetString("NotificationToken", ""));

        //await FirebaseFirestore.DefaultInstance.Collection("Users").Document(PlayerPrefs.GetString("Username")).SetAsync(new Dictionary<string, object>(){
        //{"NotificationToken", PlayerLists.notificationTokens}
      //}, SetOptions.MergeAll);

      auth.SignOut();

      string notification = PlayerPrefs.GetString("NotificationToken", "");
        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetString("NotificationToken", notification);
        
        PlayerPrefs.SetString("NextScene", "SignIn");

        SceneManager.LoadScene("LoadingScreen");
    }

    public void linkToEmail(){

        string email = emailInput.GetComponent<InputField>().text;
        string password = passwordInput.GetComponent<InputField>().text;

        GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";

        Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);

        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
        if (task.IsCanceled) {
            Debug.LogError("LinkWithCredentialAsync was canceled.");
            return;
        }
        if (task.IsFaulted) {
            Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
            Firebase.FirebaseException exception = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
            Debug.Log(exception.Message);
            GameObject.Find("Firebase-Message").GetComponent<Text>().text = exception.Message.ToUpper();
            return;
        }

        Firebase.Auth.FirebaseUser newUser = task.Result;
        Debug.LogFormat("Credentials successfully linked to Firebase user: {0} ({1})",
        newUser.DisplayName, newUser.UserId);

        emailInput.GetComponent<InputField>().text = "";
        passwordInput.GetComponent<InputField>().text = "";
        emailInput.SetActive(false);
        passwordInput.SetActive(false);
        GameObject.Find("Link-Email-Main-Text").GetComponent<Text>().enabled = false;
        GameObject.Find("Link-Email-Input-Button").GetComponent<Image>().enabled = false;
        GameObject.Find("Link-Email-Input-Button-Text").GetComponent<Text>().enabled = false;

        GameObject.Find("Link-Email-Completed-Text").GetComponent<Text>().text = "ACCOUNT SUCCESSFULLY CREATED FOR "+email;
        GameObject.Find("Link-Email-Completed-Text").GetComponent<Text>().enabled = true;
        GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";

        linkEmailButton.SetActive(false);
        });
    }


}

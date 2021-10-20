using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;
//using Firebase.Functions;
//using Facebook.Unity;
public class FirebaseSignin : MonoBehaviour
{
    // Start is called before the first frame update

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    //private FirebaseFunctions functions;

    public GameObject emailInput;
    public GameObject passwordInput;
    public GameObject usernameInput;
    public GameObject emailResetInput;

    private bool initializeNow;

    void Start()
    {
      //functions = FirebaseFunctions.DefaultInstance;

      InitializeFirebase();


/*if (!FB.IsInitialized) {
        FB.Init(initCallback, onHideUnity);
    } else {
        // Already initialized
        FB.ActivateApp();
    }*/



    }
    /*private void initCallback ()
{
    if (FB.IsInitialized) {
        // Signal an app activation App Event
        FB.ActivateApp();
        // Continue with Facebook SDK
        // ...
    } else {
        Debug.Log("Something went wrong to Initialize the Facebook SDK");
    }
}*/
/*private void onHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }*/

private void InitializeFirebase() {
  Debug.Log("Setting up Firebase Auth");
  auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
  auth.StateChanged += AuthStateChanged;
  AuthStateChanged(this, null);

  if(PlayerPrefs.GetString("Username","").Length == 0){
      //signinAnonymous();
        GameObject.Find("Block-Background").transform.SetAsLastSibling();
        GameObject.Find("Sign-In-Screen").transform.SetAsLastSibling();
  }else{

    StartCoroutine(loadHomeScreen());

    StartCoroutine(getData());
    
  }
}

// Track state changes of the auth object.
private void AuthStateChanged(object sender, System.EventArgs eventArgs) {
  if (auth.CurrentUser != user) {
    bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
    if (!signedIn && user != null) {
      Debug.Log(user.DisplayName + " Signed out " + user.UserId);
    }
    user = auth.CurrentUser;
    if (signedIn) {
      Debug.Log(user.DisplayName + " Signed in " + user.UserId);
    }

  }
  }


void OnDestroy() {
  auth.StateChanged -= AuthStateChanged;
  auth = null;
}

public async void createNewAccountEmail(){

  string name = usernameInput.GetComponent<InputField>().text;
  string email = emailInput.GetComponent<InputField>().text;
  string password = passwordInput.GetComponent<InputField>().text;

        if(name.Length < 5 || name.Length > 50){

          GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
          GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
          GameObject.Find("Too-Short").GetComponent<Text>().enabled = true;
          return;
        }else if(name == "Unknown"){
            GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
            GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
            GameObject.Find("Username-Taken").GetComponent<Text>().enabled = true;
            return;
        }else{

          DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(name).GetSnapshotAsync();

          if(snapshot.Exists){
            GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
            GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
            GameObject.Find("Username-Taken").GetComponent<Text>().enabled = true;
            return;

          }
        }

    auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
  if (task.IsCanceled) {
    Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    Firebase.FirebaseException exception = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
    
    GameObject.Find("Firebase-Message").GetComponent<Text>().text = exception.Message.ToUpper();
    GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
    GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
    Debug.Log(exception.Message);
    return;
  }

  // Firebase user has been created.
  Firebase.Auth.FirebaseUser newUser = task.Result;
  Debug.LogFormat("Firebase user created successfully: {0} ({1})",
      newUser.DisplayName, newUser.UserId);

      submitUsername(name, email, true);
});

}

public void signinWithEmail(){

  string email = emailInput.GetComponent<InputField>().text;
  string password = passwordInput.GetComponent<InputField>().text;

auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
  if (task.IsCanceled) {
    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    Firebase.FirebaseException exception = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
    Debug.Log(exception.Message);
    GameObject.Find("Firebase-Message").GetComponent<Text>().text = exception.Message.ToUpper();
      GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
      GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
    return;
  }

  Firebase.Auth.FirebaseUser newUser = task.Result;
  Debug.LogFormat("User signed in successfully: {0} ({1})",
      newUser.DisplayName, newUser.UserId);

  submitUsername(newUser.DisplayName, email, false);
});

}

/*public void loginBtnForFB (){
// Permission option list      https://developers.facebook.com/docs/facebook-login/permissions/
var permissons = new List<string>(){"email","user_friends" ,"public_profile"};
    FB.LogInWithReadPermissions(permissons, authStatusCallback);
}
private void authStatusCallback (ILoginResult result) {
    if (FB.IsLoggedIn) {
        // AccessToken class will have session details
        var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken;
        // current access token's User ID : aToken.UserId
    loginviaFirebaseFacebook(accessToken.TokenString); 
    
    } else {
        Debug.Log("User cancelled login");
    }
}

    private void loginviaFirebaseFacebook(string accessToken){
    Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
    auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
  if (task.IsCanceled) {
    Debug.LogError("SignInWithCredentialAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
    return;
  }

  Firebase.Auth.FirebaseUser newUser = task.Result;
  Debug.LogFormat("User signed in successfully: {0} ({1})",
      newUser.DisplayName, newUser.UserId);
});
    }*/

    public async void signinAnonymous(){

      string name = usernameInput.GetComponent<InputField>().text;

        if(name.Length < 5 || name.Length > 50){

          GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
          GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
          GameObject.Find("Too-Short").GetComponent<Text>().enabled = true;
          return;
        }else if(name == "Unknown"){
            GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
            GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
            GameObject.Find("Username-Taken").GetComponent<Text>().enabled = true;
            return;
        }else{

          DocumentSnapshot snapshot = await FirebaseFirestore.DefaultInstance.Collection("Users").Document(name).GetSnapshotAsync();

          if(snapshot.Exists){
            GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";
            GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
            GameObject.Find("Username-Taken").GetComponent<Text>().enabled = true;
            return;

          }
        }

        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task => {
  if (task.IsCanceled) {
    Debug.LogError("SignInAnonymouslyAsync was canceled.");
    return;
  }
  if (task.IsFaulted) {
    Debug.Log("SignInAnonymouslyAsync encountered an error: " + task.Exception);
    Firebase.FirebaseException exception = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
    Debug.Log(exception.Message);
      GameObject.Find("Firebase-Message").GetComponent<Text>().text = exception.Message.ToUpper();
      GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
      GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
    return;
  }

  Firebase.Auth.FirebaseUser newUser = task.Result;
  Debug.LogFormat("User signed in successfully: {0} ({1})",
      newUser.DisplayName, newUser.UserId);

  submitUsername(name, "", true);
});

    }



    public void resetPassword(){

      string emailAddress = emailResetInput.GetComponent<InputField>().text;

      auth.SendPasswordResetEmailAsync(emailAddress).ContinueWithOnMainThread(task => {
        if (task.IsCanceled) {
        Debug.LogError("SendPasswordResetEmailAsync was canceled.");
        return;
      }
      if (task.IsFaulted) {
        Debug.Log("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
        Firebase.FirebaseException exception = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;
        Debug.Log(exception.Message);
        GameObject.Find("Firebase-Message-2").GetComponent<Text>().text = exception.Message.ToUpper();
        return;
      }

      Debug.Log("Password reset email sent successfully.");

      emailResetInput.GetComponent<InputField>().text = "";
      emailResetInput.SetActive(false);
      GameObject.Find("Forgot-Password-Main-Text").GetComponent<Text>().enabled = false;
      GameObject.Find("Reset-Password-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Reset-Password-Button-Text").GetComponent<Text>().enabled = false;

      GameObject.Find("Forgot-Password-Completed-Text").GetComponent<Text>().text = "EMAIL WAS SENT TO "+emailAddress+", FOLLOW THE INSTRUCTIONS TO RESET YOUR PASSWORD";
      GameObject.Find("Forgot-Password-Completed-Text").GetComponent<Text>().enabled = true;
      GameObject.Find("Firebase-Message-2").GetComponent<Text>().text = "";

    });

    }

    public async void submitUsername(string username, string email, bool newAccount){

        user = auth.CurrentUser;

        GameObject.Find("Sign-In-Screen").transform.SetAsFirstSibling();
        GameObject.Find("Block-Background").transform.SetAsFirstSibling();

        PlayerPrefs.SetString("ID", user.UserId);
        PlayerPrefs.SetString("Username", username);
        PlayerPrefs.SetString("Email", email);

        StartCoroutine(loadHomeScreen());


        Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile {
          DisplayName = username,
        };
        
        await user.UpdateUserProfileAsync(profile);

        if(newAccount){

          //await setDisplayName(PlayerPrefs.GetString("ID"), PlayerPrefs.GetString("Username"));

        DataControl.DC.setAccountData();

        
        }else{

          DataControl.DC.getAccountData();
        }

        
    }

    /*private Task<string> setDisplayName(string firebaseid, string username) {
  // Create the arguments to the callable function.
  var data = new Dictionary<string, object>();
  data["FirebaseID"] = firebaseid;
  data["Username"] = username;
  
  return functions.GetHttpsCallable("setDisplayName").CallAsync(data).ContinueWith((task) => {
    return "Completed";
  });
}*/

  public void openWebPage(string url){
    Application.OpenURL(url);
  }

    public void openForgotPassword(){

      GameObject.Find("Sign-In-Screen").transform.SetAsFirstSibling();
      GameObject.Find("Forgot-Password-Panel").transform.SetAsLastSibling();

    }

    public void closeForgotPassword(){

      changeToSignIn();

      GameObject.Find("Forgot-Password-Panel").transform.SetAsFirstSibling();
      GameObject.Find("Sign-In-Screen").transform.SetAsLastSibling();

      
      emailResetInput.SetActive(true);
      emailResetInput.GetComponent<InputField>().text = "";

      GameObject.Find("Forgot-Password-Main-Text").GetComponent<Text>().enabled = true;
      GameObject.Find("Reset-Password-Button").GetComponent<Image>().enabled = true;
      GameObject.Find("Reset-Password-Button-Text").GetComponent<Text>().enabled = true;


      GameObject.Find("Forgot-Password-Completed-Text").GetComponent<Text>().enabled = false;
      GameObject.Find("Firebase-Message-2").GetComponent<Text>().text = "";

      

    }


    public void changeToGuest(){

      GameObject.Find("Sign-In-Text").GetComponent<Text>().text = "CREATE ACCOUNT";

      //Change Inputs that are showing

      emailInput.GetComponent<InputField>().text = "";
      passwordInput.GetComponent<InputField>().text = "";
      usernameInput.GetComponent<InputField>().text = "";
      emailInput.SetActive(false);
      passwordInput.SetActive(false);
      usernameInput.SetActive(true);

      GameObject.Find("Guest-Text").GetComponent<Text>().enabled = true;

      //Change Sign in button that is showing

      GameObject.Find("Create-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Create-Button-Text").GetComponent<Text>().enabled = false;
      GameObject.Find("Create-Guest-Button").GetComponent<Image>().enabled = true;
      GameObject.Find("Create-Guest-Button-Text").GetComponent<Text>().enabled = true;
      GameObject.Find("Sign-In-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Sign-In-Button-Text").GetComponent<Text>().enabled = false;

      GameObject.Find("Create-Account-Guest-Button").GetComponent<Text>().enabled = false;
      GameObject.Find("Create-Account-Email-Button").GetComponent<Text>().enabled = true;
      GameObject.Find("Forgot-Password-Button").GetComponent<Text>().enabled = false;

      //Change Bottom Question that is showing

      GameObject.Find("Current-User-Question").GetComponent<Text>().enabled = true;
      GameObject.Find("Current-User-Answer").GetComponent<Text>().enabled = true;
      GameObject.Find("New-User-Question").GetComponent<Text>().enabled = false;
      GameObject.Find("New-User-Answer").GetComponent<Text>().enabled = false;


      //Clear all error texts
      GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
      GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
      GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";


    }

    public void changeToNewAccount(){

      GameObject.Find("Sign-In-Text").GetComponent<Text>().text = "CREATE ACCOUNT";

      //Change Inputs that are showing

      
      emailInput.SetActive(true);
      passwordInput.SetActive(true);
      usernameInput.SetActive(true);

      emailInput.GetComponent<InputField>().text = "";
      passwordInput.GetComponent<InputField>().text = "";
      usernameInput.GetComponent<InputField>().text = "";

      GameObject.Find("Guest-Text").GetComponent<Text>().enabled = false;

      //Change Sign in button that is showing

      GameObject.Find("Create-Button").GetComponent<Image>().enabled = true;
      GameObject.Find("Create-Button-Text").GetComponent<Text>().enabled = true;
      GameObject.Find("Create-Guest-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Create-Guest-Button-Text").GetComponent<Text>().enabled = false;
      GameObject.Find("Sign-In-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Sign-In-Button-Text").GetComponent<Text>().enabled = false;

      GameObject.Find("Create-Account-Guest-Button").GetComponent<Text>().enabled = true;
      GameObject.Find("Create-Account-Email-Button").GetComponent<Text>().enabled = false;
      GameObject.Find("Forgot-Password-Button").GetComponent<Text>().enabled = false;

      //Change Bottom Question that is showing

      GameObject.Find("Current-User-Question").GetComponent<Text>().enabled = true;
      GameObject.Find("Current-User-Answer").GetComponent<Text>().enabled = true;
      GameObject.Find("New-User-Question").GetComponent<Text>().enabled = false;
      GameObject.Find("New-User-Answer").GetComponent<Text>().enabled = false;


      //Clear all error texts
      GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
      GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
      GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";

    }

    public void changeToSignIn(){

      GameObject.Find("Sign-In-Text").GetComponent<Text>().text = "SIGN IN";

      //Change Inputs that are showing

      if(usernameInput.activeSelf){
      usernameInput.GetComponent<InputField>().text = "";
      }
      emailInput.SetActive(true);
      passwordInput.SetActive(true);
      usernameInput.SetActive(false);

      emailInput.GetComponent<InputField>().text = "";
      passwordInput.GetComponent<InputField>().text = "";

      GameObject.Find("Guest-Text").GetComponent<Text>().enabled = false;

      //Change Sign in button that is showing

      GameObject.Find("Create-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Create-Button-Text").GetComponent<Text>().enabled = false;
      GameObject.Find("Create-Guest-Button").GetComponent<Image>().enabled = false;
      GameObject.Find("Create-Guest-Button-Text").GetComponent<Text>().enabled = false;
      GameObject.Find("Sign-In-Button").GetComponent<Image>().enabled = true;
      GameObject.Find("Sign-In-Button-Text").GetComponent<Text>().enabled = true;

      GameObject.Find("Create-Account-Guest-Button").GetComponent<Text>().enabled = false;
      GameObject.Find("Create-Account-Email-Button").GetComponent<Text>().enabled = false;
      GameObject.Find("Forgot-Password-Button").GetComponent<Text>().enabled = true;

      //Change Bottom Question that is showing

      GameObject.Find("Current-User-Question").GetComponent<Text>().enabled = false;
      GameObject.Find("Current-User-Answer").GetComponent<Text>().enabled = false;
      GameObject.Find("New-User-Question").GetComponent<Text>().enabled = true;
      GameObject.Find("New-User-Answer").GetComponent<Text>().enabled = true;


      //Clear all error texts
      GameObject.Find("Too-Short").GetComponent<Text>().enabled = false;
      GameObject.Find("Username-Taken").GetComponent<Text>().enabled = false;
      GameObject.Find("Firebase-Message").GetComponent<Text>().text = "";

    }

    public IEnumerator getData(){

      yield return new WaitForSeconds(.1f);

      if(PlayerPrefs.GetInt("InitialUpdate", 0) == 0){

        PlayerPrefs.SetInt("InitialUpdate", 1); 
        DataControl.DC.setAccountData();

      }else{

        DataControl.DC.getAccountData();

      }
    }


    public IEnumerator loadHomeScreen(){


      StartCoroutine(loadingPanel());

        yield return new WaitForSeconds(.2f);
      
        while(!DataControl.DC.dataLoaded){

				yield return new WaitForSeconds(.01f);
			  }       



      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Home");

    while(!asyncLoad.isDone){

				yield return new WaitForSeconds(.01f);
			}        
    }


    public IEnumerator loadingPanel(){

      GameObject.Find("Loading-Panel").transform.SetAsLastSibling();

      while(true){

        for(int i = 1; i <= 8; i++){
				GameObject.Find("Loading-Panel-Image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/loading-circle-"+i);

				yield return new WaitForSeconds(.2f);
			  }        
      }

    }


}

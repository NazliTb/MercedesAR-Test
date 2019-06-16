using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AuthentificationScript : MonoBehaviour
{
    #region variables

    private FirebaseAuth auth;
    private FirebaseUser user;
    public InputField Email, Pwd;

    #endregion

    #region monobehaviour

    void Start()
    {
        InitializeFirebase();
    }

    void OnApplicationQuit()
    {
        auth.SignOut();
    }
    #endregion

    #region functions 

    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public void OnClickLogin()
    {
        auth.SignInWithEmailAndPasswordAsync(Email.text, Pwd.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                Debug.Log(user.Email);

                SceneManager.LoadScene("MercedesAR");
            }
        }
    }

    public void LogOut ()
    {
        auth.SignOut();
        SceneManager.LoadScene("Login");
    }

    #endregion
}
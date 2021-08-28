using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InitialConfiguration : MonoBehaviour
{
    public enum Settings{
    UserName,
    Password,
    Channel
    }
    public GameObject txtUsername;
    public GameObject txtPassword;
    public GameObject txtChannel;
    void Start()
    {
        //Load up Player prefs.
        LoadPlayerPrefs();
    }

    void LoadPlayerPrefs(){
        string username = PlayerPrefs.GetString(Settings.UserName.ToString(),"");
        string password = PlayerPrefs.GetString(Settings.Password.ToString(),"");
        string channel = PlayerPrefs.GetString(Settings.Channel.ToString(),"");
        txtUsername.GetComponent<TMP_InputField>().text=username;
        txtPassword.GetComponent<TMP_InputField>().text=password;
        txtChannel.GetComponent<TMP_InputField>().text=channel;        
    }

    public void SaveConfig(){
        string user="";
        string pass="";
        string channel="";
        user= txtUsername.GetComponent<TMP_InputField>().text;
        pass=txtPassword.GetComponent<TMP_InputField>().text;
        channel=txtChannel.GetComponent<TMP_InputField>().text;

        //Save Player Prefs
        PlayerPrefs.SetString(Settings.UserName.ToString(),user);
        PlayerPrefs.SetString(Settings.Password.ToString(),pass);
        PlayerPrefs.SetString(Settings.Channel.ToString(),channel);        
    }

    public void GoToMainScene(){
        string pass=txtPassword.GetComponent<TMP_InputField>().text;
        if(pass!=""){
            SceneManager.LoadScene("MainScene");
        }        
    }

}

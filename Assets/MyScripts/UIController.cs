using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public enum Settings{
        MaxNumberOfAvatars,
        AvatarLifeTime,
        MaxChatMessages,
        IsShowChat
    }
    public GameObject configPanel;
    public GameObject txtMaxNumberAvatars;
    public GameObject txtAvatarLifetime;
    public GameObject txtMaxChatMessages;
    public GameObject IsShowChat;

    void Start(){
        //Load up Player prefs.
        LoadPlayerPrefs();
    }

    void LoadPlayerPrefs(){
        string maxNumAvatars = PlayerPrefs.GetString(Settings.MaxNumberOfAvatars.ToString(),"10");
        string avatarLifetime = PlayerPrefs.GetString(Settings.AvatarLifeTime.ToString(),"1");
        string maxChatMessage = PlayerPrefs.GetString(Settings.MaxChatMessages.ToString(),"20");
        int showChat = PlayerPrefs.GetInt(Settings.IsShowChat.ToString(),0);
        txtMaxNumberAvatars.GetComponent<TMP_InputField>().text=maxNumAvatars;
        txtAvatarLifetime.GetComponent<TMP_InputField>().text=avatarLifetime;
        txtMaxChatMessages.GetComponent<TMP_InputField>().text=maxChatMessage;
        if(showChat==1){
            IsShowChat.GetComponent<Toggle>().isOn=true;
        }else{
            IsShowChat.GetComponent<Toggle>().isOn=false;
        }
        
    }

    public void ShowPanel(){
        configPanel.SetActive(true);
    }
    public void HidePanel(){
        configPanel.SetActive(false);
    }

    public void SaveConfig(){
        string maxNumAvatars="";
        string avatarLifetime="";
        string maxChatMessages="";
        int showChat = 0;
        maxNumAvatars= txtMaxNumberAvatars.GetComponent<TMP_InputField>().text;
        avatarLifetime=txtAvatarLifetime.GetComponent<TMP_InputField>().text;
        maxChatMessages=txtMaxChatMessages.GetComponent<TMP_InputField>().text;
        if(IsShowChat.GetComponent<Toggle>().isOn){
            showChat=1;
        }else{
            showChat=0;
        }


        //Save Player Prefs
        PlayerPrefs.SetString(Settings.MaxNumberOfAvatars.ToString(),maxNumAvatars);
        PlayerPrefs.SetString(Settings.AvatarLifeTime.ToString(),avatarLifetime);
        PlayerPrefs.SetString(Settings.MaxChatMessages.ToString(),maxChatMessages);
        PlayerPrefs.SetInt(Settings.IsShowChat.ToString(),showChat);
        
    }

    public void GoToConfigScene(){
        SceneManager.LoadScene("ConfigScene");
    }

    public void CloseApp(){
          Application.Quit();
    }
}

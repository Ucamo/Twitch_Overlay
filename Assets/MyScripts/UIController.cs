using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public enum Settings{
        MaxNumberOfAvatars,
        AvatarLifeTime,
        MaxChatMessages
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
        txtMaxNumberAvatars.GetComponent<TMP_InputField>().text=maxNumAvatars;
        txtAvatarLifetime.GetComponent<TMP_InputField>().text=avatarLifetime;
        txtMaxChatMessages.GetComponent<TMP_InputField>().text=maxChatMessage;
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
        maxNumAvatars= txtMaxNumberAvatars.GetComponent<TMP_InputField>().text;
        avatarLifetime=txtAvatarLifetime.GetComponent<TMP_InputField>().text;
        maxChatMessages=txtMaxChatMessages.GetComponent<TMP_InputField>().text;

        //Save Player Prefs
        PlayerPrefs.SetString(Settings.MaxNumberOfAvatars.ToString(),maxNumAvatars);
        PlayerPrefs.SetString(Settings.AvatarLifeTime.ToString(),avatarLifetime);
        PlayerPrefs.SetString(Settings.MaxChatMessages.ToString(),maxChatMessages);
        
    }
}

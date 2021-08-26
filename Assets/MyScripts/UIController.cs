using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject configPanel;
    public InputField txtMaxNumberAvatars;
    public InputField txtAvatarLifetime;
    public InputField txtMaxChatMessages;
    public Toggle IsShowChat;

    void Start(){
        //Load up custom prefs.
    }

    public void ShowPanel(){
        configPanel.SetActive(true);
    }
    public void HidePanel(){
        configPanel.SetActive(false);
    }
}

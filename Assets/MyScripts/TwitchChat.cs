using System.IO;
using System.Net.Sockets;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Linq;
using TMPro;
 
[Serializable]
public class ChatAvatar
{
    public string UserName;
    public Color color;

    public string Message;

    public DateTime LastMessage;
    public bool isActive;
}
 
public class TwitchChat : MonoBehaviour
{
 
    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;
    //Oauth password can be generated at https://twitchapps.com/tmi
    // [HideInInspector]
    public string username, password, channel;
 
    public string custom_rewards_id;
 
    public GameObject emotePrefab;
 
    public Transform startPoint;
 
    public TextMeshProUGUI chatBox;
 
    public GameObject avatarSpawn;

    public int MaxNumberOfAvatars=10;
    public double LifeTimeInMinutes=5;

    public int MaxNumbersOfMessagesOnChatBox=30;
    private int currentNumberOfMessages=0;
 
    public List<ChatAvatar> avatars = new List<ChatAvatar>();
    // testing index
    int indext = 0;
    void Start()
    {
        LoadInitialConfig();
        Connect();
        LoadPlayerPrefs();
    }

    void LoadInitialConfig(){
        string user = PlayerPrefs.GetString(InitialConfiguration.Settings.UserName.ToString(),"");
        string pass = PlayerPrefs.GetString(InitialConfiguration.Settings.Password.ToString(),"");
        string chan = PlayerPrefs.GetString(InitialConfiguration.Settings.Channel.ToString(),"");

        username=user;
        password=pass;
        channel=chan;
    }

    public void LoadPlayerPrefs(){     
        string maxNumAvatars = PlayerPrefs.GetString(UIController.Settings.MaxNumberOfAvatars.ToString(),"10");
        string avatarLifetime = PlayerPrefs.GetString(UIController.Settings.AvatarLifeTime.ToString(),"1");
        string maxChatMessage = PlayerPrefs.GetString(UIController.Settings.MaxChatMessages.ToString(),"20");
        int showChat = PlayerPrefs.GetInt(UIController.Settings.IsShowChat.ToString(),0);
        MaxNumberOfAvatars = Convert.ToInt32(maxNumAvatars);
        LifeTimeInMinutes= Convert.ToDouble(avatarLifetime);
        MaxNumbersOfMessagesOnChatBox= Convert.ToInt32(maxChatMessage);
        if(showChat==1){
            ShowChat(true);
        }else{
            ShowChat(false);
        }
    }

    void ShowChat(bool showChat){
        chatBox.enabled=showChat;
    }
 
    void Update()
    {
        // if not connected, reconnect
        if (!twitchClient.Connected)
        {
            Connect();
        }
 
        // testing
        if (Input.GetKeyDown(KeyCode.A))
        {
            ConvertText("testing" + indext, "Testing Chat");
            CheckAvatars("testing" + indext);
            indext++;
 
        }
 
        ProcessChat();       
    }

    void CheckAvatarTimes(){
        DateTime now = DateTime.Now;
        foreach(ChatAvatar avatar in avatars){
           TimeSpan span = now.Subtract ( avatar.LastMessage );
           if(span.TotalMinutes>=LifeTimeInMinutes){
               GameObject avatarToDelete = GameObject.Find(avatar.UserName);
               if(avatarToDelete!=null){
                   avatar.isActive=false;
                   avatarToDelete.transform.Find("Canvas").gameObject.SetActive(false);
                   avatarToDelete.transform.Find("Sprite").gameObject.SetActive(false);
               }
           }
          
        }
    }
 
 
    IEnumerator GetTexture(string url,string chatName)
    {
        // find the emote texture
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
 
        // set it to an image, and spawn a particle with that image
        Texture2D img = DownloadHandlerTexture.GetContent(www);
        GameObject emotePart = Instantiate(emotePrefab, new Vector3(0,0,0), startPoint.rotation);
        emotePart.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.mainTexture = img;
        GameObject avatar = GameObject.Find(chatName);
        if(avatar!=null){
            emotePart.transform.parent = avatar.transform;  
            emotePart.transform.localPosition = Vector3.zero;
                      
        }
        Destroy(emotePart,2);
 
    }
 
    private void Connect()
    {
        // log into the twitch chat of a channel
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());
        writer.WriteLine("PASS " + password);
        writer.WriteLine("NICK " + username);
        writer.WriteLine("USER " + username + " 8 * :" + username);
        writer.WriteLine("JOIN #" + channel);
        // add the capabilities for more info
        writer.WriteLine("CAP REQ :twitch.tv/tags");
        writer.WriteLine("CAP REQ :twitch.tv/commands");
        writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.Flush();
    }
 
    private void ProcessChat()
    {
        // check if there is any data to read
        if (twitchClient.Available > 0)
        {
            string message = reader.ReadLine();
            print(message);
            // if its a chat message and not an event, it has PRIVMSG in it
            if (message.Contains("PRIVMSG"))
            {
                
                // check if chatter is a subscriber
                if (message.Contains("@badge-info=subscriber"))
                {
                    print("this person is a subscriber");
                }
                // custom reward
                if (message.Contains("custom-reward-id=" + custom_rewards_id))
                {
                    print("Redeemed a custom reward");
                }
                // get the name 
                string[] stringSeparators = new string[] { "display-name=" };
                string[] result = message.Split(stringSeparators, StringSplitOptions.None);
                var splitPoint = result[1].IndexOf(";", 0);
                var chatName = result[1].Substring(0, splitPoint);
 
                //Get the users message by splitting it from the string
                string[] stringSeparators2 = new string[] { "PRIVMSG" };
                string[] result2 = message.Split(stringSeparators2, StringSplitOptions.None);
                var splitPoint2 = result2[1].Split(':');
                string chatText = splitPoint2[1];
                //output the username and chat message
                ConvertText(chatName, chatText);
                CheckAvatars(chatName);
                // if the emotes list is not empty, get the emote texture
                if (!message.Contains("emotes=;"))
                {
                    string[] stringSeparatorsE = new string[] { "emotes=" };
                    string[] resultE = message.Split(stringSeparatorsE, StringSplitOptions.None);
                    // split the emote string in case of multiple emotes
                    var splitPointallEmotes = resultE[1].IndexOf(";", 0);
                    var allemotes = resultE[1].Substring(0, splitPointallEmotes);
                    string[] seperateEmotes = allemotes.Split('/');
                    // grab all emote textures
                    for (int i = 0; i < seperateEmotes.Length; i++)
                    {
                        var id = seperateEmotes[i].IndexOf(":", 0);
                        var emoteID = seperateEmotes[i].Substring(0, id);
                        // 1.0 / 2.0 / 3.0 is texture sizes
                        StartCoroutine(GetTexture("https://static-cdn.jtvnw.net/emoticons/v1/" + emoteID + "/3.0",chatName));
                    }
                }
                //Check for custom actions
                if(message.ToLower().Contains("!jump")){
                    SendActionToAvatar("jump",chatName);
                }
            }
            //reply to ping to stay connected
            if (message.Contains("PING :tmi.twitch.tv"))
            {
                writer.WriteLine("PONG " + "tmi.twitch.tv" + "\r\n");
                writer.Flush();
                print("replied");
            }
        }
    }

    void SendActionToAvatar(string command, string chatName){
        GameObject avatar = GameObject.Find(chatName);
        if(avatar!=null){
            avatar.GetComponent<ChatCommandsToActions>().SendCommand(command);                      
        }
    }
 
    void ConvertText(string chatName, string chatText)
    {
        if(currentNumberOfMessages>=MaxNumbersOfMessagesOnChatBox){
            chatBox.text="";
            currentNumberOfMessages=0;
        }
        // set the username and text to the text box
        chatBox.text += "<color=blue>" + chatName + "</color> :" + chatText + "\n";
        currentNumberOfMessages++;
        
    }
 
    ChatAvatar AddNewAvatar(string UserName)
    {
        // set avatar data
        ChatAvatar newAv = new ChatAvatar();
        newAv.UserName = UserName;
        newAv.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        return newAv;
    }
    void CheckAvatars(string name)
    {
        DateTime currentTime = DateTime.Now;
        // check if avatar is already in list
        if (avatars.Any(x => x.UserName == name))
        {
            ChatAvatar avatar = avatars.Where(x=> x.UserName==name).FirstOrDefault();
            if(avatar!=null){
                avatar.LastMessage=currentTime;
                GameObject ava = GameObject.Find(avatar.UserName);
                if(ava!=null){
                    avatar.isActive=true;
                    ava.transform.Find("Canvas").gameObject.SetActive(true);
                    ava.transform.Find("Sprite").gameObject.SetActive(true);
                }
            }
            return;
        }
        // otherwise add it and a spawn an avatar
        else
        {
            ChatAvatar newAvatar = AddNewAvatar(name);
            newAvatar.LastMessage=currentTime;
            newAvatar.isActive=true;
            avatars.Add(newAvatar);
            SpawnNewAv(newAvatar);
            int activeAvatars = avatars.Where(x=> x.isActive==true).Count();      
            if(activeAvatars>=MaxNumberOfAvatars){
                CheckAvatarTimes();
            }            
        }
    }
 
    void SpawnNewAv(ChatAvatar data)
    {
        // spawn a new avatar from startPoint, and set its color, username, and give it a little push
        GameObject spawned = Instantiate(avatarSpawn);
        spawned.name=data.UserName;
        spawned.transform.position = startPoint.transform.position;
        spawned.GetComponent<Renderer>().material.color = data.color;
        spawned.GetComponentInChildren<TextMeshProUGUI>().text = data.UserName;
        spawned.GetComponentInChildren<TextMeshProUGUI>().color = data.color;
        spawned.GetComponentInChildren<SpriteRenderer>().color = data.color;
        //spawned.GetComponent<Rigidbody>().velocity = Vector3.right * UnityEngine.Random.Range(0.5f, 1.5f);
    }
 
}
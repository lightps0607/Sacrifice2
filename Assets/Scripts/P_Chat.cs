using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using Photon.Pun;
using System;

public class P_Chat : MonoBehaviour, IChatClientListener
{

    public ChatClient chatClient;

    //public GameObject PlayerRef;
    private PhotonView MyPV;
    private string MyName;
    public Text RoomName;
    public Text Status;
    string ComChannel;
    public InputField MsgInput;
    public Text MsgArea;

    public Dropdown S_Dropdown;
    List<string> S_DropOptions = new List<string> { "All" };
    private string Recepient = "All";

    public GameObject UnreadIndicator;
    public Text UnreadMsg;
    public int UnreadMsgCount;



    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true; //Prevents Disconnection


        if (string.IsNullOrEmpty(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat))
        {
            Debug.Log("No Chat ID Provided!!!");
            return;
        }

        MyPV = GetComponentInParent<PhotonView>(); //PlayerRef.GetComponent<PhotonView>();
        MyName = MyPV.Owner.NickName;
        print("Player Chat Name = " + MyName);

        Status.text = "Connecting...";
        ComChannel = PhotonNetwork.CurrentRoom.Name;    //Communincation Channel == RoomName
        
        

        GetConnected();

        S_Dropdown.ClearOptions();

    }

    private void GetConnected()
    {
        print("Trying to Connect...");
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion, new AuthenticationValues(MyName) );
        Status.text = "Connecting to Chat...";
    }

    // Update is called once per frame
    void Update()
    {
        if (chatClient == null)
            return;

        chatClient.Service();

    }




    public void DebugReturn(DebugLevel level, string message)
    {
        
    }

    public void OnChatStateChange(ChatState state)
    {
        
    }

    public void OnConnected()
    {
        Status.text = "Connected!!!";
        RoomName.text = ComChannel;
        print("Connected!!!");

        chatClient.Subscribe(ComChannel);    //(ComChannel, creationOptions: new ChannelCreationOptions { PublishSubscribers = true });
        chatClient.SetOnlineStatus(ChatUserStatus.Online);

    }

    public void OnDisconnected()
    {
        Debug.LogError("CHAT DISCONNECTED");


        //Retry if gets disconnected accidentally
        if (PhotonNetwork.InRoom)
        {
            GetConnected();
            Debug.LogWarning("RECONNECTING...");
        }
         
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            if (senders[i] == MyName)
            {
                MsgArea.text += "ME" + ": " + messages[i] + "\n ";
            }
            else
            {
                MsgArea.text += senders[i] + ": " + messages[i] + "\n ";
            }
            
        }

        //Unread Message Indicator
        AddUnreadNotify();
            



        //Sets Dropdown Options
        foreach (string user in senders)
        {
            if (!S_DropOptions.Contains(user)   &&    user != MyName)
            {
                S_DropOptions.Add(user);
                S_Dropdown.ClearOptions();
                S_Dropdown.AddOptions(S_DropOptions);
            }
        }

        
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (sender == MyName)
        {
            MsgArea.text += "*(DM To) " + Recepient + ": " + message + "\n ";
        }
        else
        {
            MsgArea.text += "*(DM From) " + sender + ": " + message + "\n ";
        }

        //Unread Message Indicator
        AddUnreadNotify();
        
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        print("Normal Subbed!!!");

        foreach (string channel in channels)
        {
            chatClient.PublishMessage(channel, " Joined the Chat."); // you don't HAVE to send a msg on join but you could.

        }
        Status.text = "Channel Active";


    }

    public void OnUnsubscribed(string[] channels)
    {
        print("Normal UNSubbed!!!");
    }

    public void OnUserSubscribed(string channel, string user)
    {
        print(" Subscribed USER NAME - " + user);
        //S_DropOptions.Add(user);
        //S_Dropdown.AddOptions(S_DropOptions);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        print("User UNSubbed!!!");
    }



   public void sendMsg()
    {

        if (MsgInput.text == "")    //== null Doesn't work
            return;

        if (Recepient == "All")
        {
            print("Sending Public...");
            chatClient.PublishMessage(ComChannel, MsgInput.text);
        }

        else
        {
            print("Sending Private...");
            chatClient.SendPrivateMessage(Recepient, MsgInput.text);
        }

        MsgInput.text = null;

        //if(Application.platform != RuntimePlatform.Android)   //Refocus InputField on sent
        //    MsgInput.ActivateInputField();

    }



    public void OnDropOptionChanged()
    {
        //print(S_Dropdown.options[S_Dropdown.value].text);
        Recepient = S_Dropdown.options[S_Dropdown.value].text;
        print(Recepient);
    }

    
    //Adds Unread Notification
    void AddUnreadNotify()
    {
        if (!transform.GetChild(0).gameObject.activeInHierarchy)
        {
            UnreadIndicator.SetActive(true);

            UnreadMsgCount++;
            if (UnreadMsgCount > 9)
                UnreadMsg.text = "9+";
            else
                UnreadMsg.text = UnreadMsgCount.ToString();

        }
    }


}

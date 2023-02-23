using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

public class S_VivoxManager : MonoBehaviour
{

    VivoxVoiceManager VM;

    Client MyClient = new Client();



    string UserName = "LightPS";
    string MyChannelName = "MyChannel";

    private List<ChannelId> AllCID = new List<ChannelId>();


    private void Awake()
    {     

        VM = VivoxVoiceManager.Instance;


        //if (VM.LoginState == LoginState.LoggedIn)
        //{
        //    Debug.LogWarning("Already Logged in in Vivox...");
        //    Destroy(this.gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(this.gameObject);

        VivoxLogout();

        MyClient.Uninitialize();
        MyClient.Initialize();
        VM.OnUserLoggedInEvent += OnLoggedIn;

    }

    private void OnDisable()
    {
        if (VM.LoginState == LoginState.LoggedIn)
        {
            VM.Logout();
        }

        MyClient.Uninitialize();
    }

    private void Start()
    {

        if (VM.LoginState == LoginState.LoggedIn)
        {
            Debug.LogWarning("CANT START - Already Logged in in Vivox...");
            return;
        }

        Invoke("VivoxLogin", 5f);
        
    }



    //private void OnApplicationQuit()
    //{
    //    if (VM.LoginState == LoginState.LoggedIn)
    //    {
    //        VM.Logout();
    //    }

    //    MyClient.Uninitialize();

    //}



    public void VivoxLogin()
    {
        VM.Login(UserName);
    }


    public void VivoxLogout()
    {
        if (VM.LoginState == LoginState.LoggedIn)
        {
            VM.Logout();
        }
    }


    public void JoinChannel()
    {
        VM.JoinChannel(MyChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly);
        VM.JoinChannel("CH-1", ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly, false);
        VM.JoinChannel("CH-2", ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly, false);

        Debug.LogWarning("Joined Channel = " + MyChannelName);

       foreach (ChannelId CID in VM.ActiveChannels.Keys)
       {
            Debug.LogWarning("Adding Channel = " + CID.Name);
            AllCID.Add(CID);

            //IChannelSession CSession = VM.ActiveChannels[CID];

            //VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, CID);
        }

        Debug.LogWarning(VM.ActiveChannels.Count);

        Invoke("SwitchChannel", 2f);

        Debug.LogWarning("Transmitting in = " + VM.TransmittingSession.Channel.Name);

    }







    private void OnLoggedIn()
    {
        Debug.LogWarning("Vivox Session ID = " + VM.LoginSession.LoginSessionId.DisplayName);

        Invoke("JoinChannel", 2f);

    }


    // For Twst Only
    void SwitchChannel()
    {
        VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, AllCID[0]);
        Debug.LogWarning("Transmitting in = " + VM.TransmittingSession.Channel.Name);

        Invoke("SwitchChannel_2", 5f);

    }

    void SwitchChannel_2()
    {
        VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, AllCID[1]);
        Debug.LogWarning("Transmitting in = " + VM.TransmittingSession.Channel.Name);

    }


}

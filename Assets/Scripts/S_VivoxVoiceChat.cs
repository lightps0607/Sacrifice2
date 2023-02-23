using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VivoxUnity;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif


public class S_VivoxVoiceChat : MonoBehaviour
{


    //Inputs
    [SerializeField] InputAction MUTE;
    //[SerializeField] InputAction DEAFEN;
    [SerializeField] InputAction P1;
    [SerializeField] InputAction P2;
    [SerializeField] InputAction P3;
    [SerializeField] InputAction P4;


    //Mute
    private bool IsMute;
    [SerializeField] Image MuteIMG;
    [SerializeField] Sprite MuteIcon;
    [SerializeField] Sprite UnmuteIcon;


    VivoxVoiceManager VM;
    Client MyClient = new Client();

    string UserName = "NoName";
    string MyChannelName = "NoChName";

    private List<ChannelId> AllCID = new List<ChannelId>();


    //List<AU_PC> AllPRef = new List<AU_PC>();


    List<AU_PC> AllOtherPref = new List<AU_PC>();
    //AU_PC[] AllPRef;
    [SerializeField] Text[] AllPlayerNames;
    [SerializeField] Image[] AllPlayerImages;
    [SerializeField] GameObject[] TargetTalkImg;


    //Speech Detection
    [SerializeField] GameObject[] AllSpeechImages;
    List<IParticipant> AllParticipants = new List<IParticipant>();





    private void Awake()
    {
        //IF Can't Access Mic, Disable this

#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            gameObject.SetActive(false);
            return;
        }
#endif

        if (Microphone.devices.Length <= 0)
        {
            Debug.LogError("No Microphone Found : Vivox Voice Chat Disabling...");
            gameObject.SetActive(false);
            return;
        }



        MUTE.performed += OnMute;
        //DEAFEN.performed += OnDeafen;

        P1.started += OnP1;
        P2.started += OnP2;
        P3.started += OnP3;
        P4.started += OnP4;

        P1.canceled += OnReleased;
        P2.canceled += OnReleased;
        P3.canceled += OnReleased;
        P4.canceled += OnReleased;


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


    private void OnEnable()
    {
        MUTE.Enable();
        //DEAFEN.Enable();

        P1.Enable();
        P2.Enable();
        P3.Enable();
        P4.Enable();
    }

    private void OnDisable()
    {
        MUTE.Disable();
        //DEAFEN.Disable();

        P1.Disable();
        P2.Disable();
        P3.Disable();
        P4.Disable();




        if (VM.LoginState == LoginState.LoggedIn)
        {
            VM.Logout();
        }

        MyClient.Uninitialize();


    }


    void Start()
    {

        Invoke("Init", 7);

    }




    void Init()
    {

       AU_PC[] AllPRef = FindObjectsOfType<AU_PC>();

        foreach (AU_PC PRef in AllPRef)
        {
            if (PRef.PV.IsMine)
            {
                UserName = PRef.PV.Owner.NickName;
                VivoxLogin();   //Login using "UserName"
            }

            else
                AllOtherPref.Add(PRef);

        }

        if (AllOtherPref.Count < 1)
            return;

        for (int i = 0; i < AllOtherPref.Count; i++)
        {
            AllPlayerNames[i].text = AllOtherPref[i].PV.Owner.NickName;
            AllPlayerImages[i].color = AllOtherPref[i].MyColor;
        }

    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Speech Detection

    private void InitAllParticipants()
    {

        AllParticipants.Clear();

        foreach (var item in VM.LoginSession.ChannelSessions[AllCID[0]].Participants.Keys)
        {
            Debug.LogWarning(VM.LoginSession.ChannelSessions[AllCID[0]].Participants[item].SpeechDetected); //Name used for login

            AllParticipants.Add(VM.LoginSession.ChannelSessions[AllCID[0]].Participants[item]);

        }


        Debug.LogWarning("Total Parcipants = " + VM.LoginSession.ChannelSessions[AllCID[0]].Participants.Count);




        if (AllParticipants.Count < 1 )  //Reinit if there's 0 Participants added
            Invoke("InitAllParticipants", 10f);

        else
        {
            //Sorting AllParticipants to Match/Align with AllPlayerNames    ***** ( AllParticipants[1] == AllNames[0]  as  AllParticipants[0] = self) *****

            List<IParticipant> TempAllPar = new List<IParticipant>();
            foreach (var item in AllParticipants)
            {
                TempAllPar.Add(item);
            }


            for (int i = 0; i < AllParticipants.Count; i++)
            {
                if (i == 0)     //AllParticipants[0] = Self (Username);
                {
                    foreach (var item in TempAllPar)
                    {
                        if (UserName == item.Account.DisplayName)
                            AllParticipants[i] = item;
                    }
                }

                else
                    foreach (var item in TempAllPar)
                    {
                        if (AllPlayerNames[i - 1].text == item.Account.DisplayName)   // when i == 1, AllNames[1-1 = 0];
                            AllParticipants[i] = item;
                    }

            }


            Invoke("UpdateSpeechDetect", 2f);
        }
           
    }

    private void UpdateSpeechDetect()
    {
        for (int i = 0; i < AllParticipants.Count; i++)
        {
            Debug.Log(i + "_" + AllParticipants[i].Account.DisplayName + ",  ST = " + AllParticipants[i].SpeechDetected);

            AllSpeechImages[i].SetActive(AllParticipants[i].SpeechDetected);
        }

        Invoke("UpdateSpeechDetect", 1f);
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Mute/Deafen

    void OnMute(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Performed)
        {
            ToggleMute();

        }
        
    }

    //void OnDeafen(InputAction.CallbackContext context)
    //{
    //    if (!MyPV.IsMine)
    //        return;


    //    if (context.phase == InputActionPhase.Performed)
    //    {
    //        ToggleDeafen();

    //    }


    //}


    void ToggleMute()
    {
        if(VM.LoginSession.TransmissionType == TransmissionMode.None)
        {
            VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, AllCID[0]);    //Joins GlobalChannel

            IsMute = false;
            MuteIMG.sprite = UnmuteIcon;
        }
            
        else
        {
            VM.LoginSession.SetTransmissionMode(TransmissionMode.None);

            IsMute = true;
            MuteIMG.sprite = MuteIcon;
        }
            
    }


    //void ToggleDeafen()
    //{
    //    foreach(P_VoiceChat V_Ref in AllV_Ref)      //Mutes all player, Later can change to Set Vol(AudioSource) for each player
    //    {
    //        V_Ref.MySpeaker.enabled = !V_Ref.MySpeaker.enabled;
    //    }
    //}

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Player Buttons
    void OnP1(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Started)
        {
            print("Holding P1");

            PrivateChat(0);

        }


    }


    void OnP2(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Started)
        {
            print("Holding P2");

            PrivateChat(1);

        }

    }


    void OnP3(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Started)
        {
            print("Holding P3");

            PrivateChat(2);
        }

    }


    void OnP4(InputAction.CallbackContext context)
    {
       
        if (context.phase == InputActionPhase.Started)
        {
            print("Holding P4");

            PrivateChat(3);
        }

    }



    void OnReleased(InputAction.CallbackContext context)
    {
        
        if (context.phase == InputActionPhase.Canceled)
        {
            print("Released Voice Button");
            foreach (var TTImg in TargetTalkImg)
                TTImg.SetActive(false);

            if (IsMute)  //if Muted release back to Mute
            {
                VM.LoginSession.SetTransmissionMode(TransmissionMode.None);
                Debug.LogWarning("Muted Back!");
                return;
            }

            VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, AllCID[0]);    //0 = First joined Channel == GlobalChannel == RoomNameChannel
            Debug.LogWarning("Transmitting in = " + VM.TransmittingSession.Channel.Name);
        }

    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 

    void PrivateChat(int PlayerId)
    {
        //Debug.Log(AllCID[PlayerId].Name);
        //Debug.LogWarning("Total Channels = "+ VM.ActiveChannels.Count + " vs " + VM.LoginSession.ChannelSessions.Count);
        //Debug.LogWarning( "Total Participants = " +  VM.LoginSession.ChannelSessions[AllCID[PlayerId]].Participants.Count);


        ////Shows All Participants/Users
        //foreach (var item in VM.LoginSession.ChannelSessions[AllCID[PlayerId]].Participants.Keys)
        //{
        //    Debug.LogWarning(VM.LoginSession.ChannelSessions[AllCID[PlayerId]].Participants[item].Account.DisplayName); //Name used for login

        //}


        //Finds P1P2 and Transmit in the channel
        foreach (var Id in AllCID)
        {
            if (UserName + AllPlayerNames[PlayerId].text == Id.Name)
            {
                VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, Id);
                Debug.LogWarning("Transmitting in = " + VM.TransmittingSession.Channel.Name);
                TargetTalkImg[PlayerId].SetActive(true);
                return;
            }
        }


        
        

    }






    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////   Vivox
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
        if (VM.LoginState == LoginState.LoggedIn)
        {
            Debug.LogWarning("CANT START - Already Logged in in Vivox...");
            return;
        }

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
        MyChannelName = PhotonNetwork.CurrentRoom.Name; //GlobalChannel

        VM.JoinChannel(MyChannelName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly);   //Global Channel (MyChannelName == RoomName)

        //foreach (AU_PC PRef in AllPRef)
        //{
        //    if (PRef.PV.IsMine)
        //    {
        //        VM.JoinChannel(PRef.PV.Owner.NickName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly);

        //        Debug.LogWarning("Joined Channel = " + MyChannelName);
        //    }


        //}


        foreach (AU_PC PRef in AllOtherPref)
        {
            if (!PRef.PV.IsMine)
            {
                VM.JoinChannel(UserName + PRef.PV.Owner.NickName, ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly, false);    //Joins P1P2    [To Transmit]
                VM.JoinChannel(PRef.PV.Owner.NickName + UserName , ChannelType.NonPositional, VivoxVoiceManager.ChatCapability.AudioOnly, false);   //Joins P2P1    [To Listen]

                Debug.LogWarning("Joined Channel = " + MyChannelName);
            }


        }




        foreach (ChannelId CID in VM.ActiveChannels.Keys)
        {
            Debug.LogWarning("Adding Channel = " + CID.Name);
            AllCID.Add(CID);

            //IChannelSession CSession = VM.ActiveChannels[CID];

            //VM.LoginSession.SetTransmissionMode(TransmissionMode.Single, CID);
        }

        Debug.LogWarning(VM.ActiveChannels.Count);

        //Invoke("SwitchChannel", 2f);

        Debug.LogWarning("Transmitting in = " + VM.TransmittingSession.Channel.Name);



        InitAllParticipants();

    }







    private void OnLoggedIn()
    {
        Debug.LogWarning("Vivox Session ID = " + VM.LoginSession.LoginSessionId.DisplayName);

        Invoke("JoinChannel", 2f);

    }


    // For Test Only
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

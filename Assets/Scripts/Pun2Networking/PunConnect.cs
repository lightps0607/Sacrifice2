using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PunConnect : MonoBehaviourPunCallbacks
{

    [SerializeField] Text P_Status;
    [SerializeField] Text C_Status;
    [SerializeField] Text Reg_Status;
    [SerializeField] Dropdown RegDrop;
    [SerializeField] Button CreateButton;

    bool RegChanged;

    private void Start()
    {
        //Debug.Log(Application.version);

        //TO HANDLE LAG: PhotonSerializeView
        //PhotonNetwork.SendRate = 40; //Default = 20
        //PhotonNetwork.SerializationRate = 5; //Default = 10
        //+[Also use Classic "PhotonTransformViewClassic"->Dis - 10, "Estimated speed"]

        StartCoroutine(ConnectToServer(0.1f));
    }


    private void Update()
    {
        P_Status.text = "Ping - " + PhotonNetwork.GetPing() + "ms";
    }
    
    //Executes After the "time". [Works like "Delay"]
    IEnumerator ConnectToServer(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        if (PhotonNetwork.IsConnected) //Disconnect and then Reconnect if Already Connected
        {
            if (RegChanged) //Don't reconnect in switching region
            {
                yield break;
            }

            PhotonNetwork.Disconnect();
            print("Disconnecting...");
            StartCoroutine(ConnectToServer(0.1f));
        }

        if (!PhotonNetwork.IsConnected)
        {
            print("Connecting to Photon Server...");
            C_Status.text = "Connecting to Server...";


            //if (RegChanged) //Connect to the Custom Region if selected
            //{
            //    OnRegionChanged();
            //    yield break;
            //}

            PhotonNetwork.AutomaticallySyncScene = true; //Load level for all players at once from MasterClient
            PhotonNetwork.NickName = MasterManager.GSettings.NickName;
            PhotonNetwork.GameVersion = MasterManager.GSettings.GameVersion; //Application.version;  //Connects only to the Matching GameVersion
            PhotonNetwork.ConnectUsingSettings(); //Region or Other Settings can be added PhotonNetwork.Connect...
        }
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon.", this);
        C_Status.text = "CONNECTED";
        Reg_Status.text = "Region: " + PhotonNetwork.CloudRegion;

        CreateButton.interactable = true;

        print(PhotonNetwork.LocalPlayer.NickName);
        print(MasterManager.GSettings.GameVersion);

        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Failed to Connect: " + cause.ToString());

        CreateButton.interactable = false;

        //Reconnects if connection fails    [Also Works for Max CCU Limit]
        if (cause != DisconnectCause.DisconnectByClientLogic && cause != DisconnectCause.DisconnectByServerLogic)       
        {
            Debug.LogWarning("RECONNECTING...");
            C_Status.text = "RECONNECTING...";

            StartCoroutine(ConnectToServer(5f));

            //Start and Show a UI Timer for Connection Status
            //...Pending (Work In Progress)
        }
    }



    public void OnRegionChanged()
    {
        RegChanged = true;

        Reg_Status.text = "Switching Region...";

        Debug.LogWarning(RegDrop.options[RegDrop.value].text);
        PhotonNetwork.Disconnect();
        PhotonNetwork.ConnectToRegion(RegDrop.options[RegDrop.value].text);
    }


}

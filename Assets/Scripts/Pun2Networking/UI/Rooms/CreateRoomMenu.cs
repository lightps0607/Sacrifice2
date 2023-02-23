using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    //VARIABLES
    [SerializeField] private InputField _roomName;
    private RoomCanvases _roomCanvases;

    [SerializeField] InputField PlayerName;


    //METHODS or FUNCTIONS
    public void FirstInitialize(RoomCanvases canvases)
    {
        _roomCanvases = canvases;

        PlayerName.text = MasterManager.GSettings.NickName;
    }


    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnectedAndReady || PhotonNetwork.InRoom) //If Not Connected or Already in Room
            return;
        if (_roomName.text == "")   //== null Doesn't work
            return;

        if (Application.isMobilePlatform)   //Add _MD (Mobile Device) Suffix
            _roomName.text += "_m";


        //JoinOrCreateRoom
        RoomOptions ROptions = new RoomOptions();
        ROptions.MaxPlayers = 5; //Later-Will Be able to choose from UI later
        //ROptions.CleanupCacheOnLeave = false;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, ROptions, TypedLobby.Default);

        _roomName.text = null;  //Double Safety : Clears InputField
    }


    public override void OnCreatedRoom()
    {
        Debug.Log("Created Room Successfully.", this);

        //PhotonNetwork.LoadLevel(1);
        _roomCanvases.CRCanvas.Show(); //Shows CurrentRoom PlayerListingMenu

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Room Creation Failed: " + message, this);
    }


    //Sets PlayerName
    public void SetPlayerName()
    {
        MasterManager.GSettings._nickName = PlayerName.text;    //Saves The Name
        PhotonNetwork.NickName = MasterManager.GSettings.NickName;  //New NickName (+RandomNumber)
        print(PhotonNetwork.LocalPlayer.NickName);

        PlayerName.text = PhotonNetwork.NickName;   //Just In Case for Android
    }

}

using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    //VARIABLES
    [SerializeField] private Transform _content;
    [SerializeField] private PlayerListing _playerListingPF;
    [SerializeField] private Text _readyUpText;

    private List<PlayerListing> PList = new List<PlayerListing>();
    private RoomCanvases _roomCanvases;
    private bool _ready = false;


    public bool ToggleStart;
    [SerializeField] Button StartButton;
    [SerializeField] Text PCount;
    [SerializeField] Text RoomName;



    //METHODS
    public override void OnEnable()
    {
        base.OnEnable();

        RoomName.text = PhotonNetwork.CurrentRoom.Name;
        ToggleStartButton();
        StartButton.gameObject.SetActive(PhotonNetwork.IsMasterClient); //Only enables Start for Master

        SetReadyUp(false);
        GetCurrentRoomPlayers();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        for (int i = 0; i < PList.Count; i++)
            Destroy(PList[i].gameObject);

        PList.Clear();
     
    }


    public void FirstInitialize(RoomCanvases canvases)
    {
        _roomCanvases = canvases;
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        if (_ready)
            _readyUpText.text = "Ready!";
        else
            _readyUpText.text = "Not Ready!";
    }


    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach (KeyValuePair<int, Player> PInfo in PhotonNetwork.CurrentRoom.Players)
            {
                AddPlayerListing(PInfo.Value);
            }
    }


    private void AddPlayerListing(Player player)
    {
        int index = PList.FindIndex(x => x._Player == player);
        if (index != -1)
        {
            PList[index].SetPlayerInfo(player);
        }
        else
        {

            PlayerListing PListing = Instantiate(_playerListingPF, _content);
            if (PListing != null)
                PListing.SetPlayerInfo(player);
            PList.Add(PListing);

        }
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //All Players Leaves if MasterClient/Host Leaves
        _roomCanvases.CRCanvas._LeaveRoomMenu.OnClick_LeaveRoom();
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);

        ToggleStartButton();

    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = PList.FindIndex(x => x._Player == otherPlayer);
        if (index != -1)
        {
            Destroy(PList[index].gameObject);
            PList.RemoveAt(index);

            ToggleStartButton();
        }
    }


    public void OnClick_StartGame()
    {
        //Ready Status [***Will be removed for "Sacrifice"]
        // Also Remove "OnMasterClientSwitched()" above in this PlayerListingMenu
        if (PhotonNetwork.IsMasterClient)
        {

            //Players "Ready Up" Check
            //for (int i = 0; i< PList.Count; i++)
            //{
            //    if(PList[i]._Player != PhotonNetwork.LocalPlayer)
            //    {
            //        if (!PList[i].Ready)
            //            return;
            //    }
            //}

            //Close Room and Start Game [Or Open Temple Gate]
            PhotonNetwork.CurrentRoom.IsOpen = false; //Closes Room so that No one can Join in the middle of the game(after start)
            PhotonNetwork.CurrentRoom.IsVisible = false; //Makes the Room Invisible so it doesn't exist in Room List
            PhotonNetwork.LoadLevel(1); //Loads a Scene according to Build Settings Scene Index
        }

        //For Sacrifice Game Join Style
        //PhotonNetwork.LoadLevel(1); //+ "PhotonNetwork.AutomaticallySyncScene = true;" in "PunConnect.cs" "Start()"

    }


    public void OnClick_ReadyUp()
    {
        if(!PhotonNetwork.IsMasterClient)
        SetReadyUp(!_ready); //Toggles "_ready" values
        photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);

    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = PList.FindIndex(x => x._Player == player);
        if (index != -1)
            PList[index].Ready = ready;
    }




    private void ToggleStartButton()
    {
        PCount.text = PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;


        if (!ToggleStart || !PhotonNetwork.IsMasterClient)
            return;

        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            StartButton.interactable = true;
        else
            StartButton.interactable = false;

    }




}

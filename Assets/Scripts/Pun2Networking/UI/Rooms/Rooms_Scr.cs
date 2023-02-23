using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Rooms_Scr : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text Rname;
    [SerializeField] private Text Pcount;
    public RoomInfo RInfo { get; private set; }



    private void Update()
    {
        //NOT USED as RInfo isn't being updated every frame [need to set from "RoomListingMenu.cs"]

        //if (RInfo != null)
        //{
        //    Pcount.text = "(" + RInfo.PlayerCount + "/" + RInfo.MaxPlayers + ")";
        //}
        
    }

    public void SetRoomInfo(RoomInfo rInfo)
    {
        RInfo = rInfo;
        Rname.text = RInfo.Name;
        Pcount.text = "(" + RInfo.PlayerCount + "/" + RInfo.MaxPlayers + ")";

    }

    public void OnClick_Join()
    {
        PhotonNetwork.JoinRoom(RInfo.Name);        
    }

    //public override void OnJoinedRoom()
    //{
    //    PhotonNetwork.LoadLevel(1);
    //}

}

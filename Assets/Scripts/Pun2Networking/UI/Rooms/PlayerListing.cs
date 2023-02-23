using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private Text _text;

    public Player _Player { get; private set; }
    public bool Ready = false;

    public void SetPlayerInfo(Player player)
    {
        _Player = player;
        _text.text = player.NickName;


    }


}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class RaiseEventEx : MonoBehaviourPun
{
    //VARIABLES
     private SpriteRenderer _spriteRenderer;
    private const byte COLOR_CHNAGE_EVENT = 0;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == COLOR_CHNAGE_EVENT)
        {
            object[] datas = (object[])obj.CustomData;
            float r = (float)datas[0];
            float g = (float)datas[1];
            float b = (float)datas[2];
            _spriteRenderer.color = new Color(r, g, b, 1f);
        }
    }

    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Space))
            ChangeColor();
    }

    private void ChangeColor()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);

        _spriteRenderer.color = new Color(r, g, b, 1f);

        object[] datas = new object[] { r, g, b };
        PhotonNetwork.RaiseEvent(COLOR_CHNAGE_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);

    }

}
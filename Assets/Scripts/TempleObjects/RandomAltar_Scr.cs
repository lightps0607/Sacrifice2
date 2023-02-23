using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAltar_Scr : MonoBehaviour
{

    [SerializeField] PhotonView MyPV;
    [SerializeField] GameObject AltarSet;
    [SerializeField] bool RandomActive;

    private bool Opened;


    private void Start()
    {
        if (PhotonNetwork.IsMasterClient && RandomActive)
        {
            bool Status = (Random.value > 0.5f);
            MyPV.RPC("RPC_Active", RpcTarget.All, Status);

            Debug.Log("RandomAltar = " + Status);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient && Opened == false && collision.tag == "Player")
        {
            MyPV.RPC("RPC_Open", RpcTarget.All);
        }
        
    }

    
    [PunRPC]
    void RPC_Open()
    {
        Opened = true;
        AltarSet.SetActive(true);

        GetComponent<BoxCollider2D>().enabled = false;

        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.BGM_AltarAppear);
    }


    //Random Active on Start
    [PunRPC]
    void RPC_Active(bool Status)
    {
        gameObject.SetActive(Status);
    }

}

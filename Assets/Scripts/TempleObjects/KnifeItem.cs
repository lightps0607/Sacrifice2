using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeItem : MonoBehaviour
{

    public PhotonView PV;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }


    public void Interacting(AU_PC P_Ref)
    {
        if (P_Ref.HasKnife) //Can't pick if already has one
            return;


        P_Ref.HasKnife = true;
        P_Ref.ShootButton.SetActive(true);
        
        PV.RPC("DestroyObject", RpcTarget.AllBuffered);
    }



    [PunRPC]
    public void DestroyObject()
    {
        Destroy(gameObject);
    }



}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItem : MonoBehaviour
{
    public PhotonView PV;

    
    //private void Start()
    //{
    //    PV = GetComponent<PhotonView>();
    //}



    public void Interacting(AU_PC P_Ref)
    {
        P_Ref.AddCoins(1);

        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_CoinPick, transform.position);


        PV.RPC("DestroyObject", RpcTarget.AllBuffered);
    }



    [PunRPC]
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

}

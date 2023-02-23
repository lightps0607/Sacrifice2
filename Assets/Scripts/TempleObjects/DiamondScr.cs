using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondScr : MonoBehaviour
{
    PhotonView PV;
    private bool IsAttached = false;
    AU_PC PlayerRef;






    private void Start()
    {
        PV = GetComponent<PhotonView>();    //gameObject.GetPhotonView();
    }


    private void Update()
    {
        if (!IsAttached)
            return;
        transform.position = PlayerRef.transform.position;
    }





    public void Interacting(AU_PC P_Ref)
    {
        int PVID = P_Ref.PV.ViewID;

        PV.RPC("RPC_Pickup", RpcTarget.All, PVID);


    }


    [PunRPC]
    void RPC_Pickup(int PVID)
    {

        PlayerRef = PhotonView.Find(PVID).transform.gameObject.GetComponent<AU_PC>();
        //Debug.Log(PlayerRef.name + " = " + PlayerRef.PV.ViewID);


        if (!PlayerRef)
            return;


        if (transform.parent && transform.parent.tag == "Player")   //Detaches
        {
            transform.position = PlayerRef.attackHitBoxPos.position;
            transform.parent = null;
            IsAttached = false;

            PlayerRef.HasDiamond = false;

        }
        else    //Attaches
        {
            transform.position = PlayerRef.transform.position;
            transform.parent = PlayerRef.transform;
            IsAttached = true;
            
            PlayerRef.HasDiamond = true;

        }


    }
}

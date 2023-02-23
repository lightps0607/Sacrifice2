using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarChecker : MonoBehaviour
{

    PhotonView PV;

    public bool CanOpenDoor = false;

    public AU_DeadBodyScr DeadBodyRef;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        SacrificeButtonStatus(collision, true);

        SetCanOpenDoor(collision, true);
       
    }



    private void OnTriggerExit2D(Collider2D collision)
    {

        SacrificeButtonStatus(collision, false);

        SetCanOpenDoor(collision, false);

    }





    // Sets CanOpenDoor Variable
    private void SetCanOpenDoor(Collider2D collision, bool status)
    {
        if (CanOpenDoor != status)
        {
            if (collision.tag == "DeadBody")
            {
                CanOpenDoor = status;

                if (CanOpenDoor)
                    DeadBodyRef = collision.gameObject.GetComponent<AU_DeadBodyScr>();
                //else
                //    DeadBodyRef = null;

                Debug.Log("Has Sacrifice = " + CanOpenDoor);
            }
        }
    }


    //Sets Visibility of Self Sacrifice Button
    private void SacrificeButtonStatus(Collider2D collision, bool status)
    {
        if (collision.tag == "Player")
        {
            AU_PC PlayerRef = collision.GetComponentInParent<AU_PC>();
            PlayerRef.SacrificeButton.SetActive(status);
        }
    }




    public void OnUsed()
    {
        DeadBodyRef.DestroyObj();
        PV.RPC("RPC_OnUsed", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_OnUsed()
    {
        GetComponent<Collider2D>().enabled = false;
    }


}

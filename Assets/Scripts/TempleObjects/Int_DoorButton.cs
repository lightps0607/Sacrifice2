using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int_DoorButton : MonoBehaviour
{
   
    PhotonView ButtonPV;
    [SerializeField] GameObject[] DoorRef;

    private bool DoorStatus;


    public void Start()
    {
        ButtonPV = GetComponent<PhotonView>();
    }

    public void Interacting()
    {

        ////SFX
        //S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_LeverPull, transform.position);

        ButtonPV.RPC("RPC_Interact", RpcTarget.All, !DoorStatus);
        

    }


    [PunRPC]
    void RPC_Interact(bool Status)
    {

        DoorStatus = Status;

        foreach (GameObject Door in DoorRef)
        {
            Door.SendMessage("OpenDoor", Status);    //Calls and Forget OpenDoor function like Event Dispatcher
            Debug.LogWarning("Door = " + Status);
        }


        //Flips
        transform.localScale = transform.localScale * new Vector2(-1, 1);
        
        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_LeverPull, transform.position);


    }


}

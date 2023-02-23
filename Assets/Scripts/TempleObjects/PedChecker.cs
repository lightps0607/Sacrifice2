using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PedChecker : MonoBehaviour
{

    [SerializeField] PhotonView MyPV;

    //private Vector3 UID;  //For Raise Event


    private Pedestal[] AllPeds;
    private bool Opened = false;

    [SerializeField] GameObject[] DoorRef;

    [SerializeField] bool HoldType = false;

    [SerializeField] bool UsingAltar;
    [SerializeField] GameObject AltarRef = null;
    //private AltarChecker AltarCheckerRef;

    private bool DoorStatus;


    private void Awake()
    {
        //UID = transform.position;
        MyPV =  GetComponent<PhotonView>();
    }

    void Start()
    {
        AllPeds = GetComponentsInChildren<Pedestal>();
        
    }



    //private void OnEnable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    //}


    //private void OnDisable()
    //{
    //    PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    //}

    //private void NetworkingClient_EventReceived(EventData obj)    //RaiseEvent (Receive)
    //{
    //    if (obj.Code == 1)
    //    {
    //        object[] datas = (object[]) obj.CustomData;
    //        if ((Vector3)datas[0] == UID)
    //        {
    //            bool D_Status = (bool)datas[1];
    //            foreach (GameObject Door in DoorRef)
    //            {
    //                Door.SendMessage("OpenDoor", D_Status);    //Calls and Forget OpenDoor function like Event Dispatcher
    //                Debug.LogWarning("Door = " + D_Status);
    //            }
    //        }
    //    }
    //}




    void Update()
    {

        if (!PhotonNetwork.IsMasterClient)  //Only MasterClient Checks and Updates Doors for others through RPC
            return;


        if (!HoldType)
        {
            if (Opened)
                return;
        }
        

        //Check If all Pedestal are On
        foreach (Pedestal peds in AllPeds)
        {

            if (!peds.IsOn)
            {
                if (HoldType)
                {
                    if (!DoorStatus)    //Prevent Spam
                        return;

                    DoorStatus = false;

                    MyPV.RPC("RPC_ChangeDoorStatus", RpcTarget.All, DoorStatus);

                    //RE_ChangeDoorStatus(DoorStatus);

                    return;

                }
                return;
            }
        }

        //Check If Altar Has Sacrifice
        if (UsingAltar)
        {
            AltarChecker ACRef = AltarRef.GetComponent<AltarChecker>();
            if (!ACRef.CanOpenDoor)
                return;
        }


        if (DoorStatus)     //Prevent Spam
            return;

        DoorStatus = true;

        MyPV.RPC("RPC_ChangeDoorStatus", RpcTarget.All, DoorStatus);

        //RE_ChangeDoorStatus(DoorStatus);

        Opened = true;

        Invoke("OnOpenedDoor", 2f);

    }

    private void OnOpenedDoor()
    {
        if (UsingAltar)
        {
            AltarChecker ACRef = AltarRef.GetComponent<AltarChecker>();

            ACRef.OnUsed();
        }
    }




    //*****************     Currently Using : RPC    *********************



    [PunRPC]
    void RPC_ChangeDoorStatus(bool Status)
    {
        foreach (GameObject Door in DoorRef)
        {
            Door.SendMessage("OpenDoor", Status);    //Calls and Forget OpenDoor function like Event Dispatcher
            Debug.LogWarning("Door = " + Status);
        }
    }



    ////RaiseEvent (Send)

    //private void RE_ChangeDoorStatus(bool Status)
    //{

    //    object[] datas = new object[] { UID, Status };

    //    RaiseEventOptions REOptions = new RaiseEventOptions { CachingOption = EventCaching.DoNotCache, Receivers = ReceiverGroup.All };

    //    PhotonNetwork.RaiseEvent(1, datas, REOptions, SendOptions.SendReliable);
    //}




    

    //For RAISE EVENT Instead of RPC : If you don't want to use PhotonView or ViewId, Then Generate a Number Based on Unique Position, which must be synced for all clients.
    // Eg. Vector3 UID = transform.position;
    // object[] datas = newobject[]{UID, D_Status}; //Pass this through RaiseEvent();
    //if(UID == (Vector3)datas[0]) {Debug.Log("Working")};   //Check if same UID (Similar to checking ViewID)

    //***********   Position Must Be Different for all PedChecker   **************


}

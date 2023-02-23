using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_ChatSystem : MonoBehaviour
{
    
    //public AU_PC MyPC;

    private PhotonView MyPV;
    //private List<AU_PC> AllPC = new List<AU_PC> {};
    //private List<S_ChatSystem> All_CS = new List<S_ChatSystem> { };
    
    public string MyName;
    public Text Status;
    public InputField MsgInput;
    //public Text MsgArea;

    private Dropdown S_Dropdown;
    string[] S_DropOptions;
    Dictionary<string, AU_PC> PList = new Dictionary<string, AU_PC>();







    // Start is called before the first frame update
    void Start()
    {
        MyPV = GetComponentInParent<PhotonView>();

        MyName = MyPV.Owner.NickName;
        Invoke("SetAllPC", 2.0f);

    }

    // Update is called once per frame
    void Update()
    {
       //foreach (string PName in PList.Keys)
       //{
       //     print(PName);
       //}
        
    }


    void SetAllPC()
    {
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject Player in AllPlayers)
        {
            AU_PC Player_PC = Player.GetComponent<AU_PC>();
            //Debug.Log("Player is - " + Player.name + " -ID: " + Player_PC.PV.ViewID);

            PList.Add(Player_PC.PV.Owner.NickName, Player_PC);
            if (MyName == "")
            {
                MyName = Player_PC.PV.Owner.NickName;
            }

        }

        

    }


    public void SendPublicMsg()
    {
        foreach (string PName in PList.Keys )
        {
            //print("His Name is : " + PName);
            //print("His MSG is : " + PList[PName].PV.Owner.NickName);

            AU_PC TargetCS = PList[PName];

            TargetCS.PV.RPC("RPC_SendMsg", RpcTarget.All, new string[] {MyName, MsgInput.text });
        }
    }

    

}

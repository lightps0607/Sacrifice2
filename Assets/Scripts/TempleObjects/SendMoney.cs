using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendMoney : MonoBehaviour
{

    [SerializeField] AU_PC MyPlayer;

    public InputField MoneyInput;
    public Dropdown DD;
    public Button SB;

    List<AU_PC> PList= new List<AU_PC>();

    [SerializeField] MoneyNotify MNotifyPF;
    [SerializeField] Transform NotifyContents;

    void Start()
    {
        ToggleSendButton(); //Just in case Init button interactable

        Invoke("AddPlayers", 2);
    }

    
    void Update()
    {
        
    }

    
    void AddPlayers()
    {
        GameObject[] AllPObj = GameObject.FindGameObjectsWithTag("Player");

        List<string> PNames = new List<string>();

        foreach (GameObject PlayerObj in AllPObj)
        {
            AU_PC PlayerRef = PlayerObj.GetComponent<AU_PC>();

            if (!PlayerRef.PV.IsMine)
            {
                PList.Add(PlayerRef);
                PNames.Add(PlayerRef.PV.Owner.NickName);
            }
        }


        //Set DropDown
        DD.ClearOptions();
        DD.AddOptions(PNames);

    }


    void TransferMoney()
    {

        int SendAmount = Mathf.Abs(int.Parse(MoneyInput.text));     //convert to Absolute Int

        MyPlayer.Coins -= SendAmount;   //Deduct For Sender
        PList[DD.value].AddCoins(SendAmount);   //Add for Receiver


        //Notification
        MoneyNotify MNotify = Instantiate(MNotifyPF, NotifyContents);
        MNotify.Updateinfo("You", SendAmount);

        //***NEED TO UPDATE COIN TEXT FOR BOTH PLAYERS***
    }


    public void OnClickedSend()
    {
        TransferMoney();
        MoneyInput.text = "";   //Empties once submitted
    }


    public void ToggleSendButton()
    {
        if(!int.TryParse(MoneyInput.text, out int x))
        {
            SB.interactable = false;
            return;
        }


        int SendAmount = int.Parse(MoneyInput.text);
        bool CanSend = (MyPlayer.Coins >= SendAmount) && (DD.options.Count > 0);

        SB.interactable = CanSend;
    }

}

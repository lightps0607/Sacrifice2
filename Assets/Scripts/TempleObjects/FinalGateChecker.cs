using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGateChecker : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AU_PC PlayerRef = collision.GetComponent<AU_PC>();
            if (PlayerRef.HasDiamond)
            {
                Debug.Log(PlayerRef.PV.Owner.NickName + " WON!!!");


                GameObject[] AllPlayers =  GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject Player in AllPlayers )
                {
                    AU_PC Player_PC = Player.GetComponent<AU_PC>();
                    //Debug.Log("Player is - " + Player.name + " -ID: " + PlayerRef.PV.ViewID);

                    Player_PC.PV.RPC("OnGameOver", RpcTarget.All, PlayerRef.PV.ViewID);

                }
            }
        }


    }


    

    
}

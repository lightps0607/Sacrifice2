using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverConditions : MonoBehaviour
{

    [SerializeField] public GameObject LoseHUD;

    public int CurrentState = 0;
    int TotalDeadCount = 0;

    bool IsGameOver = false;


    void Update()
    {

        if (IsGameOver)
            return;


        GameObject[] AllPObj = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject PlayerObj in AllPObj)
        {
            AU_PC PlayerRef = PlayerObj.GetComponent<AU_PC>();

            if (PlayerRef.IsDead)
            {
                TotalDeadCount++;
            }
        }

        //print(CurrentState);
        FullGOCheck();
        

    }





    void FullGOCheck()
    {

        if (TotalDeadCount > 4)     //Game over if ALL DEAD at any moment.
        {
            GameOver();
        }


        CurrentStateCheck(0, 0);    //Must Remain 5 Players until GATE 1 Opens. If Kills in Lobby Game Over...
        CurrentStateCheck(1, 1);    //Must Remain 4 Players until GATE 3 Opens. //NOT USED in "TempleNew"
        CurrentStateCheck(2, 3);    //Must Remain 2 Players BEFORE Diamond is taken.
        CurrentStateCheck(3, 4);    //Must Remain 1 Players AFTER Diamond is taken.

        TotalDeadCount = 0;
    }



    void CurrentStateCheck(int C_State, int RqD_Count)
    {
        if (CurrentState == C_State)
        {
            if (TotalDeadCount > RqD_Count)
            {
                GameOver();
            }
        }
    }


    void GameOver()
    {
        IsGameOver = true;
        LoseHUD.SetActive(true);
        print("Game Over. Total Dead = " + TotalDeadCount);

        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.BGM_Lose);


        ////WinHUD
        //GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        //foreach (GameObject Player in AllPlayers)
        //{
        //    AU_PC Player_PC = Player.GetComponent<AU_PC>();
        //    //Player_PC.PV.RPC("OnGameOver", RpcTarget.All, PlayerRef.PV.Owner.NickName);

        //   //Show Win HUD
        //   Player_PC.LoseHUD.SetActive(true);

        //   //Detach All Attachments
        //   Player_PC.DetachAllActors(Player_PC);

        //}
    }


}

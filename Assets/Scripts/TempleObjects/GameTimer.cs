using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour, IPunObservable
{

    [SerializeField] Text TimerText;
    [SerializeField] GameObject LoseHUD;


    public float Second = 20;

    private bool IsGameOver;



    // Update is called once per frame
    void Update()
    {

        //Only for MasterClient
        if (PhotonNetwork.IsMasterClient && Second > 0)
        {
            Second -= Time.deltaTime;

        }


        //For All
        if (Second >= 0)
        {
            int mins = Mathf.FloorToInt(Second / 60F);
            int secs = Mathf.FloorToInt(Second - mins * 60);
            TimerText.text = string.Format("{0:00}:{1:00}", mins, secs);
        }

        if (Second <= 0)
            GameOver();

    }



    void GameOver()
    {
        if (IsGameOver)
            return;


        //***** This can be used independent to "GameChecker" *****

        //IsGameOver = true;
        //LoseHUD.SetActive(true);

        ////SFX
        //S_SoundManager.PlaySound(S_SoundManager.SoundEnum.BGM_Lose);




        //***** Use this only if using "GameChecker" *****

        ////Without DeadBody
        //AU_PC PRef = FindObjectOfType<AU_PC>();
        //PRef.Die(PRef);

        //Kills local player for all
        AU_PC[] AllPrefs = FindObjectsOfType<AU_PC>();
        foreach (AU_PC PRef in AllPrefs)
        {
            if (PRef.PV.IsMine)
            {
                PRef.Die(PRef);
            }
        }

        IsGameOver = true;
        LoseHUD.SetActive(true);    //Just a overlay (Timers UP!!!)

    }




    //***** MUST ADD "Photonview" to the Gameobject *****

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Sent by Local Player[Is Mine]
        if (stream.IsWriting)
        {
            stream.SendNext(Second);
        }

        //Received by Others
        else
        {
            Second = (float)stream.ReceiveNext();
        }
    }




}

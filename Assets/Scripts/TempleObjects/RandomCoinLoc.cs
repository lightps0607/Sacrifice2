using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class RandomCoinLoc : MonoBehaviour
{
    
     List<Transform> AllLocs = new List<Transform>();
     List<GameObject> AllObjs = new List<GameObject>();

    public int TotalCoins = 1;
    public GameObject CoinPrefab;
    public Transform AllCoinsParent;


    void Start()
    {
        //Transform[] Children;
        //Children = Array.FindAll(GetComponentsInChildren<Transform>(), child => child != this.transform);     //This gets all children Transform + also removes Parent

        Invoke("RandomizeCoinLoc", 5f);
    }


    void RandomizeCoinLoc()
    {

        //Deprecated

        ////Only Runs on MasterClient and Syncs others (Best way to sync random)
        //if (PhotonNetwork.IsMasterClient)           
        //{

        //    //Generates Coins
        //    for (int i = 0; i < TotalCoins; i++)
        //    {
        //        GameObject Coin = PhotonNetwork.InstantiateRoomObject(CoinPrefab.name, transform.position, Quaternion.identity, 0);
        //        AllObjs.Add(Coin);

        //        //Just to Organize stuffs for Master
        //        Coin.transform.SetParent(AllCoinsParent);
        //    }




        //    foreach (Transform child in transform)
        //    {
        //        AllLocs.Add(child);                     
        //    }

        //    AllLocs.Shuffle(AllLocs.Count);    //Shuffle for array length times


        //    for (int i = 0; i < AllObjs.Count; i++)
        //    {
        //        AllObjs[i].transform.position = AllLocs[i].transform.position;   //Sets Coins Location


        //    }
        //}



        //***More Efficient***

        //Only Runs on MasterClient and Syncs others (Best way to sync random)
        if (PhotonNetwork.IsMasterClient)
        {

            foreach (Transform child in transform)
            {
                AllLocs.Add(child);
            }

            AllLocs.Shuffle(AllLocs.Count);    //Shuffle for array length times


            //Generates Coins
            for (int i = 0; i < TotalCoins; i++)
            {
                GameObject Coin = PhotonNetwork.InstantiateRoomObject(CoinPrefab.name, AllLocs[i].transform.position, Quaternion.identity, 0);
                AllObjs.Add(Coin);

                //Just to Organize stuffs for Master
                //Coin.transform.SetParent(AllCoinsParent);
            }
                                  

        }


    }



    


}

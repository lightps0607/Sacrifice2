using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuy : MonoBehaviour
{

    [SerializeField] AU_PC PlayerRef;
    [SerializeField] ShopItem[] SI;
    [SerializeField] Text CoinText;


    public int[] Cost;
    public float[] NewValue;
    public float[] MaxValue;

   

    void Start()
    {
        if (SI.Length != 4) Debug.LogError("Make sure SI Array has 4 Valid ShopItem Refs");

        else
        {
            DedCoins(0);    //Normal Update No Deduction

            SI[0].Updateinfo(PlayerRef.MovementSpeed, NewValue[0], Cost[0]);
            SI[1].Updateinfo(PlayerRef.CD_Dash, NewValue[1], Cost[1]);
            SI[2].Updateinfo(PlayerRef.CD_Kill, NewValue[2], Cost[2]);
            SI[3].Updateinfo(PlayerRef.CD_Shoot, NewValue[3], Cost[3]);

        }
       
    }

    private void Update()
    {
        CoinText.text = PlayerRef.Coins.ToString(); //Updates Coins as long as shop menu is kept open
    }


    void DedCoins(int price)
    {
        //PlayerRef.Coins -= price;     //Locally

        PlayerRef.AddCoins(-price);     //Works with RPC
    }


   public void BuySpeed()
    {
        if (PlayerRef.Coins < Cost[0])    //If don't have enough coin can't buy
            return;


        DedCoins(Cost[0]);
        PlayerRef.NormalSpeed += NewValue[0];   //Test with DASH + If no work = Make new RPC to set both movement speed + normal speed
        PlayerRef.ResetSpeed();

        //PlayerRef.MovementSpeed += NewValue[0];
        
        //NewValue[0] += 1;
        Cost[0] += 5;


        bool ReachedMax = false;
        if (PlayerRef.MovementSpeed >= MaxValue[0])
            ReachedMax = true;

        SI[0].Updateinfo(PlayerRef.MovementSpeed, NewValue[0], Cost[0], ReachedMax);
    }


    public void BuyC_Dash()
    {
        if (PlayerRef.Coins < Cost[1])    //If don't have enough coin can't buy
            return;


        DedCoins(Cost[1]);
        PlayerRef.CD_Dash -= NewValue[1];

        Cost[1] += 5;


        bool ReachedMax = false;
        if (PlayerRef.CD_Dash <= MaxValue[1])
            ReachedMax = true;

        SI[1].Updateinfo(PlayerRef.CD_Dash, NewValue[1], Cost[1], ReachedMax);
    }


    public void BuyC_Kill()
    {
        if (PlayerRef.Coins < Cost[2])    //If don't have enough coin can't buy
            return;


        DedCoins(Cost[2]);
        PlayerRef.CD_Kill -= NewValue[2];

        Cost[2] += 5;


        bool ReachedMax = false;
        if (PlayerRef.CD_Kill <= MaxValue[2])
            ReachedMax = true;

        SI[2].Updateinfo(PlayerRef.CD_Kill, NewValue[2], Cost[2], ReachedMax);
    }


    public void BuyC_Throw()
    {
        if (PlayerRef.Coins < Cost[3])    //If don't have enough coin can't buy
            return;


        DedCoins(Cost[3]);
        PlayerRef.CD_Shoot -= NewValue[3];

        Cost[3] += 5;


        bool ReachedMax = false;
        if (PlayerRef.CD_Shoot <= MaxValue[3])
            ReachedMax = true;

        SI[3].Updateinfo(PlayerRef.CD_Shoot, NewValue[3], Cost[3], ReachedMax);
    }


}

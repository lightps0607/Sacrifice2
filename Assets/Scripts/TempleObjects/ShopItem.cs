using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public Text CurrentValue;
    public Text NewValue;
    public Text Cost;

    public Button BuyButton;


    public void Updateinfo(float CVal, float NVal, int Price, bool RMax=false)
    {
        CurrentValue.text = CVal.ToString();

        if (!RMax)
        {
            NewValue.text = NVal.ToString();
            Cost.text = Price.ToString();
        }

        else
        {
            BuyButton.interactable = false;
            NewValue.text = "(Max)";
        }

        
    }

    

}

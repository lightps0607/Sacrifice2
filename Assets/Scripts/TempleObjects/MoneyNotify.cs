using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyNotify : MonoBehaviour
{
    public Text SenderName;
    public Text TransferType;
    public Text Amount;

    public Color SendColor;
    public Color ReceiveColor;

    public float Lifespan = 3f;

    public void Updateinfo(string sender, int money, bool receiving = false)
    {

        Image CanvasIMG = GetComponent<Image>();

        SenderName.text = sender;

        if (receiving)
        {
            TransferType.text = "Received";
            CanvasIMG.color = ReceiveColor;
        }
        else
        {
            TransferType.text = "Sent";
            CanvasIMG.color = SendColor;
        }

        Amount.text = money.ToString();


        //Destroy after some time
        Destroy(this.gameObject, Lifespan);

    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AU_NPDetection : MonoBehaviour
{

    //VARIABLES
    [SerializeField] private Text NPText;
    [HideInInspector] public int NPCount = 0;
    [SerializeField] GameObject KillButton;




    //METHODS
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            NPCount++;
            SetNPCountText();
            SetKillButton();
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            NPCount--;
            SetNPCountText();
            SetKillButton();
        }

    }



    private void SetNPCountText()
    {
        NPText.text = NPCount.ToString();
    }

    private void SetKillButton()
    {
        if (NPCount > 1)
            KillButton.SetActive(false);
            
        else
            KillButton.SetActive(true);
            
    }



    private void OnDisable()
    {
        KillButton.SetActive(true);
        NPText.text = "";
    }

}

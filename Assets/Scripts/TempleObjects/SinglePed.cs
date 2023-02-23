using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePed : MonoBehaviour
{


    public bool IsOn = false;

    SpriteRenderer SR;
    Sprite DefaultSp;
    [SerializeField] Sprite ActiveSp;

    [SerializeField] GameObject DoorRef;
    [SerializeField] GameObject PedCages;


    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        DefaultSp = SR.sprite;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (IsOn)
                return;

            Debug.Log("Single Ped Is On");
            IsOn = true;
            SR.sprite = ActiveSp;

            DoorRef.SetActive(false);   //Opens Door
            PedCages.SetActive(true); //Ped Cage On

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!IsOn)
                return;

            IsOn = false;
            SR.sprite = DefaultSp;

            DoorRef.SetActive(true);   //Closes Door
            PedCages.SetActive(false);  //Ped Cages Off
        }
    }



}

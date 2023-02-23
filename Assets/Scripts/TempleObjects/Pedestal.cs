using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public bool IsOn { get; set; } = false; //{get; set;} makes the variable hiddden the Editor

    SpriteRenderer SR;
    Sprite DefaultSp;
    [SerializeField] Sprite ActiveSp;

    [SerializeField] bool NeedKey;



    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        DefaultSp = SR.sprite;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //For Dead Body on (collision.tag == "Player" || collision.tag == "DeadBody")
        {
            if (IsOn)
              return;

            if (NeedKey)
            {
                AU_PC playerRef = collision.GetComponent<AU_PC>();
                if (!playerRef || !playerRef.HasKey)
                {
                    return;
                }
            }

            Debug.Log("HAHA Ped Working!!!");
            IsOn = true;
            SR.sprite = ActiveSp;

            //SFX
            S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_PedDown, transform.position);

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

            //SFX
            S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_PedUp, transform.position);
        }
    }






}

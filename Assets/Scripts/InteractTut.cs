using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTut : MonoBehaviour
{

    [SerializeField] GameObject TutRef;


    public void Interacting()
    {

        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_LeverPull, transform.position);


        TutRef.SetActive(!TutRef.activeInHierarchy);

    }



}

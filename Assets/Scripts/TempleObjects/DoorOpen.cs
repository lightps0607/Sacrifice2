using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{

    public S_SoundManager.SoundEnum SFX = S_SoundManager.SoundEnum.SFX_MainGate;

    void OpenDoor()
    {
        //SFX
        S_SoundManager.PlaySound(SFX, transform.position);

        gameObject.SetActive(false);
        Debug.Log( name +" = OPENED!!!");
    }


}

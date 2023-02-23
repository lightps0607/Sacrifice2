using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedAppear : MonoBehaviour
{
       
    public bool IsToggle = false;

    void OpenDoor(bool IsVisible)
    {

        if (IsToggle)
        {
            IsVisible = !IsVisible;
        }

        foreach (Transform child in transform)  //Gets all children and iterate ***Unity doesn't have any GetAllChildren() but All children are available while iterating (Strange!!!)
        {

            if (child.gameObject.activeInHierarchy == IsVisible)
                return;
                                   
            child.gameObject.SetActive(IsVisible);
            Debug.Log(name + " = Visible = " + IsVisible);



            // SFX
            SpikeDamage SD = child.GetComponent<SpikeDamage>();

            if (IsVisible)
            {
                if (SD != null) //If Spike
                {
                    S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_SpikeUp, child.position);
                } 
                else
                {
                    S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_BarClose, child.position);
                }
                
            } 
            else
            {
                if (SD != null) //If Spike
                {
                    S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_SpikeDown, child.position);
                }
                else
                {
                    S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_BarOpen, child.position);
                }
            }



        }

    }
}

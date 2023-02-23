using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class S_SoundManager
{

    public enum SoundEnum
    {
        SFX_BarOpen,
        SFX_BarClose,
        SFX_SpikeUp,
        SFX_SpikeDown,
        SFX_MainGate,
        SFX_PedUp,
        SFX_PedDown,
        SFX_BoxOpen,
        SFX_LeverPull,
        SFX_CoinPick,
        //SFX_PickCrown,
        //SFX_PickKnife,
        BGM_AltarAppear,
        BGM_Win,
        BGM_Lose,
    }


    /// SFX Used In :
    /// 
    /// PedAppear
    /// DoorOpen
    /// Pedestal
    /// ChestInteract [RPC Call]
    /// Int_DoorButton
    /// CoinItem
    /// 
    /// </summary>



    private static GameObject oneShotGO;
    private static AudioSource oneShotAS;



    // 2D Sound
    public static void PlaySound(SoundEnum SE)
    {
        
        if (oneShotGO == null)
        {
            oneShotGO = new GameObject("2D Sound");
            oneShotAS = oneShotGO.AddComponent<AudioSource>();
        }

        oneShotAS.clip = GetAudioClip(SE);

        ////Settings
        //oneShotAS.volume = 1f;

        oneShotAS.PlayOneShot(oneShotAS.clip);
        
    }

    // Override : With Position 
    public static void PlaySound(SoundEnum SE, Vector3 position)
    {

        GameObject soundGameObject = new GameObject("3D Sound");
        soundGameObject.transform.position = position;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(SE);

        //Settings
        audioSource.spatialBlend = 1f;

        audioSource.Play();

        Object.Destroy(soundGameObject, audioSource.clip.length);   //Use Pooling Instead

        //if (oneShotGO == null)
        //{
        //    oneShotGO = new GameObject("One Shot Sound");
        //    oneShotAS = oneShotGO.AddComponent<AudioSource>();
        //}
                        
        //oneShotAS.clip = GetAudioClip(SE);

        ////Settings
        //oneShotAS.spatialBlend = 1f;

        //oneShotAS.Play();

    }



    private static AudioClip GetAudioClip(SoundEnum Sound)
    {
        foreach (S_GameAssets.SoundAudioClip soundClip in S_GameAssets.i.SoundAudioClipArray)
        {
            if(soundClip.SEnum == Sound)
            {
                return soundClip.audioClip;
            }
        }

        Debug.LogError("Sound" + Sound + " - Not Found!!!");
        return null;
    }



    


    
}

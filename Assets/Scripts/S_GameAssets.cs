using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GameAssets : MonoBehaviour
{
    private static S_GameAssets _i;

    public static S_GameAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<S_GameAssets>("GameAssetsLoad"));
            return _i;
        }
    }


    public SoundAudioClip[] SoundAudioClipArray;


    [System.Serializable]
    public class SoundAudioClip
    {
        public S_SoundManager.SoundEnum SEnum;
        public AudioClip audioClip;
    }


}

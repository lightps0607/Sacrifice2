using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{

    [SerializeField] private string _gameVersion = "0.1";
    public string GameVersion { get { return _gameVersion; } }

    [SerializeField] public string _nickName = "MyNickName";
    public string NickName
    { get
        {
            int value = Random.Range(0, 99);
            return _nickName + value.ToString();
        }
    }

}

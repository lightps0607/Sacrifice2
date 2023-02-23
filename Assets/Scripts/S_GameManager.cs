using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***MUST ADD "PHOTON VIEW" to the PREFAB
public class S_GameManager : MonoBehaviour
{
    //VARIABLES
    [SerializeField] private GameObject _prefab;

    [SerializeField] GameObject[] SpawnPoints;


    //METHODS
    private void Awake()
    {
        Debug.LogWarning("SPAWNING...");
        SpawnPlayer();

    }

    //private void OnEnable()
    //{
    //    SpawnPlayer();
    //}


    public void SpawnPlayer()
    {
        int RandomID = Random.Range(0, SpawnPoints.Length);
        MasterManager.NetworkInstantiate(_prefab, SpawnPoints[RandomID].transform.position, Quaternion.identity);
    }

    //public void LeaveRoom()
    //{
    //    PhotonNetwork.LeaveRoom();
    //    PhotonNetwork.LoadLevel(0);
    //}
}

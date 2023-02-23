using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//***MUST ADD "PHOTON VIEW" to the PREFAB
public class InstantiateEx : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    private void Awake()
    {
        MasterManager.NetworkInstantiate(_prefab, transform.position, Quaternion.identity);
    }
}

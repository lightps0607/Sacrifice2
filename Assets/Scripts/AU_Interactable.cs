using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_Interactable : MonoBehaviour
{

    [SerializeField] GameObject ConnectedObj;
    

    public void Interacting()
    {
        ConnectedObj.SetActive(!ConnectedObj.activeInHierarchy);
    }


}

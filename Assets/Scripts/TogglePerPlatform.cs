using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePerPlatform : MonoBehaviour
{

    [SerializeField] GameObject GO;
    [SerializeField] RuntimePlatform[] TargetPlatforms;
    [SerializeField] bool Visible;


    private void Awake()
    {
        foreach (var TP in TargetPlatforms)
        {
            if (Application.platform == TP)
            {
                GO.SetActive(Visible);
                return;
            }
        }

    }



}

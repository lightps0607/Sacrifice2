using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutKeyInChest : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        LocketKeyWear[] AllKeys = GameObject.FindObjectsOfType<LocketKeyWear>();


        foreach (LocketKeyWear LKey in AllKeys)
        {
            
            int SelectedBoxId = Random.Range(0, (transform.childCount));
            print("Selected BOX = " + SelectedBoxId);
            Vector3 BoxPos = transform.GetChild(SelectedBoxId).position;

            LKey.transform.position = BoxPos + new Vector3(Random.Range(0,0.5f), Random.Range(0, 0.5f), -BoxPos.z); //Resets z pos [Only sets X and Y]

        }


    }

}

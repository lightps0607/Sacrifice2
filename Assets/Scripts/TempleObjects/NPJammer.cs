using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPJammer : MonoBehaviour
{

    public bool NormalOnExit;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AU_NPDetection DetectorRef = collision.GetComponentInChildren<AU_NPDetection>(true);    //Include Inactive
            DetectorRef.gameObject.SetActive(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (!NormalOnExit)
            return;

        if (collision.tag == "Player")
        {
            AU_NPDetection DetectorRef = collision.GetComponentInChildren<AU_NPDetection>(true);    //Include Inactive
            DetectorRef.gameObject.SetActive(true);
        }
        
    }


}

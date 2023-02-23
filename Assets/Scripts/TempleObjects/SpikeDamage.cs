using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            AU_PC PRef = collision.GetComponent<AU_PC>();
            PRef.Die(PRef);
        }
    }


}

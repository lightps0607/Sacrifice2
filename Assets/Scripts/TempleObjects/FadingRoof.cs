using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingRoof : MonoBehaviour
{

    int TotalPlayerInside = 0;
    [SerializeField] SpriteRenderer SR;
    [SerializeField] float NewAlpha = 0.5f;

    Color OriginalCol;
    Color NewCol;


    private void Start()
    {
        OriginalCol = SR.color;
        NewCol = new Color(SR.color.r, SR.color.g, SR.color.b, NewAlpha);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            TotalPlayerInside++;

            SR.color = NewCol;
        }   

    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TotalPlayerInside--;

            if (TotalPlayerInside <= 0)
            {
                SR.color = OriginalCol;
            }

        }
    }


}

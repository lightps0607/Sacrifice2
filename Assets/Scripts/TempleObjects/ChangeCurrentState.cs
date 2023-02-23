using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentState : MonoBehaviour
{

    [SerializeField] GameOverConditions GOCon;
    public int NewState;
    [SerializeField] GameObject TargetObj;  //***Make sure TargetObj has Rigidbody2d***



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == TargetObj)
        {
            GOCon.CurrentState = NewState;

            print("State Changed to = " + GOCon.CurrentState);
        }
    }


}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_DeadBodyScr : MonoBehaviour
{

    [SerializeField] SpriteRenderer DeadBodySR;
    [SerializeField] PhotonView PV;
    private bool IsAttached = false;
    AU_PC PlayerRef;

    
    private void Start()
    {
        //PV = GetComponent<PhotonView>();    //gameObject.GetPhotonView();     //Setting at runtime causes issues
    }


    private void Update()
    {
        if (!IsAttached)
            return;

        transform.position = PlayerRef.transform.position;
        transform.rotation = PlayerRef.transform.rotation;
        transform.localScale = new Vector3(PlayerRef.DirectionX, transform.localScale.y);

    }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void SetColor(Color Col)
    {

        float[] RGBA = { Col.r, Col.g, Col.b, Col.a };

        PV.RPC("RPC_SetColor", RpcTarget.All, RGBA);
    }


    

    [PunRPC]
    void RPC_SetColor(float[] RGBA)
    {
        Color NewCol = new Color(RGBA[0], RGBA[1], RGBA[2], RGBA[3]);
        DeadBodySR.color = NewCol;
    }

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void Interacting(AU_PC P_Ref)
    {
        int PVID = P_Ref.PV.ViewID;
        
        PV.RPC("RPC_Pickup", RpcTarget.All, PVID);


    }

    [PunRPC]
    void RPC_Pickup(int PVID)
    {
        
        PlayerRef = PhotonView.Find(PVID).transform.gameObject.GetComponent<AU_PC>();
        Debug.Log(PlayerRef.name + " = " + PlayerRef.PV.ViewID);
        

        if (!PlayerRef)
            return;


        if (transform.parent)
        {
            transform.position = PlayerRef.attackHitBoxPos.position;
            transform.parent = null;
            IsAttached = false;
            GetComponent<Collider2D>().isTrigger = false;

            
        }
        else
        {
            transform.position = PlayerRef.transform.position;
            transform.parent = PlayerRef.transform;
            IsAttached = true;
            GetComponent<Collider2D>().isTrigger = true;
            
        }



        ////WITH FIXED JOINT 2D = ***[PHYSICS LAGS with Multi Player]

        //Rigidbody2D PlayerRB = PlayerRef.GetComponent<Rigidbody2D>();
        //Vector3 AnchorLoc = PlayerRef.attackHitBoxPos.localPosition;

        //if (FJointRef.isActiveAndEnabled)
        //    FJointRef.enabled = false;
        //else
        //{
        //    FJointRef.enabled = true;
        //    FJointRef.connectedBody = PlayerRB;
        //    FJointRef.connectedAnchor = AnchorLoc;
        //}

    }


    public void DestroyObj()
    {
        PV.RPC("DestroyObject", RpcTarget.All);
    }


    [PunRPC]
    private void DestroyObject()
    {
        Destroy(gameObject);
    }

}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    PhotonView PV;
    private SpriteRenderer SR;
    [SerializeField] Sprite OpenSP;







    private void Start()
    {
        PV = GetComponent<PhotonView>();    //gameObject.GetPhotonView();
        SR = GetComponent<SpriteRenderer>();
    }








    public void Interacting()
    {
        //SFX
        //S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_BoxOpen, transform.position);

        PV.RPC("RPC_Opened", RpcTarget.All);
    }


    [PunRPC]
    void RPC_Opened()
    {

        GetComponent<Collider2D>().enabled = false;
        SR.sprite = OpenSP;
        //transform.localPosition = transform.localPosition + new Vector3(0, 0, transform.localPosition.z + 5);   //Pushes Back
        SR.sortingOrder = (SR.sortingOrder - 1);    //Pushes Back

        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.SFX_BoxOpen, transform.position);
    }


}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScr : MonoBehaviour
{


    private Vector2 MoveDir = new Vector2(1,0);

    public float MoveSpeed = 1f;
    public float Lifespan = 5f;
    public PhotonView PV;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();

        StartCoroutine("DestroyByTime");
    }


    private void Update()
    {
        transform.Translate(new Vector2 (MoveDir.x, 0) * MoveSpeed * Time.deltaTime);
        transform.localScale = new Vector2(MoveDir.x, 1);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PV.IsMine) //Needed - Otherwise Both Players will die
            return;

        if (collision.tag == "Player")
        {

            AU_PC PlayerRef = collision.GetComponentInParent<AU_PC>();

            if (!PlayerRef.IsDead)  //Filter out Self and Dead ones
            {

                if (PlayerRef.PV.IsOwnerActive)
                {
                    if (PlayerRef.PV.IsMine)
                        return;
                }

                PlayerRef.Die(PlayerRef);

                //Temporary Fix [Minor Bug : If player is inactive and hit by MasterClient then Spawns Duplicate DeadBodies]
                AU_DeadBodyScr MyDeadBody = PhotonNetwork.Instantiate(PlayerRef.DeadBodyPF.name, transform.position, transform.rotation).GetComponent<AU_DeadBodyScr>();    //Should not be called from RPC, otherwise create duplicates...
                MyDeadBody.SetColor(PlayerRef.MyColor);
                PlayerRef.DetachAllActors(PlayerRef);
                //...

                PV.RPC("DestroyObject", RpcTarget.All);

            }

            //if (!PlayerRef.PV.IsMine && !PlayerRef.IsDead)  //Filter out Self and Dead ones
            //{
            //    PlayerRef.Die(PlayerRef);

            //    //Temporary Fix
            //    AU_DeadBodyScr MyDeadBody = PhotonNetwork.Instantiate(PlayerRef.DeadBodyPF.name, transform.position, transform.rotation).GetComponent<AU_DeadBodyScr>();    //Should not be called from RPC, otherwise create duplicates...
            //    MyDeadBody.SetColor(PlayerRef.MyColor);
            //    PlayerRef.DetachAllActors(PlayerRef);
            //    //...

            //    PV.RPC("DestroyObject", RpcTarget.All);

            //}


        }

        else
        {

            Collider2D Col2D = collision.GetComponentInChildren<Collider2D>();  //Make sure to Add RigidBody2D to detect Collision
            if (Col2D && (Col2D.isTrigger == false))
                PV.RPC("DestroyObject", RpcTarget.All);


            //PhotonView TargetPV = collision.gameObject.GetComponent<PhotonView>();
            //if (TargetPV != null && (!TargetPV.IsMine || TargetPV.IsRoomView))
            //{
            //    PV.RPC("DestroyObject", RpcTarget.AllBuffered);
            //}

        }




    }


    IEnumerator DestroyByTime()
    {
        yield return new WaitForSeconds(Lifespan);
        PV.RPC("DestroyObject", RpcTarget.All);
    }


    [PunRPC]
    public void ChangeDir(Vector2 Dir)
    {
        MoveDir = Dir;
    }


    [PunRPC]
    public void DestroyObject()
    {
        Destroy(gameObject);
    }


}

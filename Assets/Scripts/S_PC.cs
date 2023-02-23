using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_PC : MonoBehaviour
{
    //VARIABLES

    //Player INPUTS
    public InputAction BACK;
    public InputAction MOVEMENT;
        private Vector2 MovementInput;
        [SerializeField] private float MovementSpeed = 10f;
    public InputAction KILL;
    public InputAction INTERACTION;

    //Components
    public Transform Avatar;
    public Rigidbody2D RB;
    public Animator Anim;
    public GameObject PlayerCam;
    public PhotonView PV;
    public GameObject BackMenu;
    [SerializeField] Collider2D CapsuleCollider;

    //Player Color
    static Color MyColor;
    SpriteRenderer AvatarSR;

    //Player State
    bool IsDead;
    [SerializeField] GameObject DeadBodyPF;
    [SerializeField] LayerMask IgnoreForBody;

    //Ping
    [SerializeField] private Text PingText;
    [SerializeField] private bool ShowPing = false;

    //Box Push Pull + Add a "RigidBody2d" and "FixedJoint2D" with the Box.
    //public float Distance = 1f;
    //public LayerMask BoxMask;
    //GameObject PullBox;




    //METHODS
    private void Awake()
    {
        Avatar = transform.GetChild(0);
        RB = GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            BACK.performed += ToggleBackMenu;
            KILL.performed += KillTarget;
            INTERACTION.performed += Interact;
        }
    }


    private void OnEnable()
    {
        BACK.Enable();
        MOVEMENT.Enable();
        KILL.Enable();
        INTERACTION.Enable();
    }

    private void OnDisable()
    {
        BACK.Disable();
        MOVEMENT.Disable();
        KILL.Disable();
        INTERACTION.Disable();
    }


    private void Start()
    {
        if (!PV.IsMine) //Turn off Camera and AudioListener
        {
            PlayerCam.gameObject.SetActive(false);
            PlayerCam.GetComponent<AudioListener>().enabled = false;
            return;
        }
    }


    private void Update()
    {
        if (!PV.IsMine)
            return;

        ViewShowPing();

        CheckInput();
        SetAnimParam();


    }


    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        //RB.velocity = MovementInput * MovementSpeed; //by InfoGamer
        //RB.MovePosition(RB.position + MovementInput * MovementSpeed * Time.deltaTime); //by Brackeys "TopDownMovement" tutorial
        transform.position += (Vector3)MovementInput * MovementSpeed * Time.deltaTime; //by Diving Squid
        
    }


    private void CheckInput()
    {
        MovementInput = MOVEMENT.ReadValue<Vector2>();
        
    }

    private void SetAnimParam()
    {
        if (MovementInput.sqrMagnitude != 0) //For "Idle_BT" [Never set (0,0)]
        {
            Anim.SetFloat("DirectionX", MovementInput.x);
            Anim.SetFloat("DirectionY", MovementInput.y);
        }
        Anim.SetFloat("Speed", MovementInput.sqrMagnitude);
        
        //bool IsMoving = false;
        //if (MovementInput.sqrMagnitude == 0) IsMoving = false;
        //else IsMoving = true;
        //Anim.SetBool("IsMoving", IsMoving);
    }


    private void KillTarget(InputAction.CallbackContext context)
    {

    }


    private void Interact(InputAction.CallbackContext context)
    {
        //CheckPullBox();
    }


    ////Check For PullBox
    //private void CheckPullBox()
    //{
    //    Physics2D.queriesStartInColliders = false;
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, Distance, BoxMask);

    //    if (hit.collider != null && Keyboard.current.eKey.isPressed)
    //    {
    //        PullBox = hit.collider.gameObject;
    //        print(PullBox);
    //        PullBox.GetComponent<FixedJoint2D>().enabled = true;
    //        PullBox.GetComponent<FixedJoint2D>().connectedBody = this.GetComponent<Rigidbody2D>();
    //    }
    //}

    //    private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * Distance);
    //}


    public void Die()
    {
        IsDead = true;
        Anim.SetBool("IsDead", IsDead);
        CapsuleCollider.enabled = false;

        //AU_DeadBodyScr TempDeadBody = Instantiate(DeadBodyPF, transform.position, transform.rotation).GetComponent<AU_DeadBodyScr>();
        //TempDeadBody.SetColor(AvatarSR.color);
    }





    //HUD CONTROL
    private void ToggleBackMenu(InputAction.CallbackContext context)
    {
        if (PV.IsMine)
        {
            //print("ESC Pressed!");
                                    
            if (BackMenu.gameObject.activeInHierarchy != true)
                BackMenu.gameObject.SetActive(true);
            else
                BackMenu.gameObject.SetActive(false);
        }
    }

    private void ViewShowPing()
    {
        if (ShowPing)
            PingText.text = "Ping: " + PhotonNetwork.GetPing();
    }

    public void OnClicked_LeaveButton()
    {
        //Disconnection is Handled in "PunConnect.cs"
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.LoadLevel(0);
       
    }


}

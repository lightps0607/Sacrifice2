using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AU_PC : MonoBehaviour, IPunObservable
{

    //Inputs
    [SerializeField] InputAction WASD;
    [SerializeField] InputAction KILL;
    [SerializeField] InputAction SHOOT;
    [SerializeField] InputAction INTERACT;
    [SerializeField] InputAction DASH;
    [SerializeField] InputAction SUICIDE;
    [SerializeField] InputAction CHAT;
    [SerializeField] InputAction SHOP;
    [SerializeField] InputAction BACK;
    [SerializeField] InputAction MAP;

    //PlayerInfo
    [SerializeField] Text PlayerName;

    //Photon
    public PhotonView PV;
    public float DirectionX { get; set; } = 1; 
    public float DirectionY { get; set; } = 1;


    //Ping
    [SerializeField] private Text PingText;
    [SerializeField] private bool ShowPing = false;


    public static AU_PC LocalPlayer;

    //Components
    Transform Avatar;
    Rigidbody2D RB;
    Animator Anim;
    [SerializeField] Camera MyCamera;
    [SerializeField] GameObject MapCamera;
    [SerializeField] GameObject MyHUD;
    [SerializeField] GameObject OnScreenControls;
    [SerializeField] CapsuleCollider2D CapsuleCollider;
    [SerializeField] GameObject PrivateObjects;
    public GameObject SacrificeButton;


    //Player Movement
    Vector2 MovementInput;
    public float MovementSpeed;
    public float NormalSpeed { get; set; }
    [SerializeField] float DashMultiplier = 3f;



    //Player Color
    public Color MyColor = Color.white;
    SpriteRenderer AvatarSR;

    //Player Hat
    Sprite HatSprite;
    SpriteRenderer HatSR;

    //TEST
    [SerializeField] GameObject CustomMenu;
    public Color[] AllColors { get; set; }
    public Sprite[] AllHats { get; set; }



    //Deadbody
    public bool IsDead { get; set; }
    [SerializeField] public GameObject DeadBodyPF;
    [SerializeField] LayerMask IgnoreForBody;

    //Interaction
    [SerializeField] LayerMask InteractLayer;
    [SerializeField] private float InteractRadius = 0.5f;

    //New Attack/Kill System [By Bardent - https://www.youtube.com/watch?v=YaXcwc5Evjk&list=LL&index=3&t=605s&ab_channel=Bardent]
    [SerializeField] private float attackRadius, attackDamage;
    [SerializeField] public Transform attackHitBoxPos;
    [SerializeField] LayerMask whatIsDamageable;


    //Pickup-Throw/Drop System [By Smart Penguins-GameDev - https://www.youtube.com/watch?v=-V1O5FGQVY8&ab_channel=SmartPenguins-GameDev]

    //Win System
    public bool HasKey { get; set; }
    public bool HasDiamond { get; set; }
    [SerializeField] public GameObject WinHUD;
    [SerializeField] public Text WinnerText;
    [SerializeField] public Image WinIMG;
    //[SerializeField] public GameObject LoseHUD;


    [SerializeField] GameObject ChatPanel;

    [SerializeField] GameObject BackMenu;

    //Shoot System
    public bool HasKnife = false;
    public GameObject BulletObject;




    //Cool Down
    public float CD_Kill = 2f;
    public float CD_Shoot = 5f;
    public float CD_Dash = 5f;
    [SerializeField] GameObject KillButton;
    public GameObject ShootButton;
    [SerializeField] GameObject DashButton;
    [SerializeField] Text KillTimer;
    [SerializeField] Text ShootTimer;
    [SerializeField] Text DashTimer;


    //Money/Shop System
    public int Coins = 10;
    [SerializeField] GameObject ShopMenu;
    [SerializeField] MoneyNotify MNotifyPF;
    [SerializeField] Transform NotifyContents;


    //Loading Menu
    [SerializeField] GameObject LoadingMenu;




    //Audio
    [SerializeField] AudioSource MyAudio;
    //Sounds List
    [SerializeField] AudioClip SFX_Footstep;
    [SerializeField] AudioClip SFX_Dash;
    //[SerializeField] AudioClip SFX_KnifeAttack;
    //[SerializeField] AudioClip SFX_ThrowingKnife;
    [SerializeField] AudioClip SFX_Die;
    





    //METHODS
    private void Awake()
    {
        if (!PV.IsMine)
            return;

        KILL.performed += KillTarget;
        SHOOT.performed += OnShoot;
        INTERACT.performed += OnInteract;
        DASH.performed += OnDash;
        SUICIDE.performed += OnSuicide;
        SUICIDE.started += OnPressedSuicide;
        CHAT.performed += OnToggleChat;
        SHOP.performed += OnToggleShop;
        BACK.performed += OnBack;
        MAP.performed += OnToggleMap;
    }

    public void EnableAllControls()
    {
        WASD.Enable();
        KILL.Enable();
        SHOOT.Enable();
        INTERACT.Enable();
        DASH.Enable();
        SUICIDE.Enable();
        CHAT.Enable();
        SHOP.Enable();
        BACK.Enable();
        MAP.Enable();
    }

    public void DisableAllControls()
    {
        WASD.Disable();
        KILL.Disable();
        SHOOT.Disable();
        INTERACT.Disable();
        DASH.Disable();
        SUICIDE.Disable();
        CHAT.Disable();
        SHOP.Disable();
        BACK.Disable();
        MAP.Disable();
    }

    private void OnEnable()
    {
        if (PV.IsMine)
            EnableAllControls();

    }

    private void OnDisable()
    {
        DisableAllControls();        
    }

    private void Start()
    {
        PlayerName.text = PV.Owner.NickName;
        PlayerName.color = MyColor;

        NormalSpeed = MovementSpeed;    //Init Normal Speed

        AU_Customizer CustomizerRef = CustomMenu.GetComponentInChildren<AU_Customizer>(true);   //True = IncluedeInactive
        AllColors = CustomizerRef.AllColors;
        AllHats = CustomizerRef.AllHats;



        if (PV.IsMine)
        {
            LocalPlayer = this;

            //Sets Random Color at start
            Invoke("SetRandomColor", 3f);   //Error without Invoke [Possible Reason: Delay while instantiating player]

        }

        Avatar = transform.GetChild(0);
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        AvatarSR = Avatar.GetComponent<SpriteRenderer>();
        HatSR = transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>();



        if (!PV.IsMine) //Turn off PrivateObjects [HUD,Camera,DetectionCollisions etc.]
        {
            PrivateObjects.SetActive(false);
            return;
        }

        if (MyColor == Color.clear)
            MyColor = Color.white;

        if (!PV.IsMine)
            return;
        AvatarSR.color = MyColor;

        if (HatSprite != null)
            HatSR.sprite = HatSprite;


        Invoke("HideLoadingMenu", 10);     //Hide Loading Menu  (For Emergency)

    }

    private void Update()
    {
        //Works For All [All Local Avatars/GameObjects *NOT Auto Replicated. Use "RPC" or "Stream.Write" to Replicate to other players]

        //Flip according to "Direction"
        Avatar.localScale = new Vector2(DirectionX, 1);
        attackHitBoxPos.localPosition = new Vector3(Mathf.Abs(attackHitBoxPos.localPosition.x) * DirectionX, attackHitBoxPos.localPosition.y);


        

        


        if (!PV.IsOwnerActive || !PV.IsMine)
            return;
        

        //Works Only for LocalPlayer/IsMine Player.
        ViewShowPing();


        MovementInput = WASD.ReadValue<Vector2>();
        Anim.SetFloat("Speed", MovementInput.magnitude);

        if (MovementInput.x != 0)
        {
            DirectionX = Mathf.Sign(MovementInput.x);
        }
        if (MovementInput.y != 0)
        {
            DirectionY = Mathf.Sign(MovementInput.y);
        }

    }

    private void FixedUpdate()
    {
        if (!PV.IsOwnerActive || !PV.IsMine)
            return;

        //transform.position += (Vector3)MovementInput * MovementSpeed * Time.deltaTime; //by Diving Squid
        RB.velocity = MovementInput * MovementSpeed;

        // FIX: High Velocity Dash through walls = Set collision detection of player RigidBody2D to "Continuous" not Discrete;        
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Cool Down


    void StartCooldown(float CooldownTime, InputAction INPUTKEY, GameObject ButtonRef, Text TimerText)
    {
        INPUTKEY.Disable();
        ButtonRef.GetComponent<Button>().interactable = false;

        StartCoroutine(OnStartCooldown(CooldownTime, INPUTKEY, ButtonRef, TimerText));
    }


    IEnumerator OnStartCooldown(float CooldownTime, InputAction INPUTKEY, GameObject ButtonRef, Text TimerText)
    {
        yield return new WaitForSeconds(0.1f);

        float CD_Time = CooldownTime;   //Holds as a Local Var so doesn't change Original

        CD_Time -= 0.1f;
        TimerText.text = CD_Time.ToString("0.0");

        if (CD_Time <= 0)   //On End
        {
            INPUTKEY.Enable();
            ButtonRef.GetComponent<Button>().interactable = true;
            TimerText.text = "";
        }

        else
        {
            StartCoroutine(OnStartCooldown(CD_Time, INPUTKEY, ButtonRef, TimerText));   //Passes modified local "CD_Time" not original "CooldownTime" so no need to reset
        }

    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Customize Menu


    public void ToggleCustomMenu()
    {
        CustomMenu.SetActive(!CustomMenu.activeInHierarchy);

        if (CustomMenu.activeInHierarchy)
        {
            WASD.Disable();
            KILL.Disable();
        }
        else
        {
            WASD.Enable();
            KILL.Enable();
        }

    }


    void SetRandomColor()       // Each player will have different colors;
    {
        if (PhotonNetwork.IsMasterClient)   //Only for MasterClient (Best way to sync random)
        {
            print("I AM MASTER!!!!!");

            List<int> AllColId = new List<int>();       //Init AllColId
            for(int i = 0; i < AllColors.Length; i++)
            {
                AllColId.Add(i);
            }

            AllColId.Shuffle(AllColId.Count);       //Shuffle Color Id

            GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < AllPlayers.Length; i++)
            {
                AllPlayers[i].GetComponent<AU_PC>().SetColor(AllColId[i]);      //Sets colors according to the shuffled Color Id List

                AllPlayers[i].GetComponent<AU_PC>().HideLoadingMenu();      //Hides Loading Menu Once Colors are set
            }
        }
        
    }

    public void SetColor(int ColID)
    {
        PV.RPC("RPC_SetColor", RpcTarget.All, ColID);
    }

    public void SetHat(int HatID)
    {
        PV.RPC("RPC_SetHat", RpcTarget.All, HatID);
    }


    [PunRPC]
    void RPC_SetColor(int ColorIndex)
    {
        MyColor = AllColors[ColorIndex];

        AvatarSR.color = MyColor;
        //PlayerName.color = MyColor;
    }

    [PunRPC]
    void RPC_SetHat(int HatIndex)
    {
        HatSprite = AllHats[HatIndex];

        HatSR.sprite = HatSprite;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// KIll Target

    void KillTarget(InputAction.CallbackContext context)
    {
        if (!PV.IsMine)
            return;

        if (IsDead) //Dead can't kill
            return;

        if (!KillButton.activeInHierarchy)     //If Kill Buttond Hidden Can't kill (NP Detection)
            return;

        if (context.phase == InputActionPhase.Performed)
        {
           
            Anim.SetTrigger("KillTrg");

            CheckAttackHitBox();

            StartCooldown(CD_Kill, KILL, KillButton, KillTimer);

        }
    }


    private void CheckAttackHitBox()
    {

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attackHitBoxPos.position, attackRadius, whatIsDamageable);

        foreach (Collider2D collider in detectedObjects)
        {
            Debug.Log("Col Name = " + collider.name);
            Debug.Log("Col Tag = " + collider.tag);

            if (collider.tag == "Player")
            {
                AU_PC KTarget = collider.GetComponentInParent<AU_PC>();
                if (!KTarget.IsDead && !KTarget.PV.IsMine)  //Filter out Self and Dead ones
                {
                    Die(KTarget);
                    //return;   //Single Kill (Prevents "Multikill" at Single Hit)  //Not possible though as NPDetector
                }

                //Only works for inactive players  //Bypasses IsMine Check
                if (!KTarget.IsDead && !KTarget.PV.IsOwnerActive)   
                {
                    Die(KTarget);
                }

            }

            //collider.transform.parent.SendMessage("Damage", attackDamage, SendMessageOptions.DontRequireReceiver);
            //Instantiate Hit Particle
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackHitBoxPos.position, attackRadius);
        Gizmos.DrawWireSphere(transform.position, InteractRadius);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// For Throwing Knives (Shoot)

    void OnShoot(InputAction.CallbackContext context)
    {
        if (!PV.IsMine)
            return;

        if (IsDead) //Dead can't kill
            return;

        if (context.phase == InputActionPhase.Performed)
        {
            if (HasKnife)
            {

                Anim.SetTrigger("ThrowTrg");

                //Shoot();  //Being called from the animation "Throw" as an event (similar to Anim Notify)

                StartCooldown(CD_Shoot, SHOOT, ShootButton, ShootTimer);
            }

        }
    }

    private void Shoot()
    {
        if (!PV.IsMine) //Needed as this is being called as Anim event/notify
            return;

        BulletScr BulletRef = PhotonNetwork.Instantiate(BulletObject.name, (Vector2)attackHitBoxPos.position, Quaternion.identity, 0).GetComponent<BulletScr>();
        BulletRef.PV.RPC("ChangeDir", RpcTarget.All, new Vector2(DirectionX, DirectionY));
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Self Sacrifice (Suicide)

    private void OnPressedSuicide(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;


        if (Context.phase == InputActionPhase.Started)
        {
            Debug.Log("STARTED HOLDING>>>");
        }

    }

    private void OnSuicide(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;

        if (IsDead) //Dead can't Suicide [Just in case]
            return;


        if (Context.phase == InputActionPhase.Performed)
        {
            Die(LocalPlayer);

        }

    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Dash

    private void OnDash(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;


        if (Context.phase == InputActionPhase.Performed)
        {
            if (MovementInput.sqrMagnitude == 0)    //Can't Dash if Doesn't move
                return;

            MyAudio.PlayOneShot(SFX_Dash);

            PV.RPC("RPC_SpeedChange", RpcTarget.All, (NormalSpeed * DashMultiplier) );  //Increases Speed
            Invoke("ResetSpeed", 0.1f);    //Reset to Normal Speed

            StartCooldown(CD_Dash, DASH, DashButton, DashTimer);

        }

    }


    public void ResetSpeed()
    {
        PV.RPC("RPC_SpeedChange", RpcTarget.All, NormalSpeed);
    }

    [PunRPC]
    void RPC_SpeedChange(float NewSpeed)
    {
        MovementSpeed = NewSpeed;
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Game Over

    [PunRPC]
    void OnGameOver(int WinnerPVID)
    {
        AU_PC WinnerRef = PhotonView.Find(WinnerPVID).transform.gameObject.GetComponent<AU_PC>();
        
        //Show Win HUD
        WinHUD.SetActive(true);
        WinnerText.text = (WinnerRef.PV.Owner.NickName + " - Is The Winner!!!");
        WinIMG.color = WinnerRef.MyColor;

        //SFX
        S_SoundManager.PlaySound(S_SoundManager.SoundEnum.BGM_Win);

        //Detach All Attachments
        DetachAllActors(LocalPlayer);
    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Die

    public void Die(AU_PC P_Ref)
    {
        if (!IsDead)
        {
            Debug.LogWarning("KTarget Killed = " + P_Ref.PV.Owner.NickName);

            //RPC Call
            P_Ref.PV.RPC("RPC_Die", RpcTarget.All);


            if (!PV.IsMine)
                return;
            
            //Normal Call   [PhotonNetwork.InstantiateRoomObjects will cause issues]
            AU_DeadBodyScr MyDeadBody = PhotonNetwork.Instantiate(DeadBodyPF.name, transform.position, transform.rotation).GetComponent<AU_DeadBodyScr>();    //Should not be called from RPC, otherwise create duplicates...
            MyDeadBody.SetColor(P_Ref.MyColor);

            DetachAllActors(P_Ref);

            
        }

    }

    [PunRPC]
    void RPC_Die()
    {
        //if (!PV.IsMine)   [WARNING : NEVER USE IsMine Return; with RPC Functions - Otherwise It will not update RPC for the Instigator(Who calls the RPC)]
        //    return;

        if (!IsDead)
        {
            IsDead = true;
            Anim.SetBool("IsDead", IsDead);
            CapsuleCollider.enabled = false;

            MyAudio.PlayOneShot(SFX_Die);
            
            if (!PV.IsMine)     //Only for others [Hides for others]
            {
                //AvatarSR.enabled = false;
                PlayerName.enabled = false;
            }


        }
    }


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Detach All

    public void DetachAllActors(AU_PC P_Ref)
    {
        foreach (Transform child in P_Ref.transform)
        {
            Debug.Log("C - " + child.name);
            if (child.tag == "Interactable" || child.tag == "DeadBody")
            {
                Debug.Log("DETACHING ATTACHMENTS");
                child.SendMessage("Interacting", P_Ref);

                DetachAllActors(P_Ref);

            }
        }

    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Interaction

    void OnInteract(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;

        if (IsDead) //Dead can't Interact
            return;


        if (Context.phase == InputActionPhase.Performed)
        {
           
            Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, InteractRadius, whatIsDamageable);

            foreach (Collider2D collider in detectedObjects)
            {
                
                if (collider.tag == "Interactable" || collider.tag == "DeadBody")
                {
                    Debug.Log("Interacting!!!");

                    collider.SendMessage("Interacting", LocalPlayer);

                    return; // Interact with One At a Time
                    
                }
            }
        }
                        
    }



//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Chat

    void OnToggleChat(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;

        
        if (Context.phase == InputActionPhase.Performed)
        {
            ChatPanel.SetActive(!ChatPanel.activeInHierarchy);

            if (ChatPanel.activeInHierarchy)
            {
                P_Chat P_ChatRef = ChatPanel.GetComponentInParent<P_Chat>();
                P_ChatRef.UnreadIndicator.gameObject.SetActive(false);   //Hide Unread Indicator on Open
                P_ChatRef.UnreadMsgCount = 0;

                //ChatPanel.GetComponentInChildren<InputField>().ActivateInputField();


                ////All Except (CHAT, BACK, DASH)
                //WASD.Disable();
                //KILL.Disable();
                //SHOOT.Disable();
                //INTERACT.Disable();
                //SUICIDE.Disable();
                //SHOP.Disable();
                //MAP.Disable();
            }
            //else
            //{
            //    WASD.Enable();
            //    KILL.Enable();
            //    SHOOT.Enable();
            //    INTERACT.Enable();
            //    SUICIDE.Enable();
            //    SHOP.Enable();
            //    MAP.Enable();
            //}

        }


    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Shop

    void OnToggleShop(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;


        if (Context.phase == InputActionPhase.Performed)
        {
            ShopMenu.SetActive(!ShopMenu.activeInHierarchy);

            if (ShopMenu.activeInHierarchy)
            {
                //WASD.Disable();
                //KILL.Disable();
                //SHOOT.Disable();
                //INTERACT.Disable();
                //SUICIDE.Disable();
            }
            //else
            //{
            //    WASD.Enable();
            //    KILL.Enable();
            //    SHOOT.Enable();
            //    INTERACT.Enable();
            //    SUICIDE.Enable();
            //}

        }


    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Back

    void OnBack(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;


        if (Context.phase == InputActionPhase.Performed)
        {
            BackMenu.SetActive(!BackMenu.activeInHierarchy);
        }


    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Add Coins

        //RPC not needed for Coin Deduction

    public void AddCoins(int NewCoins)
    {
        PV.RPC("RPC_AddCoins", RpcTarget.All, NewCoins);
    }

    [PunRPC]
    void RPC_AddCoins(int NC)
    {
        Coins += NC;

        //Notification
        MoneyNotify MNotify = Instantiate(MNotifyPF, NotifyContents);
        MNotify.Updateinfo("You", NC, true);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Loading Menu

    void HideLoadingMenu()
    {
        LoadingMenu.SetActive(false);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// MAP Toggle

    void OnToggleMap(InputAction.CallbackContext Context)
    {
        if (!PV.IsMine)
            return;


        if (Context.phase == InputActionPhase.Performed)
        {
            ToggleMap();

            //Invoke("ToggleMap", 0.5f);
        }


    }



    public void ToggleMap()
    {
        MapCamera.transform.position = new Vector3(transform.position.x, transform.position.y, MapCamera.transform.position.z); //Resets Cam pos
        MapCamera.SetActive(!MapCamera.activeInHierarchy);

        //OnScreenControls.SetActive(!OnScreenControls.activeInHierarchy);
        //DisableAllControls();
        //MAP.Enable();
    }




    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Sounds

    public void PlaySFX(AudioClip SFX)
    {
        MyAudio.PlayOneShot(SFX);
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// EXTRAS


    private void ViewShowPing()
    {
        if (ShowPing)
            PingText.text = "Ping: " + PhotonNetwork.GetPing();
    }




    //PHOTON SERIALIZEVIEW [Similar to Variable Replication - RepNotify].
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //Sent by Local Player[Is Mine]
        if (stream.IsWriting)
        {
            stream.SendNext(DirectionX);
            stream.SendNext(DirectionY);
        }
        
        //Received by Others
        else
        {
            DirectionX = (float)stream.ReceiveNext();
            DirectionY = (float)stream.ReceiveNext();
        }
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCamController : MonoBehaviour
{

    [SerializeField] InputAction SCROLL;
    [SerializeField] InputAction F1_Pos;
    [SerializeField] InputAction F2_Pos;
    [SerializeField] InputAction F1_TouchContact;
    [SerializeField] InputAction F2_TouchContact;

    [SerializeField] Camera CamRef;
    [SerializeField] float ZoomSPeed = 4f;
    [SerializeField] float MinZoom = 5f;
    [SerializeField] float MaxZoom = 40f;
    [SerializeField] float PinchThresold = 0.1f;
    [Tooltip("Use This if you wanna tweak the ZoomSpeed separately for Pinch. (-1=Invert; 0.1=Slow; 2=Double fast.)")]
    [SerializeField] float TouchZoomSpeed = 1f;

    Coroutine PanCoroutine;
    Coroutine ZoomCoroutine;


    Vector3 DragOrigin;



    private void OnEnable()
    {
        if (CamRef.transform.parent != null)
        {
            CamRef.transform.parent = null;
        }

        SCROLL.Enable();
        F1_Pos.Enable();
        F2_Pos.Enable();
        F1_TouchContact.Enable();
        F2_TouchContact.Enable();
    }


    private void OnDisable()
    {
        SCROLL.Disable();
        F1_Pos.Disable();
        F2_Pos.Disable();
        F1_TouchContact.Disable();
        F2_TouchContact.Disable();
    }



    void Start()
    {
        F1_TouchContact.started += _ => PanStart();
        F1_TouchContact.canceled += _ => PanEnd();

        F2_TouchContact.started += _ => ZoomStart();
        F2_TouchContact.canceled += _ => ZoomEnd();

    }


    private void Update()
    {

        //Scrolling only for PC
        if (!Application.isMobilePlatform)
        {
            float ScrollAxisVal = SCROLL.ReadValue<float>();
            if (ScrollAxisVal != 0)
            {
                ScrollZoom(ScrollAxisVal);
            }
        }
        
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Scroll Zoom
    public void ScrollZoom(float increment)
    {
        float CurZoom = CamRef.orthographicSize;
        float TargetZoom = Mathf.Clamp(CurZoom + increment, MinZoom, MaxZoom);

        CamRef.orthographicSize = Mathf.Lerp(CurZoom, TargetZoom, ZoomSPeed * Time.deltaTime);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Pan

    void PanStart()
    {
        if (ZoomCoroutine != null)  //Stops Zoom while Panning
            ZoomEnd();

        DragOrigin = CamRef.ScreenToWorldPoint(F1_Pos.ReadValue<Vector2>());
        PanCoroutine = StartCoroutine(PanDetection());
    }


    void PanEnd()
    {
        StopCoroutine(PanCoroutine);
    }


    IEnumerator PanDetection()  //Same As Update [Like a Timer]
    {
        while (true)
        {
            
            Vector3 difference = DragOrigin - CamRef.ScreenToWorldPoint(F1_Pos.ReadValue<Vector2>());
            CamRef.transform.position += difference;

            yield return null;
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// Zoom

    void ZoomStart()
    {
        if(PanCoroutine != null)
            PanEnd();

        ZoomCoroutine = StartCoroutine(ZoomDetection());
    }


    void ZoomEnd()
    {
        StopCoroutine(ZoomCoroutine);
    }


    IEnumerator ZoomDetection()
    {
        //Init First 2-Finger Touch
        float CurDistance = Vector2.Distance(F1_Pos.ReadValue<Vector2>(), F2_Pos.ReadValue<Vector2>());
        float PrevDistance = CurDistance;

        //Similar to Update() [Each Frame]
        while (true)
        {
            CurDistance = Vector2.Distance(F1_Pos.ReadValue<Vector2>(), F2_Pos.ReadValue<Vector2>());

            if (Mathf.Abs(CurDistance - PrevDistance) > PinchThresold)
            {
                ScrollZoom((CurDistance - PrevDistance) * TouchZoomSpeed * -1); //-1 == Inverse, 1 == Normal (For Pinch)
            }

            //Keeps Track of Previous DIstance for next loop
            PrevDistance = CurDistance;

            yield return null;

        }


    }



}

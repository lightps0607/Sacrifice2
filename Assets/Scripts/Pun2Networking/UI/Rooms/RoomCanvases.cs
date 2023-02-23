using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCanvases : MonoBehaviour
{
    [SerializeField] private CreateJoinRoomCanvas _createJoinRoomCanvas;
    public CreateJoinRoomCanvas CJRoomCanvas { get { return _createJoinRoomCanvas; } }

    [SerializeField] private CurrentRoomCanvas _currentRoomCanvas;
    public CurrentRoomCanvas CRCanvas { get { return _currentRoomCanvas; } }

    private void Awake()
    {
        FirstInit();
    }

    private void FirstInit()
    {
        CJRoomCanvas.FirstInitialize(this);
        CRCanvas.FirstInitialize(this);
    }


}

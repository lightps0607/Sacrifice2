using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    //VARIABLES
    private RoomCanvases _roomCanvases;
    [SerializeField] private PlayerListingMenu _playerListingMenu;
    [SerializeField] private LeaveRoomMenu _leaveRoomMenu;

    public LeaveRoomMenu _LeaveRoomMenu { get { return _leaveRoomMenu; } }



    //METHODS
    public void FirstInitialize(RoomCanvases canvases)
    {
        _roomCanvases = canvases;
        _playerListingMenu.FirstInitialize(canvases);
        _leaveRoomMenu.FirstInitialize(canvases);
    }



    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


}

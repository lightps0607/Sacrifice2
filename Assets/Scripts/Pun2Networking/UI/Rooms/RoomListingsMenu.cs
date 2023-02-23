using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    //VARIABLES
    [SerializeField] private Transform _content;
    [SerializeField] private Rooms_Scr _roomsPF;
    private List<Rooms_Scr> RList = new List<Rooms_Scr>();
    private RoomCanvases _roomCanvases;


    //METHODS

    public void FirstInitialize(RoomCanvases canvases)
    {
        _roomCanvases = canvases;
    }


    public override void OnJoinedRoom()
    {
        _roomCanvases.CRCanvas.Show();
        _content.DestroyChildren();
        RList.Clear();
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //Removed from RList
            if (info.RemovedFromList)
            {
                int index = RList.FindIndex(x => x.RInfo.Name == info.Name);
                if (index != -1)
                {
                    Destroy(RList[index].gameObject);
                    RList.RemoveAt(index);
                }
            }

            //Added to RList
            else
            {
                int index = RList.FindIndex(x => x.RInfo.Name == info.Name);
                if(index == -1)
                {
                    Rooms_Scr RScr = Instantiate(_roomsPF, _content);
                    if (RScr != null)
                    {
                        RScr.SetRoomInfo(info);
                        RList.Add(RScr);
                    }
                }
                else
                {
                    //Modify Rlist here.
                    //Eg. Rlist[index].DoWhatever
                }
                

            }
        }
    }


}

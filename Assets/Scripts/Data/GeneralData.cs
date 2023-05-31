using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "General Data", menuName = "GoldProject/General Data", order = 1)]
public class GeneralData : ScriptableObject
{
    private RoomList _roomList;

    public RoomList RoomList
    {
        get { return _roomList; }
    }
}

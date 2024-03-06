using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomIndicator : MonoBehaviour
{
    RoomData roomData;
    int padding;

    public void SetRoomData(RoomData roomData, int padding)
    {
        this.roomData = roomData;
        this.padding = padding;
    }

    public void SetPositionScale()
    {
        transform.position = new Vector2(padding + roomData.position.x + ((float)roomData.size.x) / 2, padding + roomData.position.y + ((float)roomData.size.y) / 2);
        transform.localScale = new Vector2(roomData.size.x, roomData.size.y);
    }
}

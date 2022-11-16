using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsController : MonoBehaviour
{
    public Room[] rooms;
    public int currentRoomId;
    public Room currentRoom;

    public void Start()
    {
        currentRoom = rooms[currentRoomId];
    }
    private void Update()
    {
        currentRoom = rooms[currentRoomId];
    }
}
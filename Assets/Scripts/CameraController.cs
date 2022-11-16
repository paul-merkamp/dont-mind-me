using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RoomsController r;

    public bool isBeingControlled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBeingControlled)
        {
            transform.position = new Vector3(r.currentRoom.roomPos.x, r.currentRoom.roomPos.y, -10);
        }
    }

    public void ForceLookRoom(int roomId)
    {
        isBeingControlled = true;
        transform.position = new Vector3(r.rooms[roomId].roomPos.x, r.rooms[roomId].roomPos.y, -10);
    }
    public void ReleaseForceLook()
    {
        isBeingControlled = false;
    }
}

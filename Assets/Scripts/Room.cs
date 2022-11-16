using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject player;
    public Vector2 roomPos;
    public int id;
    public bool roomActive;

    public GameObject entryGate;
    public GameObject exitGate;

    // Start is called before the first frame update
    void Start()
    {
        roomPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < roomPos.x + 8 && player.transform.position.x > roomPos.x - 8 && player.transform.position.y < roomPos.y + 6 && player.transform.position.y > roomPos.y - 6)
        {
            roomActive = true;
            transform.parent.gameObject.GetComponent<RoomsController>().currentRoomId = id;
        }
        else  if (player.transform.position.x > roomPos.x + 8 || player.transform.position.x < roomPos.x - 8 || player.transform.position.y > roomPos.y + 6 || player.transform.position.y < roomPos.y - 6)
        {
            roomActive = false;
        }
    }

    public void StartRoom()
    {
        if (entryGate != null) { entryGate.SetActive(true); }
        if (exitGate != null) { exitGate.SetActive(true); }
    }

    public void FinishRoom()
    {
        if (entryGate != null) { entryGate.SetActive(false); }
        if (exitGate != null) { exitGate.SetActive(false); }
    }
}

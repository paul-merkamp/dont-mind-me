using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    CameraController c = GameObject.Find("Main Camera").GetComponent<CameraController>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerEvent(int id)
    {
        // title flash at beginning
        if (id == 0)
        {
            c.ForceLookRoom(0);
        }
    }
}

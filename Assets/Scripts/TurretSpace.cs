using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpace : MonoBehaviour
{
    public bool occupied;
    public int turretId;

    public int maxHealth;
    int health;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceTurret(int type)
    {
        if (occupied == false)
        {
            transform.Find("Turret").GetComponent<Turret>().turretType = type;
            transform.Find("Turret").gameObject.SetActive(true);
            occupied = true;
        }
    }
    
    public void DestroyTurret()
    {

    }
}

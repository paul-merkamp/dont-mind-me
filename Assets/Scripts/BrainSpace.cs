using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainSpace : MonoBehaviour
{
    public bool OccupiedByBrain = false;
    public bool SpaceLocked = false;
    public GameObject TargetToDestroy;

    public bool TriggersEnemies = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int enemiesToSpawn = 0;

        EnemySpawner[] es = gameObject.transform.parent.gameObject.GetComponentsInChildren<EnemySpawner>();
        for (int i = 0; i < es.Length; i++)
        {
            enemiesToSpawn += es[i].enemyCount;
        }

        if (transform.Find("GroundJar").gameObject.GetComponent<Entity>().health == 0)
        {
            GameObject.Find("Player").GetComponent<PlayerController>().GameOver();
        }

        if (enemiesToSpawn == 0)
        {
            SpaceLocked = false;
        }
    }

    public void PlaceBrain()
    {
        if (OccupiedByBrain == false)
        {
            transform.Find("GroundJar").gameObject.SetActive(true);

            if (TargetToDestroy != null)
            {
                Destroy(TargetToDestroy);
            }

            if (TriggersEnemies == true)
            {
                EnemySpawner[] es = gameObject.transform.parent.gameObject.GetComponentsInChildren<EnemySpawner>();
                for (int i = 0; i < es.Length; i++)
                {
                    es[i].ActivateSpawner();
                }
                gameObject.transform.parent.gameObject.transform.parent.GetComponent<RoomsController>().currentRoom.StartRoom();
            }

            OccupiedByBrain = true;
        }
    }

    public void PickUpBrain()
    {
        if (OccupiedByBrain == true)
        {
            transform.Find("GroundJar").gameObject.SetActive(false);
            OccupiedByBrain = false;
        }

        GameObject.Find("Rooms").GetComponent<RoomsController>().currentRoom.FinishRoom();

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int spawnerCooldown;
    int timeLastEnemySpawned;

    public bool spawnerActive = false;

    public GameObject enemyPrefab;
    // 0: basic
    public int enemyCount;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerActive)
        {
            if (Time.time >= timeLastEnemySpawned + spawnerCooldown)
            {
                timeLastEnemySpawned = (int)Time.time;
                Instantiate(enemyPrefab, transform);
                enemyCount -= 1;
            }
            if (enemyCount == 0)
            {
                DeactivateSpawner();
            }
        }
    }

    public void ActivateSpawner()
    {
        spawnerActive = true;
        GetComponent<ParticleSystem>().Play();
        transform.Find("enemyPortal").GetComponent<Animator>().SetBool("SpawnerActive", true);
        timeLastEnemySpawned = (int)Time.time;
    }

    public void DeactivateSpawner()
    {
        StartCoroutine("DeactivateSpawnerIE");
    }
    IEnumerator DeactivateSpawnerIE()
    {
        spawnerActive = false;
        transform.Find("enemyPortal").GetComponent<Animator>().SetBool("SpawnerActive", false);
        yield return new WaitForSeconds(1);
        GetComponent<ParticleSystem>().Stop();
    }
}
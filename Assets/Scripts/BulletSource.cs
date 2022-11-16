using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSource : MonoBehaviour
{
    public float bulletSpeed = 15.0f;
    public float bulletLifeTime = 0.5f;
    public float sourceCooldown = 1f;

    public GameObject bulletPrefab;
    GameObject bullet;
    float lastFiredTime;

    Rigidbody2D bulletRb2d;

    public AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        bulletRb2d = bulletPrefab.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireBullet(string direction)
    {
        if (Time.time >= lastFiredTime + sourceCooldown)
        {
            lastFiredTime = Time.time;
            GameObject bullet = Instantiate(bulletPrefab, transform.parent);
            Destroy(bullet, bulletLifeTime);

            if (direction == "right")
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed, 0);
            }
            else if (direction == "left")
            {
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletSpeed * -1, 0);
                bullet.GetComponent<SpriteRenderer>().flipX = true;
            }

            try
            {
                GetComponent<AudioSource>().PlayOneShot(shootSound, 1);
            }
            catch
            {
                throw;
            }
        }
    }
}

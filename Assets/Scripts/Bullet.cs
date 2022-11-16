using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageDealsAmt = 1;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 oldGOposition = transform.position;
        Destroy(gameObject);
        try
        {
            Entity whatBulletHit = collision.gameObject.GetComponent<Entity>();
            if (!whatBulletHit.isPlayer)
            {
                whatBulletHit.TakeDamage(damageDealsAmt);
            }
            if (whatBulletHit.isPlayer)
            {
                PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();
                pc.TakeDamage(damageDealsAmt, oldGOposition);
                pc.StartCoroutine("TemporaryInvincibility");
            }
        }
        catch
        {
            return;
        }
    }
}

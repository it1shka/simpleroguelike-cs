using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var scr = collision.GetComponent<Health>();
        if(scr)
        scr.TakeDamage(damage);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public enum BoostType { Speed, Health, Bullet, Spread, Damage, NextLevel};
    public BoostType myType = BoostType.Speed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (myType)
        {
            case BoostType.Speed:
                collision.GetComponent<Movement>().speed += .3f;
                break;
            case BoostType.Health:
                var scr = collision.GetComponent<Health>();
                scr.maxHealth += 3;
                scr.health = scr.maxHealth;
                break;
            case BoostType.Bullet:
                collision.GetComponent<Weapon>().bulletPerShoot += 1;
                break;
            case BoostType.Spread:
                var scr2 = collision.GetComponent<Weapon>();
                scr2.bulletSpread -= 3f;
                if (scr2.bulletSpread < 0f) scr2.bulletSpread = 0f;
                break;
            case BoostType.Damage:
                collision.GetComponent<Weapon>().damage += 1;
                break;
            case BoostType.NextLevel:
                FindObjectOfType<DungeonGenerator>().GenerateMap();
                break;
        }

        Destroy(gameObject);
    }
}

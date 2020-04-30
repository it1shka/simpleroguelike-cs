using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ammo;
    public Transform firePoint;
    public float timeBtwShot = 1f;
    public float timeBtwBullet = .1f;
    private float currentTimeBtwShot;
    public float bulletSpread = 30f;
    private bool isShooting;
    public int bulletPerShoot = 1;
    public float bulletSpeed = 10f;
    public int damage = 1;
    void Start()
    {
        currentTimeBtwShot = 0f;
        isShooting = false;
    }

    void Update()
    {
        currentTimeBtwShot -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (currentTimeBtwShot > 0 || isShooting) return;
        currentTimeBtwShot = timeBtwShot;
        StartCoroutine(startShooting());
    }

    IEnumerator startShooting()
    {
        isShooting = true;
        for(int i=0; i<bulletPerShoot; i++)
        {
            var bullet = Instantiate(ammo, firePoint.position, firePoint.rotation);
            bullet.transform.Rotate(0f,0f,Random.Range(-bulletSpread / 2, bulletSpread / 2));
            bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right * bulletSpeed, ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().damage = this.damage;
            yield return new WaitForSeconds(timeBtwBullet);
        }
        isShooting = false;
        yield break;
    }
}

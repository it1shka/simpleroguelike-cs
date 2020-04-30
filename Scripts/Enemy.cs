using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class Enemy : MonoBehaviour
{
    private Transform player;
    public float speed = 5f;
    public float attackDist = 10f;
    public float stopDist = 5f;
    public LayerMask checkMask;
    private Weapon weapon;

    public float offset = 90;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = GetComponent<Weapon>();
    }

    void Update()
    {
        if (!player) return;
        var direction = (player.position - transform.position).normalized;
        var ray = Physics2D.Raycast(transform.position, 
            direction, attackDist, checkMask);
        var visible = ray && ray.transform.tag == "Player";

        if (visible)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f,0f,angle - offset);

            if(Vector2.Distance(transform.position, player.position) > stopDist)
            {
                transform.position += direction * speed * Time.deltaTime;
            }
            else
            {
                weapon.Shoot();
            }
        }
    }
}

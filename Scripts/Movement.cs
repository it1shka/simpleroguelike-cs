using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Health))]
public class Movement : MonoBehaviour
{
    [HideInInspector] public Vector2 input1, input2;
    public float speed = 5f;
    private Transform tf;
    private Health health;
    public Slider hpbar;

    public Transform scope;
    public float scopeDist = 1f;
    void Start()
    {
        tf = transform;
        health = GetComponent<Health>();
    }

    void Update()
    {
        tf.position += (Vector3)input1 * speed * Time.deltaTime;
        var direction = input2.normalized;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tf.rotation = Quaternion.Euler(0f,0f,angle);

        hpbar.value = (float)health.health / health.maxHealth;

        scope.position = (Vector2)tf.position + input2 * scopeDist;
    }

}

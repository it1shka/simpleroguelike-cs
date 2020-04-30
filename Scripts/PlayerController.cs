using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement)), RequireComponent(typeof(Weapon))]
public class PlayerController : MonoBehaviour
{
    private Movement movement;
    private Weapon weapon;
    public Joystick joystick1, joystick2;
    void Start()
    {
        movement = GetComponent<Movement>();
        weapon = GetComponent<Weapon>();
    }

    void Update()
    {
#if UNITY_ANDROID
        movement.input1 = new Vector2(joystick1.Horizontal, joystick1.Vertical);
        var delta = new Vector2(joystick2.Horizontal, joystick2.Vertical);
        movement.input2 = delta;
        if (delta != Vector2.zero)
            weapon.Shoot();
#endif

#if UNITY_EDITOR
        movement.input1 = new Vector2(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        movement.input2 = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (Input.GetKey(KeyCode.Mouse0))
            weapon.Shoot();
#endif
    }

}

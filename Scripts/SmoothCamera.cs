using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    public Transform target;
    public float speedX = 2f, speedY = 2f;
    void Update()
    {
        if (!target) return;
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, target.position.x, Time.deltaTime * speedX),
            Mathf.Lerp(transform.position.y, target.position.y, Time.deltaTime * speedY),
            -10f);
    }
}
